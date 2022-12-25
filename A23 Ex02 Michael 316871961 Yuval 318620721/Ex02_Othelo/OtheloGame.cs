using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ex02_Othelo
{
    internal class OtheloGame
    {
        private readonly int[] difficulties = { 6, 8 };

        public void StartGame()
        {
            GameUI gameUI = new GameUI();

            int gameDifficulty = gameUI.GetDifficultyFromUser(difficulties);

            GameState gameState = new GameState(gameDifficulty);

            gameUI.DisplayBoard(gameState.board);

            char winner = ' ';

            while (!OtheloMove.isGameFinished(gameState, out winner))
            {
                OtheloMove move = GetMove(gameState, gameUI);

                ValidMoves validMoves = OtheloMove.IsMoveValid(move, gameState.board, gameState.currentPlayer);

                bool isValidMove = validMoves.RowValid || validMoves.ColumnValid || validMoves.MainDiagonalValid || validMoves.SubDiagonalValid;

                if (isValidMove)
                {
                    gameState.InsertMoveToBoard(move, validMoves);
                    gameState.SwitchPlayers();
                    gameUI.CleanBoard();
                    gameUI.DisplayBoard(gameState.board);
                }
                else if (gameState.isPlayerTurn())
                {
                    gameUI.DisplayInvalidMoveMessage(gameState.GetCurrentPlayerSymbol());
                }
            }

            gameUI.DisplayWinnerMessage(winner);
        }

        private OtheloMove GetMove(GameState gameState, GameUI gameUI)
        {
            OtheloMove move = null;

            if (gameState.isPlayerTurn())
            {
                move = gameUI.GetMoveFromUser(gameState);
            }
            else
            {
                move = OtheloMove.GenerateMove(gameState.difficulty);
            }

            return move;
        }
    }
}
