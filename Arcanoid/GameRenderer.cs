using System;
using SFML.Graphics;
using SFML.System;
using Arkanoid.Game;
using Arkanoid.Entities;

namespace Arkanoid
{
    public class GameRenderer : IDisposable
    {
        private readonly IGameStateManager _gameState;
        private readonly ArkanoidGame _game;
        private readonly Font _font;
        private readonly Text _scoreText;
        private readonly Text _livesText;
        private readonly Text _levelText;
        private readonly Text _stateText;
        public GameRenderer(ArkanoidGame game)
        {
            _gameState = game;
            _game = game;

            _font = LoadSystemFont();

            _scoreText = new Text("Score: 0", _font, 20);
            _scoreText.Position = new Vector2f(10, 10);
            _scoreText.FillColor = Color.White;

            _livesText = new Text("Lives: 3", _font, 20);
            _livesText.Position = new Vector2f(200, 10);
            _livesText.FillColor = Color.White;

            _levelText = new Text("Level: 1", _font, 20);
            _levelText.Position = new Vector2f(350, 10);
            _levelText.FillColor = Color.White;

            _stateText = new Text("", _font, 30);
            _stateText.FillColor = Color.Yellow;
            _stateText.Style = Text.Styles.Bold;

            game.OnScoreChanged += UpdateScoreText;
            game.OnLivesChanged += UpdateLivesText;
            game.LevelChanged += UpdateLevelText;
            game.StateChanged += UpdateStateText;

            UpdateScoreText();
            UpdateLivesText();
            UpdateLevelText(1);
            UpdateStateText(GameState.Menu);
        }

        private Font LoadSystemFont()
        {
            string[] fontPaths =
            {
                "arial.ttf",
                "C:\\Windows\\Fonts\\arial.ttf",
                "C:\\Windows\\Fonts\\calibri.ttf",
                "/usr/share/fonts/truetype/arial.ttf",
                "fonts/arial.ttf"
            };

            foreach (var path in fontPaths)
            {
                try
                {
                    if (System.IO.File.Exists(path))
                    {
                        return new Font(path);
                    }
                }
                catch
                {
                    // Продолжаем пробовать другие пути
                }
            }

            
            return CreateFallbackFont();
        }

        private Font CreateFallbackFont()
        {
            try
            {
                return new Font("arial.ttf");
            }
            catch
            {
                throw new InvalidOperationException("Не удалось загрузить шрифт. Поместите файл arial.ttf в папку с игрой.");
            }
        }

        public void Render(RenderWindow window)
        {
            window.Clear(_game.BackgroundColor);

            RenderBlocks(window);
            RenderPaddle(window);
            RenderBall(window);
            RenderUI(window);

            if (_gameState.IsMenuVisible)
            {
                RenderMenu(window);
            }
        }

        private void RenderBlocks(RenderWindow window)
        {
            foreach (var block in _game.GetBlocks())
            {
                if (block.IsDestroyed)
                    continue;

                var renderData = block.GetRenderData();
                var gameColor = renderData.Color;
                var sfmlColor = new Color(gameColor.R, gameColor.G, gameColor.B, gameColor.A);

                var rectangle = new RectangleShape(new Vector2f(renderData.Size.X, renderData.Size.Y))
                {
                    Position = new Vector2f(renderData.Position.X, renderData.Position.Y),
                    FillColor = sfmlColor
                };

                if (renderData.HasOutline)
                {
                    rectangle.OutlineColor = Color.White;
                    rectangle.OutlineThickness = 1;
                }

                window.Draw(rectangle);

                if (!string.IsNullOrEmpty(renderData.Text))
                {
                    var hpText = new Text(renderData.Text, _font, 12);
                    hpText.FillColor = Color.Black;
                    hpText.Style = Text.Styles.Bold;
                    hpText.Position = new Vector2f(
                        renderData.Position.X + renderData.Size.X / 2 - 5,
                        renderData.Position.Y + renderData.Size.Y / 2 - 8
                    );
                    window.Draw(hpText);
                }
            }
        }

