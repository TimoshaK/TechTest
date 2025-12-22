using Arkanoid.Game;
using System.Collections.Generic;

namespace Arkanoid.Entities
{
    public class LevelData
    {
        public int LevelNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float BallSpeed { get; set; }
        public float PaddleSpeed { get; set; }
        public float BallRadius { get; set; }
        public float PaddleWidth { get; set; }
        public int InitialLives { get; set; }
        public float BlockSpacing { get; set; }
        public List<string> BlockLayout { get; set; }
        public ColorData BackgroundColor { get; set; }

        public LevelData()
        {
            BlockLayout = new List<string>();
            BackgroundColor = new ColorData(0, 0, 0);
            BlockSpacing = GameConstants.BLOCK_MARGIN;
        }
    }

    public class ColorData
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
        public byte A { get; set; }

        public ColorData() { }

        public ColorData(byte r, byte g, byte b, byte a = 255)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public ColorData(int r, int g, int b, int a = 255)
        {
            R = (byte)Math.Clamp(r, 0, 255);
            G = (byte)Math.Clamp(g, 0, 255);
            B = (byte)Math.Clamp(b, 0, 255);
            A = (byte)Math.Clamp(a, 0, 255);
        }

        public GameColor ToGameColor()
        {
            return new GameColor(R, G, B, A);
        }
    }
}