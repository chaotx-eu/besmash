namespace BesmashGame.Debug {
    using Microsoft.Xna.Framework;
    using BesmashContent;
    using BesmashContent.Collections;
    using GSMXtended;
    using System.Collections.Generic;
    using System.Linq;

    public class DebugPane : BesmashMenuPane {
        public TileMap Map {get; set;}

        private TextItem fpsText;
        private TextItem slaveXText;
        private TextItem slaveYText;
        private TextItem mapWidthText;
        private TextItem mapHeightText;
        private TextItem entitiesText;
        private TextItem projectilesText;
        private TextItem animationsText;

        public DebugPane() {
            PercentWidth = 40;
            HAlignment = HAlignment.Left;
            VAlignment = VAlignment.Top;

            TextItem[] valueTextItems = {
                fpsText = new TextItem("", "fonts/menu_font1"),
                slaveXText = new TextItem("", "fonts/menu_font1"),
                slaveYText = new TextItem("", "fonts/menu_font1"),
                mapWidthText = new TextItem("", "fonts/menu_font1"),
                mapHeightText = new TextItem("", "fonts/menu_font1"),
                entitiesText = new TextItem("", "fonts/menu_font1"),
                projectilesText = new TextItem("", "fonts/menu_font1"),
                animationsText = new TextItem("", "fonts/menu_font1")
            };
            
            TextItem[] titleTextItems = {
                new TextItem("FPS: ", "fonts/menu_font1"),
                new TextItem("Slave X: ", "fonts/menu_font1"),
                new TextItem("Slave Y: ", "fonts/menu_font1"),
                new TextItem("Map Width: ", "fonts/menu_font1"),
                new TextItem("Map Height", "fonts/menu_font1"),
                new TextItem("Entities: ", "fonts/menu_font1"),
                new TextItem("Projectiles: ", "fonts/menu_font1"),
                new TextItem("Animations: ", "fonts/menu_font1")
            };

            for(int i = 0; i < valueTextItems.Length; ++i) {
                HPane leftPad = new HPane();
                HPane title = new HPane(titleTextItems[i]);
                HPane value = new HPane(valueTextItems[i]);
                HPane row = new HPane(leftPad, title, value);

                leftPad.HAlignment = HAlignment.Left;
                title.HAlignment = HAlignment.Left;
                value.HAlignment = HAlignment.Left;

                titleTextItems[i].HAlignment = HAlignment.Left;
                valueTextItems[i].HAlignment = HAlignment.Left;
                titleTextItems[i].DefaultScale = 0.5f;
                valueTextItems[i].DefaultScale = 0.5f;
                titleTextItems[i].Color = Color.Black;
                valueTextItems[i].Color = Color.Black;

                leftPad.PercentWidth = 5;
                title.PercentWidth = 20;
                row.PercentWidth = 100;
                row.EffectAlpha = 0.7f;
                row.Color = i%2 == 0
                    ? Color.DarkGray
                    : Color.Gray;

                add(row);
            }
        }

        public override void load() {
            base.load();
            hide();
        }

        public override void hide(bool takeFocus, float alpha) {
            base.hide(takeFocus, alpha);
            Scale = 0;
        }

        public override void show(bool giveFocus, float alpha) {
            base.show(giveFocus, alpha);
            Scale = 1;
        }

        private int timer = 0;
        public override void update(GameTime time) {
            if(!IsFocused && IsHidden) return;
            base.update(time);
            timer += time.ElapsedGameTime.Milliseconds;

            if(Map != null) {
                if(Map.Slave != null) {
                    slaveXText.Text = Map.Slave.Position.X.ToString("0.00");
                    slaveYText.Text = Map.Slave.Position.Y.ToString("0.00");
                }

                mapWidthText.Text = Map.Width.ToString();
                mapHeightText.Text = Map.Height.ToString();
                entitiesText.Text = Map.Entities.Count.ToString();
                projectilesText.Text = Map.Entities
                    .Where(e => e is Projectile)
                    .Count().ToString();
                animationsText.Text = Map.Animations.Count.ToString();
            }
        }

        private FixedList<int> lastFPS = new FixedList<int>(60);
        public override void draw() {
            base.draw();
            lastFPS.Add((int)(1000f/(timer == 0 ? 1000 : timer)));
            fpsText.Text = (lastFPS.Sum()/lastFPS.Count).ToString("0.00");
            timer = 0;
        }
    }
}