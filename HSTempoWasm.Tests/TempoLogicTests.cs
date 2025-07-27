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
    }
}
