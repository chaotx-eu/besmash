namespace BesmashGame {
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework;
    using BesmashContent;

    public class FloatingText : Entity {
        public string Text {get; set;}
        public SpriteFont Font {get; set;}
        public float ScaleMod {get; set;} = 1;

        public int Duration {get; set;} = 1000;
        public Vector2 Path {get; set;} = new Vector2(0, -2f);

        private float scale, alpha, x, y;
        private Vector2 origin, offset, path;
        private int timer;
        
        public FloatingText(string text, SpriteFont font) {
            Text = text;
            Font = font;
            LayerLevel = 10;
        }

        public void init() {
            scale = alpha = timer = 0;
            offset = Vector2.Zero;

            path = new Vector2(
                Path.X*ContainingMap.TileWidth,
                Path.Y*ContainingMap.TileHeight);

            origin = new Vector2(
                DestinationRectangle.X + DestinationRectangle.Width/2f,
                DestinationRectangle.Y + DestinationRectangle.Height/2f);
        }

        public override void update(GameTime gameTime) {
            if(timer > Duration) {
                ContainingMap.removeEntity(this);
                return;
            }

            base.update(gameTime);
            int elapsed = gameTime.ElapsedGameTime.Milliseconds;
            timer += elapsed;

            float fragment = elapsed/(float)Duration;
            offset.X += path.X*fragment;
            offset.Y += path.Y*fragment;

            alpha = timer < Duration/2 ? 2f*timer/Duration
                : 2f*(Duration-timer)/Duration;

            scale = alpha;
        }

        public override void draw(SpriteBatch batch) {
            // Vector2 position = origin + offset;
            Vector2 position = new Vector2(
                DestinationRectangle.X + DestinationRectangle.Width/2f + offset.X,
                DestinationRectangle.Y + DestinationRectangle.Height/2f + offset.Y);
                
            batch.DrawString(Font, Text, position,
                Color*alpha, 0, Vector2.Zero,
                ScaleMod*scale, SpriteEffects.None, 1);
        }
    }
}