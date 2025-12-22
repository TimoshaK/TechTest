using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Arkanoid.Core.Entities;

namespace Arkanoid.Core.Game
{
    public class ArkanoidGame : IGameStateManager, IGameObjects
    {
        private DateTime _lastUpdateTime;
        private ISpatialIndex _spatialIndex;
        private ILevelManager _levelManager;
        private SFML.Graphics.Color _backgroundColor;

        public Paddle Paddle { get; private set; }
        public Ball Ball { get; private set; }
        public List<Block> Blocks { get; private set; }
        public GameState CurrentState { get; private set; }
        public int Score { get; set; }
        public int Lives { get; set; }
        public float FieldWidth => GameConstants.FIELD_WIDTH;
        public float FieldHeight => GameConstants.FIELD_HEIGHT;
        public int CurrentLevel => _levelManager?.CurrentLevel ?? 1;
        public string CurrentLevelName => _levelManager?.CurrentLevelData?.Name ?? "Уровень 1";
        public bool HasNextLevel => _levelManager?.HasNextLevel() ?? false;
        public bool AllLevelsCompleted => _levelManager != null && !_levelManager.HasNextLevel() && CurrentState == GameState.LevelComplete;

        public bool IsMenuVisible =>
            CurrentState == GameState.Menu ||
            CurrentState == GameState.GameOver ||
            CurrentState == GameState.LevelComplete;

        public bool IsPlaying => CurrentState == GameState.Playing;

        public SFML.Graphics.Color BackgroundColor => _backgroundColor;

        public event Action<GameState> StateChanged;
        public event Action<int> LevelChanged;
        public event Action OnScoreChanged;
        public event Action OnLivesChanged;
        public event Action OnBallLost;
        public event Action OnBlockDestroyed;
        public event Action OnLevelComplete;
        public event Action OnAllLevelsCompleted;

        public ArkanoidGame(ISpatialIndex spatialIndex = null, ILevelManager levelManager = null)
        {
            _spatialIndex = spatialIndex ?? new SpatialGrid();
            _levelManager = levelManager ?? new LevelManager();
            _backgroundColor = SFML.Graphics.Color.Black;

            _levelManager.LoadLevels();
            _levelManager.LevelChanged += OnLevelChanged;
            _levelManager.AllLevelsCompleted += OnAllLevelsCompletedHandler;

            InitializeGame();
        }

        private void InitializeGame()
        {
            _levelManager.LoadLevel(1);
            LoadLevelData(_levelManager.CurrentLevelData);

            Score = 0;
            CurrentState = GameState.Menu;
            _lastUpdateTime = DateTime.Now;

            LevelChanged?.Invoke(CurrentLevel);
        }

        private void LoadLevelData(LevelData levelData)
        {
            float ballSpeed = levelData.BallSpeed > 0 ? levelData.BallSpeed : GameConstants.BALL_SPEED;
            float paddleSpeed = levelData.PaddleSpeed > 0 ? levelData.PaddleSpeed : GameConstants.PADDLE_SPEED;
            float ballRadius = levelData.BallRadius > 0 ? levelData.BallRadius : GameConstants.BALL_RADIUS;
            float paddleWidth = levelData.PaddleWidth > 0 ? levelData.PaddleWidth : GameConstants.PADDLE_WIDTH;
            Lives = levelData.InitialLives > 0 ? levelData.InitialLives : GameConstants.INITIAL_LIVES;

            if (levelData.BackgroundColor != null)
            {
                _backgroundColor = levelData.BackgroundColor.ToGameColor().ToSFMLColor();
            }
            else
            {
                _backgroundColor = SFML.Graphics.Color.Black;
            }

            Paddle = new Paddle(
                GameConstants.PADDLE_START_POSITION,
                new Vector2(paddleWidth, GameConstants.PADDLE_HEIGHT),
                paddleSpeed,
                GameConstants.FIELD_WIDTH
            );

            Ball = new Ball(
                new Vector2(Paddle.Position.X, Paddle.Position.Y - Paddle.Size.Y / 2 - ballRadius - 5),
                ballRadius,
                ballSpeed
            );
            Ball.IsSticky = true;
            Ball.Velocity = new Vector2(0, 0);

            CreateBlocksFromLayout(levelData);
        }

