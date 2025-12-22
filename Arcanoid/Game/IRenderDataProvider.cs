using Arkanoid.Entities;

namespace Arkanoid.Game
{
    public class RenderData
    {
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public GameColor Color { get; set; }
        public string Text { get; set; }
        public bool HasOutline { get; set; }
    }

    public interface IRenderDataProvider
    {
        RenderData GetRenderData();
    }
}