namespace BiomSharp.Imaging.Tests.Models;

internal sealed record WsqFrame(int Width, 
    int Height, 
    byte Black, 
    byte White,
    byte Encoder, 
    short Implementer, 
    byte LowFilterLength, 
    byte HighFilterLength);
