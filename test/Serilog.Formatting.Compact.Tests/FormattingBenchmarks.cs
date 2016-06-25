using System;
using System.IO;
using BenchmarkDotNet.Attributes;
using Serilog.Events;
using Serilog.Formatting.Compact.Tests.Support;
using Serilog.Formatting.Json;

namespace Serilog.Formatting.Compact.Tests
{
    [Config(typeof(FormattingBenchmarksConfig))]
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

        StringWriter _buffer;

        [Setup]
        public void InitBuffer()
        {
            _buffer = new StringWriter();
        }

        [Benchmark(Baseline = true)]
        public void JsonFormatter()
        {
            _jsonFormatter.Format(_evt, _buffer);
        }

        [Benchmark]
        public void CompactJsonFormatter()
        {
            _compactFormatter.Format(_evt, _buffer);
        }

        [Benchmark]
        public void RenderedJsonFormatter()
        {
            _renderedJsonFormatter.Format(_evt, _buffer);
        }

        [Benchmark]
        public void RenderedCompactJsonFormatter()
        {
            _renderedCompactFormatter.Format(_evt, _buffer);
        }
    }
}