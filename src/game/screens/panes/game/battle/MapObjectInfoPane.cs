namespace BesmashGame {
    using GSMXtended;
    using BesmashContent;
    using Microsoft.Xna.Framework;

    public class MapObjectInfoPane : BesmashMenuPane {
        public Cursor MapCursor {get; set;}

        private TextItem tiType;
        private TextItem tiLife;
        private TextItem tiInfo;
        private TextItem[] textItems;

        public MapObjectInfoPane() : this(null) {}
        public MapObjectInfoPane(Cursor cursor) {
            VPane vpMain = new VPane();
            vpMain.PercentWidth = vpMain.PercentHeight = 100;
            vpMain.PPSFactor = 1000;

            MapCursor = cursor;
            tiType = new TextItem("", "fonts/game_font1");
            tiLife = new TextItem("", "fonts/game_font1");
            tiInfo = new TextItem("", "fonts/game_font1");
            textItems = new TextItem[]{tiType, tiLife, tiInfo};

            foreach(TextItem ti in textItems) {
                HPane gapL = new HPane();
                HPane cent = new HPane(gapL, ti);
                ti.HAlignment = HAlignment.Left;
                ti.DefaultScale = 0.5f;
                ti.PPSFactor = 1000;
                cent.PercentWidth = 100;
                gapL.PercentWidth = 5;
                gapL.HAlignment = HAlignment.Left;
                vpMain.add(cent);
            }

            add(vpMain);
        }

        public override void show(bool giveFocus, float alpha) {
            base.show(giveFocus, alpha);
            setPosition(ParentScreen.Width, ParentScreen.Height);
            foreach(TextItem ti in textItems)
                ti.setPosition(ParentScreen.Width, ParentScreen.Height);
                
            Scale = 1;
        }

        public override void hide(bool takeFocus, float alpha) {
            base.hide(takeFocus, alpha);
            Scale = 0;
        }

        public override void update(GameTime gameTime) {
            if(!IsFocused && IsHidden || MapCursor == null) return;
            base.update(gameTime); 
            MapObject mapObject = MapCursor.getObject();
            if(mapObject == null) return;

            string type = "Type:  " + mapObject.GetType().ToString();
            string life = "", info = "";

            if(mapObject is Creature) {
                Creature creature = (Creature)mapObject;
                life = string.Format("HP: {0:0000}/{1:0000}, AP: {2:000}/{3:000}",
                    creature.HP, creature.MaxHP, creature.AP, creature.MaxAP);

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