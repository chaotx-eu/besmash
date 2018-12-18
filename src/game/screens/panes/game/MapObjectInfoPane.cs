namespace BesmashGame {
    using GSMXtended;
    using BesmashContent;
    using Microsoft.Xna.Framework;

    public class MapObjectInfoPane : VPane {
        public Cursor MapCursor {get; private set;}

        private TextItem tiType;
        private TextItem tiLife;
        private TextItem tiInfo;

        public MapObjectInfoPane(Cursor cursor) {
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
                add(cent);
            }
        }

        public override void update(GameTime gameTime) {
            base.update(gameTime); 
            MapObject mapObject = MapCursor.getObject();
            if(mapObject == null) return;

            string type = "Type:  " + mapObject.GetType().ToString();
            string life = "", info = "";

            if(mapObject is Creature) {
                Creature creature = (Creature)mapObject;
                life = string.Format("Life:  {0:0000} / {1:0000}",
                    creature.CurrentHP, creature.MaxHP);

                info = "Info:  ";
                if(creature is Player) info += ((Player)creature).Name;
                else  info += "'put creature info here'"; // TODO: creature.Description
            } else if(mapObject is Tile) {
                info = "Info:  " + (((Tile)mapObject).Solid ? "Is Solid" : "Is Passable");
            }

            tiType.Text = type;
            tiLife.Text = life;
            tiInfo.Text = info;
        }
    }
}