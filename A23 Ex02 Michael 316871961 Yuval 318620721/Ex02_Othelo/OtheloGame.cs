﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex02_Othelo
{
    internal class OtheloGame
    {
        private readonly int[] difficulties = { 6, 8 };
        public OtheloGame() { }

        public void StartGame()
        {
            GameUI gameUI = new GameUI();

            int gameDifficulty = gameUI.GetDifficultyFromUser(difficulties);

            GameState gameState = new GameState(gameDifficulty);

            gameUI.DisplayBoard(gameState.Board);

            

            Console.ReadKey();
        }


        public bool IsMoveValid(string move)
        {
            return true;
        }
    }
}