        private void RenderPaddle(RenderWindow window)
        {
            var paddlePos = _game.GetPaddlePosition();
            var paddleSize = _game.GetPaddleSize();

            var rectangle = new RectangleShape(new Vector2f(paddleSize.X, paddleSize.Y))
            {
                Position = new Vector2f(paddlePos.X - paddleSize.X / 2, paddlePos.Y - paddleSize.Y / 2),
                FillColor = new Color(0, 128, 255),
                OutlineColor = Color.White,
                OutlineThickness = 2
            };

            window.Draw(rectangle);
        }

        private void RenderBall(RenderWindow window)
        {
            var ballPos = _game.GetBallPosition();
            float radius = _game.GetBallRadius();

            var circle = new CircleShape(radius)
            {
                Position = new Vector2f(ballPos.X - radius, ballPos.Y - radius),
                FillColor = Color.White,
                OutlineColor = Color.Yellow,
                OutlineThickness = 1
            };

            window.Draw(circle);
        }

        private void RenderUI(RenderWindow window)
        {
            window.Draw(_scoreText);
            window.Draw(_livesText);
            window.Draw(_levelText);

            if (_gameState.CurrentState == GameState.Paused)
            {
                window.Draw(_stateText);
            }
        }

        private void RenderMenu(RenderWindow window)
        {
            var menuBackground = new RectangleShape(new Vector2f(400, 400))
            {
                Position = new Vector2f(200, 125),
                FillColor = new Color(0, 0, 0, 200),
                OutlineColor = Color.White,
                OutlineThickness = 2
            };
            window.Draw(menuBackground);

            string titleTextStr = "";
            Color titleColor = Color.Yellow;

            if (_gameState.CurrentState == GameState.GameOver)
            {
                titleTextStr = "ВЫ ПРОИГРАЛИ";
                titleColor = Color.Red;
            }
            else if (_gameState.CurrentState == GameState.LevelComplete)
            {
                if (_game.AllLevelsCompleted)
                {
                    titleTextStr = "ВЫ ПРОШЛИ ИГРУ!";
                    titleColor = new Color(0, 255, 255); // Cyan
                }
                else
                {
                    titleTextStr = "ВЫ ПРОШЛИ УРОВЕНЬ";
                    titleColor = Color.Green;
                }
            }
            else
            {
                titleTextStr = "ARKANOID";
                titleColor = Color.Yellow;
            }

            var titleText = new Text(titleTextStr, _font, 40);
            titleText.FillColor = titleColor;
            titleText.Style = Text.Styles.Bold;

            FloatRect titleBounds = titleText.GetLocalBounds();
            titleText.Origin = new Vector2f(titleBounds.Width / 2, titleBounds.Height / 2);
            titleText.Position = new Vector2f(400, 150);

            window.Draw(titleText);

            var levelInfoText = new Text($"Уровень {_game.CurrentLevel}: {_game.CurrentLevelName}", _font, 24);
            levelInfoText.FillColor = Color.Cyan;
            FloatRect levelInfoBounds = levelInfoText.GetLocalBounds();
            levelInfoText.Origin = new Vector2f(levelInfoBounds.Width / 2, levelInfoBounds.Height / 2);
            levelInfoText.Position = new Vector2f(400, 200);
            window.Draw(levelInfoText);

            var scoreText = new Text($"Счет: {_game.Score}", _font, 24);
            scoreText.FillColor = Color.White;
            FloatRect scoreBounds = scoreText.GetLocalBounds();
            scoreText.Origin = new Vector2f(scoreBounds.Width / 2, scoreBounds.Height / 2);
            scoreText.Position = new Vector2f(400, 240);
            window.Draw(scoreText);

            if (_gameState.CurrentState == GameState.GameOver)
            {
                // Меню проигрыша: 2 кнопки
                RenderMenuButton(window, "НОВАЯ ИГРА", new Vector2f(400, 300));
                RenderMenuButton(window, "ВЫХОД", new Vector2f(400, 370));
            }
            else if (_gameState.CurrentState == GameState.LevelComplete)
            {
                if (_game.AllLevelsCompleted)
                {
                    // Все уровни пройдены: 2 кнопки
                    RenderMenuButton(window, "НОВАЯ ИГРА", new Vector2f(400, 300));
                    RenderMenuButton(window, "ВЫХОД", new Vector2f(400, 370));
                }
                else
                {
                    // Уровень пройден (но есть еще уровни): 3 кнопки
                    RenderMenuButton(window, "СЛЕДУЮЩИЙ УРОВЕНЬ", new Vector2f(400, 300));
                    RenderMenuButton(window, "НОВАЯ ИГРА", new Vector2f(400, 370));
                    RenderMenuButton(window, "ВЫХОД", new Vector2f(400, 440));
                }
            }
            else
            {
                // Главное меню (при запуске или паузе): 2 кнопки
                RenderMenuButton(window, "НОВАЯ ИГРА", new Vector2f(400, 300));
                RenderMenuButton(window, "ВЫХОД", new Vector2f(400, 370));
            }

            if (_gameState.CurrentState == GameState.Menu)
            {
                var escHint = new Text("ESC - продолжить/пауза", _font, 16);
                escHint.Position = new Vector2f(10, 570);
                escHint.FillColor = new Color(200, 200, 200);
                window.Draw(escHint);

                var saveHint = new Text("Ctrl+S - сохранить, Ctrl+L - загрузить", _font, 14);
                saveHint.Position = new Vector2f(10, 550);
                saveHint.FillColor = new Color(200, 200, 200);
                window.Draw(saveHint);
            }
        }

