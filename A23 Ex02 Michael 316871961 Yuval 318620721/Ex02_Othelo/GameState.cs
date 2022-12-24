using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ex02_Othelo
{
    internal class GameState
    {
        public int Difficulty { get; set; }
        public int[][] Board { get; set; }
        private int currentPlayer { get; set; }

        private const int PlayerX = 1;
        private const int PlayerO = -1;
        private const int EmptyCell = 0;
        private const char PlayerXSymbol = 'X';
        private const char PlayerOSymbol = 'O';

        public GameState(int rowLength)
        {
            this.Difficulty = rowLength;
            this.Board = new int[rowLength][];
            this.currentPlayer = 1;

            for (int rowNumber = 0; rowNumber < this.Board.Length; rowNumber++)
            {
                this.Board[rowNumber] = new int[rowLength];
            }

            int middleCell = (rowLength / 2) - 1;

            this.Board[middleCell][middleCell] = -1;
            this.Board[middleCell][middleCell + 1] = 1;
            this.Board[middleCell + 1][middleCell] = 1;
            this.Board[middleCell + 1][middleCell + 1] = -1;
        }

        public char GetCurrentPlayerSymbol()
        {
            return this.currentPlayer == 1 ? PlayerXSymbol : PlayerOSymbol;
        }

        public bool isPlayerTurn()
        {
            return this.currentPlayer == 1;
        }

        public void InsertMoveToBoard(OtheloMove move, ValidMoves validMoves)
        {
            int index = 0;

            if (validMoves.RowValid)
            {
                index = 0;
                int row_indexHolder = findOffsetPositionRow(move);
                if (row_indexHolder != -1) // רצה על טווח שורות בתכלס משבצת עמודות 
                {
                    for (int i = move.column; i < row_indexHolder; i++)
                    {
                        Board[move.row][i] = currentPlayer;
                    }

                    for (int i = move.column; i > row_indexHolder; i--)
                    {
                        Board[move.row][i] = currentPlayer;
                    }
                }

            }

            if (validMoves.ColumnValid)
            {
                index = 0;
                int column_indexHolder = findOffsetPositionColunm(move);
                //======> רצה על עמודה מלמעלה למטה
                if (column_indexHolder != -1) //רצה על טווח עמודות (בתכלס משבץ שורה)  
                {
                    for (int i = move.row; i < column_indexHolder; i++)
                    {
                        Board[i][move.column] = currentPlayer;
                    }

                    for (int i = move.row; i > column_indexHolder; i--)
                    {
                        Board[i][move.column] = currentPlayer;
                    }
                }
            }

            if (validMoves.MainDiagonalValid)
            {
                index = 0;
                int main_dianagonalIndexHolder = findOffsetPositionMainDiagonal(move);
                if (main_dianagonalIndexHolder != -1)
                {
                    for (int j = move.column; j < main_dianagonalIndexHolder; j++)
                    {
                        Board[move.row + index][move.column + index] = currentPlayer;
                        index++;
                    }

                    for (int j = main_dianagonalIndexHolder + 1; j <= move.column; j++)
                    {
                        Board[move.row - index][move.column - index] = currentPlayer;
                        index++;

                    }
                }
            }

            if (validMoves.SubDiagonalValid)
            {
                index = 0;
                int sub_dianagonalIndexHolder = findOffsetPositionSubDiagonal(move);
                if (sub_dianagonalIndexHolder != -1)
                {
                    for (int j = sub_dianagonalIndexHolder + 1; j <= move.column; j++)
                    {
                        Board[move.row + index][move.column - index] = currentPlayer;
                        index++;
                    }
                    index = 0;
                    for (int j = move.column; j < sub_dianagonalIndexHolder; j++)
                    {
                        Board[move.row - index][move.column + index] = currentPlayer;
                        index++;
                    }
                }
            }

            currentPlayer *= -1;
        }

        public bool IsGameFinished(out char winner,bool noMovesLeft)
        {
            bool isFinished = false;

            int[] playersSymbolsCount = new int[3];

            winner = ' ';

            for (int row = 0; row < Difficulty; row++)
            {
                for (int col = 0; col < Difficulty; col++)
                {
                    switch (Board[row][col])
                    {
                        case EmptyCell:
                            playersSymbolsCount[2]++;
                            break;
                        case PlayerX:
                            playersSymbolsCount[0]++;
                            break;
                        case PlayerO:
                            playersSymbolsCount[1]++;
                            break;
                        default:
                            break;
                    }
                }
            }

            if (playersSymbolsCount[2] == 0 || noMovesLeft)
            {
                winner = playersSymbolsCount[0] > playersSymbolsCount[1] ? PlayerXSymbol : PlayerOSymbol;
                isFinished = true;
            }

            return isFinished;
        }

        public ValidMoves IsMoveValid(OtheloMove currentMove)
        {
            ValidMoves validMoves = new ValidMoves();

            if (Board[currentMove.row][currentMove.column] == EmptyCell)
            {
                validMoves.RowValid = isValidRowMove(currentMove.row, currentMove.column, Board, currentPlayer, false);
                validMoves.ColumnValid = isValidColumnMove(currentMove.row, currentMove.column, Board, currentPlayer, true);
                validMoves.MainDiagonalValid = isValidMainDiagonalMove(currentMove.row, currentMove.column, Board, currentPlayer, true);
                validMoves.SubDiagonalValid = isValidSubDiagonalMove(currentMove.row, currentMove.column, Board, currentPlayer, false);
            }
            return validMoves;
        }

        /// <summary>
        /// This method can be altered with AI logic
        /// </summary>
        public OtheloMove GenerateMoveByComputer()
        {
            Random rnd = new Random();

            OtheloMove generatedMove = null;

            bool moveFound = false;

            for (int row = 0; row < Difficulty && !moveFound; row++)
            {
                for (int col = 0; col < Difficulty && !moveFound ; col++)
                {
                    switch (Board[row][col])
                    {
                        case EmptyCell:
                            generatedMove = new OtheloMove(row,col);
                            
                            ValidMoves validMoves = IsMoveValid(generatedMove);

                            if (validMoves.RowValid || validMoves.ColumnValid || validMoves.MainDiagonalValid || validMoves.SubDiagonalValid)
                            {
                                moveFound = true;
                            }
                            else
                            {
                                generatedMove = null;
                            }
                            break;
                        case PlayerX:
                        case PlayerO:
                            break;
                        default:
                            break;
                    }
                }
            }

            return generatedMove;
        }

        public static bool isValidColumnMove(int row, int col, int[][] Board, int currentPlayer, bool isColCheck)
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
        public static bool isValidRowMove(int row, int col, int[][] Board, int currentPlayer, bool isColCheck)
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
        public static bool isValidMainDiagonalMove(int row, int col, int[][] Board, int currentPlayer, bool isMainDiagonalCheck)
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

                for (int currentCol = col; currentCol < Board.Length && col - index > 0 && row - index > 0; currentCol++) // main diagonal
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
        public bool isValidSubDiagonalMove(int row, int col, int[][] Board, int currentPlayer, bool isMainDiagonalCheck)
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

        public int findOffsetPositionRow(OtheloMove move)
        {
            int lastShow_index = -1;

            for (int currentCol = move.column - 1; currentCol >= 0; currentCol--)//Check to the left
            {
                if (Board[move.row][currentCol] == currentPlayer) // find index of last show
                {
                    lastShow_index = currentCol;
                    break;
                }
                else if (Board[move.row][currentCol] == EmptyCell) // we find empty cell
                {
                    break;
                }
            }

            if (lastShow_index == -1)
            {
                for (int currentCol = move.column + 1; currentCol < Board.Length; currentCol++)
                {
                    if (Board[move.row][currentCol] == currentPlayer) // find index of last show
                    {
                        lastShow_index = currentCol;
                        break;
                    }
                    else if (Board[move.row][currentCol] == EmptyCell) // we find empty cell
                    {
                        break;
                    }
                }
            }

            return lastShow_index;
        }
        public int findOffsetPositionColunm(OtheloMove move)
        {
            int lastShow_index = -1;

            for (int currentRow = move.row - 1; currentRow >= 0; currentRow--)//Check to the left
            {
                if (Board[currentRow][move.column] == currentPlayer) // find index of last show
                {
                    lastShow_index = currentRow;
                    break;
                }
                else if (Board[currentRow][move.column] == EmptyCell) // we find empty cell
                {
                    break;
                }
            }

            if (lastShow_index == -1)
            {
                for (int currentRow = move.row + 1; currentRow < Board.Length; currentRow++)
                {
                    if (Board[currentRow][move.column] == currentPlayer) // find index of last show
                    {
                        lastShow_index = currentRow;
                        break;
                    }
                    else if (Board[currentRow][move.column] == EmptyCell) // we find empty cell
                    {
                        break;
                    }
                }
            }

            return lastShow_index;
        }

        public int findOffsetPositionMainDiagonal(OtheloMove move)
        {
            int lastShow_index = -1;
            int index = 1;

            for (int currentCol = move.column; move.row - index > 0 && currentCol > 0; currentCol--) // main diagonal
            {
                if (Board[move.row - index][move.column - index] == currentPlayer)
                {
                    lastShow_index = move.column - index;
                    break;
                }
                else if (Board[move.row - index][move.column - index] != currentPlayer && Board[move.row - index][move.column - index] != (-1) * currentPlayer) // we find empty cell
                {
                    break;
                }
                index++;
            }

            if (lastShow_index == -1)
            {
                index = 1;
                for (int currentCol = move.column; currentCol < Board.Length - 1; currentCol++) // main diagonal
                {
                    if (Board[move.row + index][move.column + index] == currentPlayer)
                    {
                        lastShow_index = move.column + index;
                        break;
                    }
                    else if (Board[move.row + index][move.column + index] != currentPlayer && Board[move.row + index][move.column + index] != (-1) * currentPlayer) // we find empty cell
                    {
                        break;
                    }

                    index++;
                }

            }

            return lastShow_index;
        }

        public int findOffsetPositionSubDiagonal(OtheloMove move)
        {
            int lastShow_index = -1;
            int index = 1;

            for (int currentCol = move.column; move.row - index > 0 && move.column + index < Difficulty; currentCol--)
            {
                if (Board[move.row - index][move.column + index] == currentPlayer)
                {
                    lastShow_index = move.column + index;
                    break;
                }
                else if (Board[move.row - index][move.column + index] != currentPlayer && Board[move.row - index][move.column + index] != (-1) * currentPlayer)
                {
                    break;
                }

                index++;
            }

            if (lastShow_index == -1)
            {
                index = 1;

                for (int currentCol = move.column; move.column < Board.Length; currentCol++)
                {
                    if (Board[move.row + index][move.column - index] == currentPlayer)
                    {
                        lastShow_index = move.column - index;
                        break;
                    }
                    else if (Board[move.row + index][move.column - index] != currentPlayer && Board[move.row + index][move.column - index] != (-1) * currentPlayer)
                    {
                        break;
                    }

                    index++;
                }
            }

            return lastShow_index;
        }
    }
}