using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex02_Othelo
{
    internal class OtheloGame
    {
        private readonly int[] difficulties = { 6, 8 };
        public OtheloGame() { }

        public void StartGame()
        {
            GameUI gameUI = new GameUI();

            int gameDifficulty = gameUI.GetDifficultyFromUser(difficulties);

            GameState gameState = new GameState(gameDifficulty);

            while(true)
            {
                gameUI.DisplayBoard(gameState.Board);

                OtheloMove move = gameUI.GetMoveFromUser(gameState.Difficulty);

                bool isMoveValid = gameState.IsMoveValid(move);

                if (!isMoveValid)
                {
                    Ex02.ConsoleUtils.Screen.Clear();
                    gameUI.DisplayInvalidMoveMessage();
                }
                else
                {
                    gameState.InsertMoveToBoard(move);

                    if (gameState.IsGameFinished())
                    {
                        gameUI.DisplayWinnerMessage();
                        break;
                    }
                }
            }
        }
    }
}
