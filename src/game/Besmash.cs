

namespace BesmashGame {
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    using GameStateManagement;
    using BesmashContent;
    using BesmashGame.Config;
    using System.Collections.Generic;
    using System.Linq;
    using Screens;

    public class Besmash : Game {
        /// Manager of this game.
        public GameManager Manager {get; set;}

        private string gameStateFile = "";
        private GraphicsDeviceManager graphics;
        private ScreenManager screenManager;
        private SpriteBatch batch;

        public Besmash() : this("") {}
        public Besmash(string gameStateFile) {
            this.gameStateFile = gameStateFile;
            Content.RootDirectory = "Content";
            graphics = new GraphicsDeviceManager(this);

            screenManager = new ScreenManager(this);
            screenManager.AddScreen(new BackgroundScreen("menu/texture/background"), null);
            screenManager.AddScreen(new MainMenuScreen(), null);
            Components.Add(screenManager);
        }

        protected override void LoadContent() {
            Manager = GameManager.newInstance();
            Content.Load<object>("menu/texture/gradient");
        }

        protected override void Initialize() {
            base.Initialize();
            // graphics.PreferredBackBufferWidth = 1280;
            // graphics.PreferredBackBufferHeight = 768;
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
