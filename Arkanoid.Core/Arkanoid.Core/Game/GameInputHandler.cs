using Arkanoid.Core.Entities;
using System;

namespace Arkanoid.Core.Game
{
    public class GameInputHandler
    {
        private readonly ArkanoidGame _game;




        public GameInputHandler(ArkanoidGame game)
        {
            _game = game;
        }

        public void ProcessKeyboardInput(bool leftPressed, bool rightPressed, bool spacePressed, float deltaTime)//обработчик кнопок
        {
            if (!_game.IsPlaying)
                return;

            if (leftPressed)
            {
                _game.MovePaddleLeft(deltaTime);
            }

            if (rightPressed)
            {
                _game.MovePaddleRight(deltaTime);
            }

            if (spacePressed)
            {
                _game.LaunchBall();
            }
        }

        public void ProcessMouseInput(float mouseX)
        {
            if (_game.IsPlaying)
            {
                _game.MovePaddleToMouse(mouseX);
            }
        }

        public void ProcessMenuInput(float mouseX, float mouseY, bool mouseClick)
        {
            if (!mouseClick || !_game.IsMenuVisible)
                return;

            switch (_game.CurrentState)
            {
                case GameState.GameOver:
                    HandleGameOverMenu(mouseX, mouseY);
                    break;
                case GameState.LevelComplete:
                    HandleLevelCompleteMenu(mouseX, mouseY);
                    break;
                case GameState.Menu:
                    HandleMainMenu(mouseX, mouseY);
                    break;
            }
        }

        private void HandleGameOverMenu(float mouseX, float mouseY)
        {
            if (MenuConstants.IsPointInButton(mouseX, mouseY, MenuConstants.NewGameButtonPosition))
            {
                _game.StartNewGame();
            }
            else if (MenuConstants.IsPointInButton(mouseX, mouseY, MenuConstants.ExitButtonPosition))
            {
                Environment.Exit(0);
            }
        }

        private void HandleLevelCompleteMenu(float mouseX, float mouseY)
        {
            if (_game.AllLevelsCompleted)
            {
                HandleAllLevelsCompleteMenu(mouseX, mouseY);
            }
            else
            {
                HandleSingleLevelCompleteMenu(mouseX, mouseY);
            }
        }

        private void HandleAllLevelsCompleteMenu(float mouseX, float mouseY)
        {
            if (MenuConstants.IsPointInButton(mouseX, mouseY, MenuConstants.NewGameButtonPosition))
            {
                _game.StartNewGame();
            }
            else if (MenuConstants.IsPointInButton(mouseX, mouseY, MenuConstants.ExitButtonPosition))
            {
                Environment.Exit(0);
            }
        }

        private void HandleSingleLevelCompleteMenu(float mouseX, float mouseY)
        {
            if (MenuConstants.IsPointInButton(mouseX, mouseY, MenuConstants.NextLevelButtonPosition))
            {
                _game.StartNextLevel();
            }
            else if (MenuConstants.IsPointInButton(mouseX, mouseY, MenuConstants.NewGameAfterLevelPosition))
            {
                _game.StartNewGame();
            }
            else if (MenuConstants.IsPointInButton(mouseX, mouseY, MenuConstants.ExitAfterLevelPosition))
            {
                Environment.Exit(0);
            }
        }

        private void HandleMainMenu(float mouseX, float mouseY)
        {
            if (MenuConstants.IsPointInButton(mouseX, mouseY, MenuConstants.NewGameButtonPosition))
            {
                _game.StartNewGame();
            }
            else if (MenuConstants.IsPointInButton(mouseX, mouseY, MenuConstants.ExitButtonPosition))
            {
                Environment.Exit(0);
            }
        }
    }
}

        