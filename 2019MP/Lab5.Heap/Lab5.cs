using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Lab5.Heap
{
    public static class Lab5
    {
        public static void Solve(string path)
        {
            using (var sw = new StreamReader(path))
            {
                var lists = sw.ReadToEnd().Split(new[] {'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries)
                    .Select(line =>
                        line.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries)
                            .Select(int.Parse)
                            .ToList())
                    .ToArray();
                Solve(lists);
            }
        }

        public static void Solve<T>(List<T>[] lists)
        {
            var sw = new Stopwatch();
            sw.Start();

            var countOfElements = 0;
            var countOfLists = lists.Length;

            var pointers = new int[countOfLists];
            for (var i = 0; i < countOfLists; i++)
                pointers[i] = 0;
            var heap = new BinaryHeap<T, int>();
            for (var i = 0; i < countOfLists; i++)
            {
                countOfElements += lists[i].Count;
                if (lists[i].Count != 0)
                    heap.Push(lists[i][0], i);
            }

            var res = new List<T>(countOfElements);

            for (var i = 0; i < countOfElements; i++)
            {
                var cur = heap.PopMin();
                res.Add(cur.Key);
                pointers[cur.Value]++;
                if (pointers[cur.Value] < lists[cur.Value].Count)
                    heap.Push(lists[cur.Value][pointers[cur.Value]], cur.Value);
            }

            sw.Stop();

            Console.WriteLine("-----Output-----");
            Console.WriteLine("count of lists {0}, count of elements {1}", countOfLists, countOfElements);
            Console.WriteLine("time:" + sw.ElapsedMilliseconds);

            if (countOfElements < 1000)
            {
                foreach (var x in res)
                {
                    Console.Write(x + " ");
                }
            }


            Console.WriteLine("\n-----O(nlogn)-----");

            sw.Start();

            res = new List<T>(countOfElements);
            foreach (var l in lists)
            {
                foreach (var x in l)
                {
                    res.Add(x);
                }
            }

            res.Sort();

            sw.Stop();

            Console.WriteLine("-----Output-----");
            Console.WriteLine("count of lists {0}, count of elements {1}", countOfLists, countOfElements);
            Console.WriteLine("time:" + sw.ElapsedMilliseconds);

            if (countOfElements < 1000)
            {
                foreach (var x in res)
                {
                    Console.Write(x + " ");
                }
            }
        }
    }
}
