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
        public GameplayScreen(BesmashScreen parent)
        : base(parent) {
            MainContainer.PercentWidth = 100;
            MainContainer.PercentHeight = 100;
            // MainContainer.Color = Color.Black;
        }

        public override void LoadContent() {
            base.LoadContent();
            // should be loaded alread at this point
            // GameManager.ActiveSave.ActiveMap.load(Content);
            TileMap.MapAlpha = 0;
        }

        public override void Update(GameTime gameTime,
        bool otherScreenHasFocus, bool coveredByOtherScreen) {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            TileMap.MapAlpha = MainContainer.Alpha;
            GameManager.ActiveSave.update(gameTime);
        }

        public override void Draw(GameTime gameTime) {
            base.Draw(gameTime);
            GameManager.ActiveSave.ActiveMap.draw(ImageBatch);
        }

        /// Handling user input, e.g. moving player, interacting
        /// with objects or opening menus (moving through menus
        /// is handled internally by the GSMXtended lib) (TODO)
        public override void HandleInput(InputState inputState) {
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

            if(game.isActionTriggered("game", "move_up")) map.Slave.move(0, -1, cr);
            if(game.isActionTriggered("game", "move_right")) map.Slave.move(1, 0, cr);
            if(game.isActionTriggered("game", "move_down")) map.Slave.move(0, 1, cr);
            if(game.isActionTriggered("game", "move_left")) map.Slave.move(-1, 0, cr);
            if(game.isActionTriggered("game", "menu"))
                ScreenManager.AddScreen(new GameMenuScreen(this), null);

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