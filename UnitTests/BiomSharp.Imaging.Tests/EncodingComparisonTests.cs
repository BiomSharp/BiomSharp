using BiomSharp.Imaging.Tests.Models;
using System.Globalization;
using System.Text;

namespace BiomSharp.Imaging.Tests;

[Trait("Category", "Comparator")]
public class EncodingComparisonTests
{
    [Theory]
    [InlineData(99600, true)]
    [InlineData(100400, true)]
    [InlineData(99599, false)]
    [InlineData(100401, false)]
    public void Size_tolerance_is_inclusive_and_reference_relative(long size, bool passes)
    {
        // Arrange.
        WsqInspection reference = Example();
        WsqInspection actual = reference with { SizeWithoutComments = size };

        // Act.
        EncodingTestComparison result = Compare(actual, reference);

        // Assert.
        Assert.Equal(passes, result.Failures.Count == 0);
    }

    [Theory]
    [InlineData("99.949", true)]
    [InlineData("100.051", true)]
    [InlineData("99.948999", false)]
    [InlineData("100.051001", false)]
    public void Both_bin_width_tolerances_are_checked_without_rounding(string value, bool passes)
    {
        // Arrange.
        WsqInspection reference = Example();
        WsqInspection actual = Example();
        decimal width = decimal.Parse(value, CultureInfo.InvariantCulture);
        actual.BinWidths[59] = width;
        actual.ZeroWidths[59] = width;

        // Act.
        EncodingTestComparison result = Compare(actual, reference);

        // Assert.
        Assert.Equal(passes, result.Failures.Count == 0);
        Assert.Equal(passes ? 0 : 2, result.Bins.Count(b => !b.Passed));
    }

    [Theory]
    [InlineData(0, true)]
    [InlineData(1, false)]
    public void Zero_reference_bins_require_exact_zero(int value, bool passes)
    {
        // Arrange.
        WsqInspection reference = Example();
        WsqInspection actual = Example();
        reference.BinWidths[0] = reference.ZeroWidths[0] = 0;
        actual.BinWidths[0] = actual.ZeroWidths[0] = value;

        // Act.
        EncodingTestComparison result = Compare(actual, reference);

        // Assert.
        Assert.Equal(passes, result.Failures.Count == 0);
        Assert.All(result.Bins.Where(b => b.Subband == 0), b => Assert.Null(b.DifferencePercent));
    }

    [Theory]
    [InlineData(10000, 1, 1, true)]
    [InlineData(10000, 2, 1, false)]
    [InlineData(9999, 1, 1, false)]
    [InlineData(19999, 1, 1, true)]
    [InlineData(19999, 2, 1, false)]
    [InlineData(20000, 2, 1, true)]
    [InlineData(10000, 1, 2, false)]
    [InlineData(10000, 1, -2, false)]
    public void Coefficient_identity_and_maximum_difference_are_separate_limits(
        int count, int changed, int difference, bool passes)
    {
        // Arrange.
        WsqInspection reference = Example(count);
        WsqInspection actual = Example(count);
        Array.Fill(actual.Coefficients, difference, 0, changed);

        // Act.
        EncodingTestComparison result = Compare(actual, reference);

        // Assert.
        Assert.Equal(passes, result.Failures.Count == 0);
        Assert.Equal(changed, result.Mismatches);
        Assert.Equal(count / 10000, result.AllowedMismatches);
        Assert.Equal(Math.Abs(difference), result.MaximumIndexDifference);
    }

    [Fact]
    public void Different_coefficient_counts_cannot_pass()
    {
        // Arrange.
        WsqInspection reference = Example();
        WsqInspection actual = Example(9999);

        // Act.
        EncodingTestComparison result = Compare(actual, reference);

        // Assert.
        Assert.Contains(result.Failures, f => f.Contains("counts differ"));
    }

    [Fact]
    public void Frame_dimensions_and_encoder_number_are_checked()
    {
        // Arrange.
        WsqInspection reference = Example();
        WsqInspection actual = reference with { Header = reference.Header with { Width = 1, Encoder = 0 } };

        // Act.
        EncodingTestComparison result = Compare(actual, reference);

        // Assert.
        Assert.Equal(2, result.Failures.Count);
    }

