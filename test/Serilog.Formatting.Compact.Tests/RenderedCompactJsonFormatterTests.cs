using System;
using Newtonsoft.Json.Linq;
using Xunit;
using Serilog.Formatting.Compact.Tests.Support;
using Serilog.Events;
using System.Collections.Generic;

namespace Serilog.Formatting.Compact.Tests
{
    public class RenderedCompactJsonFormatterTests
    {
        JObject AssertValidJson(Action<ILogger> act, bool useLocalTime = false)
        {
            return Assertions.AssertValidJson(new RenderedCompactJsonFormatter(useLocalTime: useLocalTime), act);
        }

        [Fact]
        public void AnEmptyEventIsValidJson()
        {
            AssertValidJson(log => log.Information("No properties"));
        }

        [Fact]
        public void AMinimalEventIsValidJson()
        {
            var jobject = AssertValidJson(log => log.Information("One {Property}", 42));

            JToken m;
            Assert.True(jobject.TryGetValue("@m", out m));
            Assert.Equal("One 42", m.ToObject<string>());

            JToken i;
            Assert.True(jobject.TryGetValue("@i", out i));
            Assert.Equal(EventIdHash.Compute("One {Property}").ToString("x8"), i.ToObject<string>());

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

            JToken val;
            Assert.True(jobject.TryGetValue("@@Mistake", out val));
            Assert.Equal(42, val.ToObject<int>());
        }

        [Fact]
        public void UtcTimestampsAreOutputByDefault()
        {
            var testDateTime = new DateTimeOffset(2020, 10, 21, 12, 30, 30, TimeSpan.FromHours(-6));
            var logEvent = new LogEvent(testDateTime, LogEventLevel.Information, null, MessageTemplate.Empty, new List<LogEventProperty>());
            var jobject = AssertValidJson(log => log.Write(logEvent));

            JToken t;
            Assert.True(jobject.TryGetValue("@t", out t));
            Assert.Equal("2020-10-21T18:30:30.0000000Z", t.Value<string>());
        }

        [Fact]
        public void UtcTimestampsAreOutputInLocalTimeWhenConfigured()
        {
            var testDateTime = new DateTimeOffset(2020, 10, 21, 12, 30, 30, TimeSpan.FromHours(-6));
            var logEvent = new LogEvent(testDateTime, LogEventLevel.Information, null, MessageTemplate.Empty, new List<LogEventProperty>());
            var jobject = AssertValidJson(log => log.Write(logEvent), true);

            JToken t;
            Assert.True(jobject.TryGetValue("@t", out t));
            Assert.Equal("2020-10-21T12:30:30.0000000-06:00", t.Value<string>());
        }
    }
}
