namespace RougeLikeDemo {
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Content;

    public class MapComponent : GObject {
        public bool Solid {get;}

        public MapComponent(string spriteSheet, int spriteX, int spriteY, int spriteWidth, int spriteHeight, bool solid)
            : base(spriteSheet)
        {
            Solid = solid;
            SpriteRectangle = new Rectangle(spriteX, spriteY, spriteWidth, spriteHeight);
        }

        public MapComponent(string spriteSheet, int spriteX, int spriteY, int spriteSize, bool solid)
            : this(spriteSheet, spriteX, spriteY, spriteSize, spriteSize, solid) {}

        public MapComponent(int spriteX, int spriteY, int spriteWidth, int spriteHeight, bool solid)
            : this(null, spriteX, spriteY, spriteWidth, spriteHeight, solid) {}

        public MapComponent(int spriteX, int spriteY, int spriteSize, bool solid)
            : this(null, spriteX, spriteY, spriteSize, solid) {}
    }
}