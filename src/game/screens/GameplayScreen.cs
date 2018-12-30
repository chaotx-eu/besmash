namespace BesmashGame {
    using Config;
    using BesmashContent;
    using GSMXtended;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using GameStateManagement;
    using System.Linq;

    public class GameplayScreen : BesmashScreen {
        private BattleOverlayPane battleOverlay;

        public GameplayScreen(BesmashScreen parent)
        : base(parent) {
            MainContainer.PercentWidth = 100;
            MainContainer.PercentHeight = 100;
            // MainContainer.Color = Color.Black;
        }

        public override void LoadContent() {
            battleOverlay = new BattleOverlayPane(GameManager.ActiveSave);
            MainContainer.remove(MainContainer.Children.ToArray());
            MainContainer.add(battleOverlay);
            base.LoadContent();
            TileMap.MapAlpha = 0;
        }
        
        private int millisPerAction = 256;
        private int actionTimer;

        public override void Update(GameTime gameTime,
        bool otherScreenHasFocus, bool coveredByOtherScreen) {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            TileMap.MapAlpha = MainContainer.Alpha;
            GameManager.ActiveSave.update(gameTime);

            Besmash game = (Besmash)ScreenManager.Game;
            if(actionTimer < millisPerAction)
                actionTimer += gameTime.ElapsedGameTime.Milliseconds;
            else if(game.isActionTriggered("game", "move_up")
            || game.isActionTriggered("game", "move_right")
            || game.isActionTriggered("game", "move_down")
            || game.isActionTriggered("game", "move_left")
            || game.isActionTriggered("game", "interact")
            || game.isActionTriggered("game", "inspect")
            || game.isActionTriggered("game", "cancel")
            || game.isActionTriggered("game", "menu"))
                actionTimer = -1;
        }

        public override void Draw(GameTime gameTime) {
            GameManager.ActiveSave.ActiveMap.draw(ImageBatch);
            base.Draw(gameTime);
        }

        /// Handling user input, e.g. moving player, interacting
        /// with objects or opening menus (moving through menus
        /// is handled internally by the GSMXtended lib) (TODO)
        public override void HandleInput(InputState inputState) {
            base.HandleInput(inputState);
            GameConfig config = GameManager.Configuration;
            Besmash game = (Besmash)ScreenManager.Game;
            TileMap map = GameManager.ActiveSave.ActiveMap;
            Team team = GameManager.ActiveSave.Team;

            if(map.Slave != null) {
                if(game.isActionTriggered("game", "move_up")) map.Slave.move(0, -1);
                if(game.isActionTriggered("game", "move_right")) map.Slave.move(1, 0);
                if(game.isActionTriggered("game", "move_down")) map.Slave.move(0, 1);
                if(game.isActionTriggered("game", "move_left")) map.Slave.move(-1, 0);

                if(actionTimer >= 0) return;
                GameManager.ActiveSave.Team.Player.ForEach(p => {
                    if(game.isActionTriggered("game", "cancel"))
                        p.StepTimeMultiplier = 0.7f;
                    else p.StepTimeMultiplier = 1;
                });

                if(game.isActionTriggered("game", "interact")) {
                    int x = (int)map.Slave.Position.X + (
                        map.Slave.Facing == Facing.EAST ? 1 :
                        map.Slave.Facing == Facing.WEST ? -1 : 0);

                    int y = (int)map.Slave.Position.Y + (
                        map.Slave.Facing == Facing.SOUTH ? 1 :
                        map.Slave.Facing == Facing.NORTH ? -1 : 0);

                    map.getTiles(x, y).ForEach(tile => tile.trigger(map.Slave));
                }

                // if(game.isActionTriggered("game", "inspect")) {
                //     if(battleOverlay.IsActive) {
                //         battleOverlay.hide();
                //         GameManager.ActiveSave.ActiveMap
                //             .setRoamingState(GameManager.ActiveSave.Team);
                //     } else {
                //         battleOverlay.show();
                //         GameManager.ActiveSave.ActiveMap
                //             .setFightingState(GameManager.ActiveSave.Team.Leader.Target);
                //     }
                // }
            }

            if(game.isActionTriggered("game", "menu"))
                ScreenManager.AddScreen(new GameMenuScreen(this), null);
        }

        /// Closes the screen and may save the game
        public void quit(bool save) {
            if(save) GameManager.save();
            GameManager.ActiveSave.Content.Unload();
            ExitScreen();
            Alpha = 0;
        }
    }
}