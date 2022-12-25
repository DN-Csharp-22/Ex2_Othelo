using System;

namespace Ex02_Othelo
{
    internal class OtheloMove
    {
        public int row { get; set; }
        public int column { get; set; }
        public OtheloMove(int row, int col)
        {
            this.row = row;
            this.column = col;
        }

        public static bool isGameFinished(GameState gameState, out char winner)
        {
            int difficulty = gameState.board.Length;

            winner = ' ';

            bool gameFinished = true;

            int[] playersSymbolsCount = new int[3];

            for (int row = 0; row < difficulty && gameFinished; row++)
            {
                for (int col = 0; col < difficulty && gameFinished; col++)
                {
                    switch (gameState.board[row][col])
                    {
                        case GameState.emptyCell:
                            playersSymbolsCount[2]++;

                            OtheloMove generatedMove = new OtheloMove(row, col);

                            ValidMoves validMoves = IsMoveValid(generatedMove, gameState.board, gameState.currentPlayer);

                            if (validMoves.RowValid || validMoves.ColumnValid || validMoves.MainDiagonalValid || validMoves.SubDiagonalValid)
                            {
                                gameFinished = false;
                            }
                            else
                            {
                                generatedMove = null;
                            }
                            break;
                        case GameState.player_X:
                            playersSymbolsCount[0]++;
                            break;
                        case GameState.player_O:
                            playersSymbolsCount[1]++;
                            break;
                        default:
                            break;
                    }
                }
            }

            if (playersSymbolsCount[2] == 0 || gameFinished)
            {
                winner = playersSymbolsCount[0] > playersSymbolsCount[1] ? GameState.playerXSymbol : GameState.playerOSymbol;
                gameFinished = true;
            }

            return gameFinished;
        }

        public static ValidMoves IsMoveValid(OtheloMove currentMove, int[][] board, int currentPlayer)
        {
            ValidMoves validMoves = new ValidMoves();

            if (board[currentMove.row][currentMove.column] == GameState.emptyCell)
            {
                validMoves.RowValid = isValidRowMove(currentMove.row, currentMove.column, board, currentPlayer, false);
                validMoves.ColumnValid = isValidColumnMove(currentMove.row, currentMove.column, board, currentPlayer, true);
                validMoves.MainDiagonalValid = isValidMainDiagonalMove(currentMove.row, currentMove.column, board, currentPlayer, true);
                validMoves.SubDiagonalValid = isValidSubDiagonalMove(currentMove.row, currentMove.column, board, currentPlayer, false);
            }
            return validMoves;
        }

        public static OtheloMove GenerateMove(int difficulty)
        {
            Random rnd = new Random();

            OtheloMove generatedMove = new OtheloMove(rnd.Next(difficulty), rnd.Next(difficulty));

            return generatedMove;
        }

        private static bool isValidColumnMove(int row, int col, int[][] board, int currentPlayer, bool isColCheck)
        {
            bool isMoveValid = false;

            int oppositePlayerSymbolsCount = 0;

            int oppositePlayer = (-1) * currentPlayer;

            for (int currentRow = row - 1; currentRow >= 0; currentRow--)//Check to the left
            {
                if (board[currentRow][col] == oppositePlayer)
                {
                    oppositePlayerSymbolsCount++;
                }
                else if (board[currentRow][col] == currentPlayer && oppositePlayerSymbolsCount > 0)
                {
                    isMoveValid = true;
                    break;
                }
                else//Found an empty cell
                {
                    break;
                }
            }

            if (!isMoveValid)
            {
                for (int currentRow = row + 1; currentRow < board.Length; currentRow++)//Check to the right
                {
                    if (board[currentRow][col] == oppositePlayer)
                    {
                        oppositePlayerSymbolsCount++;
                    }
                    else if (board[currentRow][col] == currentPlayer && oppositePlayerSymbolsCount > 0)
                    {
                        isMoveValid = true;
                        break;
                    }
                    else//Found an empty cell
                    {
                        break;
                    }
                }
            }

            return isMoveValid;
        }
        private static bool isValidRowMove(int row, int col, int[][] board, int currentPlayer, bool isColCheck)
        {
            bool isMoveValid = false;

            int oppositePlayerSymbolsCount = 0;

            int oppositePlayer = (-1) * currentPlayer;

            for (int currentCol = col - 1; currentCol >= 0; currentCol--)//Check to the left
            {
                if (board[row][currentCol] == oppositePlayer)
                {
                    oppositePlayerSymbolsCount++;
                }
                else if (board[row][currentCol] == currentPlayer && oppositePlayerSymbolsCount > 0)
                {
                    isMoveValid = true;
                    break;
                }
                else
                {
                    break;
                }
            }

            if (!isMoveValid)
            {
                for (int currentCol = col + 1; currentCol < board.Length; currentCol++)//Check to the right
                {
                    if (board[row][currentCol] == oppositePlayer)
                    {
                        oppositePlayerSymbolsCount++;
                    }
                    else if (board[row][currentCol] == currentPlayer && oppositePlayerSymbolsCount > 0)
                    {
                        isMoveValid = true;
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return isMoveValid;
        }
        private static bool isValidMainDiagonalMove(int row, int col, int[][] board, int currentPlayer, bool isMainDiagonalCheck)
        {
            bool isMoveValid = false;

            int oppositePlayerSymbolsCount = 0;

            int oppositePlayer = (-1) * currentPlayer;

            int index = 1;

            for (int currentCol = col; currentCol > 0 && col + index < board.Length && row + index < board.Length; currentCol--) // main diagonal
            {
                if (board[row + index][col + index] == oppositePlayer)
                {
                    oppositePlayerSymbolsCount++;
                }
                else if (board[row + index][col + index] == currentPlayer && oppositePlayerSymbolsCount > 0)
                {
                    isMoveValid = true;
                    break;
                }
                index++;
            }

            if (!isMoveValid)
            {
                index = 1;

                for (int currentCol = col; currentCol < board.Length && col - index >= 0 && row - index >= 0; currentCol++) // main diagonal
                {
                    if (board[row - index][col - index] == oppositePlayer)
                    {
                        oppositePlayerSymbolsCount++;
                    }
                    else if (board[row - index][col - index] == currentPlayer && oppositePlayerSymbolsCount > 0)
                    {
                        isMoveValid = true;
                        break;
                    }
                    index++;
                }
            }

            return isMoveValid;
        }
        private static bool isValidSubDiagonalMove(int row, int col, int[][] board, int currentPlayer, bool isMainDiagonalCheck)
        {
            bool isMoveValid = false;

            int oppositePlayerSymbolsCount = 0;

            int oppositePlayer = (-1) * currentPlayer;

            int index = 1;

            for (int currentCol = col; currentCol > 0 && col + index < board.Length && row - index >= 0; currentCol--) // main diagonal
            {
                if (board[row - index][col + index] == oppositePlayer)
                {
                    oppositePlayerSymbolsCount++;
                }
                else if (board[row - index][col + index] == currentPlayer && oppositePlayerSymbolsCount > 0)
                {
                    isMoveValid = true;
                    break;
                }
                index++;
            }

            if (!isMoveValid)
            {
                for (index = 1; index < board.Length && col - index >= 0 && row + index < board.Length; index++) // main diagonal
                {
                    if (board[row + index][col - index] == oppositePlayer)
                    {
                        oppositePlayerSymbolsCount++;
                    }
                    else if (board[row + index][col - index] == currentPlayer && oppositePlayerSymbolsCount > 0)
                    {
                        isMoveValid = true;
                        break;
                    }
                }
            }

            return isMoveValid;
        }

    }
}
