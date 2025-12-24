using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using Arkanoid.Game;
using Arkanoid.Entities;

namespace Arkanoid
{
    public class GameWindow : IDisposable
    {
        private readonly RenderWindow _window;
        private readonly ArkanoidGame _game;
        private readonly GameInputHandler _inputHandler;
        private readonly GameRenderer _renderer;
        private readonly Clock _clock;

        private bool _leftPressed;
        private bool _rightPressed;
        private bool _spacePressed;

        public GameWindow()
        {
            var videoMode = new VideoMode((uint)GameConstants.FIELD_WIDTH, (uint)GameConstants.FIELD_HEIGHT);
            _window = new RenderWindow(videoMode, "Arkanoid");

            _game = new ArkanoidGame();
            _inputHandler = new GameInputHandler(_game);
            _renderer = new GameRenderer(_game);
            _clock = new Clock();

            SetupEventHandlers();
        }

        private void SetupEventHandlers()
        {
            _window.Closed += (sender, e) => _window.Close();

            _window.KeyPressed += (sender, e) =>
            {
                switch (e.Code)
                {
                    case Keyboard.Key.Left:
                        _leftPressed = true;
                        break;
                    case Keyboard.Key.Right:
                        _rightPressed = true;
                        break;
                    case Keyboard.Key.Space:
                        _spacePressed = true;
                        break;
                    case Keyboard.Key.Escape:
                        _game.TogglePause();
                        break;
                    case Keyboard.Key.S:
                        if (Keyboard.IsKeyPressed(Keyboard.Key.LControl) ||
                            Keyboard.IsKeyPressed(Keyboard.Key.RControl))
                        {
                            _game.SaveGame("save.xml");
                            
                        }
                        break;
                    case Keyboard.Key.L:
                        if (Keyboard.IsKeyPressed(Keyboard.Key.LControl) ||
                            Keyboard.IsKeyPressed(Keyboard.Key.RControl))
                        {
                            try
                            {
                                _game.LoadGame("save.xml");
                                
                            }
                            catch (Exception ex)
                            {
                                
                            }
                        }
                        break;
                }
            };

            _window.KeyReleased += (sender, e) =>
            {
                switch (e.Code)
                {
                    case Keyboard.Key.Left:
                        _leftPressed = false;
                        break;
                    case Keyboard.Key.Right:
                        _rightPressed = false;
                        break;
                    case Keyboard.Key.Space:
                        _spacePressed = false;
                        break;
                }
            };

            _window.MouseMoved += (sender, e) =>
            {
                _inputHandler.ProcessMouseInput(e.X);
            };

            _window.MouseButtonPressed += (sender, e) =>
            {
                if (e.Button == Mouse.Button.Left)
                {
                    _inputHandler.ProcessMenuInput(e.X, e.Y, true);
                }
            };
        }

        public void Run()
        {
            _window.SetFramerateLimit(60);

            while (_window.IsOpen)
            {
                ProcessEvents();
                Update();
                Render();
            }
        }

        private void ProcessEvents()
        {
            _window.DispatchEvents();
        }

        private void Update()
        {
            float deltaTime = _clock.Restart().AsSeconds();

            _inputHandler.ProcessKeyboardInput(_leftPressed, _rightPressed, _spacePressed, deltaTime);
            _game.Update();
        }

        private void Render()
        {
            _window.Clear(Color.Black);
            _renderer.Render(_window);
            _window.Display();
        }

        public void Dispose()
        {
            _window?.Dispose();
        }
    }
}