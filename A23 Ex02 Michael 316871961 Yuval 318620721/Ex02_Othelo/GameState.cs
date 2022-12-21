using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex02_Othelo
{
    internal class GameState
    {
        private int Score { get; set; }
        private int[][] Board { get; set; }
        public GameState(int rowLength)
        {
            this.Score = 0;
            this.Board = new int[rowLength][];

            for (int rowNumber = 0; rowNumber < this.Board.Length; rowNumber++)
            {
                this.Board[rowNumber] = new int[rowLength];
            }
        }
    }
}