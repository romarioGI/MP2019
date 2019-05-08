using System.Collections.Generic;

namespace Lab6.Substrings
{
    public class Kmp : ISubstringsFinder
    {
        private int[] PrefixFunction(string s)
        {
            var p = new int[s.Length];
            for (var i = 1; i < s.Length; i++)
            {
                var j = p[i - 1];
                while (j > 0 && s[i] != s[j])
                    j = p[j - 1];
                if (s[i] == s[j])
                    j++;
                p[i] = j;
            }

            return p;
        }

        public List<int> FindAll(string text, string pattern)
        {
            var result = new List<int>();
            var patternPrefFunc = PrefixFunction(pattern);
            var curPrefFuncValue = 0;
            for (var i = 0; i < text.Length; i++)
            {
                var j = curPrefFuncValue;
                while (j > 0 &&  text[i] != pattern[j])
                    j = patternPrefFunc[j - 1];
                if (text[i] == pattern[j])
                    j++;
                curPrefFuncValue = j;

                if (curPrefFuncValue == pattern.Length)
                {
                    result.Add(i - pattern.Length + 1);
                    curPrefFuncValue = patternPrefFunc[j - 1];
                }
            }

            return result;
        }
    }
}
