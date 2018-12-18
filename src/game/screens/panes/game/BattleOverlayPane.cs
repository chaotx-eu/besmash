namespace BesmashGame {
    using GSMXtended;
    using Microsoft.Xna.Framework;

    public class BattleOverlayPane : VPane {
        private TeamInfoPane tiPane;

        public BattleOverlayPane(SaveState activeSave) {
            PercentWidth = PercentHeight = 100;

            // team info pane
            tiPane = new TeamInfoPane(activeSave.Team);
            tiPane.HAlignment = HAlignment.Right;
            tiPane.VAlignment = VAlignment.Bottom;
            tiPane.PercentWidth = 35;
            tiPane.PercentHeight = 25;
            tiPane.Color = Color.Black;
            tiPane.Alpha = 0.5f;

            // player action pane

            // action info pane

            // battle order pane

            // map object info pane

            // put it all together
            add(tiPane);
        }

        public override void load() {
            base.load();
            hide();
        }

        private float tiAlpha;
        private bool isHidden;
        public void show() {
            Container.applyAlpha(tiPane, 1);
            tiPane.Alpha = tiAlpha;
            isHidden = false;
        }

        public void hide() {
            if(!isHidden) {
                tiAlpha = tiPane.TargetAlpha;
                Container.applyAlpha(tiPane, 0);
                isHidden = true;
            }
        }
    }
}