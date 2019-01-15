

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
    using System;

    public class Besmash : Game {
        /// Manager of this game
        public GameManager Manager {get; set;}

        /// Manager for handling battle logic
        public BattleManager BattleManager {get; set;}

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
                Manager.ActiveSave.load(this);
        }

        /// Loads the game manager and the config
        protected override void LoadContent() {
            Manager = GameManager.newInstance();
            BattleManager = BattleManager.newInstance();
            mainMenu.GameManager = Manager;
            // loadConfig(); // moved to update
            // loadSave();
        }

        private HashSet<Keys> pressedKeys = new HashSet<Keys>();
        private HashSet<Buttons> pressedButtons = new HashSet<Buttons>();
        private Dictionary<Keys, TimeSpan> keyTimerMap = new Dictionary<Keys, TimeSpan>();
        private Dictionary<Buttons, TimeSpan> buttonTimerMap = new Dictionary<Buttons, TimeSpan>();
        
        /// Overload for convenience, always checks input for
        /// the gamepad at index 0 with ongoing set to false
        public bool isActionTriggered(string context, string action) {
            return isActionTriggered(context, action, false);
        }

        /// Overload for convenience, always checks
        /// input for the gamepad at index 0
        public bool isActionTriggered(string context, string action, bool ongoing) {
            return isActionTriggered(context, action, ongoing, 0);
        }

        /// Overload for convenience. Check input with idleTime, gamePad
        /// index will always be 0
        public bool isActionTriggered(string context, string action, int idleTime) {
            return isActionTriggered(context, action, idleTime, 0);
        }

        /// Checks wether an action is triggered but will return false in any
        /// case for following calls until idleTime milliseconds have passed
        public bool isActionTriggered(string context, string action, int idleTime, int gamepadIndex) {
            if(Manager.Configuration.KeyMaps.ContainsKey(context)
            && Manager.Configuration.KeyMaps[context].ContainsKey(action)) {
                foreach(Keys key in Manager.Configuration
                .KeyMaps[context][action].TriggerKeys) {
                    if(Keyboard.GetState().IsKeyDown(key)) {
                        if(!keyTimerMap.ContainsKey(key)) {
                            keyTimerMap.Add(key, gameTime.TotalGameTime);
                            return true;
                        }

                        if(gameTime.TotalGameTime
                        .Subtract(keyTimerMap[key])
                        .TotalMilliseconds >= idleTime) {
                            keyTimerMap[key] = gameTime.TotalGameTime;
                            return true;
                        }
                    }
                }

                foreach(Buttons button in Manager.Configuration
                .KeyMaps[context][action].TriggerButtons) {
                    if(GamePad.GetState(gamepadIndex).IsButtonDown(button)) {
                        if(!buttonTimerMap.ContainsKey(button)) {
                            buttonTimerMap.Add(button, gameTime.TotalGameTime);
                            return true;
                        }

                        if(gameTime.TotalGameTime
                        .Subtract(buttonTimerMap[button])
                        .TotalMilliseconds >= idleTime) {
                            buttonTimerMap[button] = gameTime.TotalGameTime;
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// Checks wether an action key or button, defined in the
        /// current game config of the game manager is pressed.
        /// If ongoing is true the function will return true
        /// whenever this function is called while the key/button
        /// is held down otherwise it will return true only on its
        /// first call and false until the pressed key/button is
        /// released and then pressed again
        public bool isActionTriggered(string context, string action, bool ongoing, int gamepadIndex) {
            if(Manager.Configuration.KeyMaps.ContainsKey(context)
            && Manager.Configuration.KeyMaps[context].ContainsKey(action)) {
                foreach(Keys key in Manager.Configuration
                .KeyMaps[context][action].TriggerKeys) {
                    bool keyPressed = Keyboard.GetState().IsKeyDown(key);

                    if(keyPressed && (ongoing || !pressedKeys.Contains(key))) {
                        pressedKeys.Add(key);
                        return true;
                    }

                    if(!keyPressed)
                        pressedKeys.Remove(key);
                }

                foreach(Buttons button in Manager.Configuration
                .KeyMaps[context][action].TriggerButtons) {
                    bool buttonPressed = GamePad.GetState(gamepadIndex).IsButtonDown(button);
                    if(buttonPressed && (ongoing || !pressedButtons.Contains(button))) {
                        pressedButtons.Add(button);
                        return true;
                    }

                    if(!buttonPressed)
                        pressedButtons.Remove(button);
                }
            }

            return false;
        }

        protected override void Initialize() {
            base.Initialize();
        }

        // temporary (real update in ScreenManager)
        private GameTime gameTime;
        protected override void Update(GameTime gameTime) {
            // if(Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();
            base.Update(gameTime);
            this.gameTime = gameTime;

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
