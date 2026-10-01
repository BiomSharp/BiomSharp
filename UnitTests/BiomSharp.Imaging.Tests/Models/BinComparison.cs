namespace BiomSharp.Imaging.Tests.Models;

internal sealed record BinComparison(int Subband, 
    string Kind, 
    decimal Actual, 
    decimal Reference,
   decimal? DifferencePercent, 
   bool Passed);
