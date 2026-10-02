using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using BiomSharp.Imaging.Wsq;

namespace BiomSharp.Imaging.Tests.Models;

internal sealed record EncodingTestReport(
    string Image,
    float BitRate,
    string Filter,
    string Runtime,
    string OS,
    string Architecture,
    string? BiomSharpVersion,
    WsqFrame ActualHeader,
    WsqFrame ReferenceHeader,
    long ActualBytesExcludingComments,
    long ReferenceBytesExcludingComments,
    int[] ActualBlockCounts,
    int[] ReferenceBlockCounts,
    EncodingTestComparison Comparison,
    string CertificationLimitation)
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? WsqImage { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SequentialWsqImage { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ReferenceWsqImage { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Batch { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTimeOffset? EncodingStartedUtc { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTimeOffset? EncodingEndedUtc { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? EncodingMilliseconds { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? BatchMilliseconds { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public SequentialComparison? SequentialComparison { get; init; }

    public static EncodingTestReport Create(string imageName, WsqParameters parameters,
        WsqInspection actual, WsqInspection reference, int width, int height) =>
        new(imageName, parameters.BitRate, parameters.Filter.ToString(),
            RuntimeInformation.FrameworkDescription, RuntimeInformation.OSDescription,
            RuntimeInformation.ProcessArchitecture.ToString(), typeof(WsqCodec).Assembly.GetName().Version?.ToString(),
            actual.Header, reference.Header, actual.SizeWithoutComments, reference.SizeWithoutComments,
            actual.BlockCounts, reference.BlockCounts, EncodingTestComparison.Compare(actual, reference, width, height),
            "Local encoder evaluation only; Sf=0 is not an assigned implementation number.");
}
