namespace BesmashGame {
    using Microsoft.Xna.Framework;
    using GSMXtended;
    using System.Linq;

    public class OverlayPane : StackPane {
        public float BackgroundAlpha {get; set;}
        public bool IsActive {get; protected set;}

        public OverlayPane() {
            PercentWidth = PercentHeight = 100;
            PixelPerSecond = MillisPerScale = -1;
            BackgroundAlpha = 0.5f;
            Color = Color.Black;
            Alpha = 0;
        }

        public virtual void show() {
            Alpha = BackgroundAlpha;
            IsActive = true;
            showMenuItems(this);
        }

        public virtual void hide() {
            Container.applyAlpha(this, 0);
            IsActive = false;
        }

        public override void load() {
            base.load();
            hide();
        }

        private void showMenuItems(ScreenComponent component) {
            if(component is Container)
                ((Container)component).Children
                    .ToList().ForEach(showMenuItems);

            if(component is MenuItem)
                component.Alpha = 1;
        }
    }
}