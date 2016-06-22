```ini

BenchmarkDotNet=v0.9.7.0
OS=Windows
Processor=?, ProcessorCount=8
Frequency=2533317 ticks, Resolution=394.7394 ns, Timer=TSC
HostCLR=CORE, Arch=64-bit RELEASE [RyuJIT]
JitModules=?
1.0.0-preview1-002702

Type=FormattingBenchmarks  Mode=Throughput  Toolchain=Core  

```
                       Method |     Median |    StdDev | Scaled |
----------------------------- |----------- |---------- |------- |
                JsonFormatter | 12.4016 us | 0.8737 us |   1.00 |
         CompactJsonFormatter |  6.0621 us | 0.0359 us |   0.49 |
        RenderedJsonFormatter | 15.6869 us | 0.9942 us |   1.26 |
 RenderedCompactJsonFormatter |  6.8628 us | 0.4178 us |   0.55 |
| 0.0535 ms |   0.61 |
