// Dictionary je z daleka nejrychlejsi protoze dictionary ma konstantni operace a nemusi se kontrolovat poradi na keys
// SortedDictionary, SortedList jsou temer nastejno protoze porad kontroluji poradi klicu
// vypisovani je zanedbatelne a diky pomerne rychlemu sortu na Array tak ani Sorted struktury nejsou o mod rychlejsi
// vystup na konci file

using System;
using BenchmarkDotNet;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace TestCode
{
    public class Benchmarks
    {
        public Dictionary<string,int> UnSortedDict = new Dictionary<string, int>();
        public SortedList<string,int> SortedL = new SortedList<string, int>();
        public SortedDictionary<string,int> SortedDict = new SortedDictionary<string, int>();
        

        [Benchmark]
        public int SortedList_Adding()
        {
            TestDictionaries.AddingDifferentWords_V3(SortedL);
            return SortedL.Values[2];
        }

        [Benchmark]
        public int SortedDictionary_Adding()
        {
            TestDictionaries.AddingDifferentWords_V3(SortedDict);
            return SortedDict[TestDictionaries.words[1]];
        }

        [Benchmark]
        public int Dictionary_Sort_Adding()
        {
            TestDictionaries.AddingDifferentWords_V3(UnSortedDict);
            return UnSortedDict[TestDictionaries.words[4]];
        }

        [Benchmark]
        public OutputArrays SortedList_GettingValues_After_Adding()
        {
            TestDictionaries.AddingDifferentWords_V3(SortedL);
            return GetOutputArrays_In_Sorted(SortedL);
        }

        [Benchmark]
        public OutputArrays SortedDictionary_GettingValues_After_Adding()
        {
            TestDictionaries.AddingDifferentWords_V3(SortedDict);
            return GetOutputArrays_In_Sorted(SortedDict);
        }

        [Benchmark]
        public OutputArrays Dictionary_Sort_GettingValues_After_Adding()
        {
            TestDictionaries.AddingDifferentWords_V3(UnSortedDict);
            return GetOutputArrays_In_UnSorted(UnSortedDict);
        }
        
        public OutputArrays GetOutputArrays_In_Sorted(IDictionary<string, int> dictionary)
        {
            string[] keys = new string[dictionary.Count];
            int[] values = new int[dictionary.Count];

            int index = 0;
            foreach (var node in dictionary)
            {
                keys[index] = node.Key;
                values[index] = node.Value;
                index++;
            }

            return new OutputArrays { Keys = keys, Values = values };
        }
        public OutputArrays GetOutputArrays_In_UnSorted(Dictionary<string, int> dictionary)
        {
            string[] keys = dictionary.Keys.ToArray();
            int[] values = new int[dictionary.Count];
            Array.Sort(keys);
            for(int i = 0; i < values.Length ;i++)
            {
                values[i] = dictionary[keys[i]];
            }
            
            return new OutputArrays { Keys = keys, Values = values };
        }

    }

    public struct OutputArrays
    {
        public string[] Keys{get;set;}
        public int[] Values{get;set;}
    }

    public class TestDictionaries
    { 
		public static string[] words = {
            "a","b","c","d","e","f","g","h","i","j","k","l","m","n","o",
            "Aa","Bb","Cc","D","E","F","G","H","I","J","K","L","M","N","O",
            "SC","SX","SB","LOAD","EE","EXIT","HELP","HH","GA","OM","OS","AS","SA","AAAD","OO",
            "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O",
            "aa","bb","cc","d","e","f","g","h","i","j","k","l","m","n","o",
            "sc","sx","sb","load","ee","exit","help","hh","ga","om","os","as","sa","aaad","oo",
            "a","b","c","d","e","f","g","h","i","j","k","l","m","n","o",
            "Aa","Bb","Cc","D","E","F","G","H","I","J","K","L","M","N","O",
            "SC","SX","SB","LOAD","EE","EXIT","HELP","HH","GA","OM","OS","AS","SA","AAAD","OO",
            "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O",
            "aa","bb","cc","d","e","f","g","h","i","j","k","l","m","n","o",
            "sc","sx","sb","load","ee","exit","help","hh","ga","om","os","as","sa","aaad","oo",
            "a","b","c","d","e","f","g","h","i","j","k","l","m","n","o",
            "AA","BB","CC","D","E","F","G","H","I","J","K","L","M","N","O",
            "SC","SX","SB","LOAD","EE","EXIT","HELP","HH","GA","OM","OS","AS","SA","AAAD","OO",
            "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O",
            "AA","BB","CC","D","E","F","G","H","I","J","K","L","M","N","O",
            "SC","SX","SB","LOAD","EE","EXIT","HELP","HH","GA","OM","OS","AS","SA","AAAD","OO",
            "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O",
            "AA","BB","CC","D","E","F","G","H","I","J","K","L","M","N","O",
            "SC","SX","SB","LOAD","EE","EXIT","HELP","HH","GA","OM","OS","AS","SA","AAAD","OO",
            "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O",
            "AA","BB","CC","D","E","F","G","H","I","J","K","L","M","N","O",
            "SC","SX","SB","LOAD","EE","EXIT","HELP","HH","GA","OM","OS","AS","SA","AAAD","OO",
            "a","b","c","d","e","f","g","h","i","j","k","l","m","n","o",
            "Aa","Bb","Cc","D","E","F","G","H","I","J","K","L","M","N","O",
            "SC","SX","SB","LOAD","EE","EXIT","HELP","HH","GA","OM","OS","AS","SA","AAAD","OO",
            "a","b","c","d","e","f","g","h","i","j","k","l","m","n","o",
            "aa","bb","cc","d","e","f","g","h","i","j","k","l","m","n","o",
            "sc","sx","sb","load","ee","exit","help","hh","ga","om","os","as","sa","aaad","oo",
            "a","b","c","d","e","f","g","h","i","j","k","l","m","n","o",
            "aa","bb","cc","d","e","f","g","h","i","j","k","l","m","n","o",
            "sc","sx","sb","load","ee","exit","help","hh","ga","om","os","as","sa","aaad","oo",
            "a","b","c","d","e","f","g","h","i","j","k","l","m","n","o",
            "aa","bb","cc","d","e","f","g","h","i","j","k","l","m","n","o",
            "sc","sx","sb","load","ee","exit","help","hh","ga","om","os","as","sa","aaad","oo",
            "a","b","c","d","e","f","g","h","i","j","k","l","m","n","o",
            "Aa","Bb","Cc","D","E","F","G","H","I","J","K","L","M","N","O",
            "SC","SX","SB","LOAD","EE","EXIT","HELP","HH","GA","OM","OS","AS","SA","AAAD","OO",
            "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O",
            "aa","bb","cc","d","e","f","g","h","i","j","k","l","m","n","o",
            "sc","sx","sb","load","ee","exit","help","hh","ga","om","os","as","sa","aaad","oo",
            "a","b","c","d","e","f","g","h","i","j","k","l","m","n","o",
            "Aa","Bb","Cc","D","E","F","G","H","I","J","K","L","M","N","O",
            "SC","SX","SB","LOAD","EE","EXIT","HELP","HH","GA","OM","OS","AS","SA","AAAD","OO",
            "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O",
            "aa","bb","cc","d","e","f","g","h","i","j","k","l","m","n","o",
            "sc","sx","sb","load","ee","exit","help","hh","ga","om","os","as","sa","aaad","oo",
            "a","b","c","d","e","f","g","h","i","j","k","l","m","n","o",
            "AA","BB","CC","D","E","F","G","H","I","J","K","L","M","N","O",
            "SC","SX","SB","LOAD","EE","EXIT","HELP","HH","GA","OM","OS","AS","SA","AAAD","OO",
            "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O",
            "AA","BB","CC","D","E","F","G","H","I","J","K","L","M","N","O",
            "SC","SX","SB","LOAD","EE","EXIT","HELP","HH","GA","OM","OS","AS","SA","AAAD","OO",
            "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O",
            "AA","BB","CC","D","E","F","G","H","I","J","K","L","M","N","O",
            "SC","SX","SB","LOAD","EE","EXIT","HELP","HH","GA","OM","OS","AS","SA","AAAD","OO",
            "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O",
            "AA","BB","CC","D","E","F","G","H","I","J","K","L","M","N","O",
            "SC","SX","SB","LOAD","EE","EXIT","HELP","HH","GA","OM","OS","AS","SA","AAAD","OO",
            "a","b","c","d","e","f","g","h","i","j","k","l","m","n","o",
            "Aa","Bb","Cc","D","E","F","G","H","I","J","K","L","M","N","O",
            "SC","SX","SB","LOAD","EE","EXIT","HELP","HH","GA","OM","OS","AS","SA","AAAD","OO",
            "a","b","c","d","e","f","g","h","i","j","k","l","m","n","o",
            "aa","bb","cc","d","e","f","g","h","i","j","k","l","m","n","o",
            "sc","sx","sb","load","ee","exit","help","hh","ga","om","os","as","sa","aaad","oo",
            "a","b","c","d","e","f","g","h","i","j","k","l","m","n","o",
            "aa","bb","cc","d","e","f","g","h","i","j","k","l","m","n","o",
            "sc","sx","sb","load","ee","exit","help","hh","ga","om","os","as","sa","aaad","oo",
            "a","b","c","d","e","f","g","h","i","j","k","l","m","n","o",
            "aa","bb","cc","d","e","f","g","h","i","j","k","l","m","n","o",
            "sc","sx","sb","load","ee","exit","help","hh","ga","om","os","as","sa","aaad","oo"
        };
        public static void IncrementWordCount_V3(IDictionary<string, int> wordToCountDictionary, string word) {
			_ = wordToCountDictionary.TryGetValue(word, out int value);		// If not found, value == default(int) == 0
			value++;
			wordToCountDictionary[word] = value;
		}
        
        public static void AddingDifferentWords_V3(IDictionary<string, int> wordToCountDictionary)
        {
            foreach (var word in words)
            {
                TestDictionaries.IncrementWordCount_V3(wordToCountDictionary, word);
            }
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


| Method                                      | Mean       | Error    | StdDev   |
|-------------------------------------------- |-----------:|---------:|---------:|
| SortedList_Adding                           | 7,468.4 us | 89.22 us | 79.09 us |
| SortedDictionary_Adding                     | 7,435.0 us | 96.80 us | 85.81 us |
| Dictionary_Sort_Adding                      |   401.7 us |  4.68 us |  4.38 us |
| SortedList_GettingValues_After_Adding       | 7,133.2 us | 48.68 us | 43.15 us |
| SortedDictionary_GettingValues_After_Adding | 7,226.9 us | 80.57 us | 71.42 us |
| Dictionary_Sort_GettingValues_After_Adding  |   704.8 us | 13.71 us | 13.47 us |

// * Hints *
Outliers
  Benchmarks.SortedList_Adding: Default                           -> 1 outlier  was  removed (7.74 ms)
  Benchmarks.SortedDictionary_Adding: Default                     -> 1 outlier  was  removed (7.84 ms)
  Benchmarks.SortedList_GettingValues_After_Adding: Default       -> 1 outlier  was  removed (7.27 ms)
  Benchmarks.SortedDictionary_GettingValues_After_Adding: Default -> 1 outlier  was  removed (7.66 ms)

// * Legends *
  Mean   : Arithmetic mean of all measurements
  Error  : Half of 99.9% confidence interval
  StdDev : Standard deviation of all measurements
  1 us   : 1 Microsecond (0.000001 sec)
*/
