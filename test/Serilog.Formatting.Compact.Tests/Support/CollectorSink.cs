using Serilog.Core;
using Serilog.Events;

namespace Serilog.Formatting.Compact.Tests.Support
{
    public class CollectorSink : ILogEventSink
    {
        public LogEvent LastCollected { get; private set; }

        public void Emit(LogEvent logEvent)
        {
            LastCollected = logEvent;
        }
    }
}
