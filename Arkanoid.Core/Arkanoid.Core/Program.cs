using System;
using Arkanoid.Core.Game;

namespace Arkanoid.Core
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