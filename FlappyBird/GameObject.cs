using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FlappyBird
{
    internal class GameObject : Game
    {
        // The object's render texture
        protected Texture2D Texture { get; private set; }

        // The tag used for referencing objects
        public string Tag { get; set; }

        // The width & height of the object
        public Vector2 Size { get; private set; }

        // The x & y positions of the object
        public Vector2 Position { get; set; }

        // Whether the object is visible or not
        public bool IsVisible { get; set; } = true;

        // The origin point
        protected Vector2 Origin { get; private set; }

        // The rotation angles
        public float Angle { get; set; }

        // Collider used for collision detection
        public Collider Collider { get; private set; }

        // Default constructor
        public GameObject(string tag)
        {
            // Default size and position to (0, 0)
            Size = Vector2.Zero;
            Position = Vector2.Zero;

            Tag = tag;
        }

        // Load sprite texture to this game object
        public virtual void Load(ContentManager content, string textureName)
        {
            Texture = content.Load<Texture2D>(textureName);

            // Set actual size
            Size = new Vector2(Texture.Width, Texture.Height);
            // Set origin to the middle of the sprite
            Origin = new Vector2(Texture.Width / 2f, Texture.Height / 2f);
            // Initialize the collider
            Collider = new Collider(Size);
        }

        // Update object states
        public void Update()
        {
            // Update collider position
            Collider.Position = Position;
        }

        // Draw game object
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteEffects spriteEffects, int layer)
        {
            // Only draw if object is visible
            if (IsVisible)
                spriteBatch.Draw(Texture, Position, null, Color.White, Angle, Origin, 1f, spriteEffects, layer);
        }
    }
}