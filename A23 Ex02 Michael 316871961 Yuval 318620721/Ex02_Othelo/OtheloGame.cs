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

            gameUI.DisplayBoard(gameState.Board);

            OtheloMove move = gameUI.GetMoveFromUser(gameState.Difficulty);

            while(!IsMoveValid(move,gameState.Board))
            {

            }

            Console.ReadKey();
        }

        public bool IsMoveValid(OtheloMove currentMove, int[][] gameBoard)
        {
            return true;
        }


        public void InsertMoveToBoard(int row, int col, int currentPlayer)
        {
            char playerSymbol = currentPlayer == 1 ? 'X' : 'O';
        }

        /// <summary>
        /// This method can be altered with AI logic
        /// </summary>
        public void InsertMoveByComputer()
        {
            //random
        }
    }
}
