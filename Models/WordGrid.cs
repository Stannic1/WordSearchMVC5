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
        public List<string> UserWords { get; set; }
        public int GridSize { get; set; }
        public int RowCol { get; set; }
        public char[] GridContent { get; set; }
        private List<Direction> directions;
        private Random rand;

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
            GridContent = new char[GridSize];
            directions = GenerateTries();
            rand = new Random();
            GenerateTable();
        }
        public Boolean ValidWord(string userEntry)
        {
            char[] reversedEntry = userEntry.ToCharArray();
            Array.Reverse(reversedEntry);
            return UserWords.Contains(userEntry) || UserWords.Contains(reversedEntry.ToString());
        }

        #region Word Placement

        public void GenerateTable()
        {
            int capacity;
            int attempts = 50;

            while (attempts != 0)
            {
                capacity = GridSize;
                GridContent = new char[GridSize];
                RandomPlacement(UserWords, capacity, GridContent, 0);

                attempts--;
            }

            PopulateGrid();
        }

        public Boolean RandomPlacement(List<string> wordlist, int capacity, char[] placeGrid, int index)
        {
            //Trace.WriteLine("Entering RandomPlacement.");
            //Trace.WriteLine(String.Format("The Index is at: {0}", index));
            if (index == wordlist.Count)
            {
                return true;
            }

            while(capacity > 0)
            {
                //Trace.WriteLine(String.Format("This is the {0} loop.", index));
                int cell = rand.Next(GridSize);
                Direction tryDir = GetRandomDirection();
                //Trace.WriteLine(String.Format("Attempting cell at {0}", cell));
                //Trace.WriteLine(String.Format("Direction is: x = {0} , y = {1}", tryDir.x, tryDir.y));


                if (TryPlaceWord(wordlist[index], cell, GridContent, tryDir))
                {
                    capacity = capacity - wordlist[index].Length;
    
                    var gridBackup = GridContent;
                    GridContent = PlaceWord(wordlist[index], GridContent, cell, tryDir);
                    index++;
                    //Trace.WriteLine(String.Format("About to enter recursion"));
                    if (RandomPlacement(wordlist, capacity, GridContent, index) == true)
                    {
                        return true;
                    }
                    RemoveWord(wordlist[index], GridContent, cell, tryDir);
                }
            }
            return false;
        }

        public void BruteForcePlacement()
        {

        }

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

        private char[] RemoveWord(string word, char[] placeGrid, int cell, Direction dir)
        {
            char[] wordtochar = word.ToArray();
            int cellNumber = cell;


            for (int i = 0; i < wordtochar.Length; i++)
            {
                placeGrid[cellNumber] = ' ';
                cellNumber = cellNumber + (RowCol * dir.x + (1 * dir.y));
            }
            return placeGrid;
        }

        private Boolean TryPlaceWord(string word, int cell, char[] placeGrid, Direction dir)
        {
            int wordlength = word.Length;
            char[] testWord = word.ToCharArray();
            int curRow = cell / RowCol;
            int curCol = cell % RowCol;
            int cellNumber = cell;
            //Trace.WriteLine(String.Format("curRow: {0}, curCol: {1}, cellNumber: {2}", curRow, curCol, cellNumber));
            //Trace.WriteLine(String.Format("Word is: {0}", word));


            for (int i = 0; i < testWord.Length; i++)
            {
                if (IsRowInvalid(curRow) ||
                    IsColInvalid(curCol)
                    )
                {
                    return false;
                }

                //Trace.WriteLine(String.Format("The character and i is: {0} {1}", testWord[i], i));
                //Trace.WriteLine(String.Format("Is it a whitespace?: {0} {1}", Char.IsControl(GridContent[cellNumber]), GridContent[cellNumber]));
                
                if (Char.IsControl(GridContent[cellNumber]) || testWord[i] == GridContent[cellNumber])
                {

                    curRow = curRow + (dir.x);
                    curCol = curCol + (dir.y);
                    cellNumber = cellNumber + (RowCol * dir.x + (1 * dir.y));
                } else
                {
                    return false;
                }
            }
            return true;
        }

        private void PopulateGrid()
        {
            //for (int i = 0; i < GridSize; i++)
            //{
            //    if (!Char.IsLetter(GridContent[i]))
            //        GridContent[i] = (char)rand.Next(97, 122);
            //    Trace.WriteLine(String.Format("GridContent[{0}] is {1}", i, GridContent[i]));
            //}

        }
        #endregion

        #region Helper Functions 

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

        private Boolean IsRowInvalid(int row)
        {
            return ((row < 0) || (row > RowCol-1));
        }

        private Boolean IsColInvalid(int col)
        {
            return ((col < 0) || (col > RowCol-1));
        }

        #endregion
    }
}