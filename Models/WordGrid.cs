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
        public int GridSize { get; set; }
        public int RowCol { get; set; }

        public WordGrid()
        {
            UserWords = new List<string>();
        }

        public void InitWordGrid(WordUserInput input)
        {
            string[] conversion = input.UserWords.Split(' ');
            foreach (var items in conversion)
            {
                UserWords.Add(items);
            }
            RowCol = input.GridSize;
            GridSize = input.GridSize * input.GridSize;
        }
        public Boolean ValidWord(string userEntry)
        {
            char[] reversedEntry = userEntry.ToCharArray();
            Array.Reverse(reversedEntry);
            return UserWords.Contains(userEntry) || UserWords.Contains(reversedEntry.ToString());
        }
    }
}