using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace FlappyBird
{
    public class Main : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        // Initialize random generator
        private readonly Random random = new Random();

        // Screen sizes
        private const int ScreenWidth = 288;
        private const int ScreenHeight = 512;

        // Render layers for background and game objects
        private const int BackgroundLayer = 0;
        private const int ObjectLayer = 1;

        // Current state of the game
        private GameState gameState = GameState.NotStarted;

        // Scrolling background
        private readonly Background background = new Background();
        private readonly string[] backgroundTextures = { "sprites/background-day", "sprites/background-night" };

        // The list of pipes in the scene
        private readonly List<Pipe> pipes = new List<Pipe>();
        private readonly Pipe pipe1 = new Pipe();
        private readonly Pipe pipe2 = new Pipe();
        private readonly Pipe pipe3 = new Pipe();
        private float pipeDistance;

        private readonly string[] pipeTextures = { "sprites/pipe-green", "sprites/pipe-red" };
        private int pipeIndex;

        // Base ground
        private readonly Background baseGround = new Background();

        // Bird
        private readonly Bird bird = new Bird();

        private readonly string[] birdTextures =
            {"sprites/bluebird-midflap", "sprites/yellowbird-midflap", "sprites/redbird-midflap"};

        // Game over & start up messages
        private readonly GameObject gameOverMessage = new GameObject();
        private readonly GameObject startUpMessage = new GameObject();

        private int score = 0;
        private ScoreDisplayer scoreDisplayer = new ScoreDisplayer();

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Main()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// Initial set up.
        /// </summary>
        protected override void Initialize()
        {
            SetScreenSize();

            // Add pipes to pipe list for better tracking
            pipes.Add(pipe1);
            pipes.Add(pipe2);
            pipes.Add(pipe3);

            base.Initialize();
        }

        /// <summary>
        /// Load sprites textures.
        /// </summary>
        private void LoadSprites()
        {
            // Load one of the backgrounds 
            background.Load(Content, backgroundTextures[random.Next(0, backgroundTextures.Length)]);

            // Load pipe
            pipeIndex = random.Next(0, 2);
            foreach (Pipe pipe in pipes)
                pipe.Load(Content, pipeTextures[pipeIndex]);

            // Load base
            baseGround.Load(Content, "sprites/base");

            // Load one of the birds
            bird.Load(Content, birdTextures[random.Next(0, birdTextures.Length)]);

            // Load the UI elements
            gameOverMessage.Load(Content, "sprites/gameover");
            startUpMessage.Load(Content, "sprites/message");

            // Load all score sprites
            scoreDisplayer.Load(Content);
        }

        /// <summary>
        /// Initialize all objects.
        /// </summary>
        private void Init()
        {
            // Center
            background.Position = new Vector2(ScreenWidth / 2f, ScreenHeight / 2f);

            // Low on the ground and over to the right
            pipeDistance = (ScreenWidth - 2 * pipe1.Size.X) / 2f + pipe1.Size.X;
            pipe1.Position = new Vector2(ScreenWidth + pipe1.Size.X / 2f, 0f);
            pipe2.Position = new Vector2(ScreenWidth + pipe1.Size.X / 2f + pipeDistance, 0f);
            pipe3.Position = new Vector2(ScreenWidth + pipe1.Size.X / 2f + 2 * pipeDistance, 0f);
            foreach (Pipe pipe in pipes)
                pipe.RandomizePosition(ScreenHeight, baseGround.Size.Y);

            // Low on the ground
            baseGround.Position = new Vector2(0f, ScreenHeight - baseGround.Size.Y / 2f);

            // Slightly to the left
            bird.Position = new Vector2(ScreenWidth / 2f - 75f, ScreenHeight / 2f);
            bird.Angle = 0f;

            // Center
            gameOverMessage.Position = new Vector2(ScreenWidth / 2f, ScreenHeight / 2f - baseGround.Size.Y / 2f);
            startUpMessage.Position = new Vector2(ScreenWidth / 2f, ScreenHeight / 2f - baseGround.Size.Y / 2f);
        }

        /// <summary>
        /// Load textures and audio files.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            LoadSprites();
            Init();
        }

        /// <summary>
        /// Update game loop.
        /// </summary>
        /// <param name="gameTime">Elapsed game time</param>
        protected override void Update(GameTime gameTime)
        {
            // Get keyboard & gamepad states to handle input
            InputManager.GetKeyState();
            InputManager.GetGamePadState(PlayerIndex.One);

            // If escape or back button pressed then exit game
            if (InputManager.OnKeyDown(Keys.Escape) || InputManager.OnButtonDown(Buttons.Start)) Exit();

            // If space bar or A button pressed then flap bird or start game depending on the game's state
            if (InputManager.OnKeyDown(Keys.Space) || InputManager.OnButtonDown(Buttons.A))
                switch (gameState)
                {
                    case GameState.NotStarted:
                        Start();
                        bird.Flap();
                        break;

                    case GameState.Started:
                        bird.Flap();
                        break;

                    case GameState.GameOver:
                        Restart();
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

            // Update dynamic objects
            bird.Update();

            // Update pipes
            foreach (Pipe pipe in pipes)
            {
                pipe.Update();

                // Scroll pipes if game started
                if (gameState == GameState.Started) pipe.Scroll(ScreenWidth, ScreenHeight, baseGround.Size.Y);

                // If colliding with bird then game over
                if (pipe.Collider.IsColliding(bird.Collider) || pipe.SecondaryCollider.IsColliding(bird.Collider)) GameOver();

                if (pipe.TriggerCollider.IsColliding(bird.Collider) && !pipe.scoreAdded)
                {
                    score++;
                    pipe.scoreAdded = true;
                }
            }

            // If bird touches the edge of the screen the game over
            if (bird.Collider.IsEdgedVertically(ScreenHeight - (int)baseGround.Size.Y))
            {
                if (bird.Position.Y > ScreenHeight / 2f) GameOver();
            }

            // If game started then drop bird to gravity
            if (gameState == GameState.Started) bird.Drop(gameTime);

            // Scroll background & base if not game over
            if (gameState != GameState.GameOver)
            {
                background.Scroll(ScreenWidth);
                baseGround.Scroll(ScreenWidth);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Render game sprites.
        /// </summary>
        /// <param name="gameTime">Elapsed game time</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            // Draw background
            background.Draw(gameTime, spriteBatch, SpriteEffects.None, BackgroundLayer);

            // Draw pipe
            foreach (Pipe pipe in pipes) pipe.Draw(gameTime, spriteBatch, SpriteEffects.None, BackgroundLayer);

            // Draw base
            baseGround.Draw(gameTime, spriteBatch, SpriteEffects.None, BackgroundLayer);

            // Draw bird
            bird.Draw(gameTime, spriteBatch, SpriteEffects.None, ObjectLayer);

            switch (gameState)
            {
                case GameState.Started:
                    // Display score
                    scoreDisplayer.Draw(score, new Vector2(ScreenWidth / 2f, ScreenHeight / 8f), gameTime, spriteBatch, SpriteEffects.None, ObjectLayer);
                    break;

                case GameState.GameOver:
                    // If game over then display game over message
                    gameOverMessage.Draw(gameTime, spriteBatch, SpriteEffects.None, ObjectLayer);
                    // Display score
                    scoreDisplayer.Draw(score, new Vector2(ScreenWidth / 2f, ScreenHeight / 8f), gameTime, spriteBatch, SpriteEffects.None, ObjectLayer);
                    break;

                case GameState.NotStarted:
                    // If game not started then display startup message
                    startUpMessage.Draw(gameTime, spriteBatch, SpriteEffects.None, ObjectLayer);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Set screen rendering resolution.
        /// </summary>
        private void SetScreenSize()
        {
            graphics.PreferredBackBufferWidth = ScreenWidth;
            graphics.PreferredBackBufferHeight = ScreenHeight;

            // Apply screen size buffer
            graphics.ApplyChanges();
        }

        /// <summary>
        /// When player die, call game over.
        /// </summary>
        private void GameOver()
        {
            gameState = GameState.GameOver;
        }

        /// <summary>
        /// Restart game.
        /// </summary>
        private void Restart()
        {
            foreach (Pipe pipe in pipes)
                pipe.scoreAdded = false;

            gameState = GameState.NotStarted;
            score = 0;

            // Reload textures for dynamic background and birds
            LoadContent();
        }

        /// <summary>
        /// Start the game.
        /// </summary>
        private void Start()
        {
            score = 0;
            gameState = GameState.Started;
        }
    }
}