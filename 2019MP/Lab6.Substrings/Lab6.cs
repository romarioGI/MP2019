using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6.Substrings
{
    public static class Lab6
    {
        public static void Solve(string pattern, string pathToText)
        {
            var sr = new StreamReader(pathToText);
            var text = sr.ReadToEnd();

            var kmp = new Kmp();
            var bm = new BoyerMoore();
            
            Console.WriteLine("KMP");
            Solve(pattern, text, kmp);
            Console.WriteLine();
            Console.WriteLine("Boyer Moore");
            Solve(pattern, text, bm);
        }

        private static void Solve(string pattern, string text, ISubstringsFinder algo)
        {
            var sw = new Stopwatch();
            sw.Start();

            var res = algo.FindAll(text, pattern);

            sw.Stop();

            Console.WriteLine("Count: {0}", res.Count);
            Console.WriteLine("Time: {0}",sw.ElapsedMilliseconds);
            //foreach (var x in algo.FindAll(text, pattern))
            //    Console.Write(x + " ");

            GC.Collect();
        }
    }
}
