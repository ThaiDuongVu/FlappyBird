using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlappyBird
{
    internal sealed class Background : GameObject
    {
        private const float ScrollSpeed = 1f;

        /// <summary>
        /// Render background.
        /// </summary>
        /// <param name="gameTime">Elapsed game time</param>
        /// <param name="spriteBatch">Sprite batch</param>
        /// <param name="spriteEffects">Render effects</param>
        /// <param name="layer">Render layer</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteEffects spriteEffects, int layer)
        {
            // Draw two sprites next to each other
            spriteBatch.Draw(Texture, Position, null, Color.White, Angle, Origin, 1f, spriteEffects, layer);
            spriteBatch.Draw(Texture, new Vector2(Position.X + Size.X, Position.Y), null, Color.White, Angle, Origin,
                1f, spriteEffects, layer);
        }

        /// <summary>
        /// Scroll the background horizontally.
        /// </summary>
        /// <param name="screenWidth">Width of current screen</param>
        public void Scroll(float screenWidth)
        {
            Position = new Vector2(Position.X - ScrollSpeed, Position.Y);

            // Reset position of scroll past the edge of the screen
            if (Position.X < -screenWidth / 2f) Position = new Vector2(screenWidth / 2f, Position.Y);
        }
    }
}