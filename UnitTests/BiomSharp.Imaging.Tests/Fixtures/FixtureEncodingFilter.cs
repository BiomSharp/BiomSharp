using BiomSharp.Imaging.Pgm;
using BiomSharp.Imaging.Tests.Models;
using BiomSharp.Imaging.Wsq;
using BiomSharp.Imaging.Wsq.Tree;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text.Json;
using Xunit.Abstractions;

namespace BiomSharp.Imaging.Tests.Fixtures;

public class FixtureEncodingFilter : FixtureEncodingBase<FixtureEncodingFilter>
{
    public ITestOutputHelper TestOutput { get; set; }       


    internal EncodingTestScenario ArrangeParallelEncodingScenario(WsqParameters parameters)
    {
        var filterType = parameters.Filter;
        Filter sharedFilter = filterType switch
        {
            WsqFilterType.Odd7x9 => Filter.Odd7x9,
            WsqFilterType.Even8x8 => Filter.Even8x8,
            _ => throw new ArgumentOutOfRangeException(nameof(filterType), filterType, "Unsupported built-in filter.")
        };
        
        float[] originalHi = sharedFilter.Hi.ToArray();
        float[] originalLo = sharedFilter.Lo.ToArray();
        string[] imageNames = 
        {
            "a001.pgm", "a002.pgm", "a018.pgm", "a039.pgm", "a070.pgm",
            "a076.pgm", "a089.pgm", "a107.pgm", "a129.pgm", "a165.pgm"
        };

        var cases = imageNames.Select(name => ArrangeEncoding(name, parameters)).ToArray();

        return new EncodingTestScenario(filterType, sharedFilter, originalHi, originalLo, cases);
    }
      

    private (bool Hi, bool Lo) CheckSharedCoefficients(EncodingTestScenario scenario)
    {
      return (scenario.OriginalHi.AsSpan().SequenceEqual(scenario.SharedFilter.Hi.ToArray()),
          scenario.OriginalLo.AsSpan().SequenceEqual(scenario.SharedFilter.Lo.ToArray()));
    }
    
    internal async Task<EncodingTestResults> EncodeSequentiallyAndInParallel(EncodingTestScenario scenario)
    {
        const int repetitions = 3;
        var sequential = new List<SequentialEncoding>();
        foreach (EncodingTestCase test in scenario.Cases)
        {
            byte[] expected = EncodeToMemory(test).Bytes;
            byte[] repeated = EncodeToMemory(test).Bytes;
            sequential.Add(new SequentialEncoding(test, expected, repeated));
        }
        var sequentialUnchanged = CheckSharedCoefficients(scenario);

        var results = new List<ParallelEncodingTest>();
        for (int batch = 1; batch <= repetitions; batch++)
        {
            using var start = new Barrier(scenario.Cases.Length);
            var batchTimer = Stopwatch.StartNew();
            // Dedicated workers avoid thread-pool starvation while all ten wait at the start barrier.
            Task<TimedEncoding>[] tasks = scenario.Cases.Select(test => Task.Factory.StartNew(
                    () => EncodeToMemory(test, start),
                    CancellationToken.None,
                    TaskCreationOptions.LongRunning,
                    TaskScheduler.Default)
                ).ToArray();

            TimedEncoding[] encodings = await Task.WhenAll(tasks);
            batchTimer.Stop();
            var unchanged = CheckSharedCoefficients(scenario);
            for (int i = 0; i < scenario.Cases.Length; i++)
            {
                results.Add(new ParallelEncodingTest(scenario.Cases[i], batch, encodings[i],
                    batchTimer.Elapsed, unchanged.Hi, unchanged.Lo));
            }
        }
        return new EncodingTestResults(sequential, results, sequentialUnchanged.Hi, sequentialUnchanged.Lo);
    }

    internal void AssertEncodingResults(EncodingTestScenario scenario, EncodingTestResults results)
    {
        WsqFilterType filterType = scenario.FilterType;
        var failures = new List<string>();
        var expectedByName = new Dictionary<string, byte[]>();
        foreach (SequentialEncoding result in results.Sequential)
        {
            Assert.NotEmpty(result.Expected);
            Assert.NotEmpty(result.Repeated);
            Assert.True(result.Expected.AsSpan().SequenceEqual(result.Repeated),
                $"{result.Test.ImageName}: sequential encoding is not deterministic.");
            expectedByName.Add(result.Test.ImageName, result.Expected);
        }

        AssertSharedCoefficients("Sequential encoding",
            results.SequentialHiUnchanged, results.SequentialLoUnchanged);

        foreach (var batch in results.Parallel.GroupBy(result => result.Batch))
        {
            var first = batch.First();
            AssertSharedCoefficients($"Batch {batch.Key}", first.HiUnchanged, first.LoUnchanged);
        }

        foreach (var result in results.Parallel)
        {
            Assert.NotEmpty(result.Encoding.Bytes);

            var sequentialComparison = new SequentialComparison("Byte-for-byte", expectedByName[result.Test.ImageName]
                .AsSpan()
                .SequenceEqual(result.Encoding.Bytes));

            var report = new
            {
                Image = result.Test.ImageName,
                BitRate = result.Test.Parameters.BitRate,
                Filter = filterType.ToString(),
                Batch = result.Batch,
                EncodingStartedUtc = result.Encoding.StartedUtc,
                EncodingEndedUtc = result.Encoding.EndedUtc,
                EncodingMilliseconds = result.Encoding.Duration.TotalMilliseconds,
                BatchMilliseconds = result.BatchDuration.TotalMilliseconds,
                SequentialComparison = sequentialComparison,
                HighPassUnchanged = result.HiUnchanged,
                LowPassUnchanged = result.LoUnchanged
            };

            TestOutput.WriteLine(JsonSerializer.Serialize(report, new JsonSerializerOptions { WriteIndented = true }));

            if (!sequentialComparison.Matches)
            {
                failures.Add($"{result.Test.ImageName}, batch {result.Batch}: parallel output differs " +
                    "from the sequential baseline.");
            }

            TestOutput.WriteLine($"{filterType}, batch {result.Batch}, {result.Test.ImageName}: " +
                $"encoding {result.Encoding.Duration.TotalMilliseconds:F3} ms; " +
                $"batch {result.BatchDuration.TotalMilliseconds:F3} ms.");
        }

        Assert.True(failures.Count == 0,
            $"{filterType} encoding failed:{Environment.NewLine}" + string.Join(Environment.NewLine, failures));

        void AssertSharedCoefficients(string phase, bool hiUnchanged, bool loUnchanged)
        {
            if (!hiUnchanged || !loUnchanged)
            {
                failures.Add($"{phase}: shared coefficients changed " +
                    $"(high-pass unchanged: {hiUnchanged}, low-pass unchanged: {loUnchanged}).");
            }

            TestOutput.WriteLine($"{filterType}, {phase}: high-pass unchanged: {hiUnchanged}; " +
                $"low-pass unchanged: {loUnchanged}.");
        }
    }
}
