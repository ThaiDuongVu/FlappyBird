﻿using Microsoft.Xna.Framework;

namespace FlappyBird
{
    class Bird : GameObject
    {
        private const float GravityAcceleration = 9.8f;
        private float _acceleration;

        // Default constructor
        public Bird(string tag) : base(tag) { }

        // Drop the bird by gravity
        public void Drop(GameTime gameTime)
        {
            // Drop the bird
            _position.Y += (float)gameTime.ElapsedGameTime.TotalSeconds * _acceleration;
            // Increase acceleration over time
            _acceleration += GravityAcceleration;

            // Rotate the bird to the current velocity
            if (_angle < 0.5f) _angle += (float)gameTime.ElapsedGameTime.TotalSeconds;

        }

        // Flap the bird up
        public void Flap()
        {
            _acceleration = -GravityAcceleration * 30f;
            _angle = -0.5f;
        }
    }
}
