using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FlappyBird
{
    internal class GameObject : Microsoft.Xna.Framework.Game
    {
        // The object's render texture
        protected Texture2D _texture;
        public Texture2D Texture => _texture;

        // The tag used for referencing objects
        private string _tag;
        public string Tag => _tag;

        // The width & height of the object
        protected Vector2 _size;
        public Vector2 Size
        {
            get => _size;
            set => _size = value;
        }

        // The x & y positions of the object
        protected Vector2 _position;
        public Vector2 Position
        {
            get => _position;
            set => _position = value;
        }

        // Whether the object is visible or not
        protected bool _isVisible = true;
        public bool IsVisible
        {
            get => _isVisible;
            set => _isVisible = value;
        }

        // The origin point
        protected Vector2 _origin;
        // The rotation angles
        protected float _angle;

        // Collider used for collision detection
        protected Collider _collider;
        public Collider Collider => _collider;

        // Default constructor
        public GameObject(string tag)
        {
            // Default size and position to (0, 0)
            _size = Vector2.Zero;
            _position = Vector2.Zero;

            _tag = tag;
        }

        // Load sprite texture to this game object
        public void Load(ContentManager content, string textureName)
        {
            _texture = content.Load<Texture2D>(textureName);

            // Set actual size
            _size = new Vector2(_texture.Width, _texture.Height);
            // Set origin to the middle of the sprite
            _origin = new Vector2(_texture.Width / 2f, _texture.Height / 2f);
            // Initialize the collider
            _collider = new Collider(_size);
        }

        // Draw game object
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteEffects spriteEffects, int layer)
        {
            // Only draw if object is visible
            if (_isVisible) spriteBatch.Draw(_texture, _position, null, Color.White, _angle, _origin, 1f, spriteEffects, layer);

        }
    }
}
