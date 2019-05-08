using System.Collections.Generic;

namespace Lab6.Substrings
{
    public interface ISubstringsFinder
    {
        List<int> FindAll(string text, string pattern);
    }
}
