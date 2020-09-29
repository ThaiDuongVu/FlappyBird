using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlappyBird
{
    class Background : GameObject
    {
        private const float ScrollSpeed = 1f;

        public Background(string tag) : base(tag) { }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteEffects spriteEffects, int layer)
        {
            // Draw two sprites next to each other & scroll
            spriteBatch.Draw(_texture, Position, null, Color.White, _angle, _origin, 1f, spriteEffects, layer);
            spriteBatch.Draw(_texture, new Vector2(Position.X + Size.X, Position.Y), null, Color.White, _angle, _origin, 1f, spriteEffects, layer);
        }

        // Scroll the background horizontally
        public void Scroll(int screenWidth)
        {
            _position.X -= ScrollSpeed;

            // Reset position of scroll past the edge of the screen
            if (_position.X < -screenWidth / 2f) _position.X = screenWidth / 2f;
        }
    }
}
