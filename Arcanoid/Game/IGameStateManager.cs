using Arkanoid.Entities;

namespace Arkanoid.Game
{
    public interface IGameStateManager
    {
        GameState CurrentState { get; }
        bool IsMenuVisible { get; }
        bool IsPlaying { get; }

        event Action<GameState> StateChanged;

        void StartNewGame();
        void Pause();
        void Resume();
        void GameOver();
        void LevelComplete();
    }
}