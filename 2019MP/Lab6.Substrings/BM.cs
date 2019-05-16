using System;
using System.Collections.Generic;

namespace Lab6.Substrings
{
    public class BoyerMoore : ISubstringsFinder
    {
        private Dictionary<char, int> GetBadCharShifts(string s)
        {
            var lastEntryOfChar = new Dictionary<char, int>();
            // -2 так как нет смысла "сдвигать" в последний символ
            for (var i = s.Length - 2; i >= 0; i--)
                if (!lastEntryOfChar.ContainsKey(s[i]))
                    lastEntryOfChar.Add(s[i], i);

            return lastEntryOfChar;
        }

        private int[] GetReverseZFunction(string s)
        {
            var z = new int[s.Length];
            var left = 0;
            var right = 0;
            for (var i = 1; i < s.Length; i++)
            {
                if (i <= right)
                    z[i] = Math.Min(z[i - left], right - i + 1);
                while (z[i] + i < s.Length && s[s.Length - 1 -(i + z[i])] == s[s.Length - 1 - z[i]])
                    z[i]++;
                if (i + z[i] - 1 > right)
                {
                    right = i + z[i] + 1;
                    left = i;
                }
            }

            return z;
        }

        private int[] GetGoodSuffixShifts(string s)
        {
            var result = new int[s.Length + 1];
            var reverseZFunction = GetReverseZFunction(s);

            for (var i = 0; i < result.Length; i++)
                result[i] = s.Length;

            for (var i = s.Length - 1; i > 0; i--)
                result[s.Length - reverseZFunction[i]] = i;

            var currentPref = 0;
            for (var j = 1; j < s.Length; j++) 
                if (j + reverseZFunction[j] == s.Length)
                    for (; currentPref <= j; currentPref++)
                        if (result[currentPref] == s.Length)
                            result[currentPref] = j;

            return result;
        }

        public List<int> FindAll(string text, string pattern)
        {
            var result = new List<int>();
            var badCharShifts = GetBadCharShifts(pattern);
            var goodSuffixShifts = GetGoodSuffixShifts(pattern);
            var i = 0;
            var rBound = 0;
            var lBound = -1;
            while (i < text.Length - pattern.Length + 1)
            {
                var j = pattern.Length - 1;
                while (j >= rBound && text[i + j] == pattern[j])
                    j--;
                if (j < rBound)
                {
                    j = lBound;
                    while (j >= 0 && text[i + j] == pattern[j])
                        j--;
                }

                if (j < 0)
                {
                    result.Add(i);
                    i += goodSuffixShifts[0];
                    lBound = -1;
                    rBound = pattern.Length - goodSuffixShifts[0];
                    continue;
                }

                int badCharShift;
                if (badCharShifts.ContainsKey(text[i + j]))
                    badCharShift = j - badCharShifts[text[i + j]];
                else
                    badCharShift = j + 1;

                var goodSuffixShift = goodSuffixShifts[j + 1];

                if (goodSuffixShift >= badCharShift)
                {
                    i += goodSuffixShift;
                    rBound = pattern.Length - goodSuffixShift;
                    lBound = j - goodSuffixShift;
                }
                else
                {
                    i += badCharShift;
                    rBound = 0;
                    lBound = -1;
                }
            }

            return result;
        }
    }
}
