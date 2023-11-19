// Nejlepsi je V3, tedy TryGetValue viz me vysledku na konci file
// Oduvodneni: Funkce TryGetValue je od zacatku pripravena na to ze value nemusi existovat a funguje jako try s catch blokem
// Try catch je ale pomalejsi protoze kontroluje chybi vsude mozne v kodu a neni specifikovana jen na to vytazeni hodnoty ze slovniku
// Funkce ContainsKey je nejmene vhodna protoze kontroluje klice a provadi mnoho zbytecnych porovnani
using System;
using System.Collections.Generic;
using BenchmarkDotNet;
using BenchmarkDotNet.Attributes;


namespace TestCode2
{
    public class Benchmarks
    {
        public static string word1 = "A";
        public static string word2 = "B";
        public static string word3 = "C";

        public static string[] words = {
            "a","b","c","d","e","f","g","h","i","j","k","l","m","n","o",
            "Aa","Bb","Cc","D","E","F","G","H","I","J","K","L","M","N","O",
            "SC","SX","SB","LOAD","EE","EXIT","HELP","HH","GA","OM","OS","AS","SA","AAAD","OO"
        };

        public Dictionary<string,int> dictionaryV1 = new Dictionary<string,int>();
        public Dictionary<string,int> dictionaryV2 = new Dictionary<string,int>();
        public Dictionary<string,int> dictionaryV3 = new Dictionary<string,int>();

        [Benchmark]
        public int IncrementWordCount_V1_FirstAdd()
        {
           TestDictionaries.IncrementWordCount_V1(dictionaryV1, word1);
           return dictionaryV1[word1];
        }

        [Benchmark]
        public int IncrementWordCount_V2_FirstAdd()
        {
            TestDictionaries.IncrementWordCount_V2(dictionaryV2, word2);
            return dictionaryV2[word2];
        }

        [Benchmark]
        public int IncrementWordCount_V3_FirstAdd()
        {
            TestDictionaries.IncrementWordCount_V3(dictionaryV3, word3);
            return dictionaryV3[word3];
        }
        
        [Benchmark]
        public int IncrementWordCount_V1_Add_More_Words()
        {
            AddingMultipleWords_V1();
            return dictionaryV1[word2];
        }

        [Benchmark]
        public int IncrementWordCount_V2_Add_More_Words()
        {
            AddingMultipleWords_V2();
            return dictionaryV2[word3];
        }

        [Benchmark]
        public int IncrementWordCount_V3_Add_More_Words()
        {
            AddingMultipleWords_V3();
            return dictionaryV3[word1];
        }
        
        [Benchmark]
        public int IncrementWordCount_V1_Add_Original_Words()
        {
            AddingDifferentWords_V1();
            return dictionaryV1[words[1]];
        } 
        
        [Benchmark]
        public int IncrementWordCount_V2_Add_Original_Words()
        {
            AddingDifferentWords_V2();
            return dictionaryV2[words[1]];
        }

        [Benchmark]
        public int IncrementWordCount_V3_Add_Original_Words()
        {
            AddingDifferentWords_V3();
            return dictionaryV3[words[3]];
        }


        public void AddingMultipleWords_V1()
        {
            for (int i = 0; i<100; i++){
                TestDictionaries.IncrementWordCount_V1(dictionaryV1, word1);
                for (int j = 0; j <5;j++)
                    TestDictionaries.IncrementWordCount_V1(dictionaryV1, word2);
                TestDictionaries.IncrementWordCount_V1(dictionaryV1, word3);
            }
        }
        
        public void AddingMultipleWords_V2()
        {
            for (int i = 0; i<100; i++){
                TestDictionaries.IncrementWordCount_V2(dictionaryV2, word1);
                for (int j = 0; j <5;j++)
                    TestDictionaries.IncrementWordCount_V2(dictionaryV2, word2);
                TestDictionaries.IncrementWordCount_V2(dictionaryV2, word3);
            }
        }

