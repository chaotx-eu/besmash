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

        private int actionTimer;
        public override void Update(GameTime gameTime,
        bool otherScreenHasFocus, bool coveredByOtherScreen) {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            TileMap.MapAlpha = MainContainer.Alpha;
            GameManager.ActiveSave.update(gameTime);

            // TODO TEST
            if(actionTimer < 500)
                actionTimer += gameTime.ElapsedGameTime.Milliseconds;
            else if(((Besmash)ScreenManager.Game).isActionTriggered("game", "inspect")) {
                if(battleOverlay.IsActive) {
                    battleOverlay.hide();
                    GameManager.ActiveSave.ActiveMap.setRoamingState(GameManager.ActiveSave.Team);
                } else {
                    battleOverlay.show();
                    GameManager.ActiveSave.ActiveMap.setFightingState(GameManager.ActiveSave.Team.Leader.Target);
                }

                actionTimer = 0;
            }
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
            PlayerIndex player;

            // test
            CollisionResolver cr = (x, y, mo) => {
                if(mo is Tile && ((Tile)mo).Solid
                || mo is Entity && !team.Members.Contains(mo))
                    return Point.Zero;

                return null;
            };

            if(map.Slave != null) {
                if(game.isActionTriggered("game", "move_up")) map.Slave.move(0, -1, cr);
                if(game.isActionTriggered("game", "move_right")) map.Slave.move(1, 0, cr);
                if(game.isActionTriggered("game", "move_down")) map.Slave.move(0, 1, cr);
                if(game.isActionTriggered("game", "move_left")) map.Slave.move(-1, 0, cr);
            }

            if(game.isActionTriggered("game", "menu"))
                ScreenManager.AddScreen(new GameMenuScreen(this), null);

            // // TODO TEST
            // if(game.isActionTriggered("game", "interact")) {
            //     if(GameManager.ActiveSave.ActiveMap.State == TileMap.MapState.Roaming)
            //         GameManager.ActiveSave.ActiveMap.setFightingState(GameManager.ActiveSave.Team);
            //     else
            //         GameManager.ActiveSave.ActiveMap.setRoamingState(GameManager.ActiveSave.Team);
            // }

            // interaction
            // Config.KeyMaps["game"]["interact"]

            // open game menu
            // Config.KeyMaps["game"]["menu"]

            // open other menus/guis?
        }

        // closes the screen and may save the game
        public void quit(bool save) {
            if(save) GameManager.save();
            GameManager.ActiveSave.Content.Unload();
            ExitScreen();
            Alpha = 0;
        }
    }
}