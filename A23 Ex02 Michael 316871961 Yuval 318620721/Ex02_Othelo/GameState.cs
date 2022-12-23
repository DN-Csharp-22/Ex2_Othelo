using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex02_Othelo
{
    internal class GameState
    {
        public int Difficulty { get; set; }
        public int[][] Board { get; set; }

        private int currentPlayer { get; set; }

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


        public void InsertMoveToBoard(OtheloMove move)
        {
            char currentPlayerSymbol = this.currentPlayer == 1 ? 'X' : 'O';

            //TODO do here the insert of the move into the board

            currentPlayer = currentPlayer == 1 ? -1 : 1;
        }


        public bool IsMoveValid(OtheloMove currentMove)

        {
            bool row, colon, firs_dianagonal, last_dianagonal, flag = false;

            row = checkingRow(currentMove.row, currentMove.col, Board, currentPlayer);

            colon = checkingRow(currentMove.row, currentMove.col, Board, currentPlayer);

            firs_dianagonal = checkingRow(currentMove.row, currentMove.col, Board, currentPlayer);

            last_dianagonal = checkingRow(currentMove.row, currentMove.col, Board, currentPlayer);

            if (row == true || colon == true || firs_dianagonal == true || last_dianagonal == true)
            {
                flag = true;
            }

            return flag;
        }


        public static bool checkingRow(int _row, int _col, int[][] Board, int current_Player)
        {
            bool flag = true;

            int firstShowInLine = 0, lastShowInLine = 0;

            //checking line -> runinh on col : D -------> 0

            for (int j = _col - 1; j <= 0; j--)
            {
                if (Board[_row][j] == current_Player) // שומר את המופע האחרון של המשלים
                {
                    firstShowInLine = Board[_row][j];
                }

                else
                {
                    if (Board[_row][j] != -1 && Board[_row][j] != 1) //מצאנו תא ריק - נשבור
                    {
                        break;
                    }
                }
            }

            //checking line -> runinh on col : D -------> difficulty

            for (int j = _col + 1; j > Board.Length; j++)
            {
                if (Board[_row][j] == current_Player)
                {
                    lastShowInLine = Board[_row][j];
                }

                else // empty cells
                {
                    if (Board[_row][j] != 1 && Board[_row][j] != 1)
                    {
                        break;
                    }
                }
            }

            if (firstShowInLine != current_Player && lastShowInLine != current_Player) //אין חסימה מאף צד בשורה
            {
                flag = false;
            }


            return flag;

        }

        public static bool checkingColon(int _row, int _col, int[][] Board, int current_Player)
        {
            int firstShowInCol = 0, lastShowInCol = 0;

            bool flag = true;

            //checking col -> runinh on row : 3 -------> 0

            for (int i = _row - 1; i <= 0; i--)
            {
                if (Board[i][_col] == current_Player)
                {
                    firstShowInCol = Board[i][_col];
                }
                else
                {
                    if (Board[i][_col] != -1 && Board[i][_col] != 1)
                    {
                        break;
                    }

                }
            }

            //checking col -> runinh on row : 3 -------> difficulty

            for (int i = _row + 1; i <= Board.Length - 1; i++)
            {
                if (Board[i][_col] == current_Player)
                {
                    lastShowInCol = Board[i][_col];
                }

                else // emptycell
                {
                    if (Board[i][_col] != 1 && Board[i][_col] != -1)
                    {
                        break;
                    }
                }
            }

            if (firstShowInCol != current_Player && lastShowInCol != current_Player)
            {
                flag = false;
            }


            return flag;
        }

        public static bool checking_First_Dianagonal(int _row, int _col, int[][] Board, int current_Player)
        {
            int firstShowFirst_Dianagonal = 0, lastShowFirst_Dianagonal = 0;

            bool flag = true;

            int index = 1;

            for (int i = _col; i >= 0; i--)
            {
                if (Board[_row - index][_col - index] == current_Player)
                {
                    firstShowFirst_Dianagonal = Board[_row - index][_col - index];
                }

                else
                {
                    if (Board[_row - index][_col - index] != -1 && Board[_row - index][_col - index] != 1)
                    {
                        break;
                    }

                }
                index++;
            }

            index = 1;


            for (int i = _col; i <= Board.Length - 1; i++)
            {
                if (Board[_row + index][_col + index] == current_Player)
                {
                    lastShowFirst_Dianagonal = Board[_row + index][_col + index];
                }

                else
                {
                    if (Board[_row + index][_col + index] != -1 && Board[_row + index][_col + index] != 1)
                    {
                        break;
                    }
                }
                index++;
            }

            if (firstShowFirst_Dianagonal != current_Player && lastShowFirst_Dianagonal != current_Player)
            {
                flag = false;
            }

            return flag;
        }

        public static bool checking_Last_Dianagonal(int _row, int _col, int[][] Board, int current_Player)
        {
            int firstShowLast_Dianagonal = 0, lastShowLast_Dianagonal = 0;

            bool flag = true;

            int index = 1;


            for (int i = _row; i < Board.Length; i++)
            {
                if (Board[_row - index][_col - index] == current_Player)
                {
                    firstShowLast_Dianagonal = Board[_row - index][_col - index];
                }

                else
                {
                    if (Board[_row - index][_col - index] != -1 && Board[_row - index][_col - index] != 1)
                    {
                        break;
                    }
                }
                index++;
            }

            index = 1;

            for (int i = _row; i >= 0; i--)
            {
                if (Board[_row + index][_col + index] == current_Player)
                {
                    lastShowLast_Dianagonal = Board[_row + index][_col + index];
                }

                else
                {
                    if (Board[_row + index][_col + index] != -1 && Board[_row + index][_col + index] != 1)
                    {
                        break;
                    }
                }
                index++;
            }

            if (firstShowLast_Dianagonal != current_Player && lastShowLast_Dianagonal != current_Player)
            {
                flag = false;
            }

            return flag;

        }


    }
}