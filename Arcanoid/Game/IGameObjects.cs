using Arkanoid.Entities;
using System.Collections.Generic;

namespace Arkanoid.Game
{
    public interface IGameObjects
    {
        Vector2 GetPaddlePosition();
        Vector2 GetPaddleSize();
        Vector2 GetBallPosition();
        float GetBallRadius();
        IEnumerable<Block> GetBlocks();
        int Score { get; }
        int Lives { get; }
    }
}