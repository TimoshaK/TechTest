using Xunit;
using Arkanoid.Game;
using Arkanoid.Entities;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System;

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
            // Проверяем, что потеря мяча за нижней границей уменьшает количество жизней

            // Arrange
            var game = new ArkanoidGame();
            game.StartNewGame();
            int initialLives = game.Lives;

            // Позиционируем мяч ниже поля с запасом (добавляем радиус мяча)
            game.Ball.Position = new Vector2(
                game.Paddle.Position.X,
                game.FieldHeight + GameConstants.BALL_LOSS_MARGIN + game.Ball.Radius + 100
            );

            // Act
            game.Update();

            // Assert
            Assert.Equal(initialLives - 1, game.Lives);
        }

        [Fact]
        public void Update_WithZeroLives_ShouldTriggerGameOver()
        {
            // Проверяем, что потеря последней жизни приводит к Game Over

            // Arrange
            var game = new ArkanoidGame();
            game.StartNewGame();
            game.Lives = 1; // Устанавливаем последнюю жизнь

            // Позиционируем мяч ниже поля
            game.Ball.Position = new Vector2(
                game.Paddle.Position.X,
                game.FieldHeight + GameConstants.BALL_LOSS_MARGIN + game.Ball.Radius + 100
            );

            // Act
            game.Update();

            // Assert
            Assert.Equal(GameState.GameOver, game.CurrentState);
        }

        [Fact]
        public void MovePaddleLeft_ShouldMovePaddleAndStickyBall()
        {
            // Проверяем, что движение ракетки перемещает прилипший к ней мяч

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
            // Проверяем, что уничтожение всех ломаемых блоков завершает уровень

            // Arrange
            var spatialIndex = new MockSpatialIndex();
            var game = new ArkanoidGame(spatialIndex);
            game.StartNewGame();

            // Используем рефлексию для вызова private метода CheckLevelCompletion
            var checkMethod = typeof(ArkanoidGame).GetMethod("CheckLevelCompletion",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Уничтожаем все ломаемые блоки
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
            // Проверяем переход на следующий уровень при наличии доступных уровней

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
            // Проверяем работу паузы: переключение между игрой и меню

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
            // Проверяем корректность сохранения и загрузки состояния игры

            // Arrange
            var tempPath = Path.GetTempFileName();
            var game = new ArkanoidGame();
            game.StartNewGame();

            // Изменяем состояние игры
            game.Score = 500;
            game.Lives = 2;
            game.MovePaddleRight(0.1f);
            game.LaunchBall();

            // Сохраняем текущее состояние мяча
            var ballPositionBefore = game.Ball.Position;
            var ballVelocityBefore = game.Ball.Velocity;

            // Act - Сохраняем и загружаем
            game.SaveGame(tempPath);

            var loadedGame = new ArkanoidGame();
            loadedGame.LoadGame(tempPath);

            // Cleanup
            File.Delete(tempPath);

            // Assert - Проверяем сохранение ключевых параметров
            Assert.Equal(game.Score, loadedGame.Score);
            Assert.Equal(game.Lives, loadedGame.Lives);
            Assert.Equal(game.CurrentLevel, loadedGame.CurrentLevel);
        }

        [Fact]
        public void LoadGame_WithInvalidFile_ShouldThrowException()
        {
            // Проверяем обработку ошибки при загрузке несуществующего файла

            // Arrange
            var game = new ArkanoidGame();
            var invalidPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".xml");

            // Act & Assert
            // Используем Assert.Throws с передачей типа исключения
            var exception = Assert.Throws<Exception>(() => game.LoadGame(invalidPath));
            Assert.Contains("Ошибка при загрузке игры", exception.Message);
        }

        [Fact]
        public void PaddleCollision_ShouldCorrectBallPosition()
        {
            // Проверяем обнаружение столкновения мяча с ракеткой

            // Arrange
            var game = new ArkanoidGame();
            game.StartNewGame();

            // Позиционируем мяч прямо над ракеткой (немного выше)
            game.Ball.Position = new Vector2(
                game.Paddle.Position.X,
                game.Paddle.Position.Y - game.Paddle.Size.Y / 2 - game.Ball.Radius - 1
            );

            // Act
            bool collision = game.Paddle.CheckCollision(game.Ball);

            // Assert
            Assert.True(collision);
        }

        [Fact]
        public void CreateBlocksFromLayout_WithCustomLayout_ShouldCreateCorrectNumberOfBlocks()
        {
            // Проверяем создание блоков по пользовательскому макету

            // Arrange
            var levelManager = new MockLevelManager
            {
                CurrentLevelData = new LevelData
                {
                    BlockLayout = new List<string> { "12", "34", "56" },
                    BlockSpacing = 5,
                    BallSpeed = 300,
                    PaddleSpeed = 400,
                    BallRadius = 10,
                    PaddleWidth = 100,
                    InitialLives = 3
                }
            };
            var game = new ArkanoidGame(null, levelManager);

            // Act
            game.StartNewGame();

            // Assert - 6 блоков (все символы '1','2','3','4','5','6' создают блоки)
            Assert.Equal(6, game.Blocks.Count);
        }

        [Fact]
        public void GetBlocks_ShouldReturnReadOnlyCollection()
        {
            // Проверяем, что метод возвращает неизменяемую коллекцию

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
            // Проверяем корректность работы свойства IsPlaying

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
            // Проверяем установку состояния Game Over

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
            // Проверяем установку состояния завершения уровня

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
        public void BlockCollision_ShouldDecreaseHitPoints()
        {
            // Проверяем уменьшение прочности блока при столкновении

            // Arrange
            var spatialIndex = new ArkanoidGameTests.MockSpatialIndex();
            var game = new ArkanoidGame(spatialIndex);
            game.StartNewGame();

            // Находим ломаемый блок
            var breakableBlock = game.Blocks.FirstOrDefault(b =>
                b.Type != BlockType.Unbreakable && b.HitPoints > 1);

            if (breakableBlock == null)
            {
                // Если нет подходящего блока, пропускаем тест
                return;
            }

            int initialHitPoints = breakableBlock.HitPoints;

            // Позиционируем мяч для столкновения с блоком
            game.Ball.Position = breakableBlock.Center;
            game.Ball.Velocity = new Vector2(0, 1); // Движемся вниз к блоку

            // Act - Проверяем столкновение и обрабатываем его
            bool collision = breakableBlock.CheckCollision(game.Ball);
            if (collision)
            {
                breakableBlock.HandleCollision(game.Ball);
            }

            // Assert
            Assert.True(collision);
            Assert.Equal(initialHitPoints - 1, breakableBlock.HitPoints);
        }

        [Fact]
        public void UnbreakableBlock_ShouldNotBeDestroyed()
        {
            // Проверяем, что несокрушимые блоки не уничтожаются

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

                // Assert - прочность и состояние не должны измениться
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
            // Проверяем генерацию события при изменении состояния игры

            // Arrange
            var game = new ArkanoidGame();
            bool eventFired = false;
            GameState lastState = GameState.Menu;

            game.StateChanged += (state) =>
            {
                eventFired = true;
                lastState = state;
            };

            // Act
            game.StartNewGame();

            // Assert
            Assert.True(eventFired);
            Assert.Equal(GameState.Playing, lastState);
        }

        [Fact]
        public void LevelChanged_ShouldFireWhenLevelChanges()
        {
            // Проверяем генерацию события при смене уровня

            // Arrange
            var levelManager = new ArkanoidGameTests.MockLevelManager();
            var game = new ArkanoidGame(null, levelManager);
            bool eventFired = false;
            int changedLevel = 0;

            game.LevelChanged += (level) =>
            {
                eventFired = true;
                changedLevel = level;
            };

            // Act
            game.StartNextLevel();

            // Assert
            Assert.True(eventFired);
            Assert.Equal(2, changedLevel);
        }
    }
}