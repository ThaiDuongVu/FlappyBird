using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FlappyBird
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Random random = new Random();

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

        // Base ground
        private readonly Background _base = new Background("Base");

        // Bird
        private readonly Bird _bird = new Bird("Bird");

        // Game over & start up messages
        private readonly GameObject _gameOverMessage = new GameObject("GameOverMessage");
        private readonly GameObject _startUpMessage = new GameObject("StartMessage");

        // Default constructor
        public Game()
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

            Load();
            Init();
        }

        // Update game loop
        protected override void Update(GameTime gameTime)
        {
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

            // If bird touches the edge of the screen the game over
            if (_bird.Collider.IsEdgedVertically(ScreenHeight - (int)_base.Size.Y, _bird.Position)) GameOver();

            // If game started then drop bird to gravity
            if (_gameState == GameState.Started) _bird.Drop(gameTime);

            // Scroll background & base
            _background.Scroll(ScreenWidth);
            _base.Scroll(ScreenWidth);

            base.Update(gameTime);
        }

        // Render game sprites
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();

            // Draw background & base
            _background.Draw(gameTime, _spriteBatch, SpriteEffects.None, BackgroundLayer);
            _base.Draw(gameTime, _spriteBatch, SpriteEffects.None, BackgroundLayer);

            switch (_gameState)
            {
                // Draw bird only if game started
                case GameState.Started:
                    _bird.Draw(gameTime, _spriteBatch, SpriteEffects.None, ObjectLayer);
                    break;

                // If game over then display game over message
                case GameState.GameOver:
                    _gameOverMessage.Draw(gameTime, _spriteBatch, SpriteEffects.None, ObjectLayer);
                    _bird.Draw(gameTime, _spriteBatch, SpriteEffects.None, ObjectLayer);
                    break;

                // If game not started then display startup message
                case GameState.NotStarted:
                    _startUpMessage.Draw(gameTime, _spriteBatch, SpriteEffects.None, ObjectLayer);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        // Load sprites textures
        private void Load()
        {
            if (random.Next(0, 2) == 0) _background.Load(Content, "sprites/background-day");
            else _background.Load(Content, "sprites/background-night");

            _base.Load(Content, "sprites/base");

            _bird.Load(Content, "sprites/bluebird-midflap");

            _gameOverMessage.Load(Content, "sprites/gameover");
            _startUpMessage.Load(Content, "sprites/message");
        }

        // Initialize all objects
        private void Init()
        {
            // Center
            _background.Position = new Vector2(ScreenWidth / 2f, ScreenHeight / 2f);
            // Low on the ground
            _base.Position = new Vector2(0f, ScreenHeight - _base.Size.Y / 2f);

            // Slightly to the left
            _bird.Position = new Vector2(ScreenWidth / 2f - 50f, ScreenHeight / 2f);

            // Center
            _gameOverMessage.Position = new Vector2(ScreenWidth / 2f, ScreenHeight / 2f);
            _startUpMessage.Position = new Vector2(ScreenWidth / 2f, ScreenHeight / 2f);
        }

        // Set screen rendering resolution
        private void SetScreenSize()
        {
            _graphics.PreferredBackBufferWidth = ScreenWidth;
            _graphics.PreferredBackBufferHeight = ScreenHeight;

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
            LoadContent();
        }

        // Start the game
        private void Start()
        {
            _gameState = GameState.Started;
        }
    }
}