        private void CreateBlocksFromLayout(LevelData levelData)
        {
            Blocks = new List<Block>();
            _spatialIndex.Clear();

            if (levelData.BlockLayout == null || levelData.BlockLayout.Count == 0)
            {
                CreateDefaultBlocks();
                return;
            }

            int rows = levelData.BlockLayout.Count;
            int cols = levelData.BlockLayout[0].Length;

            float totalWidth = cols * GameConstants.BLOCK_WIDTH + (cols - 1) * levelData.BlockSpacing;
            float totalHeight = rows * GameConstants.BLOCK_HEIGHT + (rows - 1) * levelData.BlockSpacing;

            float startX = (GameConstants.FIELD_WIDTH - totalWidth) / 2;
            float startY = 50;

            for (int row = 0; row < rows; row++)
            {
                if (row >= levelData.BlockLayout.Count) break;

                string rowString = levelData.BlockLayout[row];
                if (rowString.Length < cols)
                {
                    rowString = rowString.PadRight(cols, '0');
                }

                for (int col = 0; col < cols; col++)
                {
                    char blockChar = rowString[col];

                    if (blockChar == '0' || blockChar == '_')
                        continue;

                    Vector2 position = new Vector2(
                        startX + col * (GameConstants.BLOCK_WIDTH + levelData.BlockSpacing),
                        startY + row * (GameConstants.BLOCK_HEIGHT + levelData.BlockSpacing)
                    );

                    var block = BlockFactory.CreateBlockFromChar(
                        blockChar,
                        position,
                        GameConstants.BLOCK_SIZE,
                        levelData.BlockSpacing
                    );

                    if (block != null)
                    {
                        Blocks.Add(block);
                        _spatialIndex.AddBlock(block);
                    }
                }
            }
        }

        private void CreateDefaultBlocks()
        {
            float startX = (GameConstants.FIELD_WIDTH -
                          (GameConstants.BLOCK_COLS *
                           (GameConstants.BLOCK_WIDTH + GameConstants.BLOCK_MARGIN))) / 2;
            float startY = 50;

            for (int row = 0; row < GameConstants.BLOCK_ROWS; row++)
            {
                for (int col = 0; col < GameConstants.BLOCK_COLS; col++)
                {
                    bool isUnbreakable = (row + col) % 3 == 0;
                    BlockType type = isUnbreakable ? BlockType.Unbreakable : BlockType.Normal;

                    int hitPoints;
                    if (isUnbreakable)
                    {
                        hitPoints = 1;
                    }
                    else
                    {
                        if (row == 0 || row == GameConstants.BLOCK_ROWS - 1) hitPoints = 1;
                        else if (row == 1 || row == GameConstants.BLOCK_ROWS - 2) hitPoints = 2;
                        else hitPoints = 3;
                    }

                    var block = new Block(
                        new Vector2(
                            startX + col * (GameConstants.BLOCK_WIDTH + GameConstants.BLOCK_MARGIN),
                            startY + row * (GameConstants.BLOCK_HEIGHT + GameConstants.BLOCK_MARGIN)
                        ),
                        GameConstants.BLOCK_SIZE,
                        type,
                        hitPoints
                    );

                    Blocks.Add(block);
                    _spatialIndex.AddBlock(block);
                }
            }
        }

        private void OnLevelChanged(int levelNumber)
        {
            
            LevelChanged?.Invoke(levelNumber);
        }

        private void OnAllLevelsCompletedHandler()
        {
            
            OnAllLevelsCompleted?.Invoke();
        }

        public void StartNewGame()
        {
            _levelManager.LoadLevel(1);
            LoadLevelData(_levelManager.CurrentLevelData);
            Score = 0;
            Lives = _levelManager.CurrentLevelData?.InitialLives ?? GameConstants.INITIAL_LIVES;

            CurrentState = GameState.Playing;
            InvokeGameStateChanged();
            InvokeScoreChanged();
            InvokeLivesChanged();
        }

        public void StartNextLevel()
        {
            if (_levelManager.NextLevel())
            {
                LoadLevelData(_levelManager.CurrentLevelData);
                ResetBall();

                CurrentState = GameState.Playing;
                InvokeGameStateChanged();
                InvokeLivesChanged();
            }
        }

