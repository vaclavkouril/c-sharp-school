```

BenchmarkDotNet v0.13.10, Arch Linux
AMD Ryzen 3 2200U with Radeon Vega Mobile Gfx, 1 CPU, 4 logical and 2 physical cores
.NET SDK 7.0.113
  [Host]     : .NET 6.0.25 (6.0.2523.51912), X64 RyuJIT AVX2
  DefaultJob : .NET 6.0.25 (6.0.2523.51912), X64 RyuJIT AVX2


```
| Method                                   | Mean         | Error       | StdDev      |
|----------------------------------------- |-------------:|------------:|------------:|
| IncrementWordCount_V1_FirstAdd           |     470.9 ns |     8.85 ns |    13.52 ns |
| IncrementWordCount_V2_FirstAdd           |     602.6 ns |     6.52 ns |     5.44 ns |
| IncrementWordCount_V3_FirstAdd           |     506.7 ns |     2.34 ns |     1.83 ns |
| IncrementWordCount_V1_Add_More_Words     | 255,991.7 ns | 2,635.23 ns | 2,057.41 ns |
| IncrementWordCount_V2_Add_More_Words     | 317,275.5 ns | 6,087.48 ns | 5,978.72 ns |
| IncrementWordCount_V3_Add_More_Words     | 211,146.0 ns | 2,141.83 ns | 1,898.68 ns |
| IncrementWordCount_V1_Add_Original_Words |  17,135.1 ns |   172.84 ns |   153.21 ns |
| IncrementWordCount_V2_Add_Original_Words |  24,322.9 ns |   429.58 ns |   477.48 ns |
| IncrementWordCount_V3_Add_Original_Words |  15,740.8 ns |    85.95 ns |    71.77 ns |
