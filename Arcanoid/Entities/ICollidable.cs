using System.Numerics;

namespace Arkanoid.Core.Entities
{
    public interface ICollidable
    {
        bool CheckCollision(Ball ball);
        void HandleCollision(Ball ball);
        Vector2 Position { get; }
        Vector2 Size { get; }
    }
}