        public void Pause()
        {
            if (CurrentState == GameState.Playing)
            {
                CurrentState = GameState.Menu;
                InvokeGameStateChanged();
            }
        }

        public void Resume()
        {
            if (CurrentState == GameState.Menu)
            {
                CurrentState = GameState.Playing;
                _lastUpdateTime = DateTime.Now;
                InvokeGameStateChanged();
            }
        }

        public void GameOver()
        {
            CurrentState = GameState.GameOver;
            InvokeGameStateChanged();
        }

        public void LevelComplete()
        {
            CurrentState = GameState.LevelComplete;
            InvokeLevelComplete();
            InvokeGameStateChanged();
        }

        public void TogglePause()
        {
            if (CurrentState == GameState.Playing)
                Pause();
            else if (CurrentState == GameState.Menu)
                Resume();
        }

        private void InvokeGameStateChanged()
        {
            StateChanged?.Invoke(CurrentState);
        }

        private void InvokeScoreChanged()
        {
            OnScoreChanged?.Invoke();
        }

        private void InvokeLivesChanged()
        {
            OnLivesChanged?.Invoke();
        }

        private void InvokeBallLost()
        {
            OnBallLost?.Invoke();
        }

        private void InvokeBlockDestroyed()
        {
            OnBlockDestroyed?.Invoke();
        }

        private void InvokeLevelComplete()
        {
            OnLevelComplete?.Invoke();
        }

        public void Update()
        {
            if (CurrentState != GameState.Playing)
                return;

            DateTime currentTime = DateTime.Now;
            float deltaTime = (float)(currentTime - _lastUpdateTime).TotalSeconds;
            _lastUpdateTime = currentTime;

            Ball.Update(deltaTime, FieldWidth, FieldHeight);

            if (Ball.Position.Y - Ball.Radius > FieldHeight + GameConstants.BALL_LOSS_MARGIN)
            {
                LoseLife();
                return;
            }

            if (Paddle.CheckCollision(Ball))
            {
                HandlePaddleCollision();
            }

            CheckBlockCollisions();

            CheckLevelCompletion();
        }

        private void HandlePaddleCollision()
        {
            if (Ball.IsSticky)
            {
                Ball.Position = new Vector2(
                    Ball.Position.X,
                    Paddle.Position.Y - Paddle.Size.Y / 2 - Ball.Radius - 5
                );
            }
            else
            {
                Paddle.HandleCollision(Ball);
                Ball.Position = new Vector2(
                    Ball.Position.X,
                    Paddle.Position.Y - Paddle.Size.Y / 2 - Ball.Radius - 5
                );
            }
        }

        private void CheckBlockCollisions()
        {
            var nearbyBlocks = _spatialIndex.GetNearbyBlocks(Ball.Position, Ball.Radius * 2).ToList();

            if (nearbyBlocks.Count == 0)
                return;

            nearbyBlocks = nearbyBlocks
                .Where(b => !b.IsDestroyed)
                .OrderBy(b => Vector2.Distance(b.Center, Ball.Position))
                .ToList();

            foreach (var block in nearbyBlocks)
            {
                if (block.IsDestroyed)
                    continue;

                if (block.CheckCollision(Ball))
                {
                    bool wasDestroyedBefore = block.IsDestroyed;
                    int hitPointsBefore = block.HitPoints;

                    HandleBallBlockCollision(block);
                    block.HandleCollision(Ball);

                    if (!wasDestroyedBefore && block.IsDestroyed)
                    {
                        int pointsToAdd = GameConstants.SCORE_PER_BLOCK * hitPointsBefore;
                        Score += pointsToAdd;

                        InvokeScoreChanged();
                        InvokeBlockDestroyed();
                    }

                    break;
                }
            }
        }

