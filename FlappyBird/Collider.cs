using Microsoft.Xna.Framework;

namespace FlappyBird
{
    internal class Collider
    {
        public Vector2 Size;

        // Default constructor
        public Collider()
        {
            Size = new Vector2(32f, 32f);
        }

        // Optional constructor
        public Collider(Vector2 size)
        {
            Size = size;
        }

        // True if this collider is colliding with another object
        public bool IsColliding(Collider other)
        {
            return true;
        }

        // True if this collider is touching the screen's horizontal edges
        public bool IsEdgedVertically(int screenHeight, Vector2 position)
        {
            if (position.Y - Size.Y / 2f <= 0f || position.Y + Size.Y / 2f >= screenHeight)
            {
                return true;
            }
            return false;
        }

        // True if this collider is touching the screen's vertical edges
        public bool IsEdgedHorizontally(int screenWidth, Vector2 position)
        {
            if (position.X - Size.X / 2f <= 0f || position.X + Size.X / 2f >= screenWidth)
            {
                return true;
            }
            return false;
        }
    }
}
