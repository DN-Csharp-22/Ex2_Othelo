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
            int difficulty = gameState.Board.Length;

            winner = ' ';

            bool gameFinished = true;

            int[] playersSymbolsCount = new int[3];

            for (int row = 0; row < difficulty && gameFinished; row++)
            {
                for (int col = 0; col < difficulty && gameFinished; col++)
                {
                    switch (gameState.Board[row][col])
                    {
                        case GameState.EmptyCell:
                            playersSymbolsCount[2]++;

                            OtheloMove generatedMove = new OtheloMove(row, col);

                            ValidMoves validMoves = IsMoveValid(generatedMove, gameState.Board, gameState.currentPlayer);

                            if (validMoves.RowValid || validMoves.ColumnValid || validMoves.MainDiagonalValid || validMoves.SubDiagonalValid)
                            {
                                gameFinished = false;
                            }
                            else
                            {
                                generatedMove = null;
                            }
                            break;
                        case GameState.PlayerX:
                            playersSymbolsCount[0]++;
                            break;
                        case GameState.PlayerO:
                            playersSymbolsCount[1]++;
                            break;
                        default:
                            break;
                    }
                }
            }

            if (playersSymbolsCount[2] == 0 || gameFinished)
            {
                winner = playersSymbolsCount[0] > playersSymbolsCount[1] ? GameState.PlayerXSymbol : GameState.PlayerOSymbol;
                gameFinished = true;
            }

            return gameFinished;
        }

        public static ValidMoves IsMoveValid(OtheloMove currentMove, int[][] board, int currentPlayer)
        {
            ValidMoves validMoves = new ValidMoves();

            if (board[currentMove.row][currentMove.column] == GameState.EmptyCell)
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

        private static bool isValidColumnMove(int row, int col, int[][] Board, int currentPlayer, bool isColCheck)
        {
            bool isMoveValid = false;

            int oppositePlayerSymbolsCount = 0;

            int oppositePlayer = (-1) * currentPlayer;

            for (int currentRow = row - 1; currentRow >= 0; currentRow--)//Check to the left
            {
                if (Board[currentRow][col] == oppositePlayer)
                {
                    oppositePlayerSymbolsCount++;
                }
                else if (Board[currentRow][col] == currentPlayer && oppositePlayerSymbolsCount > 0)
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
                for (int currentRow = row + 1; currentRow < Board.Length; currentRow++)//Check to the right
                {
                    if (Board[currentRow][col] == oppositePlayer)
                    {
                        oppositePlayerSymbolsCount++;
                    }
                    else if (Board[currentRow][col] == currentPlayer && oppositePlayerSymbolsCount > 0)
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
        private static bool isValidRowMove(int row, int col, int[][] Board, int currentPlayer, bool isColCheck)
        {
            bool isMoveValid = false;

            int oppositePlayerSymbolsCount = 0;

            int oppositePlayer = (-1) * currentPlayer;

            for (int currentCol = col - 1; currentCol >= 0; currentCol--)//Check to the left
            {
                if (Board[row][currentCol] == oppositePlayer)
                {
                    oppositePlayerSymbolsCount++;
                }
                else if (Board[row][currentCol] == currentPlayer && oppositePlayerSymbolsCount > 0)
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
                for (int currentCol = col + 1; currentCol < Board.Length; currentCol++)//Check to the right
                {
                    if (Board[row][currentCol] == oppositePlayer)
                    {
                        oppositePlayerSymbolsCount++;
                    }
                    else if (Board[row][currentCol] == currentPlayer && oppositePlayerSymbolsCount > 0)
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
        private static bool isValidMainDiagonalMove(int row, int col, int[][] Board, int currentPlayer, bool isMainDiagonalCheck)
        {
            bool isMoveValid = false;

            int oppositePlayerSymbolsCount = 0;

            int oppositePlayer = (-1) * currentPlayer;

            int index = 1;

            for (int currentCol = col; currentCol > 0 && col + index < Board.Length && row + index < Board.Length; currentCol--) // main diagonal
            {
                if (Board[row + index][col + index] == oppositePlayer)
                {
                    oppositePlayerSymbolsCount++;
                }
                else if (Board[row + index][col + index] == currentPlayer && oppositePlayerSymbolsCount > 0)
                {
                    isMoveValid = true;
                    break;
                }
                index++;
            }

            if (!isMoveValid)
            {
                index = 1;

                for (int currentCol = col; currentCol < Board.Length && col - index >= 0 && row - index >= 0; currentCol++) // main diagonal
                {
                    if (Board[row - index][col - index] == oppositePlayer)
                    {
                        oppositePlayerSymbolsCount++;
                    }
                    else if (Board[row - index][col - index] == currentPlayer && oppositePlayerSymbolsCount > 0)
                    {
                        isMoveValid = true;
                        break;
                    }
                    index++;
                }
            }

            return isMoveValid;
        }
        private static bool isValidSubDiagonalMove(int row, int col, int[][] Board, int currentPlayer, bool isMainDiagonalCheck)
        {
            bool isMoveValid = false;

            int oppositePlayerSymbolsCount = 0;

            int oppositePlayer = (-1) * currentPlayer;

            int index = 1;

            for (int currentCol = col; currentCol > 0 && col + index < Board.Length && row - index >= 0; currentCol--) // main diagonal
            {
                if (Board[row - index][col + index] == oppositePlayer)
                {
                    oppositePlayerSymbolsCount++;
                }
                else if (Board[row - index][col + index] == currentPlayer && oppositePlayerSymbolsCount > 0)
                {
                    isMoveValid = true;
                    break;
                }
                index++;
            }

            if (!isMoveValid)
            {
                for (index = 1; index < Board.Length && col - index >= 0 && row + index < Board.Length; index++) // main diagonal
                {
                    if (Board[row + index][col - index] == oppositePlayer)
                    {
                        oppositePlayerSymbolsCount++;
                    }
                    else if (Board[row + index][col - index] == currentPlayer && oppositePlayerSymbolsCount > 0)
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
