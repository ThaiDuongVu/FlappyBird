using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FlappyBird
{
    internal class ScoreDisplay : Game
    {
        private string[] numbers = { "sprites/0", "sprites/1", "sprites/2", "sprites/3", "sprites/4", "sprites/5", "sprites/6", "sprites/7", "sprites/8", "sprites/9" };
        private Texture2D[] numberTextures;

        /// <summary>
        /// Load all score textures.
        /// </summary>
        /// <param name="content"></param>
        public void Load(ContentManager content)
        {
            numberTextures = new Texture2D[numbers.Length];

            for (int i = 0; i < numberTextures.Length; i++)
                numberTextures[i] = content.Load<Texture2D>(numbers[i]);
        }

        /// <summary>
        /// Display score on screen.
        /// </summary>
        /// <param name="score">Score to display</param>
        /// <param name="centerPosition">Center position</param>
        /// <param name="gameTime">Elapsed game time</param>
        /// <param name="spriteBatch">Sprite batch</param>
        /// <param name="spriteEffects">Render effects</param>
        /// <param name="layer">Render layer</param>
        public void Draw(int score, Vector2 centerPosition, GameTime gameTime, SpriteBatch spriteBatch, SpriteEffects spriteEffects, int layer)
        {
            Texture2D scoreTexture;

            if (score == 0)
            {
                scoreTexture = numberTextures[0];
                spriteBatch.Draw(scoreTexture, centerPosition, null, Color.White, 0f, new Vector2(scoreTexture.Width / 2f, scoreTexture.Height / 2f), 1f, spriteEffects, layer);

                return;
            }

            float positionMutiplier = 0.5f * GetDigit(score) - 0.5f;

            while (score > 0)
            {
                scoreTexture = numberTextures[score % 10];
                spriteBatch.Draw(scoreTexture, new Vector2(centerPosition.X + positionMutiplier * scoreTexture.Width, centerPosition.Y), null, Color.White, 0f, new Vector2(scoreTexture.Width / 2f, scoreTexture.Height / 2f), 1f, spriteEffects, layer);

                positionMutiplier--;
                score /= 10;
            }
        }

        /// <summary>
        /// Get numbers of digit of a number.
        /// </summary>
        /// <param name="num">Number to check</param>
        /// <returns></returns>
        private int GetDigit(int num)
        {
            int digit = 0;
            while (num > 0)
            {
                digit++;
                num /= 10;
            }
            return digit;
        }
    }
}
