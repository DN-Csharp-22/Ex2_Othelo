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
        public int currentPlayer { get; set; }

        public const int PlayerX = 1;
        public const int PlayerO = -1;
        public const int EmptyCell = 0;
        public const char PlayerXSymbol = 'X';
        public const char PlayerOSymbol = 'O';

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

            
        }
        public void SwitchPlayers()
        {
            currentPlayer *= -1;
        }
        public bool IsGameFinished(out char winner)
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

            if (playersSymbolsCount[2] == 0)
            {
                winner = playersSymbolsCount[0] > playersSymbolsCount[1] ? PlayerXSymbol : PlayerOSymbol;
                isFinished = true;
            }

            return isFinished;
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