        private void HandleBallBlockCollision(Block block)
        {
            float ballLeft = Ball.Position.X - Ball.Radius;
            float ballRight = Ball.Position.X + Ball.Radius;
            float ballTop = Ball.Position.Y - Ball.Radius;
            float ballBottom = Ball.Position.Y + Ball.Radius;

            float blockLeft = block.Position.X;
            float blockRight = block.Position.X + block.Size.X;
            float blockTop = block.Position.Y;
            float blockBottom = block.Position.Y + block.Size.Y;

            // Вычисляем глубину проникновения с каждой стороны
            float overlapLeft = ballRight - blockLeft;
            float overlapRight = blockRight - ballLeft;
            float overlapTop = ballBottom - blockTop;
            float overlapBottom = blockBottom - ballTop;

            // Находим минимальное перекрытие
            float minOverlap = Math.Min(Math.Min(overlapLeft, overlapRight),
                                        Math.Min(overlapTop, overlapBottom));

            // Корректируем позицию мяча в зависимости от стороны столкновения
            if (minOverlap == overlapLeft)
            {
                Ball.Velocity = new Vector2(-Math.Abs(Ball.Velocity.X), Ball.Velocity.Y);
                Ball.Position = new Vector2(blockLeft - Ball.Radius - 0.1f, Ball.Position.Y);
            }
            else if (minOverlap == overlapRight)
            {
                Ball.Velocity = new Vector2(Math.Abs(Ball.Velocity.X), Ball.Velocity.Y);
                Ball.Position = new Vector2(blockRight + Ball.Radius + 0.1f, Ball.Position.Y);
            }
            else if (minOverlap == overlapTop)
            {
                Ball.Velocity = new Vector2(Ball.Velocity.X, -Math.Abs(Ball.Velocity.Y));
                Ball.Position = new Vector2(Ball.Position.X, blockTop - Ball.Radius - 0.1f);
            }
            else if (minOverlap == overlapBottom)
            {
                Ball.Velocity = new Vector2(Ball.Velocity.X, Math.Abs(Ball.Velocity.Y));
                Ball.Position = new Vector2(Ball.Position.X, blockBottom + Ball.Radius + 0.1f);
            }

            // Добавляем небольшой случайный компонент, чтобы избежать зацикливания
            float randomFactor = 1.02f + (float)(new Random().NextDouble() * 0.04f); // 1.02-1.06
            Ball.Velocity = Ball.Velocity * randomFactor;

            // Ограничиваем максимальную скорость
            float maxSpeed = Ball.Speed * 1.8f;
            if (Ball.Velocity.Length() > maxSpeed)
            {
                Ball.Velocity = Ball.Velocity.Normalized() * maxSpeed;
            }
        }

        private void CheckLevelCompletion()
        {
            bool allDestroyed = true;

            foreach (var block in Blocks)
            {
                if (!block.IsDestroyed && block.Type != BlockType.Unbreakable)
                {
                    allDestroyed = false;
                    break;
                }
            }

            if (allDestroyed)
            {
                LevelComplete();
            }
        }

        private void LoseLife()
        {
            Lives--;
            InvokeLivesChanged();
            InvokeBallLost();

            if (Lives <= 0)
            {
                GameOver();
            }
            else
            {
                ResetBall();
            }
        }

        private void ResetBall()
        {
            Ball.Position = new Vector2(Paddle.Position.X,
                Paddle.Position.Y - Paddle.Size.Y / 2 - Ball.Radius - 5);
            Ball.Velocity = new Vector2(0, 0);
            Ball.IsSticky = true;
        }

        private void MovePaddleAndStickyBall(float direction, float deltaTime)
        {
            Paddle.Move(direction, deltaTime);

            if (Ball.IsSticky)
            {
                Ball.Position = new Vector2(Paddle.Position.X, Ball.Position.Y);
            }
        }

        public void MovePaddleLeft(float deltaTime)
        {
            if (CurrentState == GameState.Playing)
            {
                MovePaddleAndStickyBall(-1, deltaTime);
            }
        }

        public void MovePaddleRight(float deltaTime)
        {
            if (CurrentState == GameState.Playing)
            {
                MovePaddleAndStickyBall(1, deltaTime);
            }
        }

        public void MovePaddleToMouse(float x)
        {
            if (CurrentState == GameState.Playing)
            {
                Paddle.MoveTo(x);

                if (Ball.IsSticky)
                {
                    Ball.Position = new Vector2(Paddle.Position.X, Ball.Position.Y);
                }
            }
        }

