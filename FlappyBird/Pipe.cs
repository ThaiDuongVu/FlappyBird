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

        private readonly Random _random = new Random();

        // Gap between two pipes
        private float _gap;

        // Collider for the top pipe
        public Collider SecondaryCollider;

        // Collider for triggering score
        public Collider TriggerCollider;
        public bool scoreAdded;

        public override void Load(ContentManager content, string textureName)
        {
            base.Load(content, textureName);

            _gap = Size.Y / 3f;

            SecondaryCollider = new Collider(Size);
            TriggerCollider = new Collider(new Vector2(Size.X / 2f, _gap));
        }

        public override void Update()
        {
            // Update both colliders
            Collider.Position = Position;
            SecondaryCollider.Position = new Vector2(Position.X, Position.Y - Size.Y - _gap);
            TriggerCollider.Position = new Vector2(Position.X, Position.Y - Size.Y / 2f - _gap / 2f);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteEffects spriteEffects, int layer)
        {
            // Draw the lower and upper pipe with a gap in between
            spriteBatch.Draw(Texture, Position, null, Color.White, Angle, Origin, 1f, spriteEffects, layer);
            spriteBatch.Draw(Texture, new Vector2(Position.X, Position.Y - Size.Y - _gap), null, Color.White,
                Angle + Pi, Origin, 1f, spriteEffects, layer);
        }

        // Scroll pipe past the screen
        public void Scroll(float screenWidth, float screenHeight, float baseHeight)
        {
            Position = new Vector2(Position.X - ScrollSpeed, Position.Y);

            // Reset position of scroll past the edge of the screen
            if (!(Position.X < -screenWidth / 2f)) return;

            RandomizePosition(screenHeight, baseHeight);
            Position = new Vector2(screenWidth + Size.X / 2f, Position.Y);

            scoreAdded = false;
        }

        // Set a random y position
        public void RandomizePosition(float screenHeight, float baseHeight)
        {
            // Upper and lower limit of the randomize range
            float upperLimit = screenHeight - Size.Y / 2f - baseHeight / 2f;
            float lowerLimit = screenHeight;

            Position = new Vector2(Position.X, (float)_random.NextDouble() * (lowerLimit - upperLimit) + upperLimit);
        }
    }
}