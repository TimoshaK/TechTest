using Arkanoid.Entities;

namespace Arkanoid.Game
{
    public static class MenuConstants
    {
        public const float ButtonWidth = 250;
        public const float ButtonHeight = 50;

        public static readonly Vector2 NewGameButtonPosition = new Vector2(400, 300);
        public static readonly Vector2 ExitButtonPosition = new Vector2(400, 370);
        public static readonly Vector2 NextLevelButtonPosition = new Vector2(400, 300);
        public static readonly Vector2 NewGameAfterLevelPosition = new Vector2(400, 370);
        public static readonly Vector2 ExitAfterLevelPosition = new Vector2(400, 440);

        public static bool IsPointInButton(float x, float y, Vector2 buttonCenter)
        {
            float halfWidth = ButtonWidth / 2;
            float halfHeight = ButtonHeight / 2;

            return x >= buttonCenter.X - halfWidth &&
                   x <= buttonCenter.X + halfWidth &&
                   y >= buttonCenter.Y - halfHeight &&
                   y <= buttonCenter.Y + halfHeight;
        }
    }
}