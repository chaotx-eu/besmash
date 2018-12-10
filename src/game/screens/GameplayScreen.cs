namespace BesmashGame {
    using Config;
    using BesmashContent;
    using GSMXtended;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using GameStateManagement;
    using System.Linq;

    public class GameplayScreen : BesmashScreen {
        // reference to the GameConfig object
        private GameConfig Config {get; set;}

        // reference to the Game object
        private Besmash Game {get; set;}

        // reference to the active loaded map
        private TileMap Map {get; set;}

        // reference to the active team
        private Team Team {get; set;}

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
            Config = GameManager.Configuration;
            Game = (Besmash)ScreenManager.Game;
            Map = GameManager.ActiveSave.ActiveMap;
            Team = GameManager.ActiveSave.Team;
        }

        public override void Update(GameTime gameTime,
        bool otherScreenHasFocus, bool coveredByOtherScreen) {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            GameManager.ActiveSave.ActiveMap.update(gameTime);
        }

        public override void Draw(GameTime gameTime) {
            base.Draw(gameTime);
            GameManager.ActiveSave.ActiveMap.draw(ImageBatch);
        }

        /// Handling user input, e.g. moving player, interacting
        /// with objects or opening menus (moving through menus
        /// is handled internally by the GSMXtended lib) (TODO)
        public override void HandleInput(InputState inputState) {
            PlayerIndex player;

            // test
            CollisionResolver cr = (x, y, mo) => {
                if(mo is Tile && ((Tile)mo).Solid
                || mo is Entity && !Team.Members.Contains(mo))
                    return Point.Zero;

                return null;
            };

            if(Game.isActionTriggered("game", "move_up")) Map.Slave.move(0, -1, cr);
            if(Game.isActionTriggered("game", "move_right")) Map.Slave.move(1, 0, cr);
            if(Game.isActionTriggered("game", "move_down")) Map.Slave.move(0, 1, cr);
            if(Game.isActionTriggered("game", "move_left")) Map.Slave.move(-1, 0, cr);
            if(Game.isActionTriggered("game", "menu")) {
                // quit(true);
                // return;
                // ScreenManager.AddScreen(new SettingsScreen(this), null);
                ScreenManager.AddScreen(new GameMenuScreen(this), null);
            }

            // interaction
            // Config.KeyMaps["game"]["interact"]

            // open game menu
            // Config.KeyMaps["game"]["menu"]

            // open other menus/guis?
        }

        // closes the screen and may save the game
        public void quit(bool save) {
            if(save) GameManager.save();
            // TODO
            // LoadingScreen.Load(ScreenManager, true, null,
            //     new BackgroundScreen("images/blank"),
            //     new MainMenuScreen(GameManager));
            // ScreenManager.AddScreen(new MainMenuScreen(GameManager), null);
            Alpha = 0;
            ExitScreen();
        }
    }
}