namespace RougeLikeDemo {
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Content;

    using System;

    public abstract class Entity : GObject {
        public enum Direction {UP, RIGHT, DOWN, LEFT, NOP}

        public Stats Stats {get;}
        public Map AciveMap {get; set;}

        public int MapX {get; set;}
        public int MapY {get; set;}
        public int FPS {get; set;} = 15;
        public int Frames {get; set;} = 4;
        public long MillisPerStep {get; set;} = 50;

        protected Direction Moving {get; set;}

        public Entity(string spriteSheet)
            : base(spriteSheet) {}

        public bool move(Direction d) {
            if(Moving == Direction.NOP && AciveMap.moveEntity(this, d)) {
                Moving = d;
                return true;
            };

            return false;
        }

        public override void update(GameTime gameTime) {
            MapComponent targetSpot = AciveMap.Components[MapY][MapX];
            int distanceX = Math.Abs(targetSpot.X - X);
            int distanceY = Math.Abs(targetSpot.Y - Y);

            if(distanceX + distanceY > 0) {
                long elapsedTime = gameTime.ElapsedGameTime.Milliseconds;
                if(distanceX > 0) X += (int)Math.Min(Math.Ceiling(elapsedTime*distanceX/(float)MillisPerStep), distanceX) * (targetSpot.X < X ? -1 : 1);
                if(distanceY > 0) Y += (int)Math.Min(Math.Ceiling(elapsedTime*distanceY/(float)MillisPerStep), distanceY) * (targetSpot.Y < Y ? -1 : 1);
            } else Moving = Direction.NOP;

            animate();
        }

        public override void load(ContentManager contentManager) {
            SpriteSheet = contentManager.Load<Texture2D>(SpriteSheetPath);
        }

        public virtual void animate() {}
    }
}