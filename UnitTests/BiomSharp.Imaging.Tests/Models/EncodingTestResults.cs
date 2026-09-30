namespace BiomSharp.Imaging.Tests.Models;

internal sealed record EncodingTestResults(List<SequentialEncoding> Sequential,
    List<ParallelEncodingTest> Parallel, 
    bool SequentialHiUnchanged, 
    bool SequentialLoUnchanged);
