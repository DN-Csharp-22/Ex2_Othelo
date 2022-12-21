using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex02_Othelo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            StartOtheloGame();
        }

        private static void StartOtheloGame()
        {
            while (true)
            {
                OtheloGame game = new OtheloGame();
                game.StartGame();
            }
        }
    }
}
