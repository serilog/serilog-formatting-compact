using Xunit;

namespace Serilog.Formatting.Compact.Tests
{
    public class EventIdHashTests
    {
        [Fact]
        public void HashingIsConsistent()
        {
            var h1 = EventIdHash.Compute("Template 1");
            var h2 = EventIdHash.Compute("Template 1");
            Assert.Equal(h1, h2);
        }

        [Fact]
        public void DistinctHashesAreComputed()
        {
            var h1 = EventIdHash.Compute("Template 1");
            var h2 = EventIdHash.Compute("Template 2");
            Assert.NotEqual(h1, h2);
        }
    }
}
