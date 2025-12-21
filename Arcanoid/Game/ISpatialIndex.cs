using Arkanoid.Core.Entities;
using System.Collections.Generic;

namespace Arkanoid.Core.Game
{
    public interface ISpatialIndex
    {
        void AddBlock(Block block);
        IEnumerable<Block> GetNearbyBlocks(Vector2 position, float radius);
        void Clear();
    }
}