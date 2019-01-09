namespace BesmashGame {
    using GSMXtended;
    using BesmashContent;
    using Microsoft.Xna.Framework;

    public class MapObjectInfoPane : BesmashMenuPane {
        public Cursor MapCursor {get; private set;}

        private TextItem tiType;
        private TextItem tiLife;
        private TextItem tiInfo;

        public MapObjectInfoPane(Cursor cursor) {
            VPane vpMain = new VPane();
            vpMain.PercentWidth = vpMain.PercentHeight = 100;

            MapCursor = cursor;
            tiType = new TextItem("", "fonts/game_font1");
            tiLife = new TextItem("", "fonts/game_font1");
            tiInfo = new TextItem("", "fonts/game_font1");
            TextItem[] textItems = {tiType, tiLife, tiInfo};

            foreach(TextItem ti in textItems) {
                HPane gapL = new HPane();
                HPane cent = new HPane(gapL, ti);
                ti.HAlignment = HAlignment.Left;
                ti.DefaultScale = 0.5f;
                cent.PercentWidth = 100;
                gapL.PercentWidth = 5;
                gapL.HAlignment = HAlignment.Left;
                vpMain.add(cent);
            }

            add(vpMain);
        }

        public override void show(bool giveFocus, float alpha) {
            base.show(giveFocus, alpha);
            Scale = 1;
        }

        public override void hide(bool takeFocus, float alpha) {
            base.hide(takeFocus, alpha);
            Scale = 0;
        }

        public override void update(GameTime gameTime) {
            if(!IsFocused && IsHidden) return;
            base.update(gameTime); 
            MapObject mapObject = MapCursor.getObject();
            if(mapObject == null) return;

            string type = "Type:  " + mapObject.GetType().ToString();
            string life = "", info = "";

            if(mapObject is Creature) {
                Creature creature = (Creature)mapObject;
                life = string.Format("Life:  {0:0000} / {1:0000}",
                    creature.HP, creature.MaxHP);

                info = "Info:  " + creature.Name
                    + ", Level: " + creature.Level;
            } else if(mapObject is Tile) {
                info = "Info:  " + (((Tile)mapObject).Solid ? "Is Solid" : "Is Passable");
            }

            tiType.Text = type;
            tiLife.Text = life;
            tiInfo.Text = info;
        }
    }
}