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
            while (OtheloGame.RunGame)
            {
                OtheloGame.StartGame();
            }
        }
    }
}