         public void AddingMultipleWords_V3()
        {
            for (int i = 0; i<100; i++){
                TestDictionaries.IncrementWordCount_V3(dictionaryV3, word1);
                for (int j = 0; j <5;j++)
                    TestDictionaries.IncrementWordCount_V3(dictionaryV3, word2);
                TestDictionaries.IncrementWordCount_V3(dictionaryV3, word3);
            }
        }

        public void AddingDifferentWords_V1()
        {
            foreach (var word in words)
            {
                TestDictionaries.IncrementWordCount_V1(dictionaryV1, word);
            }
        }
        
        public void AddingDifferentWords_V2()
        {
            foreach (var word in words)
            {
                TestDictionaries.IncrementWordCount_V2(dictionaryV2, word);
            }
        }
        
        public void AddingDifferentWords_V3()
        {
            foreach (var word in words)
            {
                TestDictionaries.IncrementWordCount_V3(dictionaryV3, word);
            }
        }


    }
    public class TestDictionaries
    { 
        public static void IncrementWordCount_V1(IDictionary<string, int> wordToCountDictionary, string word) {
			try {
				wordToCountDictionary[word]++;
			} catch (KeyNotFoundException) {
				wordToCountDictionary[word] = 1;
			}
		}

		public static void IncrementWordCount_V2(IDictionary<string, int> wordToCountDictionary, string word) {
			if (wordToCountDictionary.ContainsKey(word)) {
				wordToCountDictionary[word]++;
			} else {
				wordToCountDictionary[word] = 1;
			}
		}

		public static void IncrementWordCount_V3(IDictionary<string, int> wordToCountDictionary, string word) {
			_ = wordToCountDictionary.TryGetValue(word, out int value);		// If not found, value == default(int) == 0
			value++;
			wordToCountDictionary[word] = value;
		}
    }
}
/*
// * Summary *

BenchmarkDotNet v0.13.10, Arch Linux
AMD Ryzen 3 2200U with Radeon Vega Mobile Gfx, 1 CPU, 4 logical and 2 physical cores
.NET SDK 7.0.113
  [Host]     : .NET 6.0.25 (6.0.2523.51912), X64 RyuJIT AVX2
  DefaultJob : .NET 6.0.25 (6.0.2523.51912), X64 RyuJIT AVX2


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

// * Hints *
Outliers
  Benchmarks.IncrementWordCount_V1_FirstAdd: Default           -> 3 outliers were removed (530.40 ns..555.17 ns)
  Benchmarks.IncrementWordCount_V2_FirstAdd: Default           -> 2 outliers were removed (638.00 ns, 646.40 ns)
  Benchmarks.IncrementWordCount_V3_FirstAdd: Default           -> 3 outliers were removed (533.57 ns..564.34 ns)
  Benchmarks.IncrementWordCount_V1_Add_More_Words: Default     -> 3 outliers were removed (267.37 us..273.56 us)
  Benchmarks.IncrementWordCount_V2_Add_More_Words: Default     -> 3 outliers were removed (339.83 us..379.75 us)
  Benchmarks.IncrementWordCount_V3_Add_More_Words: Default     -> 1 outlier  was  removed (221.04 us)
  Benchmarks.IncrementWordCount_V1_Add_Original_Words: Default -> 1 outlier  was  removed (17.68 us)
  Benchmarks.IncrementWordCount_V2_Add_Original_Words: Default -> 1 outlier  was  removed (25.49 us)
  Benchmarks.IncrementWordCount_V3_Add_Original_Words: Default -> 2 outliers were removed (16.12 us, 16.19 us)

// * Legends *
  Mean   : Arithmetic mean of all measurements
  Error  : Half of 99.9% confidence interval
  StdDev : Standard deviation of all measurements
  1 ns   : 1 Nanosecond (0.000000001 sec) */
