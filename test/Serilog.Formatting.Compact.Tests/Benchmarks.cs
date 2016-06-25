using BenchmarkDotNet.Running;
using Xunit;

namespace Serilog.Formatting.Compact.Tests
{
    public class Benchmarks
    {
        [Fact(Skip = "Not supported on CLI 1.0.0-preview3")]
        public void Benchmark()
        {
            BenchmarkRunner.Run<FormattingBenchmarks>();
        }
    }
}
