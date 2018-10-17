namespace RougeLikeDemo {
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Content;

    public abstract class GObject {
        public Texture2D SpriteSheet {get; set;}
        public Rectangle SpriteRectangle {get; set;}
        public SpriteBatch SpriteBatch {get; set;}
        public string SpriteSheetPath {get;}

        public int X {get; set;}
        public int Y {get; set;}

        public GObject(string spriteSheet) {
            SpriteSheetPath = spriteSheet;
        }

        public void draw(int mapX, int mapY, float scale) {
            SpriteBatch.Draw(SpriteSheet, new Vector2(mapX+X*scale, mapY+Y*scale),
                SpriteRectangle, Color.White, 0f, Vector2.Zero,
                scale, SpriteEffects.None, 1f);
        }

        public virtual void load(ContentManager contentManager) {}
        public virtual void update(GameTime gameTime) {}
    }
}