using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Serilog.Formatting.Compact.Tests.Support
{
    static class Assertions
    {
        static readonly JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            DateParseHandling = DateParseHandling.None
        };

        public static JObject AssertValidJson(ITextFormatter formatter, Action<ILogger> act)
        {
            var output = new StringWriter();
            var log = new LoggerConfiguration()
                .WriteTo.Sink(new TextWriterSink(output, formatter))
                .CreateLogger();

            act(log);

            var json = output.ToString();

            // Unfortunately this will not detect all JSON formatting issues; better than nothing however.
            return JsonConvert.DeserializeObject<JObject>(json, _settings);
        }
    }
}
