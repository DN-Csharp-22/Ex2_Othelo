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
            this.currentPlayer = -1;

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

        public void InsertMoveToBoard(OtheloMove move)
        {
            char currentPlayerSymbol = GetCurrentPlayerSymbol();

            //TODO do here the insert of the move into the board

            //if (IsMoveValid(move))

            int row_indexHolder = findOffsetPositionRow(move);
            int column_indexHolder = findOffsetPositionColunm(move);
            int main_dianagonalIndexHolder = findOffsetPositionMainDiagonal(move);
            int sub_dianagonalIndexHolder = findOffsetPositionSubDiagonal(move);


            //======> רצה על עמודה מלמעלה למטה
            if (column_indexHolder != -1) //רצה על טווח עמודות (בתכלס משבץ שורה)  
            {
                for (int i = move.row; i < column_indexHolder; i++)
                {
                    Board[i][move.col] = currentPlayer;
                }

                for (int i = move.row; i > column_indexHolder; i--)
                {
                    Board[i][move.col] = currentPlayer;
                }
            }

            if (row_indexHolder != -1) // רצה על טווח שורות בתכלס משבצת עמודות 
            {
                for (int i = move.col; i < row_indexHolder; i++)
                {
                    Board[move.row][i] = currentPlayer;
                }

                for (int i = move.col; i > row_indexHolder; i--)
                {
                    Board[move.row][i] = currentPlayer;
                }
            }

            if (main_dianagonalIndexHolder != -1)
            {
                int index = 0;
                for (int j = move.col; j < main_dianagonalIndexHolder; j++)
                {
                    Board[move.row + index][move.col + index] = currentPlayer;
                    index++;
                }

                for (int j = main_dianagonalIndexHolder + 1; j <= move.col; j++)
                {
                    Board[move.row - index][move.col - index] = currentPlayer;
                    index++;

                }

                if (sub_dianagonalIndexHolder != -1)
                {
                    index = 0;
                    for (int j = sub_dianagonalIndexHolder + 1; j <= move.col; j++)
                    {
                        Board[move.row - index][move.col + index] = currentPlayer;
                        index++;

                    }
                    index = 0;
                    for (int j = move.col; j < sub_dianagonalIndexHolder; j++)
                    {
                        Board[move.row - index][move.col + index] = currentPlayer;
                        index++;
                    }
                }
            }

            currentPlayer *= -1;
        }

        public bool IsGameFinished(out char winner)
        {
            bool isGameFinished = true;

            int[] playersSymbolsCount = new int[2];

            winner = ' ';

            for (int row = 0; row < Difficulty && isGameFinished; row++)
            {
                for (int col = 0; col < Difficulty && isGameFinished; col++)
                {
                    switch (Board[row][col])
                    {
                        case EmptyCell:
                            isGameFinished = false;
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

            if (isGameFinished)
            {
                winner = playersSymbolsCount[0] > playersSymbolsCount[1] ? PlayerXSymbol : PlayerOSymbol;
            }

            return isGameFinished;
        }

        public bool IsMoveValid(OtheloMove currentMove)
        {
            bool isValidMove = isValidRowMove(currentMove.row, currentMove.col, Board, currentPlayer, false);

            if (!isValidMove)
            {
                isValidMove = isValidColumnMove(currentMove.row, currentMove.col, Board, currentPlayer, true);
                if (!isValidMove)
                {
                    isValidMove = isValidMainOrSubDiagonalMove(currentMove.row, currentMove.col, Board, currentPlayer, true);
                    if (!isValidMove)
                    {
                        isValidMove = isValidMainOrSubDiagonalMove(currentMove.row, currentMove.col, Board, currentPlayer, false);
                    }
                }
            }

            return isValidMove;
        }

        /// <summary>
        /// This method can be altered with AI logic
        /// </summary>
        public void GenerateMoveByComputer()
        {
            //random
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
                else//Found an empty cell
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
                }
            }

            return isMoveValid;
        }
        public static bool isValidMainOrSubDiagonalMove(int row, int col, int[][] Board, int currentPlayer, bool isMainDiagonalCheck)
        {
            int reverseCol = isMainDiagonalCheck ? 1 : -1;
            int reverseRow = 1;

            bool isMoveValid = false;

            int oppositePlayerSymbolsCount = 0;

            int oppositePlayer = (-1) * currentPlayer;

            int index = 1;

            for (int currentCol = col; currentCol >= 0; currentCol--)//Check to the left
            {
                if (Board[row - index * reverseRow][col - index * reverseCol] == oppositePlayer)
                {
                    oppositePlayerSymbolsCount++;
                }
                else if (Board[row - index * reverseRow][col - index * reverseCol] == currentPlayer && oppositePlayerSymbolsCount > 0)
                {
                    isMoveValid = true;
                    break;
                }
                index++;
            }

            if (!isMoveValid)
            {
                index = 1;

                reverseCol = 1;
                reverseRow = isMainDiagonalCheck ? 1 : -1;

                for (int currentCol = col + 1; currentCol < Board.Length; currentCol++)//Check to the right
                {
                    if (Board[row + index * reverseRow][col + index * reverseCol] == oppositePlayer)
                    {
                        oppositePlayerSymbolsCount++;
                    }
                    else if (Board[row + index * reverseRow][col + index * reverseCol] == currentPlayer && oppositePlayerSymbolsCount > 0)
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

            for (int currentCol = move.col - 1; currentCol >= 0; currentCol--)//Check to the left
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
                for (int currentCol = move.col + 1; currentCol < Board.Length; currentCol++)
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
                if (Board[currentRow][move.col] == currentPlayer) // find index of last show
                {
                    lastShow_index = currentRow;
                    break;
                }
                else if (Board[currentRow][move.col] == EmptyCell) // we find empty cell
                {
                    break;
                }
            }

            if (lastShow_index == -1)
            {
                for (int currentRow = move.row + 1; currentRow < Board.Length; currentRow++)
                {
                    if (Board[currentRow][move.col] == currentPlayer) // find index of last show
                    {
                        lastShow_index = currentRow;
                        break;
                    }
                    else if (Board[currentRow][move.col] == EmptyCell) // we find empty cell
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

            for (int currentCol = move.col; currentCol >= 0; currentCol--) // main diagonal
            {
                if (Board[move.row - index][move.col - index] == currentPlayer)
                {
                    lastShow_index = move.col - index;
                }
                else if (Board[move.row - index][move.col - index] != currentPlayer && Board[move.row - index][move.col - index] != (-1) * currentPlayer) // we find empty cell
                {
                    break;
                }
                index++;
            }

            if (lastShow_index == -1)
            {
                index = 1;
                for (int currentCol = move.col; currentCol < Board.Length; currentCol++) // main diagonal
                {
                    if (Board[move.row + index][move.col + index] == currentPlayer)
                    {
                        lastShow_index = move.col + index;
                    }
                    else if (Board[move.row + index + 1][move.col + index] != currentPlayer && Board[move.row + index][move.col + index] != (-1) * currentPlayer) // we find empty cell
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

            for (int currentCol = move.col; move.col >= 0; currentCol--)
            {
                if (Board[move.row - index][move.col + index] == currentPlayer)
                {
                    lastShow_index = move.col + index;
                }
                else if (Board[move.row - index][move.col + index] != currentPlayer && Board[move.row - index][move.col + index] != (-1) * currentPlayer)
                {
                    break;
                }

                index++;
            }

            if (lastShow_index == -1)
            {
                for (int currentCol = move.col; move.col < Board.Length; currentCol++)
                {
                    if (Board[move.row + index][move.col - index] == currentPlayer)
                    {
                        lastShow_index = move.col - index;
                    }
                    else if (Board[move.row + index][move.col - index] != currentPlayer && Board[move.row + index][move.col - index] != (-1) * currentPlayer)
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