using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Text;

namespace Lab1
{
    public static class Lab1
    {
        /*
         *  популярные будем искать следующим образом:
         *  находим самое популярное слово, запоминаем его и удаляем из струтктуры.
         *  Повторяем это 10 раз
         */
        public static void Solve(string pathToFile)
        {
            var text = ReadFile(pathToFile);
            var words = ParseToWords(text);

            SolveSortedList(words);
            SolveSortedDict(words);
            SolveList(words);
            SolveDict(words);

            Console.ReadKey();
        }

        private static string ReadFile(string pathToFile)
        {
            var sr = new StreamReader(pathToFile, Encoding.Default);
            var ret = sr.ReadToEnd();

            return ret;
        }

        private static string[] ParseToWords(string text)
        {
            var ret = Regex.Matches(text, @"([^\W\d]+-[^\W\d]+|[^\W\d]+)", RegexOptions.IgnoreCase).Cast<Match>().Select(x => x.Value)
                .ToArray();

            return ret;
        }

        private static Dictionary<string, int> GetDictionary(string[] words)
        {
            var ret = new Dictionary<string, int>();

            ret = words.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());

            return ret;
        }

        private static SortedDictionary<string, int> GetSortedDictionary(string[] words)
        {
            var ret = new SortedDictionary<string, int>();
            
            foreach (var g in words.GroupBy(x => x))
                ret.Add(g.Key, g.Count());

            return ret;
        }

        private static SortedList<string, int> GetSortedList(string[] words)
        {
            var ret = new SortedList<string, int>();

            foreach (var g in words.GroupBy(x => x))
                ret.Add(g.Key, g.Count());

            return ret;
        }

        private class Pair<TFirst, TSecond>
        {
            public TFirst First;
            public TSecond Second;

            public Pair(TFirst first, TSecond second)
            {
                First = first;
                Second = second;
            }
        }

        private static List<Pair<string, int>> GetList(string[] words)
        {
            var ret = new List<Pair<string, int>>();

            ret = words.GroupBy(x => x)
                .Select(g => new Pair<string, int>(g.Key, g.Count()))
                .ToList();

            return ret;
        }

        private static void SolveDict(string[] words)
        {
            Console.WriteLine("***Dictionary***");
            GC.Collect();

            var sw = new Stopwatch();
            sw.Start();
            var dict = GetDictionary(words);
            sw.Stop();

            Console.WriteLine("Transform array: {0}", sw.ElapsedMilliseconds);

            sw.Reset();

            Console.WriteLine("Count: {0}", dict.Count);

            Console.WriteLine("Most popular words:");
            sw.Start();
            for (var i = 0; i < 10; i++)
            {
                var popularWord = dict.Aggregate((x, y) => (x.Value < y.Value) ? y : x).Key;

                Console.WriteLine("{0} {1}", popularWord, dict[popularWord]);
                dict.Remove(popularWord);
            }

            sw.Stop();

            Console.WriteLine("Find most popular: {0}", sw.ElapsedMilliseconds);
        }

        private static void SolveSortedDict(string[] words)
        {
            Console.WriteLine("***Sorted Dictionary***");
            GC.Collect();

            var sw = new Stopwatch();

            sw.Start();
            var sortedDict = GetSortedDictionary(words);

            sw.Stop();

            Console.WriteLine("Transform array: {0}", sw.ElapsedMilliseconds);
            sw.Reset();

            Console.WriteLine("Count: {0}", sortedDict.Count);

            Console.WriteLine("Most popular words:");
            sw.Start();
            for (var i = 0; i < 10; i++)
            {
                var popularWord = String.Empty;
                var cnt = 0;

                foreach (var pair in sortedDict)
                    if (pair.Value > cnt)
                    {
                        popularWord = pair.Key;
                        cnt = pair.Value;
                    }
                //var popularWord = sortedDict.Aggregate((x, y) => (x.Value < y.Value) ? y : x).Key;

                Console.WriteLine("{0} {1}", popularWord, cnt);
                sortedDict.Remove(popularWord);
            }

            sw.Stop();

            Console.WriteLine("Find most popular: {0}", sw.ElapsedMilliseconds);
        }

        private static void SolveSortedList(string[] words)
        {
            Console.WriteLine("***Sorted List***");
            GC.Collect();

            var sw = new Stopwatch();
            sw.Start();

            var sortedList = GetSortedList(words);

            sw.Stop();
            Console.WriteLine("Transform array: {0}", sw.ElapsedMilliseconds);
            sw.Reset();

            Console.WriteLine("Count: {0}", sortedList.Count);

            Console.WriteLine("Most popular words:");
            sw.Start();
            for (var i = 0; i < 10; i++)
            {
                var popularWord = String.Empty;
                var cnt = 0;

                foreach (var pair in sortedList)
                    if (pair.Value > cnt)
                    {
                        popularWord = pair.Key;
                        cnt = pair.Value;
                    }

                Console.WriteLine("{0} {1}", popularWord, cnt);
                sortedList.Remove(popularWord);
            }

            sw.Stop();

            Console.WriteLine("Find most popular: {0}", sw.ElapsedMilliseconds);
        }

        private static void SolveList(string[] words)
        {
            Console.WriteLine("***List***");
            GC.Collect();

            var sw = new Stopwatch();
            sw.Start();

            var list = GetList(words);

            sw.Stop();
            Console.WriteLine("Transform array: {0}", sw.ElapsedMilliseconds);
            sw.Reset();

            Console.WriteLine("Count: {0}", list.Count);

            Console.WriteLine("Most popular words:");
            sw.Start();
            for (var i = 0; i < 10; i++)
            {
                var popularWord = list.Aggregate((x, y) => x.Second < y.Second ? y : x);

                Console.WriteLine("{0} {1}", popularWord.First, popularWord.Second);
                list.Remove(popularWord);
            }

            sw.Stop();

            Console.WriteLine("Find most popular: {0}", sw.ElapsedMilliseconds);
        }
    }
}
