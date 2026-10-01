namespace BiomSharp.Imaging.Tests.Models;

internal sealed record TimedEncoding(byte[] Bytes, 
    TimeSpan Duration,
    DateTimeOffset StartedUtc, 
    DateTimeOffset EndedUtc);
