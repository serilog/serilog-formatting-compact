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
--------------------------------- |----------- |---------- |------- |
                JsonFormatter1000 | 11.6479 ms | 0.3343 ms |   1.00 |
         CompactJsonFormatter1000 |  6.1992 ms | 0.0448 ms |   0.53 |
        RenderedJsonFormatter1000 | 14.9243 ms | 0.1439 ms |   1.28 |
 RenderedCompactJsonFormatter1000 |  7.0540 ms | 0.0535 ms |   0.61 |
