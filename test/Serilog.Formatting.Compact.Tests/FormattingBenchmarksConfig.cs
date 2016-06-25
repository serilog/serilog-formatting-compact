using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

namespace Serilog.Formatting.Compact.Tests
{
    public class FormattingBenchmarksConfig : ManualConfig
    {
        public FormattingBenchmarksConfig()
        {
            this.Add(Job.Default.WithTargetCount(new Count(10)));
        }
    }
}