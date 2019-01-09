namespace BesmashGame {
    using GSMXtended;
    using Microsoft.Xna.Framework;

    public class BattleOverlayPane : BesmashMenuPane {
        private BattleOrderPane battleOrderPane;
        private PlayerActionPane playerActionPane;

        public BattleOverlayPane(SaveState activeSave) {
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
            StackPane sp = new StackPane(
                battleOrderPane,
                playerActionPane,
                playerActionPane.TargetSelectionPane
            );

            add(sp);
            sp.PercentWidth = sp.PercentHeight = 100;
            PercentWidth = PercentHeight = 100;
        }

        public override void show(bool giveFocus, float alpha) {
            base.show(giveFocus, alpha);
            battleOrderPane.show(giveFocus, alpha);
            playerActionPane.show(giveFocus, alpha);
        }

        public override void hide(bool takeFocus, float alpha) {
            base.hide(takeFocus, alpha);
            battleOrderPane.hide(takeFocus, alpha);
            playerActionPane.hide(takeFocus, alpha);
        }
    }
}