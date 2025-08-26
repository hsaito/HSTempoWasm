using Xunit;
using FluentAssertions;
using HSTempoWasm;

namespace HSTempoWasm.Tests
{
    public class TempoLogicTests
    {
        [Theory]
        [InlineData(120, 500)]
        [InlineData(60, 1000)]
        [InlineData(0, 0)]
        public void CalculateBPMtoMS_ReturnsExpectedMilliseconds(double bpm, short expectedMs)
        {
            // Act
            var result = TempoLogic.CalculateBPMtoMS(bpm);

            // Assert
            result.Should().Be(expectedMs);
        }

        [Theory]
        [InlineData(-10, 0)]
        [InlineData(-0.1, 0)]
        [InlineData(0.0001, 600000000)] // extremely small positive BPM
        [InlineData(0.5, 120000)]
        [InlineData(1, 60000)]
        [InlineData(1000, 60)] // very high BPM
        [InlineData(123.45, 486)] // fractional BPM
        [InlineData(double.MaxValue, 0)] // extremely large BPM
        [InlineData(double.MinValue, 0)] // extremely small (negative) BPM
        public void CalculateBPMtoMS_HandlesEdgeCases(double bpm, short expectedMs)
        {
            var result = TempoLogic.CalculateBPMtoMS(bpm);
            result.Should().Be(expectedMs);
        }
    }
}