        public void LaunchBall()
        {
            if (CurrentState == GameState.Playing && Ball.IsSticky)
            {
                Ball.IsSticky = false;
                Ball.Launch();
            }
        }

        public void SaveGame(string filePath)
        {
            try
            {
                var saveData = CreateSaveData();

                var serializer = new XmlSerializer(typeof(GameSaveData));

                using (var writer = XmlWriter.Create(filePath, new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "  "
                }))
                {
                    serializer.Serialize(writer, saveData);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при сохранении игры: {ex.Message}", ex);
            }
        }

        public void LoadGame(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    throw new FileNotFoundException("Файл сохранения не найден");

                var serializer = new XmlSerializer(typeof(GameSaveData));

                using (var reader = XmlReader.Create(filePath))
                {
                    var saveData = (GameSaveData)serializer.Deserialize(reader);

                    if (saveData == null)
                        throw new Exception("Неверный формат файла сохранения");

                    LoadFromSaveData(saveData);
                    CurrentState = GameState.Menu;

                    InvokeGameStateChanged();
                    InvokeScoreChanged();
                    InvokeLivesChanged();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при загрузке игры: {ex.Message}", ex);
            }
        }

        private GameSaveData CreateSaveData()
        {
            var levelData = _levelManager.CurrentLevelData;

            return new GameSaveData
            {
                Score = Score,
                Lives = Lives,
                BallPosition = new Vector2Data(Ball.Position.X, Ball.Position.Y),
                BallVelocity = new Vector2Data(Ball.Velocity.X, Ball.Velocity.Y),
                PaddlePosition = new Vector2Data(Paddle.Position.X, Paddle.Position.Y),
                Blocks = Blocks.Select(b => new BlockSaveData
                {
                    Position = new Vector2Data(b.Position.X, b.Position.Y),
                    Type = b.Type.ToString(),
                    IsDestroyed = b.IsDestroyed,
                    HitPoints = b.HitPoints
                }).ToList(),
                SaveTime = DateTime.Now,
                CurrentLevel = CurrentLevel,
                // Сохраняем параметры уровня
                LevelBallSpeed = Ball.Speed,
                LevelPaddleSpeed = Paddle.Speed,
                LevelBallRadius = Ball.Radius,
                LevelPaddleWidth = Paddle.Size.X,
                LevelBackgroundColor = levelData?.BackgroundColor ?? new ColorData(0, 0, 0)
            };
        }

