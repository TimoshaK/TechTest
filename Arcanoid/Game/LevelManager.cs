using Arkanoid.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arkanoid.Core.Game
{
    public class LevelManager : ILevelManager
    {
        private List<LevelData> _levels;
        private int _currentLevelIndex;

        public int CurrentLevel => _currentLevelIndex + 1;
        public int TotalLevels => _levels?.Count ?? 0;
        public LevelData CurrentLevelData =>
            _currentLevelIndex >= 0 && _currentLevelIndex < _levels.Count ?
            _levels[_currentLevelIndex] : null;

        public event Action<int> LevelChanged;
        public event Action AllLevelsCompleted;

        public LevelManager()
        {
            _levels = new List<LevelData>();
            _currentLevelIndex = -1;
        }

        public bool LoadLevels()
        {
            try
            {
                _levels.Clear();
                CreateDefaultLevels();
                
                return _levels.Count > 0;
            }
            catch (Exception ex)
            {
                
                CreateDefaultLevels();
                return _levels.Count > 0;
            }
        }

        private void CreateDefaultLevels()
        {
            

            // Уровень 1 - легкий
            var level1 = new LevelData
            {
                LevelNumber = 1,
                Name = "Новичок",
                Description = "Легкий уровень для начала",

                
                BallSpeed = GameConstants.BALL_SPEED * 0.7f,      
                PaddleSpeed = GameConstants.PADDLE_SPEED * 0.9f,     
                BallRadius = GameConstants.BALL_RADIUS * 1.2f,   
                PaddleWidth = GameConstants.PADDLE_WIDTH * 1.3f,      

                InitialLives = GameConstants.INITIAL_LIVES + 2,  
                BlockSpacing = GameConstants.BLOCK_MARGIN * 1.5f,  

                BackgroundColor = new ColorData(20, 20, 40)
            };

            // 3 ряда блоков
            level1.BlockLayout.Add("1111111111"); // 10 блоков (BLOCK_COLS)
            level1.BlockLayout.Add("1111111111");
            level1.BlockLayout.Add("1111111111");

            // Уровень 2 - средний
            var level2 = new LevelData
            {
                LevelNumber = 2,
                Name = "Воин",
                Description = "Средний уровень",

               
                BallSpeed = GameConstants.BALL_SPEED * 0.85f,       
                PaddleSpeed = GameConstants.PADDLE_SPEED,            
                BallRadius = GameConstants.BALL_RADIUS * 1.1f,     
                PaddleWidth = GameConstants.PADDLE_WIDTH * 1.1f,      

                InitialLives = GameConstants.INITIAL_LIVES,          
                BlockSpacing = GameConstants.BLOCK_MARGIN * 1.2f,     

                BackgroundColor = new ColorData(30, 15, 45)
            };

            // 4 ряда с разной прочностью блоков
            level2.BlockLayout.Add("1111111111"); 
            level2.BlockLayout.Add("2222222222"); 
            level2.BlockLayout.Add("2222222222");
            level2.BlockLayout.Add("1111111111");

            // Уровень 3 - сложный
            var level3 = new LevelData
            {
                LevelNumber = 3,
                Name = "Мастер",
                Description = "Сложный уровень",

                // Сложнее стандартных значений
                BallSpeed = GameConstants.BALL_SPEED * 1.1f,           
                PaddleSpeed = GameConstants.PADDLE_SPEED * 1.05f,    
                BallRadius = GameConstants.BALL_RADIUS,                
                PaddleWidth = GameConstants.PADDLE_WIDTH * 0.9f,    

                InitialLives = GameConstants.INITIAL_LIVES - 1,      
                BlockSpacing = GameConstants.BLOCK_MARGIN * 0.9f,  

                BackgroundColor = new ColorData(40, 10, 50)
            };

            // 5 рядов с прогрессирующей сложностью
            level3.BlockLayout.Add("1111111111");
            level3.BlockLayout.Add("2222222222");
            level3.BlockLayout.Add("3333333333"); 
            level3.BlockLayout.Add("2222222222"); 
            level3.BlockLayout.Add("1111111111"); 

            var level4 = new LevelData
            {
                LevelNumber = 4,
                Name = "Эксперт",
                Description = "Очень сложный уровень",

                BallSpeed = GameConstants.BALL_SPEED * 1.3f,       
                PaddleSpeed = GameConstants.PADDLE_SPEED * 1.1f,     
                BallRadius = GameConstants.BALL_RADIUS * 0.85f,        
                PaddleWidth = GameConstants.PADDLE_WIDTH * 0.75f,    

                InitialLives = GameConstants.INITIAL_LIVES - 2,       
                BlockSpacing = GameConstants.BLOCK_MARGIN * 0.7f,    

                BackgroundColor = new ColorData(50, 5, 55)
            };

            // 5 рядов с неразрушаемыми блоками
            level4.BlockLayout.Add("1U1U111U1U"); // Чередование
            level4.BlockLayout.Add("222U22222U");
            level4.BlockLayout.Add("333U3U333U");
            level4.BlockLayout.Add("4U4U4U4U4U");
            level4.BlockLayout.Add("5U5U5U5U5U");

            var level5 = new LevelData
            {
                LevelNumber = 5,
                Name = "Легенда",
                Description = "Экстремальный уровень",

                BallSpeed = GameConstants.BALL_SPEED * 1.5f,       
                PaddleSpeed = GameConstants.PADDLE_SPEED * 1.15f,    
                BallRadius = GameConstants.BALL_RADIUS * 0.7f,         
                PaddleWidth = GameConstants.PADDLE_WIDTH * 0.6f,    

                InitialLives = 1,                                   
                BlockSpacing = GameConstants.BLOCK_MARGIN * 0.5f,    

                BackgroundColor = new ColorData(60, 0, 60)
            };

            
            level5.BlockLayout.Add("U11111111U");
            level5.BlockLayout.Add("U55555555U");
            level5.BlockLayout.Add("U5UU44455U");
            level5.BlockLayout.Add("U5U333U55U");
            level5.BlockLayout.Add("U5U3U3U55U");
            level5.BlockLayout.Add("U53333335U");
            level5.BlockLayout.Add("U5U5UU5U5U");
            level5.BlockLayout.Add("U55555555U");
            level5.BlockLayout.Add("U11UUUU11U");


            _levels.Add(level1);
            _levels.Add(level2);
            _levels.Add(level3);
            _levels.Add(level4);
            _levels.Add(level5);
        }

        private void ValidateLevelData(LevelData level)
        {
            if (level.LevelNumber <= 0)
                throw new ArgumentException($"Уровень имеет неверный номер: {level.LevelNumber}");

            if (string.IsNullOrEmpty(level.Name))
                level.Name = $"Уровень {level.LevelNumber}";

            if (level.BallSpeed <= 0) level.BallSpeed = GameConstants.BALL_SPEED;
            if (level.PaddleSpeed <= 0) level.PaddleSpeed = GameConstants.PADDLE_SPEED;
            if (level.BallRadius <= 0) level.BallRadius = GameConstants.BALL_RADIUS;
            if (level.PaddleWidth <= 0) level.PaddleWidth = GameConstants.PADDLE_WIDTH;
            if (level.InitialLives <= 0) level.InitialLives = GameConstants.INITIAL_LIVES;
            if (level.BlockSpacing <= 0) level.BlockSpacing = GameConstants.BLOCK_MARGIN;

            if (level.BlockLayout == null || level.BlockLayout.Count == 0)
            {
                CreateDefaultBlockLayout(level);
            }
        }

        private void CreateDefaultBlockLayout(LevelData level)
        {
            level.BlockLayout = new List<string>();
            for (int i = 0; i < 5; i++)
            {
                string row = new string('3', 10);
                level.BlockLayout.Add(row);
            }
        }

        public bool LoadLevel(int levelNumber)
        {
            var levelIndex = _levels.FindIndex(l => l.LevelNumber == levelNumber);

            if (levelIndex >= 0)
            {
                _currentLevelIndex = levelIndex;
                LevelChanged?.Invoke(levelNumber);
                return true;
            }

            return false;
        }

        public bool NextLevel()
        {
            if (HasNextLevel())
            {
                _currentLevelIndex++;
                LevelChanged?.Invoke(CurrentLevel);
                return true;
            }
            else
            {
                AllLevelsCompleted?.Invoke();
                return false;
            }
        }

        public bool HasNextLevel()
        {
            return _currentLevelIndex < _levels.Count - 1;
        }

        public List<LevelData> GetAllLevels()
        {
            return new List<LevelData>(_levels);
        }

        public void AddLevel(LevelData level)
        {
            ValidateLevelData(level);
            _levels.Add(level);
            _levels.Sort((a, b) => a.LevelNumber.CompareTo(b.LevelNumber));
        }

        public void RemoveLevel(int levelNumber)
        {
            _levels.RemoveAll(l => l.LevelNumber == levelNumber);
            _levels.Sort((a, b) => a.LevelNumber.CompareTo(b.LevelNumber));

            if (_currentLevelIndex >= _levels.Count)
            {
                _currentLevelIndex = _levels.Count - 1;
            }
        }
    }
}