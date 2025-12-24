using Arkanoid.Entities;
namespace Arkanoid.Game
{
    public static class GameConstants
    {
        public const float FIELD_WIDTH = 800;
        public const float FIELD_HEIGHT = 600;

        public const float PADDLE_SPEED = 400;
        public const float BALL_SPEED = 600;

        public const float PADDLE_WIDTH = 100;
        public const float PADDLE_HEIGHT = 20;
        public const float BALL_RADIUS = 10;

        public const int BLOCK_ROWS = 5;
        public const int BLOCK_COLS = 10;
        public const float BLOCK_WIDTH = 70;
        public const float BLOCK_HEIGHT = 30;
        public const float BLOCK_MARGIN = 5;

        public const int INITIAL_LIVES = 3;
        public const int SCORE_PER_BLOCK = 10;

        public const float BALL_LAUNCH_THRESHOLD = 0.01f;
        public const float PADDLE_BOUNCE_ANGLE_FACTOR = 0.5f;
        public const float PADDLE_COLLISION_ZONE = 5f;
        public const float MIN_COLLISION_OVERLAP = 0.001f;
        public const float STICKY_BALL_OFFSET = -30f;
        public const float BALL_LOSS_MARGIN = 10f;

        public static readonly Vector2 PADDLE_START_POSITION = new(FIELD_WIDTH / 2, FIELD_HEIGHT - 50);
        public static readonly Vector2 PADDLE_SIZE = new(PADDLE_WIDTH, PADDLE_HEIGHT);
        public static readonly Vector2 BLOCK_SIZE = new(BLOCK_WIDTH, BLOCK_HEIGHT);
    }
}