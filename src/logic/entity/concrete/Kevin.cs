namespace RougeLikeDemo {
    using Microsoft.Xna.Framework;
    using System;

    public class Kevin : Player {
        private int sprite_x, sprite_y;
        private bool sprite_changed;

        protected int SpriteX {
            get { return sprite_x; }
            set {
                sprite_changed = !sprite_changed && sprite_x != value;
                sprite_x = value;
            }
        }

        protected int SpriteY {
            get { return sprite_y; }
            set {
                sprite_changed = !sprite_changed && sprite_y != value;
                sprite_y = value;
            }
        }

        public Kevin(): base("game/texture/sheets/entity/kevin_sheet") {}

        public override void animate() {
            SpriteX = Moving == Direction.NOP ? 0
                : (int)(DateTime.Now.Ticks/(TimeSpan.TicksPerSecond/FPS) % Frames);

            SpriteY = Moving == Direction.UP ? 0
                : Moving == Direction.RIGHT ? 1
                : Moving == Direction.DOWN ? 2
                : Moving == Direction.LEFT ? 3
                : SpriteY;

            // if(sprite_changed) {
                SpriteRectangle = new Rectangle(SpriteX*16, SpriteY*16, 16, 16);
                sprite_changed = false;
            // }
        }
    }
}