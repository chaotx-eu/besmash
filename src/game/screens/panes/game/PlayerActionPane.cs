namespace BesmashGame {
    using GSMXtended;
    using BesmashContent;
    using Microsoft.Xna.Framework;
    using System.Collections.Generic;
    
    public class PlayerActionPane : OverlayPane {
        private BattleOverlayPane battleOverlay;
        private Player player;
        private VList abilityList;

        public VList ActionList {get; protected set;}
        public TargetSelectionPane TargetSelectionPane {get; protected set;}

        private TeamInfoPane teamInfoPane;
        private ActionInfoPane actionInfoPane;
        private MapObjectInfoPane mapObjectInfoPane;
        private ActionListPane actionListPane;


        // Attack
        // Skills
        // Move
        // Defend

        public PlayerActionPane(Player player, SaveState activeSave) {
            this.player = player;
            BackgroundAlpha = 0;

            // target selection
            TargetSelectionPane = new TargetSelectionPane(player.ContainingMap);

            // TODO
            TextItem tiAttack = new TextItem(player.BasicAttack != null ? player.BasicAttack.Title : "FooBar", "fonts/game_font1");
            TextItem tiSkills = new TextItem("Skills", "fonts/game_font1");
            TextItem tiMove = new TextItem("Move", "fonts/game_font1");
            TextItem tiDefend = new TextItem("Defend", "fonts/game_font1");

            tiAttack.DefaultScale = 0.7f;
            tiSkills.DefaultScale = 0.7f;
            tiMove.DefaultScale = 0.7f;
            tiDefend.DefaultScale = 0.7f;

            ActionList = new VList(tiAttack, tiSkills, tiMove, tiDefend);
            ActionList.IsFocused = false;
            
            ActionList.ActionEvent += (sender, args) => {
                List<Point> targets = new List<Point>();
                // TODO (targets should be dependent on the used move)
                targets.Add(new Point((int)player.Position.X+1, (int)player.Position.Y));
                targets.Add(new Point((int)player.Position.X-1, (int)player.Position.Y));
                targets.Add(new Point((int)player.Position.X, (int)player.Position.Y+1));
                targets.Add(new Point((int)player.Position.X, (int)player.Position.Y-1));

                int i = args.SelectedIndex;
                if(i < 4) {
                    actionInfoPane.ActionText.Text = (
                        i == 0 ? "Attack" :
                        i == 1 ? "Skill" :
                        i == 2 ? "Move" :
                        i == 3 ? "Defend" : "Observe");

                    TargetSelectionPane.initGrid(player.ContainingMap, targets);
                    player.ContainingMap.showCursor();
                    TargetSelectionPane.show();

                    teamInfoPane.hide();
                    actionListPane.hide();
                    mapObjectInfoPane.show();
                    actionInfoPane.show();
                }
                
                ActionList.IsFocused = false;
            };

            // action pane
            actionListPane = new ActionListPane(ActionList);
            actionListPane.HAlignment = HAlignment.Left;
            actionListPane.VAlignment = VAlignment.Bottom;
            actionListPane.PercentWidth = 20;
            actionListPane.PercentHeight = 25;

            // map object info pane
            mapObjectInfoPane = new MapObjectInfoPane(activeSave.ActiveMap.Cursor);
            mapObjectInfoPane.HAlignment = HAlignment.Right;
            mapObjectInfoPane.VAlignment = VAlignment.Bottom;
            mapObjectInfoPane.PercentWidth = 25;
            mapObjectInfoPane.PercentHeight = 20;

            // team info pane
            teamInfoPane = new TeamInfoPane(activeSave.Team);
            teamInfoPane.HAlignment = HAlignment.Right;
            teamInfoPane.VAlignment = VAlignment.Bottom;
            teamInfoPane.PercentWidth = 40;
            teamInfoPane.PercentHeight = 25;

            // action info pane
            actionInfoPane = new ActionInfoPane("Observe");
            actionInfoPane.HAlignment = HAlignment.Left;
            actionInfoPane.VAlignment = VAlignment.Bottom;
            actionInfoPane.PercentWidth = 40;
            actionInfoPane.PercentHeight = 10;

            add(actionListPane);
            add(teamInfoPane);
            add(actionInfoPane);
            add(mapObjectInfoPane);
        }

        public override void hide() {
            base.hide();
            teamInfoPane.hide();
            actionListPane.hide();
            actionInfoPane.hide();
            mapObjectInfoPane.hide();
            TargetSelectionPane.hide();
        }

        public override void show() {
            base.show();
            teamInfoPane.show();
            actionListPane.show();
            actionInfoPane.hide();
            mapObjectInfoPane.hide();
            TargetSelectionPane.hide();

            ActionList.IsFocused = true;
        }

        public override void update(GameTime time) {
            base.update(time);

            if(TargetSelectionPane.IsActive) {
                Besmash game = (Besmash)((BesmashScreen)ParentScreen).ScreenManager.Game;
                Point pos = player.ContainingMap.Cursor.Position.ToPoint();

                if(game.isActionTriggered("game", "cancel")) {
                    player.ContainingMap.hideCursor();
                    hide();
                    show();
                }

                if(game.isActionTriggered("game", "interact")
                && TargetSelectionPane.Targets.Contains(pos)) {
                    // TODO trigger action
                    // TargetSelectionPane.hide();
                    int x = 42;
                }
            }
        }
    }
}