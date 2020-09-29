using Microsoft.Xna.Framework;

namespace FlappyBird
{
    internal class Collider
    {
        // The width & height of the collider
        public Vector2 Size { get; set; }

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
            (_, float y) = position;
            return y - Size.Y / 2f <= 0f || y + Size.Y / 2f >= screenHeight;
        }

        // True if this collider is touching the screen's vertical edges
        public bool IsEdgedHorizontally(int screenWidth, Vector2 position)
        {
            (float x, _) = position;
            return x - Size.X / 2f <= 0f || x + Size.X / 2f >= screenWidth;
        }
    }
}