using System.Diagnostics;
using Newtonsoft.Json.Linq;
using Serilog.Events;
using Xunit;
using Serilog.Formatting.Compact.Tests.Support;
using Serilog.Parsing;

namespace Serilog.Formatting.Compact.Tests;

public class CompactJsonFormatterTests
{
    static JObject AssertValidJson(Action<ILogger> act)
    {
        return Assertions.AssertValidJson(new CompactJsonFormatter(), act);
    }

    [Fact]
    public void AnEmptyEventIsValidJson()
    {
        AssertValidJson(log => log.Information("No properties"));
    }

    [Fact]
    public void AMinimalEventIsValidJson()
    {
        AssertValidJson(log => log.Information("One {Property}", 42));
    }

    [Fact]
    public void MultiplePropertiesAreDelimited()
    {
        AssertValidJson(log => log.Information("Property {First} and {Second}", "One", "Two"));
    }

    [Fact]
    public void ExceptionsAreFormattedToValidJson()
    {
        AssertValidJson(log => log.Information(new DivideByZeroException(), "With exception"));
    }

    [Fact]
    public void ExceptionAndPropertiesAreValidJson()
    {
        AssertValidJson(log => log.Information(new DivideByZeroException(), "With exception and {Property}", 42));
    }

    [Fact]
    public void RenderingsAreValidJson()
    {
        AssertValidJson(log => log.Information("One {Rendering:x8}", 42));
    }

    [Fact]
    public void MultipleRenderingsAreDelimited()
    {
        AssertValidJson(log => log.Information("Rendering {First:x8} and {Second:x8}", 1, 2));
    }

    [Fact]
    public void AtPrefixedPropertyNamesAreEscaped()
    {
        // Not possible in message templates, but accepted this way
        var jobject = AssertValidJson(log => log.ForContext("@Mistake", 42)
                                                .Information("Hello"));

        Assert.True(jobject.TryGetValue("@@Mistake", out var val));
        Assert.Equal(42, val.ToObject<int>());
    }

    [Fact]
    public void TimestampIsUtc()
    {
        // Not possible in message templates, but accepted this way
        var jobject = AssertValidJson(log => log.Information("Hello"));

        Assert.True(jobject.TryGetValue("@t", out var val));
        Assert.EndsWith("Z", val.ToObject<string>());
    }

    [Fact]
    public void TraceAndSpanIdsGenerateValidJson()
    {
        var traceId = ActivityTraceId.CreateRandom();
        var spanId = ActivitySpanId.CreateRandom();
        var evt = new LogEvent(DateTimeOffset.Now, LogEventLevel.Information, null,
            new MessageTemplate(Enumerable.Empty<MessageTemplateToken>()), Enumerable.Empty<LogEventProperty>(),
            traceId, spanId);
        var json = AssertValidJson(log => log.Write(evt));
        Assert.Equal(traceId.ToHexString(), json["@tr"]);
        Assert.Equal(spanId.ToHexString(), json["@sp"]);
    }
}