    [Theory]
    [InlineData(180, "", 0)]
    [InlineData(181, "", 1)]
    [InlineData(179, "", -1)]
    [InlineData(101, "11111111", 255)]
    [InlineData(102, "11111111", -255)]
    [InlineData(103, "0000000100000000", 256)]
    [InlineData(104, "0000000100000000", -256)]
    [InlineData(1, "", 0)]
    [InlineData(105, "00000001", 0)]
    [InlineData(106, "0000000000000001", 0)]
    public void Inspection_extracts_known_indices_without_reconstructing_pixels(int symbol, string extraBits, int expected)
    {
        // Arrange.
        byte[] fixture = CreateFixture((byte)symbol, extraBits);

        // Act.
        WsqInspection result = WsqInspection.Read(fixture);

        // Assert.
        Assert.Equal(new[] { expected }, result.Coefficients);
        Assert.Equal(new[] { 1, 0, 0 }, result.BlockCounts);
        Assert.Equal(1m, result.BinWidths[0]);
        Assert.Equal(1.2m, result.ZeroWidths[0]);
        Assert.Equal(0.44m, result.Center);
    }

    [Fact]
    public void Comment_segments_are_excluded_including_marker_and_length_bytes()
    {
        // Arrange.
        byte[] plain = CreateFixture();
        byte[] comment = { 0xff, 0xa8, 0, 5, 65, 66, 67 };
        byte[] commented = plain[..2].Concat(comment).Concat(plain[2..]).ToArray();
        WsqInspection reference = WsqInspection.Read(plain);

        // Act.
        WsqInspection actual = WsqInspection.Read(commented);

        // Assert.
        Assert.Equal(plain.Length, actual.SizeWithoutComments);
        Assert.Equal(reference.SizeWithoutComments, actual.SizeWithoutComments);
        Assert.Equal(reference.Coefficients, actual.Coefficients);
    }

    [Fact]
    public void Missing_coefficients_do_not_become_implicit_zeros()
    {
        // Arrange.
        byte[] fixture = CreateFixture(includeCoefficient: false);

        // Act.
        Exception? exception = Record.Exception(() => WsqInspection.Read(fixture));

        // Assert.
        Assert.IsType<InvalidDataException>(exception);
    }

    [Fact]
    public void Trailing_bytes_are_rejected()
    {
        // Arrange.
        byte[] fixture = CreateFixture().Concat(new byte[] { 0 }).ToArray();

        // Act.
        Exception? exception = Record.Exception(() => WsqInspection.Read(fixture));

        // Assert.
        Assert.IsType<InvalidDataException>(exception);
    }

    [Fact]
    public void Excess_coefficients_are_rejected()
    {
        // Arrange.
        byte[] fixture = CreateFixture(2);

        // Act.
        Exception? exception = Record.Exception(() => WsqInspection.Read(fixture));

        // Assert.
        Assert.IsType<InvalidDataException>(exception);
    }

    private static EncodingTestComparison Compare(WsqInspection actual, WsqInspection reference) =>
        EncodingTestComparison.Compare(actual, reference, 100, 100);

    private static WsqInspection Example(int coefficientCount = 10000) =>
        new(new WsqFrame(100, 100, 0, 255, 2, 0, 7, 9), 100000, 0.44m,
            Enumerable.Repeat(100m, 60).ToArray(), Enumerable.Repeat(100m, 60).ToArray(),
            new int[coefficientCount], new[] { coefficientCount, 0, 0 });

    // Hand-built entropy fixture: a 32x32 frame with only subband zero active (one coefficient).
    // Its artificial transform table is for parser tests, not certification or image reconstruction.
    private static byte[] CreateFixture(byte symbol = 180, string extraBits = "", bool includeCoefficient = true)
    {
        var bytes = new List<byte> { 0xff, 0xa0 };
        void Segment(byte marker, byte[] content)
        {
            int length = content.Length + 2;
            bytes.AddRange(new byte[] { 0xff, marker, (byte)(length >> 8), (byte)length });
            bytes.AddRange(content);
        }
        Segment(0xa4, new byte[] { 7, 9 }.Concat(new byte[54]).ToArray());
        var quantization = new List<byte> { 2, 0, 44, 0, 0, 1, 1, 0, 12 };
        quantization.AddRange(new byte[63 * 6]);
        Segment(0xa5, quantization.ToArray());
        Segment(0xa2, new byte[] { 0, 255, 0, 32, 0, 32, 0, 0, 0, 0, 0, 1, 2, 0, 0 });
        Segment(0xa6, new byte[] { 0, 1 }.Concat(new byte[15]).Append(symbol).ToArray());
        Segment(0xa3, new byte[] { 0 });
        if (includeCoefficient)
        {
            var bits = new StringBuilder("0" + extraBits);
            while (bits.Length % 8 != 0)
            {
                bits.Append('1');
            }
            for (int i = 0; i < bits.Length; i += 8)
            {
                byte value = Convert.ToByte(bits.ToString(i, 8), 2);
                bytes.Add(value);
                if (value == 0xff)
                {
                    bytes.Add(0);
                }
            }
        }
        Segment(0xa3, new byte[] { 0 });
        Segment(0xa3, new byte[] { 0 });
        bytes.AddRange(new byte[] { 0xff, 0xa1 });
        return bytes.ToArray();
    }
}
