using BenchmarkDotNet.Running;
using Xunit;

namespace Serilog.Formatting.Compact.Tests
{
    public class Benchmarks
    {
        [Fact]
        public void Benchmark()
        {
            BenchmarkRunner.Run<FormattingBenchmarks>();
        }
    }
}
