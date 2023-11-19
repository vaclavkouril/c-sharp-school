```

BenchmarkDotNet v0.13.10, Arch Linux
AMD Ryzen 3 2200U with Radeon Vega Mobile Gfx, 1 CPU, 4 logical and 2 physical cores
.NET SDK 7.0.113
  [Host]     : .NET 6.0.25 (6.0.2523.51912), X64 RyuJIT AVX2
  DefaultJob : .NET 6.0.25 (6.0.2523.51912), X64 RyuJIT AVX2


```
| Method                                      | Mean       | Error    | StdDev   |
|-------------------------------------------- |-----------:|---------:|---------:|
| SortedList_Adding                           | 7,468.4 μs | 89.22 μs | 79.09 μs |
| SortedDictionary_Adding                     | 7,435.0 μs | 96.80 μs | 85.81 μs |
| Dictionary_Sort_Adding                      |   401.7 μs |  4.68 μs |  4.38 μs |
| SortedList_GettingValues_After_Adding       | 7,133.2 μs | 48.68 μs | 43.15 μs |
| SortedDictionary_GettingValues_After_Adding | 7,226.9 μs | 80.57 μs | 71.42 μs |
| Dictionary_Sort_GettingValues_After_Adding  |   704.8 μs | 13.71 μs | 13.47 μs |
