using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex02_Othelo
{
    internal class OtheloGame
    {
        public static bool RunGame { get; set; }
        static OtheloGame()
        {
            RunGame = true;
        }
        public static void StartGame()
        {
            GameUI gameUI = new GameUI();
            
            int gameDifficulty = gameUI.InitializeGame();

            GameState gameState = new GameState(gameDifficulty);
        }
    }
}
