using Microsoft.Xna.Framework;

namespace FlappyBird
{
    internal class Bird : GameObject
    {
        private const float GravityAcceleration = 9.8f;
        private float _acceleration;

        private const float FlapForce = 25f;

        // Drop the bird by gravity
        public void Drop(GameTime gameTime)
        {
            // Drop the bird
            Position = new Vector2(Position.X,
                Position.Y + (float)gameTime.ElapsedGameTime.TotalSeconds * _acceleration);
            // Increase acceleration over time
            _acceleration += GravityAcceleration;

            // Rotate the bird to the current velocity
            Angle += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Position.Y < 0f)
            {
                Position = new Vector2(Position.X, 0f);
                _acceleration = 0f;
            }
        }

        // Flap the bird up
        public void Flap()
        {
            _acceleration = -GravityAcceleration * FlapForce;
            Angle = -0.5f;
        }
    }
}