using Microsoft.Xna.Framework;

namespace FlappyBird
{
    internal class Bird : GameObject
    {
        private const float GravityAcceleration = 9.8f;
        private float acceleration;

        private const float FlapForce = 25f;

        /// <summary>
        /// Drop the bird by gravity.
        /// </summary>
        /// <param name="gameTime">Elapsed game time</param>
        public void Drop(GameTime gameTime)
        {
            // Drop the bird
            Position = new Vector2(Position.X,
                Position.Y + (float)gameTime.ElapsedGameTime.TotalSeconds * acceleration);
            // Increase acceleration over time
            acceleration += GravityAcceleration;

            // Rotate the bird to the current velocity
            Angle += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Position.Y < 0f)
            {
                Position = new Vector2(Position.X, 0f);
                acceleration = 0f;
            }
        }

        /// <summary>
        /// Flap the bird up.
        /// </summary>
        public void Flap()
        {
            acceleration = -GravityAcceleration * FlapForce;
            Angle = -0.5f;
        }
    }
}