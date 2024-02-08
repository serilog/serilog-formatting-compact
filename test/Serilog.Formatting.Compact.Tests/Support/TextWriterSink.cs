﻿using Serilog.Core;
using Serilog.Events;

namespace Serilog.Formatting.Compact.Tests.Support;

public class TextWriterSink : ILogEventSink
{
    readonly StringWriter _output;
    readonly ITextFormatter _formatter;

    public TextWriterSink(StringWriter output, ITextFormatter formatter)
    {
        _output = output;
        _formatter = formatter;
    }

    public void Emit(LogEvent logEvent)
    {
        _formatter.Format(logEvent, _output);
    }
}