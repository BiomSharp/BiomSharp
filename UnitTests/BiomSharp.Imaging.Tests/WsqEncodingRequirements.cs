namespace BiomSharp.Imaging.Tests;

/// <summary>
/// Prescribed WSQ encoder acceptance values used by the local reference comparisons.
/// Test configuration, such as the selected bitrate, and image-specific reference data are separate.
/// </summary>
internal static class WsqEncodingRequirements
{
    /// <summary>
    /// Maximum absolute compressed-size difference, in percent of the reference size.
    /// Complete comment segments are excluded from both sizes; the limit is inclusive.
    /// </summary>
    /// <remarks>
    /// Source: WSQ Specification v3.1, Annex AA.2, item 1, printed page 37.
    /// <see href="https://fbibiospecs.fbi.gov/@@dvpdffiles/a/f/aff54b5436a846dabd4b2e110d3d819b/text/dump_45.txt">FBI encoder compliance measures</see>.
    /// </remarks>
    public const decimal MaximumCompressedSizeDifferencePercent = 0.4m;

    /// <summary>
    /// Maximum absolute difference for each quantization bin width and zero-bin width,
    /// in percent of its corresponding reference value. The limit is inclusive and
    /// applied without rounding; a zero reference width requires an exact zero.
    /// </summary>
    /// <remarks>
    /// Source: WSQ Specification v3.1, Annex AA.2, item 2, printed page 37.
    /// <see href="https://fbibiospecs.fbi.gov/@@dvpdffiles/a/f/aff54b5436a846dabd4b2e110d3d819b/text/dump_45.txt">FBI encoder compliance measures</see>.
    /// </remarks>
    public const decimal MaximumQuantizationBinWidthDifferencePercent = 0.051m;

    /// <summary>
    /// Minimum percentage of quantized coefficient bin indices that must exactly match
    /// the reference in each image, not reconstructed pixels. The allowed mismatch count
    /// is rounded down to a whole number; results are not averaged across images.
    /// </summary>
    /// <remarks>
    /// Source: WSQ Specification v3.1, Annex AA.2, item 3, printed page 37.
    /// <see href="https://fbibiospecs.fbi.gov/@@dvpdffiles/a/f/aff54b5436a846dabd4b2e110d3d819b/text/dump_45.txt">FBI encoder compliance measures</see>.
    /// </remarks>
    public const decimal MinimumIdenticalCoefficientPercent = 99.99m;

    /// <summary>
    /// Maximum absolute difference between any quantized coefficient bin index and its
    /// reference index. This inclusive limit applies independently of the identity percentage.
    /// </summary>
    /// <remarks>
    /// Source: WSQ Specification v3.1, Annex AA.2, item 3, printed page 37.
    /// <see href="https://fbibiospecs.fbi.gov/@@dvpdffiles/a/f/aff54b5436a846dabd4b2e110d3d819b/text/dump_45.txt">FBI encoder compliance measures</see>.
    /// </remarks>
    public const int MaximumCoefficientIndexDifference = 1;

    /// <summary>
    /// Required frame-header Ev value identifying FBI Encoder Number Two.
    /// This is not the NIST-assigned software implementation identifier in Sf.
    /// </summary>
    /// <remarks>
    /// Source: NIST WSQ certification procedure, encoder test instructions, referring
    /// to the WSQ Specification v3.1 frame-header definition on printed page 20.
    /// <see href="https://www.nist.gov/programs-projects/wsq-certification-procedure">NIST certification procedure</see>.
    /// </remarks>
    public const byte RequiredEncoderNumber = 2;

    /// <summary>
    /// Required low-pass filter length, in taps, for the prescribed FBI encoder.
    /// Used to check the encoded transform table, not to restrict general decoder support.
    /// </summary>
    /// <remarks>
    /// Source: WSQ Specification v3.1, Part III, FBI Encoder Number Two parameter settings.
    /// <see href="https://fbibiospecs.fbi.gov/file-repository/wsq_gray-scale_specification_version_3_1_final.pdf/view">FBI WSQ specification</see>.
    /// </remarks>
    public const byte RequiredLowFilterLength = 7;

    /// <summary>
    /// Required high-pass filter length, in taps, for the prescribed FBI encoder.
    /// Used to check the encoded transform table, not to restrict general decoder support.
    /// </summary>
    /// <remarks>
    /// Source: WSQ Specification v3.1, Part III, FBI Encoder Number Two parameter settings.
    /// <see href="https://fbibiospecs.fbi.gov/file-repository/wsq_gray-scale_specification_version_3_1_final.pdf/view">FBI WSQ specification</see>.
    /// </remarks>
    public const byte RequiredHighFilterLength = 9;

    /// <summary>
    /// Number of subbands whose quantization and zero-bin widths are compared.
    /// Covers indices 0 through 59, including zero-width (inactive) subbands.
    /// </summary>
    /// <remarks>
    /// Source: WSQ Specification v3.1, Annex AA.2, item 2, printed page 37.
    /// <see href="https://fbibiospecs.fbi.gov/@@dvpdffiles/a/f/aff54b5436a846dabd4b2e110d3d819b/text/dump_45.txt">FBI encoder compliance measures</see>.
    /// </remarks>
    public const int ComparedSubbandCount = 60;
}
