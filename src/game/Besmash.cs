

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
        /// Manager of this game
        public GameManager Manager {get; set;}
        public BattleManager Battle{get; set;}

        /// Flag which indicates that the game
        /// configuration has changed
        public bool ConfigChanged {get; set;} = true;

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

            mainMenu = new MainMenuScreen("images/blank", null);
            screenManager = new ScreenManager(this);
            LoadingScreen.Load(screenManager, false, null,
                new BackgroundScreen("images/menu/main_background"), mainMenu);

            Components.Add(screenManager);
        }

        /// Loads required resources and sets
        /// all necessery properties to match
        /// the current game configuration
        public void loadConfig() {
            GameConfig config = Manager.Configuration;
            config.load(Content);

            // set resolution
            graphics.PreferredBackBufferWidth = config.Resolution.X;
            graphics.PreferredBackBufferHeight = config.Resolution.Y;
            graphics.IsFullScreen = config.IsFullscreen;
            graphics.ApplyChanges();

            // set sfx volume
            // TODO

            // set music volume
            // TODO
        }

        /// Loads required resources for the
        /// current active save state in the
        /// game manager (i.e. active map)
        public void loadSave() {
            if(Manager != null && Manager.ActiveSave != null)
            {
                Manager.ActiveSave.load(this);
                Battle = BattleManager.newInstance();
                if(Manager.ActiveSave.ActiveMap != null)
                    Battle.map = Manager.ActiveSave.ActiveMap;
            }
        }

        /// Loads the game manager and the config
        protected override void LoadContent() {
            Manager = GameManager.newInstance();
            Battle = BattleManager.newInstance();
            mainMenu.GameManager = Manager;
            // loadConfig(); // moved to update
            // loadSave();
        }

        /// Checks wether an action key or button, defined in the
        /// current game config of the game manager, is pressed
        public bool isActionTriggered(string context, string action, int gamepadIndex) {
            if(Manager.Configuration.KeyMaps.ContainsKey(context)
            && Manager.Configuration.KeyMaps[context].ContainsKey(action)) {
                foreach(Keys key in Manager.Configuration.KeyMaps[context][action].TriggerKeys)
                    if(Keyboard.GetState().IsKeyDown(key))
                        return true;

                foreach(Buttons button in Manager.Configuration.KeyMaps[context][action].TriggerButtons)
                    if(GamePad.GetState(gamepadIndex).IsButtonDown(button))
                        return true;
            }

            return false;
        }

        /// Overload for convenience, always checks
        /// input for the gamepad at index 0
        public bool isActionTriggered(string context, string action) {
            return isActionTriggered(context, action, 0);
        }

        protected override void Initialize() {
            base.Initialize();
        }

        // temporary (real update in ScreenManager)
        protected override void Update(GameTime gameTime) {
            // if(Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();
            base.Update(gameTime);

            if(ConfigChanged) {
                loadConfig();
                ConfigChanged = false;
            }
        }

        // The real drawing happens inside the screen manager component.
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }
    }
}
