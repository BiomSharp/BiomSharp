using BiomSharp.Imaging.Tests.Fixtures;
using BiomSharp.Imaging.Tests.Models;
using BiomSharp.Imaging.Wsq;
using Xunit.Abstractions;

namespace BiomSharp.Imaging.Tests.FilterTests;

[Trait("Category", "Encoding")]
[Trait("Category", "Concurrency")]
[Collection("WSQ shared filters")]
public partial class WsqEncoding2_25_Filter_AsyncTests : IClassFixture<FixtureEncodingFilter>
{
    public WsqEncoding2_25_Filter_AsyncTests(FixtureEncodingFilter fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _fixture.TestOutput = output;
    }

    private readonly FixtureEncodingFilter _fixture;


    [Fact]
    public async Task EncodeImagesInParallelAt2_25_UsingFilter_Even8x8()
    {
        // Arrange.
        EncodingTestScenario scenario = _fixture.ArrangeParallelEncodingScenario(_fixture.GetEncoding2_25Parameters(WsqFilterType.Even8x8));

        // Act.
        EncodingTestResults results = await _fixture.EncodeSequentiallyAndInParallel(scenario);

        // Assert.
        _fixture.AssertEncodingResults(scenario, results);
    }


    [Fact]
    public async Task EncodeImagesInParallelAt2_25_UsingFilter_Odd7x9()
    {
        // Arrange.
        EncodingTestScenario scenario = _fixture.ArrangeParallelEncodingScenario(_fixture.GetEncoding2_25Parameters(WsqFilterType.Odd7x9));

        // Act.
        EncodingTestResults results = await _fixture.EncodeSequentiallyAndInParallel(scenario);

        // Assert.
        _fixture.AssertEncodingResults(scenario, results);
    }
}
