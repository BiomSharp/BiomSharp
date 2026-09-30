using BiomSharp.Imaging.Wsq.Tree;
namespace BiomSharp.Imaging.Tests.FilterTests;

[Collection("WSQ shared filters")]
public class WsqFilterTests
{
    [Fact]
    public void Odd7x9_coefficients_match_expected_values()
    {
        // Arrange.
        float[] expectedHi =
        {
            0.06453888262893845F,
            -0.04068941760955844F,
            -0.41809227322221221F,
            0.78848561640566439F,
            -0.41809227322221221F,
            -0.04068941760955844F,
            0.06453888262893845F
        };
        float[] expectedLo =
        {
            0.03782845550699546F,
            -0.02384946501938000F,
            -0.11062440441842342F,
            0.37740285561265380F,
            0.85269867900940344F,
            0.37740285561265380F,
            -0.11062440441842342F,
            -0.02384946501938000F,
            0.03782845550699546F
        };  

        // Act.
        float[] actualHi = Filter.Odd7x9.Hi.ToArray();
        float[] actualLo = Filter.Odd7x9.Lo.ToArray();

        // Assert.
        Assert.Equal(expectedHi, actualHi);
        Assert.Equal(expectedLo, actualLo);
    }

    [Fact]
    public void Even8x8_coefficients_match_expected_values()
    {
        // Arrange.
        float[] expectedHi =
        {
            0.03226944131446922F,
            -0.05261415011924844F,
            -0.18870142780632693F,
            0.60328894481393847F,
            -0.60328894481393847F,
            0.18870142780632693F,
            0.05261415011924844F,
            -0.03226944131446922F
        };
        float[] expectedLo =
        {
            0.07565691101399093F,
            -0.12335584105275092F,
            -0.09789296778409587F,
            0.85269867900940344F,
            0.85269867900940344F,
            -0.09789296778409587F,
            -0.12335584105275092F,
            0.07565691101399093F
        };

        // Act.
        float[] actualHi = Filter.Even8x8.Hi.ToArray();
        float[] actualLo = Filter.Even8x8.Lo.ToArray();

        // Assert.
        Assert.Equal(expectedHi, actualHi);
        Assert.Equal(expectedLo, actualLo);
    }
}
