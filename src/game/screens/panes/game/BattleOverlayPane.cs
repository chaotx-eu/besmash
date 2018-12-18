namespace BesmashGame {
    using GSMXtended;
    using Microsoft.Xna.Framework;

    public class BattleOverlayPane : StackPane {
        private TeamInfoPane teamInfoPane;
        private ActionInfoPane actionInfoPane;
        private MapObjectInfoPane mapObjectInfoPane;
        private BattleOrderPane battleOrderPane;
        private PlayerActionPane playerActionPane;

        public enum OverlayMode {Hidden, Action, Selection}
        public OverlayMode Mode {get; protected set;}

        public BattleOverlayPane(SaveState activeSave) {
            PercentWidth = PercentHeight = 100;

            // team info pane
            teamInfoPane = new TeamInfoPane(activeSave.Team);
            teamInfoPane.HAlignment = HAlignment.Right;
            teamInfoPane.VAlignment = VAlignment.Bottom;
            teamInfoPane.PercentWidth = 35;
            teamInfoPane.PercentHeight = 25;
            teamInfoPane.Color = Color.Black;
            teamInfoPane.Alpha = 0.5f;

            // player action pane

            // action info pane
            actionInfoPane = new ActionInfoPane("Observe");
            actionInfoPane.HAlignment = HAlignment.Left;
            actionInfoPane.VAlignment = VAlignment.Bottom;
            actionInfoPane.PercentWidth = 40;
            actionInfoPane.PercentHeight = 10;
            actionInfoPane.Color = Color.Black;
            actionInfoPane.Alpha = 0.5f;

            // battle order pane
            battleOrderPane = new BattleOrderPane(activeSave.Game.BattleManager);
            battleOrderPane.HAlignment = HAlignment.Left;
            battleOrderPane.VAlignment = VAlignment.Top;
            battleOrderPane.PercentWidth = 40;
            battleOrderPane.PercentHeight = 10;
            battleOrderPane.Color = Color.Black;
            battleOrderPane.Alpha = 0.5f;

            // map object info pane
            mapObjectInfoPane = new MapObjectInfoPane(activeSave.ActiveMap.Cursor);
            mapObjectInfoPane.HAlignment = HAlignment.Right;
            mapObjectInfoPane.VAlignment = VAlignment.Bottom;
            mapObjectInfoPane.PercentWidth = 25;
            mapObjectInfoPane.PercentHeight = 20;
            mapObjectInfoPane.Color = Color.Black;
            mapObjectInfoPane.Alpha = 0.5f;

            // put it all together
            add(teamInfoPane);
            add(actionInfoPane);
            add(battleOrderPane);
            add(mapObjectInfoPane);
        }
        
        private float tiAlpha;
        private float aiAlpha;
        private float miAlpha;
        private float boAlpha;
        private bool isHidden;

        public override void load() {
            base.load();
            tiAlpha = teamInfoPane.TargetAlpha;
            aiAlpha = actionInfoPane.TargetAlpha;
            miAlpha =  mapObjectInfoPane.TargetAlpha;
            boAlpha = battleOrderPane.TargetAlpha;
            hide();
        }

        public void show() {
            if(Mode == OverlayMode.Selection) {
                Container.applyAlpha(teamInfoPane, 1);
                Container.applyAlpha(battleOrderPane, 1);
                teamInfoPane.Alpha = tiAlpha;
                battleOrderPane.Alpha = boAlpha;
                // TODO playerActionPane
            } else if(Mode == OverlayMode.Action) {
                Container.applyAlpha(actionInfoPane, 1);
                Container.applyAlpha(mapObjectInfoPane, 1);
                actionInfoPane.Alpha = aiAlpha;
                mapObjectInfoPane.Alpha = miAlpha;
            }

            isHidden = false;
        }

        public void hide() {
            if(!isHidden) {
                Container.applyAlpha(teamInfoPane, 0);
                Container.applyAlpha(actionInfoPane, 0);
                Container.applyAlpha(mapObjectInfoPane, 0);
                Container.applyAlpha(battleOrderPane, 0);
                isHidden = true;
            }
        }

        /// Toggles the next mode (hidden -> selection -> action)
        public void toggleMode() {
            if(Mode == OverlayMode.Hidden)
                toggleMode(OverlayMode.Selection);
            else if(Mode == OverlayMode.Selection)
                toggleMode(OverlayMode.Action);
            else Mode = OverlayMode.Hidden;
        }

        /// Sets this pane to the passed mode
        public void toggleMode(OverlayMode mode) {
            Mode = mode;
            hide();
            show();
        }
    }
}