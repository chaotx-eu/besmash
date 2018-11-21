

namespace BesmashGame {
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Content;

    using GameStateManagement;
    using BesmashContent;
    using BesmashGame.Config;
    using System.Collections.Generic;
    using System.Linq;

    public class Besmash : Game {
        /// Manager of this game.
        public GameManager Manager {get; set;}

        private string gameStateFile = "";
        private GraphicsDeviceManager graphics;
        private ScreenManager screenManager;
        private SpriteBatch batch;
        private MainMenuScreen mainMenu;

        public Besmash() : this("") {}
        public Besmash(string gameStateFile) {
            this.gameStateFile = gameStateFile;
            Content.RootDirectory = "Content";
            graphics = new GraphicsDeviceManager(this);

            mainMenu = new MainMenuScreen();
            screenManager = new ScreenManager(this);
            // screenManager.AddScreen(new BackgroundScreen("menu/texture/background"), null);
            // screenManager.AddScreen(mainMenu, null);
            LoadingScreen.Load(screenManager, false, null, mainMenu);
            Components.Add(screenManager);
        }

        /// Sets all necessery properties to match
        /// the current game configuration
        public void initConfig() {
            GameConfig config = Manager.Configuration;

            // set resolution
            graphics.PreferredBackBufferWidth = config.Resolution.X;
            graphics.PreferredBackBufferHeight = config.Resolution.Y;
            graphics.IsFullScreen = config.IsFullscreen;
            graphics.ApplyChanges();

            // set sfx volume
            // TODO

            // set music volume
            // TODO

            // set language
            // TODO
        }

        protected override void LoadContent() {
            Manager = GameManager.newInstance();
            Manager.Configuration.load(Content);
            mainMenu.GameManager = Manager;
        }

        protected override void Initialize() {
            base.Initialize();
            initConfig();
        }

        // temporary (real update in ScreenManager)
        protected override void Update(GameTime gameTime) {
            // if(Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();
            base.Update(gameTime);
        }

        // The real drawing happens inside the screen manager component.
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }
    }
}
