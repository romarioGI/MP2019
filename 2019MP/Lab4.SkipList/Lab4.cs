using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Lab4.SkipList
{
    public static class Lab4
    {
        private const int N = 100000;
        private const int L = 50000;
        private const int R = 70000;

        public static void Solve()
        {
            var rd = new Random((int)DateTime.Now.Ticks ^ DateTime.Now.Millisecond);
            var array = new int[N];
            for (var i = 0; i < N; i++)
                array[i] = rd.Next();

            SolveSortedList(array);
            SolveSkipList(array);
            SolveSortedDictionary(array);
        }

        private static void SolveSortedList(int[] array)
        {
            Console.WriteLine("SortedList");
            GC.Collect();

            var sw = new Stopwatch();
            sw.Start();

            var sortedList = new SortedList<int,int>();
            foreach (var a in array)
            {
                if (sortedList.ContainsKey(a))
                    sortedList[a]++;
                else
                    sortedList.Add(a, 1);
            }

            for (var i = L; i <= R; i++)
                sortedList.Remove(array[i]);

            foreach (var a in array)
                sortedList.ContainsKey(a);

            sw.Stop();

            Console.WriteLine(sw.ElapsedMilliseconds);
        }

        private static void SolveSkipList(int[] array)
        {
            Console.WriteLine("SkipList");
            GC.Collect();

            var sw = new Stopwatch();
            sw.Start();

            var skipList = new SkipList<int,int>();
            foreach (var a in array)
            {
                if (skipList.ContainsKey(a))
                    skipList[a]++;
                else
                    skipList.Add(a, 1);
            }

            for (var i = L; i <= R; i++)
                skipList.Remove(array[i]);

            foreach (var a in array)
                skipList.ContainsKey(a);

            sw.Stop();

            Console.WriteLine(sw.ElapsedMilliseconds);
        }

        private static void SolveSortedDictionary(int[] array)
        {
            Console.WriteLine("SortedDictionary");
            GC.Collect();

            var sw = new Stopwatch();
            sw.Start();

            var tree = new SortedDictionary<int, int>();
            foreach (var a in array)
            {
                if (tree.ContainsKey(a))
                    tree[a]++;
                else
                    tree.Add(a, 1);
            }

            for (var i = L; i <= R; i++)
                tree.Remove(array[i]);

            foreach (var a in array)
                tree.ContainsKey(a);

            sw.Stop();

            Console.WriteLine(sw.ElapsedMilliseconds);
        }
    }
}
