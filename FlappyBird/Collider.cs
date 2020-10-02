using Microsoft.Xna.Framework;

namespace FlappyBird
{
    internal class Collider
    {
        // The width & height of the collider
        private Vector2 _size;

        public Vector2 Size
        {
            get => _size;
            set => _size = value;
        }

        // The collider position, can be independent of attached object's position
        private Vector2 _position;

        public Vector2 Position
        {
            get => _position;
            set => _position = value;
        }

        // Default constructor
        public Collider()
        {
            _size = new Vector2(32f, 32f);
        }

        // Optional constructor
        public Collider(Vector2 size)
        {
            _size = size;
        }

        // True if this collider is colliding with another object
        public bool IsColliding(Collider other)
        {
            return true;
        }

        // True if this collider is touching the screen's horizontal edges
        public bool IsEdgedVertically(int screenHeight)
        {
            (_, float y) = Position;
            return y - Size.Y / 2f <= 0f || y + Size.Y / 2f >= screenHeight;
        }

        // True if this collider is touching the screen's vertical edges
        public bool IsEdgedHorizontally(int screenWidth)
        {
            (float x, _) = Position;
            return x - Size.X / 2f <= 0f || x + Size.X / 2f >= screenWidth;
        }
    }
}