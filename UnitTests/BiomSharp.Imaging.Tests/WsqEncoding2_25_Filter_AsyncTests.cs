//using System.Diagnostics;
//using System.Text.Json;
//using BiomSharp.Imaging.Tests.Models;
//using BiomSharp.Imaging.Wsq;
//using BiomSharp.Imaging.Wsq.Tree;
//using Xunit.Abstractions;

//namespace BiomSharp.Imaging.Tests;

//[Trait("Category", "Encoding")]
//[Trait("Category", "Concurrency")]
//[Collection("WSQ shared filters")]
//public partial class WsqEncoding2_25_Filter_AsyncTests : WsqEncoding2_25_BaseTests
//{
//    public WsqEncoding2_25_Filter_AsyncTests(ITestOutputHelper output) : base(output) { }  

//    private EncodingTestScenario ArrangeParallelEncodingScenario(WsqFilterType filterType)
//    {
//        Filter sharedFilter = filterType switch
//        {
//            WsqFilterType.Odd7x9 => Filter.Odd7x9,
//            WsqFilterType.Even8x8 => Filter.Even8x8,
//            _ => throw new ArgumentOutOfRangeException(nameof(filterType), filterType, "Unsupported built-in filter.")
//        };
//        float[] originalHi = sharedFilter.Hi.ToArray();
//        float[] originalLo = sharedFilter.Lo.ToArray();
//        string[] imageNames =
//        {
//            "a001.pgm", "a002.pgm", "a018.pgm", "a039.pgm", "a070.pgm",
//            "a076.pgm", "a089.pgm", "a107.pgm", "a129.pgm", "a165.pgm"
//        };
//        var cases = imageNames.Select(name => ArrangeEncoding(name, filterType))
//            .ToArray();
//        return new EncodingTestScenario(filterType, sharedFilter, originalHi, originalLo, cases);
//    }

  

//    private void AssertEncodingResults(EncodingTestScenario scenario, EncodingTestResults results)
//    {
//        WsqFilterType filterType = scenario.FilterType;
//        var failures = new List<string>();
//        var expectedByName = new Dictionary<string, byte[]>();
//        foreach (SequentialEncoding result in results.Sequential)
//        {
//            Assert.NotEmpty(result.Expected);
//            Assert.NotEmpty(result.Repeated);
//            if (filterType == WsqFilterType.Odd7x9)
//            {
//                AssertMatchesReference(result.Test, result.Expected);
//            }
//            Assert.True(result.Expected.AsSpan().SequenceEqual(result.Repeated),
//                $"{result.Test.ImageName}: sequential encoding is not deterministic.");
//            expectedByName.Add(result.Test.ImageName, result.Expected);
//        }
//        AssertSharedCoefficients("Sequential encoding",
//            results.SequentialHiUnchanged, results.SequentialLoUnchanged);

//        foreach (var batch in results.Parallel.GroupBy(result => result.Batch))
//        {
//            var first = batch.First();
//            AssertSharedCoefficients($"Batch {batch.Key}", first.HiUnchanged, first.LoUnchanged);
//        }
//        foreach (var result in results.Parallel)
//        {
//            Assert.NotEmpty(result.Encoding.Bytes);
//            EncodingTestReport? referenceReport = filterType == WsqFilterType.Odd7x9
//                ? ReadComparisonReport(result.Test, result.Encoding.Bytes)
//                : null;
//            var sequentialComparison = new SequentialComparison("Byte-for-byte",
//                expectedByName[result.Test.ImageName].AsSpan().SequenceEqual(result.Encoding.Bytes));
//            var report = new
//            {
//                Image = result.Test.ImageName,
//                BitRate = result.Test.Parameters.BitRate,
//                Filter = filterType.ToString(),
//                Batch = result.Batch,
//                EncodingStartedUtc = result.Encoding.StartedUtc,
//                EncodingEndedUtc = result.Encoding.EndedUtc,
//                EncodingMilliseconds = result.Encoding.Duration.TotalMilliseconds,
//                BatchMilliseconds = result.BatchDuration.TotalMilliseconds,
//                SequentialComparison = sequentialComparison,
//                HighPassUnchanged = result.HiUnchanged,
//                LowPassUnchanged = result.LoUnchanged,
//                NistComparison = referenceReport
//            };
//            output.WriteLine(JsonSerializer.Serialize(report,
//                new JsonSerializerOptions { WriteIndented = true }));
//            if (referenceReport != null)
//            {
//                failures.AddRange(referenceReport.Comparison.Failures.Select(failure =>
//                    $"{result.Test.ImageName}, batch {result.Batch}: {failure}"));
//            }
//            if (!sequentialComparison.Matches)
//            {
//                failures.Add($"{result.Test.ImageName}, batch {result.Batch}: parallel output differs " +
//                    "from the sequential baseline.");
//            }
//            output.WriteLine($"{filterType}, batch {result.Batch}, {result.Test.ImageName}: " +
//                $"encoding {result.Encoding.Duration.TotalMilliseconds:F3} ms; " +
//                $"batch {result.BatchDuration.TotalMilliseconds:F3} ms.");
//        }
//        Assert.True(failures.Count == 0,
//            $"{filterType} encoding failed:{Environment.NewLine}" + string.Join(Environment.NewLine, failures));

