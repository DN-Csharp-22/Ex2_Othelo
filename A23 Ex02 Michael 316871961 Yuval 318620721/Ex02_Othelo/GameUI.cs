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
        }
        public void CleanBoard()
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

        public OtheloMove GetMoveFromUser(GameState gameState)
        {
            Console.WriteLine("Current player is : {0}",gameState.GetCurrentPlayerSymbol());
            Console.WriteLine("Please insert your move");

            bool inputIsValid = false;
            
            OtheloMove userMove = null;

            while (!inputIsValid)
            {
                string userInput = GetUserInput().ToLower();

                if (userInput.Length == 2)
                {
                    char firstChar = userInput[0];
                    char secondChar = userInput[1];

                    if (firstChar >= '1' && firstChar <= ('1' + gameState.Difficulty) && secondChar >= 'a' && secondChar <= ('a' + gameState.Difficulty))
                    {
                        userMove = new OtheloMove(firstChar - '1',secondChar - 'a');
                        inputIsValid = true;
                    }
                    else if (secondChar >= '1' && secondChar <= ('1' + gameState.Difficulty) && firstChar >= 'a' && firstChar <= ('a' + gameState.Difficulty))
                    {
                        userMove = new OtheloMove(secondChar - '1',firstChar - 'a');
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

        public void DisplayInvalidMoveMessage(char currentPlayerSymbol)
        {
            Console.WriteLine(string.Format("player '{0}' : Move is invalid, please insert another move according to Othelo rules",currentPlayerSymbol));
        }

        public void DisplayWinnerMessage(char winner)
        {
            Console.WriteLine("{0} is the winner!!!", winner);
            Console.ReadKey();
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
