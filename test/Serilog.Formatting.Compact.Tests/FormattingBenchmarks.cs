using System;
using System.IO;
using BenchmarkDotNet.Attributes;
using Serilog.Events;
using Serilog.Formatting.Compact.Tests.Support;
using Serilog.Formatting.Json;

namespace Serilog.Formatting.Compact.Tests
{
    public class FormattingBenchmarks
    {
        readonly LogEvent _evt;
        readonly ITextFormatter _jsonFormatter = new JsonFormatter(),
            _compactFormatter = new CompactJsonFormatter(),
            _renderedJsonFormatter = new JsonFormatter(renderMessage: true),
            _renderedCompactFormatter = new RenderedCompactJsonFormatter();

        public FormattingBenchmarks()
        {
            var collectorSink = new CollectorSink();

            new LoggerConfiguration()
                .WriteTo.Sink(collectorSink)
                .CreateLogger()
                .Information("Hello, {@User}, {N:x8} at {Now}", new { Name = "nblumhardt", Tags = new[] { 1, 2, 3 } }, 123, DateTime.Now);

            _evt = collectorSink.LastCollected;
        }

        void Run(ITextFormatter formatter, int iterations)
        {
            for (var i = 0; i < iterations; ++i)
            {
                var sw = new StringWriter();
                formatter.Format(_evt, sw);
            }
        }

        [Benchmark(Baseline = true)]
        public void JsonFormatter1000()
        {
            Run(_jsonFormatter, 1000);
        }

        [Benchmark]
        public void CompactJsonFormatter1000()
        {
            Run(_compactFormatter, 1000);
        }

        [Benchmark]
        public void RenderedJsonFormatter1000()
        {
            Run(_renderedJsonFormatter, 1000);
        }

        [Benchmark]
        public void RenderedCompactJsonFormatter1000()
        {
            Run(_renderedCompactFormatter, 1000);
        }
    }
}