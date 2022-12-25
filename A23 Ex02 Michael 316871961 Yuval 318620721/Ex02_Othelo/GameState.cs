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
        public int difficulty { get; set; }

        public int[][] board { get; set; }

        public int currentPlayer { get; set; }

        public const int k_Player_X = 1;

        public const int k_Player_O = -1;

        public const int k_EmptyCell = 0;

        public const char k_PlayerXSymbol = 'X';

        public const char k_PlayerOSymbol = 'O';

        public GameState(int rowLength)
        {
            this.difficulty = rowLength;
            this.board = new int[rowLength][];
            this.currentPlayer = 1;

            for (int rowNumber = 0; rowNumber < this.board.Length; rowNumber++)
            {
                this.board[rowNumber] = new int[rowLength];
            }

            int middleCell = (rowLength / 2) - 1;

            this.board[middleCell][middleCell] = -1;
            this.board[middleCell][middleCell + 1] = 1;
            this.board[middleCell + 1][middleCell] = 1;
            this.board[middleCell + 1][middleCell + 1] = -1;
        }

        public char GetCurrentPlayerSymbol()
        {
            return this.currentPlayer == 1 ? k_PlayerXSymbol : k_PlayerOSymbol;
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
                int rowIndexHolder = FindOffsetPositionRow(move);

                if (rowIndexHolder != -1)
                {
                    for (int i = move.column; i < rowIndexHolder; i++)
                    {
                        board[move.row][i] = currentPlayer;
                    }

                    for (int i = move.column; i > rowIndexHolder; i--)
                    {
                        board[move.row][i] = currentPlayer;
                    }
                }
            }

            if (validMoves.ColumnValid)
            {
                index = 0;
                int columnIndexHolder = findOffsetPositionColunm(move);

                if (columnIndexHolder != -1)
                {
                    for (int i = move.row; i < columnIndexHolder; i++)
                    {
                        board[i][move.column] = currentPlayer;
                    }

                    for (int i = move.row; i > columnIndexHolder; i--)
                    {
                        board[i][move.column] = currentPlayer;
                    }
                }
            }

            if (validMoves.MainDiagonalValid)
            {
                index = 0;
                int mainDianagonalIndexHolder = FindOffsetPositionMainDiagonal(move);
                if (mainDianagonalIndexHolder != -1)
                {
                    for (int j = move.column; j < mainDianagonalIndexHolder; j++)
                    {
                        board[move.row + index][move.column + index] = currentPlayer;
                        index++;
                    }

                    for (int j = mainDianagonalIndexHolder + 1; j <= move.column; j++)
                    {
                        board[move.row - index][move.column - index] = currentPlayer;
                        index++;
                    }
                }
            }

            if (validMoves.SubDiagonalValid) 
            {
                index = 0;
                int subDianagonalIndexHolder = findOffsetPositionSubDiagonal(move);
                if (subDianagonalIndexHolder != -1)
                {
                    for (int j = subDianagonalIndexHolder + 1; j <= move.column; j++)
                    {
                        board[move.row + index][move.column - index] = currentPlayer;
                        index++;
                    }

                    index = 0;

                    for (int j = move.column; j < subDianagonalIndexHolder; j++)
                    {
                        board[move.row - index][move.column + index] = currentPlayer;
                        index++;
                    }
                }
            }
        }

        public void SwitchPlayers()
        {
            currentPlayer *= -1;
        }

        public bool IsGameFinished(out char o_winner)
        {
            bool isFinished = false;

            int[] playersSymbolsCount = new int[3];

            o_winner = ' ';

            for (int row = 0; row < difficulty; row++)
            {
                for (int col = 0; col < difficulty; col++)
                {
                    switch (board[row][col])
                    {
                        case k_EmptyCell:
                            playersSymbolsCount[2]++;
                            break;
                        case k_Player_X:
                            playersSymbolsCount[0]++;
                            break;
                        case k_Player_O:
                            playersSymbolsCount[1]++;
                            break;
                        default:
                            break;
                    }
                }
            }

            if (playersSymbolsCount[2] == 0)
            {
                o_winner = playersSymbolsCount[0] > playersSymbolsCount[1] ? k_PlayerXSymbol : k_PlayerOSymbol;
                isFinished = true;
            }

            return isFinished;
        }

        public int FindOffsetPositionRow(OtheloMove move)
        {
            int lastShowIndex = -1;

            for (int currentCol = move.column - 1; currentCol >= 0; currentCol--)
            {
                if (board[move.row][currentCol] == currentPlayer)
                {
                    lastShowIndex = currentCol;
                    break;
                }
                else if (board[move.row][currentCol] == k_EmptyCell)
                {
                    break;
                }
            }

            if (lastShowIndex == -1)
            {
                for (int currentCol = move.column + 1; currentCol < board.Length; currentCol++)
                {
                    if (board[move.row][currentCol] == currentPlayer)
                    {
                        lastShowIndex = currentCol;
                        break;
                    }
                    else if (board[move.row][currentCol] == k_EmptyCell)
                    {
                        break;
                    }
                }
            }

            return lastShowIndex;
        }

        public int findOffsetPositionColunm(OtheloMove move)
        {
            int lastShowIndex = -1;

            for (int currentRow = move.row - 1; currentRow >= 0; currentRow--)
            {
                if (board[currentRow][move.column] == currentPlayer)
                {
                    lastShowIndex = currentRow;
                    break;
                }
                else if (board[currentRow][move.column] == k_EmptyCell)
                {
                    break;
                }
            }

            if (lastShowIndex == -1)
            {
                for (int currentRow = move.row + 1; currentRow < board.Length; currentRow++)
                {
                    if (board[currentRow][move.column] == currentPlayer)
                    {
                        lastShowIndex = currentRow;
                        break;
                    }
                    else if (board[currentRow][move.column] == k_EmptyCell)
                    {
                        break;
                    }
                }
            }

            return lastShowIndex;
        }

        public int FindOffsetPositionMainDiagonal(OtheloMove move)
        {
            int lastShowIndex = -1;
            int index = 1;

            for (int currentCol = move.column; move.row - index > 0 && currentCol > 0; currentCol--)
            {
                if (board[move.row - index][move.column - index] == currentPlayer)
                {
                    lastShowIndex = move.column - index;
                    break;
                }
                else if (board[move.row - index][move.column - index] != currentPlayer && board[move.row - index][move.column - index] != (-1) * currentPlayer)
                {
                    break;
                }

                index++;
            }

            if (lastShowIndex == -1)
            {
                index = 1;
                for (int currentCol = move.column; currentCol < board.Length - 1; currentCol++)
                {
                    if (board[move.row + index][move.column + index] == currentPlayer)
                    {
                        lastShowIndex = move.column + index;
                        break;
                    }
                    else if (board[move.row + index][move.column + index] != currentPlayer && board[move.row + index][move.column + index] != (-1) * currentPlayer)
                    {
                        break;
                    }

                    index++;
                }
            }

            return lastShowIndex;
        }

        public int findOffsetPositionSubDiagonal(OtheloMove move)
        {
            int lastShowIndex = -1;
            int index = 1;

            for (int currentCol = move.column; move.row - index > 0 && move.column + index < difficulty; currentCol--)
            {
                if (board[move.row - index][move.column + index] == currentPlayer)
                {
                    lastShowIndex = move.column + index;
                    break;
                }
                else if (board[move.row - index][move.column + index] != currentPlayer && board[move.row - index][move.column + index] != (-1) * currentPlayer)
                {
                    break;
                }

                index++;
            }

            if (lastShowIndex == -1)
            {
                index = 1;

                for (int currentCol = move.column; move.column < board.Length; currentCol++)
                {
                    if (board[move.row + index][move.column - index] == currentPlayer)
                    {
                        lastShowIndex = move.column - index;
                        break;
                    }
                    else if (board[move.row + index][move.column - index] != currentPlayer && board[move.row + index][move.column - index] != (-1) * currentPlayer)
                    {
                        break;
                    }

                    index++;
                }
            }

            return lastShowIndex;
        }
    }
}