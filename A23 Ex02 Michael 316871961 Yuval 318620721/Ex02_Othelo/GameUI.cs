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

        public int GetDifficultyFromUser(int[] availableDifficulties)
        {
            bool inputIsValid = false;

            int gameDifficulty = -1;

            while (!inputIsValid)
            {
                Console.WriteLine(string.Format("Choose a difficulty : {0}", string.Join(" or ",availableDifficulties)));

                string difficultyInput = GetUserInput();

                if (int.TryParse(difficultyInput, out int difficulty))
                {
                    for (int i = 0; i < availableDifficulties.Length; i++)
                    {
                        if (difficulty == availableDifficulties[i])
                        {
                            inputIsValid = true;

                            gameDifficulty = difficulty;
                        }
                    }
                }

                if (!inputIsValid)
                {
                    Console.WriteLine("Invalid input - please chose one of the given options");
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
