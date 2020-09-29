using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlappyBird
{
    internal class Background : GameObject
    {
        private const float ScrollSpeed = 1f;

        public Background(string tag) : base(tag)
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteEffects spriteEffects, int layer)
        {
            // Draw two sprites next to each other & scroll
            spriteBatch.Draw(Texture, Position, null, Color.White, Angle, Origin, 1f, spriteEffects, layer);
            spriteBatch.Draw(Texture, new Vector2(Position.X + Size.X, Position.Y), null, Color.White, Angle, Origin,
                1f, spriteEffects, layer);
        }

        // Scroll the background horizontally
        public void Scroll(int screenWidth)
        {
            _position.X -= ScrollSpeed;

            // Reset position of scroll past the edge of the screen
            if (Position.X < -screenWidth / 2f) _position.X = screenWidth / 2f;
        }
    }
}