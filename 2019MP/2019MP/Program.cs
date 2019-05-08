using System;

namespace _2019MP
{
    class Program
    {
        static void Main()
        {
            //Lab1.Lab1.Solve(@"F:\KB3_MetProg_Yakimova\WarAndWorld.txt");
            //Lab2.AVL_tree.Lab2.Solve();
            //Lab3.HashTable.Lab3.Solve(@"F:\6 семестр\KB3_MetProg_Yakimova\WarAndWorld.txt");
            //Lab4.SkipList.Lab4.Solve();
            //Lab5.Heap.Lab5.Solve(@"test (5).txt");

            var kmp = new Lab6.Substrings.Kmp();
            var bm = new Lab6.Substrings.BoyerMoore();

            var text = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            var pattern = "aaaaaaaaaaaaaaa";

            foreach (var x in kmp.FindAll(text,pattern))
            {
                Console.Write(x+" ");
            }
            Console.WriteLine();
            foreach (var x in bm.FindAll(text, pattern))
            {
                Console.Write(x + " ");
            }


            Console.ReadKey();
        }
    }
}
