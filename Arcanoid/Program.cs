using System;
using Arkanoid.Game;

namespace Arkanoid
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                using (var gameWindow = new GameWindow())
                {
                    gameWindow.Run();
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}