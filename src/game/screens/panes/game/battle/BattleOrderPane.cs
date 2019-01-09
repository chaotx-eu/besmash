namespace BesmashGame {
    using GSMXtended;
    using BesmashContent;
    using Microsoft.Xna.Framework;
    
    public class BattleOrderPane : BesmashMenuPane {
        public BattleManager BattleManager {get; private set;}
        public int MaxParticipants {get; protected set;} = 8;
        public ImageItem[] Thumbnails {get; protected set;}

        public BattleOrderPane(BattleManager battleManager) {
            HPane hpMain = new HPane();
            hpMain.PercentWidth = hpMain.PercentHeight = 100;

            BattleManager = battleManager;
            Thumbnails = new ImageItem[MaxParticipants];

            HPane gapL = new HPane();
            gapL.PercentWidth = 5;
            hpMain.add(gapL);

            // TODO
            // for(int i = 0; i < MaxParticipants
            // && i < BattleManager.fightingEntities.Count; ++i) {
            //     Thumbnails[i] = new ImageItem("");
            //     hpMain.add(Thumbnails[i]);
            // }
            add(hpMain);
        }

        public override void show(bool giveFocus, float alpha) {
            base.show(giveFocus, alpha);
            Scale = 1;
        }

        public override void hide(bool takeFocus, float alpha) {
            base.hide(takeFocus, alpha);
            Scale = 0;
        }
    }
} 