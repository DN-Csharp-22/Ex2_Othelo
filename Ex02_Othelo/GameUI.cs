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

        public int InitializeGame()
        {
            bool inputIsValid = false;

            int gameDifficulty = -1;

            while (!inputIsValid)
            {
                {
                    Console.WriteLine("Choose a difficulty!");

                    string difficultyInput = GetUserInput();

                    if (int.TryParse(difficultyInput, out int difficulty))
                    {
                        if (gameDifficulty == 8 || gameDifficulty == 6)
                        {
                            inputIsValid = true;

                            gameDifficulty = difficulty;
                        }
                    }

                    if (!inputIsValid)
                    {
                        Console.WriteLine("Invalid input - please enter a valid integer");
                    }
                }
            }

            return gameDifficulty;
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
