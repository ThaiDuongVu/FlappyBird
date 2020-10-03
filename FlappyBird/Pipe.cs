using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FlappyBird
{
    internal class Pipe : GameObject
    {
        private const float ScrollSpeed = 1f;
        private const float Pi = 3.14159f;

        private float _gap;
        private Random _random = new Random();

        // Default constructor
        public Pipe(string tag) : base(tag)
        {
        }

        public override void Load(ContentManager content, string textureName)
        {
            base.Load(content, textureName);
            _gap = Size.Y / 2.75f;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteEffects spriteEffects, int layer)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, Angle, Origin, 1f, spriteEffects, layer);
            spriteBatch.Draw(Texture, new Vector2(Position.X, Position.Y - Size.Y - _gap), null, Color.White, Angle + Pi, Origin, 1f, spriteEffects, layer);
        }

        public void Scroll(float screenWidth, float screenHeight, float baseHeight)
        {
            _position.X -= ScrollSpeed;

            // Reset position of scroll past the edge of the screen
            if (Position.X < -screenWidth / 2f)
            {
                RandomizePosition(screenHeight, baseHeight);
                _position.X = screenWidth + Size.X / 2f;
            }
        }
        
        // Set a random y position
        public void RandomizePosition(float screenHeight, float baseHeight)
        {
            float upperLimit = screenHeight - Size.Y / 2f - baseHeight / 2f;
            float lowerLimit = screenHeight;

            _position.Y = ((float) _random.NextDouble()) * (lowerLimit - upperLimit) + upperLimit;
        }
    }
}