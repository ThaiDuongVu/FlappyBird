using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlappyBird
{
    internal class Pipe : GameObject
    {
        private const float ScrollSpeed = 1f;

        // Default constructor
        public Pipe(string tag) : base(tag)
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteEffects spriteEffects, int layer)
        {
            base.Draw(gameTime, spriteBatch, spriteEffects, layer);
        }

        public void Scroll(float screenWidth)
        {
            _position.X -= ScrollSpeed;

            // Reset position of scroll past the edge of the screen
            if (Position.X < -screenWidth / 2f) _position.X = screenWidth + Size.X / 2f;
        }
    }
}