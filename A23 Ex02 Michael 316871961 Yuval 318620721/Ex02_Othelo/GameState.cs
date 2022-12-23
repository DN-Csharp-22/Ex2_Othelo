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

        public bool IsMoveValid(OtheloMove currentMove)
        {
            return true;
        }

        public void InsertMoveToBoard(OtheloMove move)
        {
            char currentPlayerSymbol = this.currentPlayer == 1 ? 'X' : 'O';

            //TODO do here the insert of the move into the board

            currentPlayer = currentPlayer == 1 ? -1 : 1;
        }
    }
}