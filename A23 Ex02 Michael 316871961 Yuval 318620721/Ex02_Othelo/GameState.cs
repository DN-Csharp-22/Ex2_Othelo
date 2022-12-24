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

        public char GetCurrentPlayer()
        {
            return this.currentPlayer == 1 ? 'X' : 'O';
        }

        public void InsertMoveToBoard(OtheloMove move)
        {
            char currentPlayerSymbol = this.currentPlayer == 1 ? 'X' : 'O';

            //TODO do here the insert of the move into the board

            currentPlayer = currentPlayer == 1 ? -1 : 1;
        }

        public bool IsGameFinished(out string winner)
        {
            bool isGameFinished = true;

            winner = "";

            int[] playersSymbolsCount = new int[2];

            for (int row = 0; row < Difficulty; row++)
            {
                for (int col = 0; col < Difficulty; col++)
                {
                    switch (Board[row][col])
                    {
                        case 0:
                            isGameFinished = false;
                            break;
                        case 1:
                            playersSymbolsCount[0]++;
                            break;
                        case -1:
                            playersSymbolsCount[1]++;
                            break;
                        default:
                            break;
                    }
                }
            }

            winner = playersSymbolsCount[0] > playersSymbolsCount[1] ? "X" : "O";

            return isGameFinished;
        }

        public bool IsMoveValid(OtheloMove currentMove)
        {
            bool isValidMove = isValidRowOrColumnMove(currentMove.row, currentMove.col, Board, currentPlayer, false);

            if (!isValidMove)
            {
                isValidMove = isValidRowOrColumnMove(currentMove.row, currentMove.col, Board, currentPlayer, true);
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

        public static bool isValidRowOrColumnMove(int row, int col, int[][] Board, int currentPlayer, bool isColCheck)
        {
            if (isColCheck)
            {
                int tempNumberForSwitch = row;
                row = col;
                col = tempNumberForSwitch;
            }

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
    }
}