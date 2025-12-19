using System;
using System.Numerics;

namespace Arkanoid.Core.Entities
{
    public class Ball : IUpdatable
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Radius { get; set; }
        public float Speed { get; set; }
        public bool IsSticky { get; set; }

        public Ball(Vector2 position, float radius, float speed)
        {
            Position = position;
            Radius = radius;
            Speed = speed;
            Velocity = new Vector2(0, 0);
            IsSticky = false;
        }

        public void Update(float deltaTime, float fieldWidth, float fieldHeight)
        {
            if (IsSticky) return;

            float currentSpeed = Velocity.Length();
            if (currentSpeed > 0)
            {
                Velocity = Velocity.Normalized() * Speed;
            }

            // позиция
            Position = Position + Velocity * deltaTime;

            // отскок
            if (Position.X - Radius < 0)
            {
                Position = new Vector2(Radius, Position.Y);
                Velocity = new Vector2(Math.Abs(Velocity.X), Velocity.Y);
            }
            else if (Position.X + Radius > fieldWidth)
            {
                Position = new Vector2(fieldWidth - Radius, Position.Y);
                Velocity = new Vector2(-Math.Abs(Velocity.X), Velocity.Y);
            }

            if (Position.Y - Radius < 0)
            {
                Position = new Vector2(Position.X, Radius);
                Velocity = new Vector2(Velocity.X, Math.Abs(Velocity.Y));
            }

            //от залипания 
            float minSpeed = Speed * 0.3f;

            //скорость по осям
            if (Math.Abs(Velocity.X) > 0 && Math.Abs(Velocity.X) < minSpeed)
            {
                Velocity = new Vector2(Math.Sign(Velocity.X) * minSpeed, Velocity.Y);
            }
            if (Math.Abs(Velocity.Y) > 0 && Math.Abs(Velocity.Y) < minSpeed)
            {
                Velocity = new Vector2(Velocity.X, Math.Sign(Velocity.Y) * minSpeed);
            }

            // Защита от слишком горизонтального движения
            float maxHorizontalRatio = 0.9f; // Максимальное соотношение горизонтальной скорости ( чтобы мяч слишком долго не отскакивал по горизонтали )
            float horizontalRatio = Math.Abs(Velocity.X) / Speed;
            if (horizontalRatio > maxHorizontalRatio)
            {
                float verticalSign = Math.Sign(Velocity.Y);
                if (verticalSign == 0) verticalSign = 1;

                float newVertical = (float)Math.Sqrt(Speed * Speed - Velocity.X * Velocity.X) * verticalSign;
                Velocity = new Vector2(Velocity.X, newVertical);
            }
        }

        private static readonly Random _random = new Random();

        public void Launch()
        {
            if (IsSticky)
            {
                IsSticky = false;

                // Более случайный угол от 30 до 60 градусов
                float minAngle = (float)(Math.PI / 6); // 30 градусов
                float maxAngle = (float)(Math.PI / 3); // 60 градусов
                float angle = minAngle + (float)_random.NextDouble() * (maxAngle - minAngle);

                // Случайное направление влево/вправо
                float randomDirection = _random.Next(0, 2) == 0 ? 1 : -1;

                Velocity = new Vector2(
                    (float)Math.Sin(angle) * Speed * randomDirection,
                    -(float)Math.Cos(angle) * Speed
                );
            }
        }
    }
}