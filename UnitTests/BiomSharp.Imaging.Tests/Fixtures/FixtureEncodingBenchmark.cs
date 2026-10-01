using BiomSharp.Imaging.Tests.Models;
using System.Diagnostics;
using Xunit.Abstractions;

namespace BiomSharp.Imaging.Tests.Fixtures;

public sealed class FixtureEncodingBenchmark : FixtureEncodingBase<FixtureEncodingBenchmark>
{
    internal const int MeasuredIterations = 5;

    internal void WarmUp(EncodingTestCase test)
    {
        _ = EncodeToMemory(test);
    }

    internal TimedEncoding[] MeasureEncoding(EncodingTestCase test)
    {
        var results = new TimedEncoding[MeasuredIterations];
        for (int i = 0; i < results.Length; i++)
        {
            results[i] = EncodeToMemory(test);
        }
        return results;
    }

    internal void WriteResults(EncodingTestCase test, TimedEncoding[] results, ITestOutputHelper output)
    {
#if DEBUG
        output.WriteLine("Build: Debug. Use Release for representative encoding timings.");
#else
        output.WriteLine("Build: Release.");
#endif
        output.WriteLine($"Debugger attached: {Debugger.IsAttached}.");
        output.WriteLine($"{test.Parameters.Filter}, {test.ImageName}, " +
            $"{test.Bitmap.Width}x{test.Bitmap.Height}, bitrate {test.Parameters.BitRate}: " +
            $"{results.Length} sequential samples after one warm-up.");
        for (int i = 0; i < results.Length; i++)
        {
            output.WriteLine($"Sample {i + 1}: {results[i].Duration.TotalMilliseconds:F3} ms; " +
                $"{results[i].Bytes.Length} encoded bytes.");
        }
        double[] milliseconds = results.Select(result => result.Duration.TotalMilliseconds).ToArray();
        output.WriteLine($"Minimum: {milliseconds.Min():F3} ms; " +
            $"average: {milliseconds.Average():F3} ms; maximum: {milliseconds.Max():F3} ms.");
    }
}
