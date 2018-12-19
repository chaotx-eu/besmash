namespace BesmashGame {
    using GSMXtended;
    using BesmashGame.Config;
    using Microsoft.Xna.Framework;
    
    public class BattleOrderPane : HPane {
        public BattleManager BattleManager {get; private set;}
        public int MaxParticipants {get; protected set;} = 8;
        public ImageItem[] Thumbnails {get; protected set;}

        public BattleOrderPane(BattleManager battleManager) {
            BattleManager = battleManager;
            Thumbnails = new ImageItem[MaxParticipants];

            HPane gapL = new HPane();
            gapL.PercentWidth = 5;
            add(gapL);

            // for(int i = 0; i < MaxParticipants
            // && i < BattleManager.fightingEntities.Count; ++i) {
            //     Thumbnails[i] = new ImageItem("");
            //     add(Thumbnails[i]);
            // }
        }

        public void update(GameTime gameTime) {
            base.update(gameTime);

            // for(int i = 0; i < MaxParticipants
            // && i < BattleManager.fightingEntities.Count; ++i) {
            //     Thumbnails[i].Image = BattleManager
            //         .fightingEntities[i]
            //         .Creature.Thumbnail; // TODO
            // }
        }
    }
} 