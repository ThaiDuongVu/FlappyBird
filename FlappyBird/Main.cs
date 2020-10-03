﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FlappyBird
{
    public class Main : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Initialize random generator
        private readonly Random _random = new Random();

        // Screen sizes
        private const int ScreenWidth = 288;
        private const int ScreenHeight = 512;

        // Render layers for background and game objects
        private const int BackgroundLayer = 0;
        private const int ObjectLayer = 1;

        // Current state of the game
        private GameState _gameState = GameState.NotStarted;

        // Scrolling background
        private readonly Background _background = new Background("Background");
        private readonly string[] _backgroundTextures = { "sprites/background-day", "sprites/background-night" };

        // The list of pipes in the scene
        private List<Pipe> _pipes = new List<Pipe>();
        private Pipe _pipe1 = new Pipe("pipe1");
        private Pipe _pipe2 = new Pipe("pipe2");
        private Pipe _pipe3 = new Pipe("pipe3");
        private float _pipeDistance;
        private readonly string[] _pipeTextures = { "sprites/pipe-green", "sprites/pipe-red" };

        // Base ground
        private readonly Background _base = new Background("Base");

        // Bird
        private readonly Bird _bird = new Bird("Bird");

        private readonly string[] _birdTextures =
            {"sprites/bluebird-midflap", "sprites/yellowbird-midflap", "sprites/redbird-midflap"};

        // Game over & start up messages
        private readonly GameObject _gameOverMessage = new GameObject("GameOverMessage");
        private readonly GameObject _startUpMessage = new GameObject("StartMessage");

        // Default constructor
        public Main()
        {
            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        // Initial set up
        protected override void Initialize()
        {
            SetScreenSize();

            base.Initialize();
        }

        // Load textures and audio files
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            LoadSprites();
            Init();
        }

        // Update game loop
        protected override void Update(GameTime gameTime)
        {
            // Get keyboard & gamepad states to handle input
            InputManager.GetKeyState();
            InputManager.GetGamePadState(PlayerIndex.One);

            // If escape or back button pressed then exit game
            if (InputManager.IsKeyPressed(Keys.Escape) || InputManager.IsButtonPressed(Buttons.Start)) Exit();

            // If space bar or A button pressed then flap bird or start game depending on the game's state
            if (InputManager.IsKeyPressed(Keys.Space) || InputManager.IsButtonPressed(Buttons.A))
                switch (_gameState)
                {
                    case GameState.NotStarted:
                        Start();
                        _bird.Flap();
                        break;

                    case GameState.Started:
                        _bird.Flap();
                        break;

                    case GameState.GameOver:
                        Restart();
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

            // Update dynamic objects
            _bird.Update();

            // If bird touches the edge of the screen the game over
            if (_bird.Collider.IsEdgedVertically(ScreenHeight - (int)_base.Size.Y)) GameOver();

            // If game started then drop bird to gravity
            if (_gameState == GameState.Started) _bird.Drop(gameTime);

            // Scroll background & base if not game over
            if (_gameState != GameState.GameOver)
            {
                _background.Scroll(ScreenWidth);
                _base.Scroll(ScreenWidth);
            }

            // Scroll pipe if game started
            if (_gameState == GameState.Started)
            {
                foreach (Pipe pipe in _pipes)
                {
                    pipe.Scroll(ScreenWidth);
                }
            }

            base.Update(gameTime);
        }

        // Render game sprites
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();

            // Draw background
            _background.Draw(gameTime, _spriteBatch, SpriteEffects.None, BackgroundLayer);

            // Draw pipe
            foreach (Pipe pipe in _pipes)
            {
                pipe.Draw(gameTime, _spriteBatch, SpriteEffects.None, BackgroundLayer);
            }

            // Draw base
            _base.Draw(gameTime, _spriteBatch, SpriteEffects.None, BackgroundLayer);

            switch (_gameState)
            {
                // Draw bird only if game started
                case GameState.Started:
                    _bird.Draw(gameTime, _spriteBatch, SpriteEffects.None, ObjectLayer);
                    break;

                // If game over then display game over message
                case GameState.GameOver:
                    _bird.Draw(gameTime, _spriteBatch, SpriteEffects.None, ObjectLayer);
                    _gameOverMessage.Draw(gameTime, _spriteBatch, SpriteEffects.None, ObjectLayer);
                    break;

                // If game not started then display startup message
                case GameState.NotStarted:
                    _bird.Draw(gameTime, _spriteBatch, SpriteEffects.None, ObjectLayer);
                    _startUpMessage.Draw(gameTime, _spriteBatch, SpriteEffects.None, ObjectLayer);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        // Load sprites textures
        private void LoadSprites()
        {
            // Load one of the backgrounds 
            _background.Load(Content, _backgroundTextures[_random.Next(0, _backgroundTextures.Length)]);

            // Add pipes to pipe list for better tracking
            _pipes.Add(_pipe1);
            _pipes.Add(_pipe2);
            _pipes.Add(_pipe3);
            // Load pipe
            foreach (Pipe pipe in _pipes)
                pipe.Load(Content, _pipeTextures[_random.Next(0, _pipeTextures.Length)]);

            _base.Load(Content, "sprites/base");

            // Load one of the birds
            _bird.Load(Content, _birdTextures[_random.Next(0, _birdTextures.Length)]);

            _gameOverMessage.Load(Content, "sprites/gameover");
            _startUpMessage.Load(Content, "sprites/message");
        }

        // Initialize all objects
        private void Init()
        {
            // Center
            _background.Position = new Vector2(ScreenWidth / 2f, ScreenHeight / 2f);

            // Low on the ground and over to the right
            _pipeDistance = (ScreenWidth - 2 * _pipe1.Size.X) / 2f + _pipe1.Size.X;
            _pipe1.Position = new Vector2(ScreenWidth + _pipe1.Size.X / 2f, ScreenHeight - _pipe1.Size.Y / 2f);
            _pipe2.Position = new Vector2(ScreenWidth + _pipe1.Size.X / 2f + _pipeDistance, ScreenHeight - _pipe2.Size.Y / 2f);
            _pipe3.Position = new Vector2(ScreenWidth + _pipe1.Size.X / 2f + 2 * _pipeDistance, ScreenHeight - _pipe3.Size.Y / 2f);

            // Low on the ground
            _base.Position = new Vector2(0f, ScreenHeight - _base.Size.Y / 2f);

            // Slightly to the left
            _bird.Position = new Vector2(ScreenWidth / 2f - 75f, ScreenHeight / 2f);
            _bird.Angle = 0f;

            // Center
            _gameOverMessage.Position = new Vector2(ScreenWidth / 2f, ScreenHeight / 2f - _base.Size.Y / 2f);
            _startUpMessage.Position = new Vector2(ScreenWidth / 2f, ScreenHeight / 2f - _base.Size.Y / 2f);
        }

        // Set screen rendering resolution
        private void SetScreenSize()
        {
            _graphics.PreferredBackBufferWidth = ScreenWidth;
            _graphics.PreferredBackBufferHeight = ScreenHeight;

            // Apply screen size buffer
            _graphics.ApplyChanges();
        }

        // When player die, call game over
        private void GameOver()
        {
            _gameState = GameState.GameOver;
        }

        // Restart game
        private void Restart()
        {
            _gameState = GameState.NotStarted;

            // Reload textures for dynamic background and birds
            LoadContent();
        }

        // Start the game
        private void Start()
        {
            _gameState = GameState.Started;
        }
    }
}