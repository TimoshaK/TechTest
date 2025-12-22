namespace Arkanoid.Entities
{
    public enum BlockType
    {
        Normal,
        Unbreakable
    }

    public enum GameState
    {
        Menu,
        Playing,
        Paused,
        GameOver,
        LevelComplete
    }

    public struct GameColor
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
        public byte A { get; set; }

        public GameColor(byte r, byte g, byte b, byte a = 255)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public GameColor(int r, int g, int b, int a = 255)
        {
            R = (byte)System.Math.Clamp(r, 0, 255);
            G = (byte)System.Math.Clamp(g, 0, 255);
            B = (byte)System.Math.Clamp(b, 0, 255);
            A = (byte)System.Math.Clamp(a, 0, 255);
        }

        public SFML.Graphics.Color ToSFMLColor()
        {
            return new SFML.Graphics.Color(R, G, B, A);
        }

        public static GameColor FromSFMLColor(SFML.Graphics.Color color)
        {
            return new GameColor(color.R, color.G, color.B, color.A);
        }
    }
}