using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

namespace Serilog.Formatting.Compact.Tests
{
    public class FormattingBenchmarksConfig : ManualConfig
    {
        public FormattingBenchmarksConfig()
        {
            AddJob(Job.Default.WithIterationCount(10));
        }
    }
}