        private void RenderMenuButton(RenderWindow window, string text, Vector2f center)
        {
            var buttonBackground = new RectangleShape(new Vector2f(MenuConstants.ButtonWidth, MenuConstants.ButtonHeight))
            {
                Position = new Vector2f(center.X - MenuConstants.ButtonWidth / 2, center.Y - MenuConstants.ButtonHeight / 2),
                FillColor = new Color(64, 64, 128),
                OutlineColor = Color.White,
                OutlineThickness = 2
            };
            window.Draw(buttonBackground);

            var buttonText = new Text(text, _font, 20);
            buttonText.FillColor = Color.White;

            FloatRect textBounds = buttonText.GetLocalBounds();
            buttonText.Origin = new Vector2f(textBounds.Width / 2, textBounds.Height / 2);
            buttonText.Position = center;

            window.Draw(buttonText);
        }

        private void UpdateScoreText()
        {
            _scoreText.DisplayedString = $"SCORE: {_game.Score}";
        }

        private void UpdateLivesText()
        {
            _livesText.DisplayedString = $"LIVES: {_game.Lives}";
        }

        private void UpdateLevelText(int level)
        {
            _levelText.DisplayedString = $"LEVEL: {level}";
        }

        private void UpdateStateText(GameState state)
        {
            switch (state)
            {
                case GameState.Paused:
                    _stateText.DisplayedString = "ПАУЗА";
                    break;
                default:
                    _stateText.DisplayedString = "";
                    break;
            }

            if (!string.IsNullOrEmpty(_stateText.DisplayedString))
            {
                FloatRect bounds = _stateText.GetLocalBounds();
                _stateText.Origin = new Vector2f(bounds.Width / 2, bounds.Height / 2);
                _stateText.Position = new Vector2f(GameConstants.FIELD_WIDTH / 2, GameConstants.FIELD_HEIGHT / 2);
            }
        }

        public void Dispose()
        {
            _font?.Dispose();
            _scoreText?.Dispose();
            _livesText?.Dispose();
            _levelText?.Dispose();
            _stateText?.Dispose();
        }
    }
}