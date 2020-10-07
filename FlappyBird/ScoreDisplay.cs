using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FlappyBird
{
    internal class ScoreDisplay : Game
    {
        private string[] numbers = {"sprites/0", "sprites/1", "sprites/2", "sprites/3", "sprites/4", "sprites/5", "sprites/6", "sprites/7", "sprites/8", "sprites/9"};
        private Texture2D[] numberTextures;

        public void Load(ContentManager content)
        {
            numberTextures = new Texture2D[numbers.Length];

            for (int i = 0; i < numberTextures.Length; i++)
            {
                numberTextures[i] = content.Load<Texture2D>(numbers[i]);
            }
        }

        public void Draw(int score, Vector2 centerPosition, GameTime gameTime, SpriteBatch spriteBatch, SpriteEffects spriteEffects, int layer)
        {
            int tempScore = score;

            int i = 0;
            Texture2D scoreTexture;

            if (tempScore == 0)
            {
                scoreTexture = numberTextures[0];
                spriteBatch.Draw(scoreTexture, centerPosition, null, Color.White, 0f, new Vector2(scoreTexture.Width / 2f, scoreTexture.Height / 2f), 1f, spriteEffects, layer);

                return;
            }

            while (tempScore > 0)
            {
                i = tempScore % 10;

                scoreTexture = numberTextures[i];
                spriteBatch.Draw(scoreTexture, centerPosition, null, Color.White, 0f, new Vector2(scoreTexture.Width / 2f, scoreTexture.Height / 2f), 1f, spriteEffects, layer);

                tempScore /= 10;
            }
        }
    }
}
