# Serilog.Formatting.Compact

A simple, compact JSON-based event format for Serilog. `CompactJsonFormatter` significantly reduces the byte count of small log events when compared with Serilog's default `JsonFormatter`. It achieves this through shorter built-in property names, a leaner format, and by excluding redundant information.

### Sample

A simple `Hello, {User}` event.

```json
{"@t":"2016-06-07T13:44:57.8532799+10:00","@mt":"Hello, {User}","User":"nblumhardt"}
```

### Getting started

Install from [NuGet](https://nuget.org/packages/Serilog.Formatting.Compact):

```powershell
Install-Package Serilog.Formatting.Compact
```

The formatter is used in conjunction with sinks that accept `ITextFormatter`. For example, the [rolling file](https://github.com/serilog/serilog-sinks-rollingfile) sink:

```csharp
Log.Logger = new LoggerConfiguration()
  .WriteTo.Sink(new RollingFileSink("./logs/myapp", new CompactJsonFormatter()
  .CreateLogger();
```

### Format details

The format written by `CompactJsonFormatter` is specified generically so that implementations for other logging libraries, including _Microsoft.Extensions.Logging_, are possible if desired.

The implementation in this repository obeys the specification but does not yet support the `@m` (rendered message) property, and has no need for the `@i` (event id) property.

##### Payload

Each event is a JSON object with event data at the top level. Any JSON property on the payload object is assumed to be a regular property of the event, apart from the reified properties below.

##### Reified properties

The format defines a handful of reified properties that have special meaning:

| Property | Name | Description | Required? |
| -------- | ---- | ----------- | --------- |
| `@t`     | Timestamp | An ISO 8601 timestamp | Yes |
| `@m`     | Message | A fully-rendered message describing the event | |
| `@mt` | Message Template | Alternative to Message; specifies a [message template](http://messagetemplates.org) over the event's properties that provides for rendering into a textual description of the event | |
| `@l` | Level | An implementation-specific level identifier (string or number) | Absence implies "informational"  |
| `@x` | Exception | A language-dependent error representation potentially including backtrace | |
| `@i` | Event id | An implementation specific event id (string or number) | |
| `@r` | Renderings | If `@mt` includes properties with programming-language-specific formatting, an array of pre-rendered values for each such property | |

The `@` sigil may be escaped at the start of a user property name by doubling, e.g. `@@name` denotes a property called `@name`.

##### Batch format

When events are batched into a single payload, a newline-delimited stream of JSON documents is required. Either `\n` or `\r\n` delimiters may be used.

##### Versioning

Versioning would be additive only, with no version identifier; implementations should treat any unrecognised reified properties as if they are user data.

### Comparison

**Event**

```csharp
Log.Information("Hello, {@User}, {N:x8} at {Now}",
  new
  {
    Name = "nblumhardt",
    Tags = new[] { 1, 2, 3 }
  },
  123,
  DateTime.Now);
```

**Default `JsonFormatter`**

292 bytes.

```
{"Timestamp":"2016-06-07T13:44:57.8532799+10:00","Level":"Information","MessageT
emplate":"Hello, {@User}, {N:x8} at {Now}","Properties":{"User":{"Name":"nblumha
rdt","Tags":[1,2,3]},"N":123,"Now":"2016-06-07T13:44:57.8532799+10:00"},"Renderi
ngs":{"N":[{"Format":"x8","Rendering":"0000007b"}]}}
```

**`CompactJsonFormatter`**

192 bytes (**0.66**).

```
{"@t":"2016-06-07T13:44:57.8532799+10:00","@mt":"Hello, {@User}, {N:x8} at {Now}
","@r":["0000007b"],"User":{"Name":"nblumhardt","Tags":[1,2,3]},"N":123,"Now":20
16-06-07T13:44:57.8532799+10:00}
```

**Formatting benchmark**

| Iterations | Formatter | Time (s) | Time (rel) |
| ---------- | --------- | -------- | ---------- |
| 5,000,000 | `JsonFormatter` | 55.368 | 1.0 |
| 5,000,000 | `CompactJsonFormatter` | 32.223 | **0.58** |

