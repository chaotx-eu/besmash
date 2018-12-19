namespace BesmashGame {
    using GSMXtended;
    using Microsoft.Xna.Framework;

    public class BattleOverlayPane : OverlayPane {
        private BattleOrderPane battleOrderPane;
        private PlayerActionPane playerActionPane;

        public BattleOverlayPane(SaveState activeSave) {
            BackgroundAlpha = 0;

            // player action pane
            playerActionPane = new PlayerActionPane(activeSave.Team.Leader, activeSave); // TODO

            // battle order pane
            battleOrderPane = new BattleOrderPane(activeSave.Game.BattleManager);
            battleOrderPane.HAlignment = HAlignment.Left;
            battleOrderPane.VAlignment = VAlignment.Top;
            battleOrderPane.PercentWidth = 40;
            battleOrderPane.PercentHeight = 10;
            battleOrderPane.Color = Color.Black;
            battleOrderPane.Alpha = 0.5f;

            // put it all together
            add(battleOrderPane);
            add(playerActionPane);
            add(playerActionPane.TargetSelectionPane);
        }

        public override void show() {
            base.show();
            battleOrderPane.show();
            playerActionPane.show();
        }

        public override void hide() {
            base.hide();
            battleOrderPane.hide();
            playerActionPane.hide();
        }
    }
}