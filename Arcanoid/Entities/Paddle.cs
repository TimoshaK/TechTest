using System;

namespace Arkanoid.Core.Entities
{
    public class Paddle : ICollidable
    {
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public float Speed { get; set; } = 1f;
        public float FieldWidth { get; set; }

        public Paddle(Vector2 position, Vector2 size, float speed, float fieldWidth)
        {
            Position = position;
            Size = size;
            Speed = speed;
            FieldWidth = fieldWidth;
        }

        public void Move(float direction, float deltaTime)
        {
            float newX = Position.X + direction * Speed * deltaTime;
            newX = Math.Max(Size.X / 2, Math.Min(FieldWidth - Size.X / 2, newX));
            Position = new Vector2(newX, Position.Y);
        }

        public void MoveTo(float x)
        {
            float newX = Math.Max(Size.X / 2, Math.Min(FieldWidth - Size.X / 2, x));
            Position = new Vector2(newX, Position.Y);
        }

        public bool CheckCollision(Ball ball)
        {
            float ballBottom = ball.Position.Y + ball.Radius;
            float paddleTop = Position.Y - Size.Y / 2;
            float paddleLeft = Position.X - Size.X / 2;
            float paddleRight = Position.X + Size.X / 2;

            const float collisionZone = 5f;
            float paddleCollisionTop = paddleTop - collisionZone;

            if (ballBottom < paddleCollisionTop || ball.Position.Y - ball.Radius > paddleTop)
                return false;

            if (ball.Position.X + ball.Radius < paddleLeft || ball.Position.X - ball.Radius > paddleRight)
                return false;

            return true;
        }

        public void HandleCollision(Ball ball)
        {
            float hitPoint = (ball.Position.X - Position.X) / (Size.X / 2);
            hitPoint = Math.Max(-1.0f, Math.Min(1.0f, hitPoint));

            float bounceAngle = hitPoint * (float)(Math.PI / 3);

            float speed = ball.Speed;

            ball.Velocity = new Vector2(
                (float)Math.Sin(bounceAngle) * speed,
                -(float)Math.Cos(bounceAngle) * speed
            );

            if (Math.Abs(ball.Velocity.Y) < speed * 0.3f)
            {
                ball.Velocity = new Vector2(
                    ball.Velocity.X,
                    -Math.Sign(ball.Velocity.Y) * speed * 0.3f
                );
            }

            float currentSpeed = (float)Math.Sqrt(
                ball.Velocity.X * ball.Velocity.X +
                ball.Velocity.Y * ball.Velocity.Y
            );

            if (currentSpeed > 0)
            {
                ball.Velocity = new Vector2(
                    ball.Velocity.X / currentSpeed * speed,
                    ball.Velocity.Y / currentSpeed * speed
                );
            }
        }
    }
}