//        void AssertSharedCoefficients(string phase, bool hiUnchanged, bool loUnchanged)
//        {
//            if (!hiUnchanged || !loUnchanged)
//            {
//                failures.Add($"{phase}: shared coefficients changed " +
//                    $"(high-pass unchanged: {hiUnchanged}, low-pass unchanged: {loUnchanged}).");
//            }
//            output.WriteLine($"{filterType}, {phase}: high-pass unchanged: {hiUnchanged}; " +
//                $"low-pass unchanged: {loUnchanged}.");
//        }
//    }

//    private static (bool Hi, bool Lo) CheckSharedCoefficients(EncodingTestScenario scenario) =>
//        (scenario.OriginalHi.AsSpan().SequenceEqual(scenario.SharedFilter.Hi.ToArray()),
//            scenario.OriginalLo.AsSpan().SequenceEqual(scenario.SharedFilter.Lo.ToArray()));

//    private static TimedEncoding EncodeToMemory(EncodingTestCase test, Barrier? start = null)
//    {
//        try
//        {
//            var codec = new WsqCodec();
//            codec.FromRaw(test.Bitmap.Clone());
//            WsqParameters parameters = CreateEncodingParameters(test.Parameters.Filter);
//            if (start != null && !start.SignalAndWait(TimeSpan.FromSeconds(30)))
//            {
//                throw new TimeoutException("Parallel WSQ encoders did not reach the start barrier within 30 seconds.");
//            }
//            DateTimeOffset startedUtc = DateTimeOffset.UtcNow;
//            var timer = Stopwatch.StartNew();
//            byte[] bytes = codec.Encode(parameters)
//                ?? throw new InvalidDataException($"WSQ encoder returned no output for {test.ImageName}.");
//            timer.Stop();
//            DateTimeOffset endedUtc = DateTimeOffset.UtcNow;
//            return new TimedEncoding(bytes, timer.Elapsed, startedUtc, endedUtc);
//        }
//        finally
//        {
//            // A worker that fails during preparation must not leave the other workers blocked.
//            start?.RemoveParticipant();
//        }
//    }

//    [Fact]
//    public async Task EncodeImagesInParallelAt2_25_UsingFilter_Even8x8()
//    {
//        // Arrange.
//        EncodingTestScenario scenario = ArrangeParallelEncodingScenario(WsqFilterType.Even8x8);

//        // Act.
//        EncodingTestResults results = await EncodeSequentiallyAndInParallel(scenario);

//        // Assert.
//        AssertEncodingResults(scenario, results);
//    }


//    [Fact]
//    public async Task EncodeImagesInParallelAt2_25_UsingFilter_Odd7x9()
//    {
//        // Arrange.
//        EncodingTestScenario scenario = ArrangeParallelEncodingScenario(WsqFilterType.Odd7x9);

//        // Act.
//        EncodingTestResults results = await EncodeSequentiallyAndInParallel(scenario);

//        // Assert.
//        AssertEncodingResults(scenario, results);
//    }
//}
