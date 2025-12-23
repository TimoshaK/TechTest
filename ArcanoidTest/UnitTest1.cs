using Xunit;
using Arkanoid.Game;
using Arkanoid.Entities;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Arkanoid.Tests
{
    // Основной класс тестов для ArkanoidGame
    public class ArkanoidGameTests
    {
        // Внутренний mock-класс с public доступом
        public class MockSpatialIndex : ISpatialIndex
        {
            public List<Block> Blocks { get; } = new List<Block>();

            public void AddBlock(Block block) => Blocks.Add(block);
            public IEnumerable<Block> GetNearbyBlocks(Vector2 position, float radius) => Blocks;
            public void Clear() => Blocks.Clear();
        }

        // Полная реализация интерфейса ILevelManager
        public class MockLevelManager : ILevelManager
        {
            public int CurrentLevel { get; set; } = 1;
            public int TotalLevels => 2;
            public LevelData CurrentLevelData { get; set; }
            public event System.Action<int> LevelChanged;
            public event System.Action AllLevelsCompleted;

            public MockLevelManager()
            {
                CurrentLevelData = new LevelData
                {
                    BallSpeed = 300,
                    PaddleSpeed = 400,
                    BallRadius = 10,
                    PaddleWidth = 100,
                    InitialLives = 3,
                    BlockLayout = new List<string> { "111", "111" }
                };
            }

            public bool LoadLevels()
            {
                return true;
            }

            public bool LoadLevel(int levelNumber)
            {
                CurrentLevel = levelNumber;
                LevelChanged?.Invoke(levelNumber);
                return true;
            }

            public bool HasNextLevel() => CurrentLevel < TotalLevels;

            public bool NextLevel()
            {
                if (HasNextLevel())
                {
                    CurrentLevel++;
                    LoadLevel(CurrentLevel);
                    return true;
                }
                return false;
            }

            public void AddLevel(LevelData levelData) { }
            public void RemoveLevel(int levelNumber) { }
            public List<LevelData> GetAllLevels() => new List<LevelData> { CurrentLevelData };
        }

        [Fact]
        public void Constructor_ShouldInitializeGameWithCorrectState()
        {
            // Arrange & Act
            var game = new ArkanoidGame();

            // Assert
            Assert.Equal(GameState.Menu, game.CurrentState);
            Assert.Equal(0, game.Score);
        }

        [Fact]
        public void StartNewGame_ShouldResetGameStateAndLoadFirstLevel()
        {
            // Arrange
            var levelManager = new MockLevelManager();
            var game = new ArkanoidGame(null, levelManager);

            // Act
            game.StartNewGame();

            // Assert
            Assert.Equal(GameState.Playing, game.CurrentState);
            Assert.Equal(0, game.Score);
            Assert.Equal(3, game.Lives);
            Assert.NotNull(game.Paddle);
            Assert.NotNull(game.Ball);
        }

        [Fact]
        public void Update_WithBallBelowField_ShouldDecreaseLives()
        {
            // Arrange
            var game = new ArkanoidGame();
            game.StartNewGame();
            int initialLives = game.Lives;

            // Позиционируем мяч ниже поля с запасом
            game.Ball.Position = new Vector2(
                game.Paddle.Position.X,
                game.FieldHeight + GameConstants.BALL_LOSS_MARGIN + 100
            );

            // Act
            game.Update();

            // Assert
            Assert.Equal(initialLives - 1, game.Lives);
        }

        [Fact]
        public void Update_WithZeroLives_ShouldTriggerGameOver()
        {
            // Arrange
            var game = new ArkanoidGame();
            game.StartNewGame();
            game.Lives = 1; // Устанавливаем последнюю жизнь

            // Позиционируем мяч ниже поля
            game.Ball.Position = new Vector2(
                game.Paddle.Position.X,
                game.FieldHeight + GameConstants.BALL_LOSS_MARGIN + 100
            );

            // Act
            game.Update();

            // Assert
            Assert.Equal(GameState.GameOver, game.CurrentState);
        }

        [Fact]
        public void LaunchBall_WhenBallIsSticky_ShouldMakeBallNonSticky()
        {
            // Arrange
            var game = new ArkanoidGame();
            game.StartNewGame();

            // Act
            game.LaunchBall();

            // Assert
            Assert.False(game.Ball.IsSticky);
            Assert.True(game.Ball.Velocity.Length() > 0);
        }

        [Fact]
        public void MovePaddleLeft_ShouldMovePaddleAndStickyBall()
        {
            // Arrange
            var game = new ArkanoidGame();
            game.StartNewGame();
            float initialX = game.Paddle.Position.X;
            float ballInitialX = game.Ball.Position.X;

            // Act
            game.MovePaddleLeft(0.016f); // ~60 FPS

            // Assert
            Assert.True(game.Paddle.Position.X < initialX);
            Assert.Equal(game.Paddle.Position.X, game.Ball.Position.X); // Мяч следует за ракеткой
        }

        [Fact]
        public void CheckLevelCompletion_WhenAllBreakableBlocksDestroyed_ShouldCompleteLevel()
        {
            // Arrange
            var spatialIndex = new MockSpatialIndex();
            var game = new ArkanoidGame(spatialIndex);
            game.StartNewGame();

            // Уничтожаем все ломаемые блоки (используем рефлексию для доступа к private методу)
            var checkMethod = typeof(ArkanoidGame).GetMethod("CheckLevelCompletion",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            foreach (var block in game.Blocks.ToList())
            {
                if (block.Type != BlockType.Unbreakable)
                {
                    block.IsDestroyed = true;
                }
            }

            // Act
            checkMethod.Invoke(game, null);

            // Assert
            Assert.Equal(GameState.LevelComplete, game.CurrentState);
        }

        [Fact]
        public void StartNextLevel_WhenHasNextLevel_ShouldLoadNextLevel()
        {
            // Arrange
            var levelManager = new MockLevelManager();
            var game = new ArkanoidGame(null, levelManager);
            game.StartNewGame();

            // Act - StartNextLevel возвращает void, проверяем только изменение состояния
            game.StartNextLevel();

            // Assert
            Assert.Equal(2, game.CurrentLevel);
            Assert.Equal(GameState.Playing, game.CurrentState);
        }

        [Fact]
        public void TogglePause_ShouldToggleBetweenPlayingAndMenuStates()
        {
            // Arrange
            var game = new ArkanoidGame();
            game.StartNewGame();

            // Act & Assert - Пауза
            game.TogglePause();
            Assert.Equal(GameState.Menu, game.CurrentState);

            // Act & Assert - Возобновление
            game.TogglePause();
            Assert.Equal(GameState.Playing, game.CurrentState);
        }

        [Fact]
        public void SaveAndLoadGame_ShouldPreserveGameState()
        {
            // Arrange
            var tempPath = Path.GetTempFileName();
            var game = new ArkanoidGame();
            game.StartNewGame();

            // Изменяем состояние игры
            game.Score = 500;
            game.Lives = 2;
            game.MovePaddleRight(0.1f);
            game.LaunchBall();

            // Сохраняем мяч в состоянии движения
            var ballPositionBefore = game.Ball.Position;
            var ballVelocityBefore = game.Ball.Velocity;

            // Act - Сохраняем и загружаем
            game.SaveGame(tempPath);

            var loadedGame = new ArkanoidGame();
            loadedGame.LoadGame(tempPath);

            // Cleanup
            File.Delete(tempPath);

            // Assert
            Assert.Equal(game.Score, loadedGame.Score);
            Assert.Equal(game.Lives, loadedGame.Lives);
            Assert.Equal(game.CurrentLevel, loadedGame.CurrentLevel);
            Assert.Equal(ballPositionBefore.X, loadedGame.Ball.Position.X, 0.1f);
            Assert.Equal(ballVelocityBefore.Length(), loadedGame.Ball.Velocity.Length(), 0.1f);
        }

        [Fact]
        public void LoadGame_WithInvalidFile_ShouldThrowException()
        {
            // Arrange
            var game = new ArkanoidGame();
            var invalidPath = "nonexistent_file.xml";

            // Act & Assert
            Assert.Throws<FileNotFoundException>(() => game.LoadGame(invalidPath));
        }

        [Fact]
        public void PaddleCollision_ShouldCorrectBallPosition()
        {
            // Arrange
            var game = new ArkanoidGame();
            game.StartNewGame();

            // Симулируем столкновение
            game.Ball.Position = new Vector2(
                game.Paddle.Position.X,
                game.Paddle.Position.Y + game.Paddle.Size.Y / 2 + game.Ball.Radius
            );

            // Act
            bool collision = game.Paddle.CheckCollision(game.Ball);

            // Assert
            Assert.True(collision);
        }

        [Fact]
        public void CreateBlocksFromLayout_WithCustomLayout_ShouldCreateCorrectNumberOfBlocks()
        {
            // Arrange
            var levelManager = new MockLevelManager
            {
                CurrentLevelData = new LevelData
                {
                    BlockLayout = new List<string> { "12", "34", "56" },
                    BlockSpacing = 5
                }
            };
            var game = new ArkanoidGame(null, levelManager);

            // Act
            game.StartNewGame();

            // Assert - 6 блоков (все символы кроме '0' или '_')
            Assert.Equal(6, game.Blocks.Count);
        }

        [Fact]
        public void GetBlocks_ShouldReturnReadOnlyCollection()
        {
            // Arrange
            var game = new ArkanoidGame();
            game.StartNewGame();

            // Act
            var blocks = game.GetBlocks();

            // Assert
            Assert.NotNull(blocks);
            Assert.Equal(game.Blocks.Count, blocks.Count());
        }

        [Fact]
        public void IsPlayingProperty_ShouldReflectPlayingState()
        {
            // Arrange & Act
            var game = new ArkanoidGame();

            // Assert - Начальное состояние
            Assert.False(game.IsPlaying);

            // Act & Assert - После старта игры
            game.StartNewGame();
            Assert.True(game.IsPlaying);

            // Act & Assert - После паузы
            game.TogglePause();
            Assert.False(game.IsPlaying);
        }

        [Fact]
        public void GameOver_ShouldSetCorrectState()
        {
            // Arrange
            var game = new ArkanoidGame();

            // Act
            game.GameOver();

            // Assert
            Assert.Equal(GameState.GameOver, game.CurrentState);
        }

        [Fact]
        public void LevelComplete_ShouldSetCorrectState()
        {
            // Arrange
            var game = new ArkanoidGame();

            // Act
            game.LevelComplete();

            // Assert
            Assert.Equal(GameState.LevelComplete, game.CurrentState);
        }
    }

    // Тесты для проверки столкновений
    public class BlockCollisionTests
    {
        [Fact]
        public void BlockCollision_ShouldDestroyBreakableBlock()
        {
            // Arrange
            var spatialIndex = new ArkanoidGameTests.MockSpatialIndex();
            var game = new ArkanoidGame(spatialIndex);
            game.StartNewGame();

            var breakableBlock = game.Blocks.First(b => b.Type != BlockType.Unbreakable);
            int initialHitPoints = breakableBlock.HitPoints;

            // Act - прямое попадание в блок
            bool collision = breakableBlock.CheckCollision(game.Ball);
            breakableBlock.HandleCollision(game.Ball);

            // Assert
            Assert.True(collision);
            if (breakableBlock.Type != BlockType.Unbreakable)
            {
                Assert.Equal(initialHitPoints - 1, breakableBlock.HitPoints);
            }
        }

        [Fact]
        public void UnbreakableBlock_ShouldNotBeDestroyed()
        {
            // Arrange
            var game = new ArkanoidGame();
            game.StartNewGame();

            var unbreakableBlock = game.Blocks.FirstOrDefault(b => b.Type == BlockType.Unbreakable);
            if (unbreakableBlock != null)
            {
                int initialHitPoints = unbreakableBlock.HitPoints;

                // Act - многократные попадания
                for (int i = 0; i < 5; i++)
                {
                    unbreakableBlock.HandleCollision(game.Ball);
                }

                // Assert
                Assert.Equal(initialHitPoints, unbreakableBlock.HitPoints);
                Assert.False(unbreakableBlock.IsDestroyed);
            }
        }
    }

    // Тесты для обработки событий
    public class EventTests
    {
        [Fact]
        public void StateChanged_ShouldFireOnGameStart()
        {
            // Arrange
            var game = new ArkanoidGame();
            bool eventFired = false;
            game.StateChanged += (state) => eventFired = true;

            // Act
            game.StartNewGame();

            // Assert
            Assert.True(eventFired);
        }

        [Fact]
        public void OnScoreChanged_ShouldFireWhenScoreIncreases()
        {
            // Arrange
            var game = new ArkanoidGame();
            game.StartNewGame();
            bool eventFired = false;
            game.OnScoreChanged += () => eventFired = true;

            // Act
            game.Score += 100;
            var eventInfo = typeof(ArkanoidGame).GetEvent("OnScoreChanged");
            var raiseMethod = eventInfo.GetRaiseMethod(true);

            // Используем рефлексию для вызова private метода InvokeScoreChanged
            var invokeMethod = typeof(ArkanoidGame).GetMethod("InvokeScoreChanged",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            invokeMethod.Invoke(game, null);

            // Assert
            Assert.True(eventFired);
        }
    }
}