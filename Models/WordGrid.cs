using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WordSearchMVC5.Models
{
    public class WordGrid
    {
        public List<char[]> UserWords { get; set; }
        public Boolean ValidWord(char[] userEntry)
        {
            char[] reversedEntry = userEntry;
            Array.Reverse(reversedEntry);
            return UserWords.Contains(userEntry) || UserWords.Contains(reversedEntry);
        }
    }
}