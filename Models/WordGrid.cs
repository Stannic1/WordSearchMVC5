using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace WordSearchMVC5.Models
{
    public class WordGrid
    {
        /*
         * [0] [1] [2]
         * [3]     [4]
         * [5] [6] [7]
         */

        struct Direction
        {
            public int x { get; set; }
            public int y { get; set; }

            public Direction(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }
        #region variables for razor pages.
        public List<string> UserWords { get; set; }
        public int GridSize { get; set; }
        public int RowCol { get; set; }
        public char[] GridContent { get; set; }
        public string SessionKey { get; set; }
        #endregion

        #region private variables for the model
        private List<string> _wordlist;
        private List<Direction> directions;
        private Random rand;
        #endregion

        public Boolean InitWordGrid(WordUserInput input, string key)
        {
            _wordlist = new List<string>();
            UserWords = _wordlist;
            RowCol = input.GridSize;
            GridSize = input.GridSize * input.GridSize;
            GridContent = new char[GridSize];
            string[] conversion = input.UserWords.Split(' ');
            int getCount = 0;
            Trace.WriteLine("Before Checking items");
            foreach (var items in conversion)
            {
                UserWords.Add(items);
                getCount = items.Length;
                if (items.Length > RowCol || getCount > GridSize)
                {
                    Trace.WriteLine("Return False" + items.Length);
                    return false;
                }
            }
            Trace.WriteLine("After Checking item list");

            SessionKey = key;
            directions = GenerateTries();
            rand = new Random();
            Trace.WriteLine("Before GenerateTable");
            if (GenerateTable())
            {
                Trace.WriteLine("In GenerateTable");
                return true;
            } 
            else
            {
                return false;
            }

        }

        public Boolean ReInitWordGrid()
        {
            GridContent = new char[GridSize];
            directions = GenerateTries();
            rand = new Random();
            if (GenerateTable())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean ValidWord(string userEntry)
        {
            char[] reversedEntry = userEntry.ToCharArray();
            Array.Reverse(reversedEntry);
            string reversed = new string(reversedEntry);
            return UserWords.Contains(userEntry) || UserWords.Contains(reversed);
        }



        // TODO: Reduce redundancy. LOADS of redundancies.
        #region Word Placement
        /// <summary>
        /// Will attempt to populate the grid. 
        /// </summary>
        // TODO: turn it into a boolean so it can give the user an error if it cannot place it all.
        public Boolean GenerateTable()
        {
            int capacity;
            int attempts = 50;

            while (attempts != 0)
            {
                capacity = GridSize;
                GridContent = new char[GridSize];
                if (RandomPlacement(UserWords, capacity, GridContent, 0))
                    break;

                attempts--;
                Trace.WriteLine("Attempts:" + attempts);
            }
            if (attempts == 0)
            {
                return false;
            } 
            else
            {
                PopulateGrid();
                return true;
            }
        }

        /// <summary>
        /// The function will attempt to place words onto the grid. It will call itself until 
        /// there is no capacity and when all words are placed. Otherwise it will fail.
        /// Other failure points are when certain directions causes it to return false that it
        /// will backtrack and attempt to place another word at a random cell with random direction.
        /// </summary>
        /// <param name="wordlist"></param>
        /// <param name="capacity">int</param>
        /// <param name="placeGrid"></param>
        /// <param name="index">int</param>
        /// <returns>boolean</returns>
        public Boolean RandomPlacement(List<string> wordlist, int capacity, char[] placeGrid, int index)
        {
            if (index == wordlist.Count)
            {
                return true;
            }

            while(capacity > 0)
            {
                int cell = rand.Next(GridSize);
                Direction tryDir = GetRandomDirection();


                if (TryPlaceWord(wordlist[index], cell, GridContent, tryDir))
                {
                    capacity = capacity - wordlist[index].Length;
    
                    var gridBackup = GridContent;
                    GridContent = PlaceWord(wordlist[index], GridContent, cell, tryDir);
                    index++;
                    if (RandomPlacement(wordlist, capacity, GridContent, index) == true)
                    {
                        return true;
                    }
                    RemoveWord(wordlist[index], GridContent, cell, tryDir);
                }
            }
            return false;
        }

        // TODO: Implement BruteForcePlacement
        public void BruteForcePlacement()
        {

        }
        /// <summary>
        /// Places words based on direction.
        /// </summary>
        /// <param name="word"></param>
        /// <param name="placeGrid"></param>
        /// <param name="cell"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        private char[] PlaceWord(string word, char[] placeGrid, int cell, Direction dir)
        {
            char[] wordtochar = word.ToArray();
            int cellNumber = cell;


            for (int i = 0; i < wordtochar.Length; i++)
            {
                placeGrid[cellNumber] = wordtochar[i];
                cellNumber = cellNumber + (RowCol * dir.x + (1 * dir.y));
            }
            return placeGrid;
        }
        /// <summary>
        /// Removes previous word to a control character of null for placing future words.
        /// </summary>
        /// <param name="word"></param>
        /// <param name="placeGrid"></param>
        /// <param name="cell"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        private char[] RemoveWord(string word, char[] placeGrid, int cell, Direction dir)
        {
            char[] wordtochar = word.ToArray();
            int cellNumber = cell;


            for (int i = 0; i < wordtochar.Length; i++)
            {
                placeGrid[cellNumber] = '\0';
                cellNumber = cellNumber + (RowCol * dir.x + (1 * dir.y));
            }
            return placeGrid;
        }

        /// <summary>
        /// The function will attempt to place a word somewhere in the grid. The rows are 
        /// invalid if it steps out of boundaries using IsRowInvalid or IsColInvalid. Otherwise
        /// it will keep checking the row/col based on the direction.
        /// </summary>
        /// <param name="word">string</param>
        /// <param name="cell">int</param>
        /// <param name="placeGrid">toberemoved</param>
        /// <param name="dir">Direction</param>
        /// <returns>boolean</returns>
        private Boolean TryPlaceWord(string word, int cell, char[] placeGrid, Direction dir)
        {
            char[] testWord = word.ToCharArray();
            int curRow = cell / RowCol;
            int curCol = cell % RowCol;
            int cellNumber = cell;


            for (int i = 0; i < testWord.Length; i++)
            {
                if (IsRowInvalid(curRow) ||
                    IsColInvalid(curCol))
                {
                    return false;
                }
                //In C#, when the grid is initialized, the 'empty squares are Control characters.
                //Please correct me if wrong.
                if (Char.IsControl(GridContent[cellNumber]) || testWord[i] == GridContent[cellNumber])
                {

                    curRow = curRow + (dir.x);
                    curCol = curCol + (dir.y);
                    cellNumber = cellNumber + (RowCol * dir.x + (1 * dir.y));
                } 
                else
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Populate the grid where there is are no characters. 
        /// </summary>
        private void PopulateGrid()
        {
            for (int i = 0; i < GridSize; i++)
            {
                //Could potentially do Char.IsControl.
                if (!Char.IsLetter(GridContent[i]))
                    GridContent[i] = (char)rand.Next(97, 122);
            }

        }
        #endregion

        #region Helper Functions 
        /// <summary>
        /// Generates all the directions that the function will try.
        /// </summary>
        /// <returns></returns>
        private List<Direction> GenerateTries()
        {
            return new List<Direction>()
            {
                new Direction(-1,-1),new Direction(0,-1) ,new Direction(1,-1),
                new Direction(-1,0) ,                     new Direction(1,0) ,
                new Direction(-1,1) , new Direction(0,1) ,new Direction(1,1) ,
            };
        }

        private Direction GetRandomDirection()
        {
            return directions[rand.Next(directions.Count)];
        }
        /// <summary>
        /// If a number happens to step out of the boundaries which is the max number
        /// of rows, then it will return false. Going to -1 or beyond max returns false.
        /// </summary>
        /// <param name="row">int</param>
        /// <returns>boolean</returns>
        private Boolean IsRowInvalid(int row)
        {
            return ((row < 0) || (row > RowCol-1));
        }

        /// <summary>
        /// If a number happens to step out of the boundaries which is the max number
        /// of columns, then it will return false. Going to -1 or beyond max returns false.
        /// </summary>
        /// <param name="col">int</param>
        /// <returns>boolean</returns>
        private Boolean IsColInvalid(int col)
        {
            return ((col < 0) || (col > RowCol-1));
        }

        #endregion
    }
}