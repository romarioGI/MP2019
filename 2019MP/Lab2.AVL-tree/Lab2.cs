using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Lab2.AVL_tree
{
    public static class Lab2
    {
        private const int N = 10000;
        private const int L = 5000;
        private const int R = 7000;

        public static void Solve()
        {
            var rd = new Random((int)DateTime.Now.Ticks^DateTime.Now.Millisecond);
            var array = new int[N];
            for (var i = 0; i < N; i++)
                array[i] = rd.Next();

            SolveAvlTree(array);
            SolveSortedDictionary(array);
        }

        private static void SolveAvlTree(int[] array)
        {
            Console.WriteLine("AVL-tree");
            GC.Collect();

            var sw = new Stopwatch();
            sw.Start();

            var tree = new AvlTree<int,int>();
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

            //tree.Print();
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
