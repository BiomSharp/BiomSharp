using BiomSharp.Imaging.Tests.Fixtures;
using BiomSharp.Imaging.Tests.Models;
using BiomSharp.Imaging.Wsq;
using Xunit.Abstractions;

namespace BiomSharp.Imaging.Tests.EncodingTests
{
    [Trait("Category", "Benchmark")]
    [Collection("WSQ shared filters")]
    public class BenchmarkEncoding : IClassFixture<FixtureEncodingBenchmark>
    {
        public BenchmarkEncoding(FixtureEncodingBenchmark fixture, ITestOutputHelper output)
        {
            _fixture = fixture;            
            _output = output;
        }

        private readonly FixtureEncodingBenchmark _fixture;
        private readonly ITestOutputHelper _output;

        private readonly string _imageName = "a165.pgm";

        [Fact]
        public void EncodeAt2_25_UsingFilter_Even8x8()
        {
            // Arrange.
            var parameters = _fixture.GetEncoding2_25Parameters(WsqFilterType.Even8x8);
            EncodingTestCase test = _fixture.ArrangeEncoding(_imageName, parameters);
            _fixture.WarmUp(test);

            // Act.
            TimedEncoding[] results = _fixture.MeasureEncoding(test);

            // Assert.
            Assert.Equal(FixtureEncodingBenchmark.MeasuredIterations, results.Length);
            Assert.All(results, result => Assert.NotEmpty(result.Bytes));
            _fixture.WriteResults(test, results, _output);
        }

        [Fact]
        public void EncodeAt2_25_UsingFilter_Odd7x9()
        {
            // Arrange.
            var parameters = _fixture.GetEncoding2_25Parameters(WsqFilterType.Odd7x9);
            EncodingTestCase test = _fixture.ArrangeEncoding(_imageName, parameters);
            _fixture.WarmUp(test);

            // Act.
            TimedEncoding[] results = _fixture.MeasureEncoding(test);

            // Assert.
            Assert.Equal(FixtureEncodingBenchmark.MeasuredIterations, results.Length);
            Assert.All(results, result => Assert.NotEmpty(result.Bytes));
            _fixture.WriteResults(test, results, _output);
        }

        [Fact]
        public void EncodeAt0_75_UsingFilter_Even8x8()
        {
            // Arrange.
            var parameters = _fixture.GetEncoding0_75Parameters(WsqFilterType.Even8x8);
            EncodingTestCase test = _fixture.ArrangeEncoding(_imageName, parameters);
            _fixture.WarmUp(test);

            // Act.
            TimedEncoding[] results = _fixture.MeasureEncoding(test);

            // Assert.
            Assert.Equal(FixtureEncodingBenchmark.MeasuredIterations, results.Length);
            Assert.All(results, result => Assert.NotEmpty(result.Bytes));
            _fixture.WriteResults(test, results, _output);
        }

        [Fact]
        public void EncodeAt0_75_UsingFilter_Odd7x9()
        {
            // Arrange.
            var parameters = _fixture.GetEncoding0_75Parameters(WsqFilterType.Odd7x9);
            EncodingTestCase test = _fixture.ArrangeEncoding(_imageName, parameters);
            _fixture.WarmUp(test);

            // Act.
            TimedEncoding[] results = _fixture.MeasureEncoding(test);

            // Assert.
            Assert.Equal(FixtureEncodingBenchmark.MeasuredIterations, results.Length);
            Assert.All(results, result => Assert.NotEmpty(result.Bytes));
            _fixture.WriteResults(test, results, _output);
        }
    }
}
