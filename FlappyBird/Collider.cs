using Microsoft.Xna.Framework;

namespace FlappyBird
{
    internal class Collider
    {
        // The width & height of the collider
        private Vector2 Size { get; set; }

        // The collider position, can be independent of attached object's position
        public Vector2 Position { get; set; }

        // Constructor
        public Collider(Vector2 size)
        {
            Size = size;
        }

        /// <summary>
        /// Returns true if this collider is colliding with another collider.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsColliding(Collider other)
        {
            // Check within x range
            bool withinXRange = other.Position.X <= Position.X + Size.X / 2f + other.Size.X / 2f &&
                                other.Position.X >= Position.X - Size.X / 2f - other.Size.X / 2f;

            // Check within y range
            bool withinYRange = other.Position.Y <= Position.Y + Size.Y / 2f + other.Size.Y / 2f &&
                                other.Position.Y >= Position.Y - Size.Y / 2f - other.Size.Y / 2f;

            return withinXRange && withinYRange;
        }

        /// <summary>
        /// Returns true if this collider is touching the screen's horizontal edges.
        /// </summary>
        /// <param name="screenHeight"></param>
        /// <returns></returns>
        public bool IsEdgedVertically(int screenHeight)
        {
            (_, float y) = Position;
            return y - Size.Y / 2f <= 0f || y + Size.Y / 2f >= screenHeight;
        }

        /// <summary>
        /// Returns true if this collider is touching the screen's vertical edges.
        /// </summary>
        /// <param name="screenWidth"></param>
        /// <returns></returns>
        public bool IsEdgedHorizontally(int screenWidth)
        {
            (float x, _) = Position;
            return x - Size.X / 2f <= 0f || x + Size.X / 2f >= screenWidth;
        }
    }
}