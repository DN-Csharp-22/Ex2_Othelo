using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex02_Othelo
{
    internal class GameUI
    {
        public GameUI()
        {
            Ex02.ConsoleUtils.Screen.Clear();
        }

        public void DisplayBoard(int[][] boardMatrix)
        {
            for (int column = 0; column < boardMatrix.Length + 1; column++)
            {
                if (column == 0)
                {
                    Console.Write("   ");

                }
                else
                {
                    Console.Write("  {0} ", (char)('A' + column - 1));
                }
            }

            Console.WriteLine();

            for (int row = 0; row < boardMatrix.Length; row++)
            {
                printLineSeperator(boardMatrix.Length);

                for (int col = 0; col < boardMatrix.Length + 1; col++)
                {
                    if (col == 0)
                    {
                        Console.Write(" {0} ", row + 1);
                    }
                    else
                    {
                        char currentCellSymbol = ' ';

                        //TODO do a switch case to 
                        if (boardMatrix[row][col - 1] != 0)
                        {
                            switch (boardMatrix[row][col - 1])
                            {
                                case 1:
                                    currentCellSymbol = 'X';
                                    break;
                                case -1:
                                    currentCellSymbol = 'O';
                                    break;
                                default:
                                    break;
                            }
                        }
                        Console.Write("| {0} ", currentCellSymbol);
                    }
                }
                Console.Write("|\n");
            }
            printLineSeperator(boardMatrix.Length);
        }

        public void printLineSeperator(int lineLength)
        {
            for (int col = 0; col < lineLength; col++)
            {
                if (col == 0)
                {
                    Console.Write("   ");

                }
                Console.Write("====");
            }
            Console.WriteLine();

        }

        public int GetDifficultyFromUser(int[] availableDifficulties)
        {
            bool inputIsValid = false;

            int gameDifficulty = -1;

            while (!inputIsValid)
            {
                Console.WriteLine(string.Format("Choose a difficulty : {0}", string.Join(" or ", availableDifficulties)));

                string difficultyInput = GetUserInput();

                if (int.TryParse(difficultyInput, out int difficulty))
                {
                    for (int i = 0; i < availableDifficulties.Length; i++)
                    {
                        if (difficulty == availableDifficulties[i])
                        {
                            inputIsValid = true;

                            gameDifficulty = difficulty;

                            break;
                        }
                    }
                }

                if (!inputIsValid)
                {
                    Console.WriteLine("Invalid input - please chose one of the given options");
                    Console.WriteLine();
                }
            }

            return gameDifficulty;
        }

        public OtheloMove GetMoveFromUser(int difficulty)
        {
            Console.WriteLine("Please insert your move");

            bool inputIsValid = false;

            string userInput = GetUserInput().ToLower();

            OtheloMove userMove = null;

            while (!inputIsValid)
            {
                if (userInput.Length == 2)
                {
                    char firstChar = userInput[0];
                    char secondChar = userInput[1];

                    if (firstChar >= '1' && firstChar <= ('1' + difficulty) && secondChar >= 'a' && secondChar <= ('a' + difficulty))
                    {
                        inputIsValid = true;
                        userMove = new OtheloMove(firstChar - 'a', secondChar - '1');
                    }
                    else if (secondChar >= '1' && secondChar <= ('1' + difficulty) && firstChar >= 'a' && firstChar <= ('a' + difficulty))
                    {
                        userMove = new OtheloMove(secondChar - 'a', firstChar - '1');
                        inputIsValid = true;
                    }
                }

                if (!inputIsValid)
                {
                    Console.WriteLine("Invalid input, enter row and column number correctly");
                }
            }

            return userMove;
        }
        public string GetUserInput()
        {
            string userInput = Console.ReadLine();

            if (userInput.ToLower() == "q")
            {
                Environment.Exit(0);
            }

            return userInput;
        }
    }
}
