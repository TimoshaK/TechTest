using Arkanoid.Core.Entities;
using System.Collections.Generic;

namespace Arkanoid.Core.Game
{
    public interface ILevelManager
    {
        int CurrentLevel { get; }
        int TotalLevels { get; }
        LevelData CurrentLevelData { get; }

        event System.Action<int> LevelChanged;
        event System.Action AllLevelsCompleted;

        bool LoadLevels();
        bool LoadLevel(int levelNumber);
        bool NextLevel();
        bool HasNextLevel();

        List<LevelData> GetAllLevels();
        void AddLevel(LevelData level);
        void RemoveLevel(int levelNumber);
    }
}