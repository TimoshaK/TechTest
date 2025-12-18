using Arkanoid.Core.Game;
using System;

namespace Arkanoid.Core.Entities
{
    public class Block : ICollidable, IRenderDataProvider
    {
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public BlockType Type { get; set; }
        public bool IsDestroyed { get; set; }
        public int HitPoints { get; set; }
        public int MaxHitPoints { get; private set; }
        public Vector2 Center => new Vector2(Position.X + Size.X / 2, Position.Y + Size.Y / 2);

        private static readonly GameColor[] BlockColors =
        {
            new GameColor(255, 0, 0),
            new GameColor(255, 128, 0),
            new GameColor(255, 255, 0),
            new GameColor(0, 255, 0),
            new GameColor(0, 0, 255)
        };

        public Block(Vector2 position, Vector2 size, BlockType type = BlockType.Normal, int hitPoints = 1)
        {
            Position = position;
            Size = size;
            Type = type;
            IsDestroyed = false;
            HitPoints = hitPoints;
            MaxHitPoints = hitPoints;
        }

        public bool CheckCollision(Ball ball)
        {
            if (IsDestroyed)
                return false;

            float ballLeft = ball.Position.X - ball.Radius;
            float ballRight = ball.Position.X + ball.Radius;
            float ballTop = ball.Position.Y - ball.Radius;
            float ballBottom = ball.Position.Y + ball.Radius;

            float blockLeft = Position.X;
            float blockRight = Position.X + Size.X;
            float blockTop = Position.Y;
            float blockBottom = Position.Y + Size.Y;

            return ballRight > blockLeft &&
                   ballLeft < blockRight &&
                   ballBottom > blockTop &&
                   ballTop < blockBottom;
        }

        public void HandleCollision(Ball ball)
        {
            if (Type == BlockType.Unbreakable)
                return;

            HitPoints--;
            if (HitPoints <= 0)
            {
                IsDestroyed = true;
            }
        }

        public RenderData GetRenderData()
        {
            return new RenderData
            {
                Position = Position,
                Size = Size,
                Color = CalculateColor(),
                Text = Type == BlockType.Normal && HitPoints > 1 ? HitPoints.ToString() : null,
                HasOutline = true
            };
        }

        private GameColor CalculateColor()
        {
            if (Type == BlockType.Unbreakable)
                return new GameColor(128, 128, 128);

            int colorIndex = Math.Min(HitPoints - 1, BlockColors.Length - 1);
            return BlockColors[colorIndex];
        }
    }
}