using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WordSearchMVC5.Models
{
    public class WordGrid
    {
        public List<string> UserWords { get; set; }
        [Required]
        public string Words { get; set; }
        [Required]
        public int GridSize { get; set; }

        public WordGrid()
        {
            UserWords = new List<string>();
        }

        public void ConvertToUserWords()
        {
            string[] conversion = Words.Split(' ');
            foreach (var items in conversion) 
            {
                UserWords.Add(items);
            }
        }
        public Boolean ValidWord(string userEntry)
        {
            char[] reversedEntry = userEntry.ToCharArray();
            Array.Reverse(reversedEntry);
            return UserWords.Contains(userEntry) || UserWords.Contains(reversedEntry.ToString());
        }
    }
}