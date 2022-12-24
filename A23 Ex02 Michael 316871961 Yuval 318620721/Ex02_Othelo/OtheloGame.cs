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

            bool isInvalidMove = false;

            gameUI.DisplayBoard(gameState.Board);

            while (true)
            {
                if (isInvalidMove)
                {
                    gameUI.DisplayInvalidMoveMessage();
                }

                OtheloMove move = null;
                bool NoMoreMovesLeft = false;
                if (gameState.isPlayerTurn())
                {
                    //move = gameUI.GetMoveFromUser(gameState);
                    move = gameState.GenerateMoveByComputer();
                }
                else
                {
                    move = gameState.GenerateMoveByComputer();
                    NoMoreMovesLeft = move == null;
                    isInvalidMove = false;
                }

                ValidMoves validMoves = gameState.IsMoveValid(move);

                if (validMoves.RowValid || validMoves.ColumnValid || validMoves.MainDiagonalValid || validMoves.SubDiagonalValid)
                {
                    if (!gameState.isPlayerTurn())
                    {
                        Console.WriteLine(string.Format("Move from computer : [row:{0},column:{1}]", (move.row + 1).ToString(), (char)(move.column + 'A')));
                    }

                    gameState.InsertMoveToBoard(move, validMoves);
                    gameUI.DisplayBoard(gameState.Board);
                    isInvalidMove = false;
                }
                else if (gameState.isPlayerTurn())
                {
                    isInvalidMove = true;
                }

                if (gameState.IsGameFinished(out char winner,NoMoreMovesLeft))
                {
                    gameUI.DisplayWinnerMessage(winner);
                    break;
                }

            }
        }
    }
}
