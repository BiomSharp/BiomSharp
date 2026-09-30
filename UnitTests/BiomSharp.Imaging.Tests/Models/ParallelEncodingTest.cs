namespace BiomSharp.Imaging.Tests.Models;

internal sealed record ParallelEncodingTest(EncodingTestCase Test, 
    int Batch, 
    TimedEncoding Encoding,
    TimeSpan BatchDuration, 
    bool HiUnchanged, 
    bool LoUnchanged);
