using System;
using System.Collections.Generic;
using BenchmarkDotNet;
using BenchmarkDotNet.Attributes;


namespace TestCode2
{
    public class Benchmarks
    {
        private static string _word1 = "A";
        private static string _word2 = "B";
        private Dictionary<string,int> _dictionary = new Dictionary<string,int>();

        [Benchmark]
        public void IncrementWordCount_V1_try_catch()
        {
            // Implement your benchmark here
        }

        [Benchmark]
        public void IncrementWordCount_V2_Contains()
        {
            // Implement your benchmark here
        }

        [Benchmark]
        public void IncrementWordCount_V3_TryGetValue()
        {
            // Implement your benchmark here
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
