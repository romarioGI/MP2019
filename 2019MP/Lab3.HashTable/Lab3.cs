using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Lab3.HashTable
{
    public static class Lab3
    {
        public static void Solve(string pathToFile)
        {
            var sr = new StreamReader(pathToFile);
            var text = sr.ReadToEnd();
            var words = Regex.Matches(text, @"([^\W\d]+-[^\W\d]+|[^\W\d]+)", RegexOptions.IgnoreCase).Cast<Match>()
                .Select(x => x.Value)
                .ToArray();

            SolveHashTable(words);
            SolveDictionary(words);
            SolveSortedDictionary(words);
        }

        private static void SolveDictionary(string[] words)
        {
            var sw = new Stopwatch();
            sw.Start();

            var dict = new Dictionary<string, int>();
            foreach (var w in words)
            {
                if (dict.ContainsKey(w))
                    dict[w]++;
                else
                    dict.Add(w, 1);
            }

            var sample = new List<string>();
            foreach (var pair in dict)
            {
                if (pair.Key.Length == 7)
                    sample.Add(pair.Key);
            }

            foreach (var s in sample)
            {
                dict.Remove(s);
            }

            sw.Stop();

            Console.WriteLine("Dictionary: {0}", sw.ElapsedMilliseconds);
        }

        private static void SolveHashTable(string[] words)
        {
            var sw = new Stopwatch();
            sw.Start();

            var dict = new HashTable<string, int>();
            foreach (var w in words)
            {
                if (dict.Contains(w))
                    dict[w]++;
                else
                    dict.Add(w, 1);
            }

            var sample = new List<string>();
            foreach (var pair in dict)
            {
                if (pair.Key.Length == 7)
                    sample.Add(pair.Key);
            }

            foreach (var s in sample)
            {
                dict.Remove(s);
            }

            sw.Stop();

            Console.WriteLine("My HashTable: {0}", sw.ElapsedMilliseconds);
        }

        private static void SolveSortedDictionary(string[] words)
        {
            var sw = new Stopwatch();
            sw.Start();

            var dict = new SortedDictionary<string, int>();
            foreach (var w in words)
            {
                if (dict.ContainsKey(w))
                    dict[w]++;
                else
                    dict.Add(w, 1);
            }

            var sample = new List<string>();
            foreach (var pair in dict)
            {
                if (pair.Key.Length == 7)
                    sample.Add(pair.Key);
            }

            foreach (var s in sample)
            {
                dict.Remove(s);
            }

            sw.Stop();

            Console.WriteLine("SortedDictionary: {0}", sw.ElapsedMilliseconds);
        }
    }
}