        private void LoadFromSaveData(GameSaveData saveData)
        {
            try
            {
                // 1. Сохраняем ТОЧНОЕ количество жизней из сохранения
                Lives = saveData.Lives;

                // 2. Сохраняем счет
                Score = saveData.Score;

                // 3. Загружаем уровень, но не переопределяем жизни
                if (saveData.CurrentLevel > 0)
                {
                    _levelManager.LoadLevel(saveData.CurrentLevel);
                }

                var levelData = _levelManager.CurrentLevelData;

                // 4. Восстанавливаем только параметры игры, но не жизни
                if (levelData != null)
                {
                    float ballSpeed = levelData.BallSpeed > 0 ? levelData.BallSpeed : GameConstants.BALL_SPEED;
                    float paddleSpeed = levelData.PaddleSpeed > 0 ? levelData.PaddleSpeed : GameConstants.PADDLE_SPEED;
                    float ballRadius = levelData.BallRadius > 0 ? levelData.BallRadius : GameConstants.BALL_RADIUS;
                    float paddleWidth = levelData.PaddleWidth > 0 ? levelData.PaddleWidth : GameConstants.PADDLE_WIDTH;

                    Ball.Speed = ballSpeed;
                    Ball.Radius = ballRadius;

                    // Создаем ракетку с параметрами уровня
                    Paddle = new Paddle(
                        new Vector2(saveData.PaddlePosition.X, saveData.PaddlePosition.Y),
                        new Vector2(paddleWidth, GameConstants.PADDLE_HEIGHT),
                        paddleSpeed,
                        GameConstants.FIELD_WIDTH
                    );

                    // Устанавливаем фон
                    if (levelData.BackgroundColor != null)
                    {
                        _backgroundColor = levelData.BackgroundColor.ToGameColor().ToSFMLColor();
                    }
                    else
                    {
                        _backgroundColor = SFML.Graphics.Color.Black;
                    }
                }
                else
                {
                    // Fallback на стандартные параметры
                    Paddle = new Paddle(
                        new Vector2(saveData.PaddlePosition.X, saveData.PaddlePosition.Y),
                        GameConstants.PADDLE_SIZE,
                        GameConstants.PADDLE_SPEED,
                        GameConstants.FIELD_WIDTH
                    );
                    _backgroundColor = SFML.Graphics.Color.Black;
                }

                // 5. Восстанавливаем состояние шарика
                Ball.Position = new Vector2(
                    saveData.BallPosition.X,
                    saveData.BallPosition.Y
                );
                Ball.Velocity = new Vector2(
                    saveData.BallVelocity.X,
                    saveData.BallVelocity.Y
                );
                Ball.IsSticky = Math.Abs(Ball.Velocity.X) < 0.01f && Math.Abs(Ball.Velocity.Y) < 0.01f;

                // 6. Восстанавливаем блоки
                Blocks.Clear();
                _spatialIndex.Clear();

                foreach (var blockData in saveData.Blocks)
                {
                    BlockType blockType;
                    if (!Enum.TryParse(blockData.Type, out blockType))
                    {
                        blockType = BlockType.Normal;
                    }

                    var block = new Block(
                        new Vector2(blockData.Position.X, blockData.Position.Y),
                        GameConstants.BLOCK_SIZE,
                        blockType,
                        blockData.HitPoints
                    )
                    {
                        IsDestroyed = blockData.IsDestroyed
                    };

                    Blocks.Add(block);
                    _spatialIndex.AddBlock(block);
                }

                
                CheckLevelCompletionFromSave();
            }
            catch (Exception ex)
            {
                // Пересоздаем игру с начальными параметрами при ошибке
                InitializeGame();
                throw new Exception($"Ошибка при загрузке игры: {ex.Message}", ex);
            }
        }

        // Вспомогательный метод для проверки завершения уровня после загрузки
        private void CheckLevelCompletionFromSave()
        {
            bool allDestroyed = true;

            foreach (var block in Blocks)
            {
                if (!block.IsDestroyed && block.Type != BlockType.Unbreakable)
                {
                    allDestroyed = false;
                    break;
                }
            }

            if (allDestroyed && CurrentState == GameState.Playing)
            {
                
                LevelComplete();
            }
        }

        public IEnumerable<Block> GetBlocks() => Blocks.AsReadOnly();
        public Vector2 GetPaddlePosition() => Paddle.Position;
        public Vector2 GetPaddleSize() => Paddle.Size;
        public Vector2 GetBallPosition() => Ball.Position;
        public float GetBallRadius() => Ball.Radius;
    }

    internal class SpatialGrid : ISpatialIndex
    {
        private readonly int _cellSize;
        private readonly Dictionary<(int, int), List<Block>> _grid;

        public SpatialGrid(int cellSize = 100)
        {
            _cellSize = cellSize;
            _grid = new Dictionary<(int, int), List<Block>>();
        }

        public void AddBlock(Block block)
        {
            var cell = GetCell(block.Position);
            if (!_grid.ContainsKey(cell))
                _grid[cell] = new List<Block>();

            _grid[cell].Add(block);
        }

        public IEnumerable<Block> GetNearbyBlocks(Vector2 position, float radius)
        {
            var centerCell = GetCell(position);
            int radiusInCells = (int)Math.Ceiling(radius / _cellSize);

            for (int dx = -radiusInCells; dx <= radiusInCells; dx++)
            {
                for (int dy = -radiusInCells; dy <= radiusInCells; dy++)
                {
                    var cell = (centerCell.Item1 + dx, centerCell.Item2 + dy);
                    if (_grid.TryGetValue(cell, out var blocks))
                    {
                        foreach (var block in blocks)
                            yield return block;
                    }
                }
            }
        }

        private (int, int) GetCell(Vector2 position)
        {
            return ((int)(position.X / _cellSize), (int)(position.Y / _cellSize));
        }

        public void Clear() => _grid.Clear();
    }
}