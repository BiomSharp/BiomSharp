namespace BiomSharp.Imaging.Tests.Models;

internal sealed record SequentialEncoding(EncodingTestCase Test, 
    byte[] Expected, 
    byte[] Repeated);
