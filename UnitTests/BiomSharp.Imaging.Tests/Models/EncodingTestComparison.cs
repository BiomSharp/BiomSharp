namespace BiomSharp.Imaging.Tests.Models;

internal sealed record EncodingTestComparison(decimal SizeDifferencePercent, BinComparison[] Bins,
    int CoefficientCount, int Mismatches, int AllowedMismatches, long MaximumIndexDifference,
    List<string> Failures)
{
    public static EncodingTestComparison Compare(WsqInspection actual, WsqInspection reference, int width, int height)
    {
        var failures = new List<string>();
        void Check(bool passed, string message)
        {
            if (!passed)
            {
                failures.Add(message);
            }
        }
        Check(actual.Header.Width == width && actual.Header.Height == height, "Encoded dimensions differ from the input PGM.");
        Check(reference.Header.Width == width && reference.Header.Height == height, "Reference dimensions differ from the input PGM.");
        Check(actual.Header.Encoder == WsqEncodingRequirements.RequiredEncoderNumber,
            $"Frame header Ev must be {WsqEncodingRequirements.RequiredEncoderNumber}.");
        Check(actual.Header.Black == reference.Header.Black && actual.Header.White == reference.Header.White,
            "Black/white calibration differs from the reference.");
        Check(actual.Header.LowFilterLength == WsqEncodingRequirements.RequiredLowFilterLength &&
            actual.Header.HighFilterLength == WsqEncodingRequirements.RequiredHighFilterLength,
            $"Expected the prescribed {WsqEncodingRequirements.RequiredLowFilterLength}/{WsqEncodingRequirements.RequiredHighFilterLength} filter.");
        Check(actual.Center == reference.Center, "Quantizer bin center differs from the reference.");
        if (reference.SizeWithoutComments <= 0 || actual.SizeWithoutComments <= 0)
        {
            throw new InvalidDataException("Cannot compare empty WSQ sizes.");
        }
        decimal sizeDifference = Math.Abs(actual.SizeWithoutComments - reference.SizeWithoutComments);
        decimal sizePercent = sizeDifference * 100m / reference.SizeWithoutComments;
        Check(sizeDifference * 100m <= reference.SizeWithoutComments * WsqEncodingRequirements.MaximumCompressedSizeDifferencePercent,
            $"Compressed size differs by {sizePercent}% (limit {WsqEncodingRequirements.MaximumCompressedSizeDifferencePercent}%, excluding comments).");

        if (actual.BinWidths.Length != WsqEncodingRequirements.ComparedSubbandCount ||
            actual.ZeroWidths.Length != WsqEncodingRequirements.ComparedSubbandCount ||
            reference.BinWidths.Length != WsqEncodingRequirements.ComparedSubbandCount ||
            reference.ZeroWidths.Length != WsqEncodingRequirements.ComparedSubbandCount)
        {
            throw new InvalidDataException($"Expected quantization widths for all {WsqEncodingRequirements.ComparedSubbandCount} subbands.");
        }
        var bins = new List<BinComparison>();
        for (int i = 0; i < WsqEncodingRequirements.ComparedSubbandCount; i++)
        {
            CompareBin(i, "Q", actual.BinWidths[i], reference.BinWidths[i]);
            CompareBin(i, "Z", actual.ZeroWidths[i], reference.ZeroWidths[i]);
        }
        void CompareBin(int subband, string kind, decimal value, decimal expected)
        {
            decimal difference = Math.Abs(value - expected);
            bool passed = difference * 100m <= expected * WsqEncodingRequirements.MaximumQuantizationBinWidthDifferencePercent;
            decimal? percent = expected == 0 ? null : difference * 100m / expected;
            bins.Add(new BinComparison(subband, kind, value, expected, percent, passed));
            Check(passed, $"Subband {subband} {kind}: actual {value}, reference {expected}; exceeds {WsqEncodingRequirements.MaximumQuantizationBinWidthDifferencePercent}% tolerance.");
        }

        bool matchingLayout = actual.BinWidths.Zip(reference.BinWidths)
            .All(pair => (pair.First == 0) == (pair.Second == 0));
        bool matchingCount = actual.Coefficients.Length == reference.Coefficients.Length;
        Check(matchingLayout, "Active subbands differ; packed coefficient positions cannot be compared.");
        Check(matchingCount, "Quantized coefficient counts differ.");
        Check(reference.Coefficients.Length > 0, "Reference has no quantized coefficients.");
        int mismatches = 0;
        long maxDifference = 0;
        int allowedMismatches = (int)(reference.Coefficients.Length *
            (100m - WsqEncodingRequirements.MinimumIdenticalCoefficientPercent) / 100m);
        if (matchingLayout && matchingCount)
        {
            for (int i = 0; i < reference.Coefficients.Length; i++)
            {
                long difference = Math.Abs((long)actual.Coefficients[i] - reference.Coefficients[i]);
                if (difference != 0)
                {
                    mismatches++;
                    maxDifference = Math.Max(maxDifference, difference);
                }
            }
            Check(mismatches <= allowedMismatches,
                $"{mismatches} of {reference.Coefficients.Length} coefficient indices differ; fewer than {WsqEncodingRequirements.MinimumIdenticalCoefficientPercent}% are identical.");
            Check(maxDifference <= WsqEncodingRequirements.MaximumCoefficientIndexDifference,
                $"Maximum coefficient index difference is {maxDifference} (limit {WsqEncodingRequirements.MaximumCoefficientIndexDifference}).");
        }
        return new EncodingTestComparison(sizePercent, bins.ToArray(), reference.Coefficients.Length,
            mismatches, allowedMismatches, maxDifference, failures);
    }
}
