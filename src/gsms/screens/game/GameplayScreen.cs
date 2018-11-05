namespace BesmashGame.Screens {
    using BesmashContent;
    using BesmashGame.Config;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Content;
    using GameStateManagement;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    internal class GameplayScreen : GameScreen {
        private ContentManager content;

        /// Game that is currently played on this screen.
        public Besmash Game {get; set;}

        // Reference to the currently active map in the game.
        public TileMap ActiveMap {get; set;}

        /// Reference to the GameManager of the Game
        public GameManager Manager {get; set;}

        public GameplayScreen() {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void LoadContent() {
            if(content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            Game = ((Besmash)ScreenManager.Game);
            Manager = Game.Manager;
            Manager.ActiveSave.load(content, Game);
            ActiveMap = Manager.ActiveSave.ActiveMap;

            // init default keys (demo)
            CollisionResolver teamCR = (x, y, mv) => {
                if(mv is Entity && !(mv is Player)
                || mv is Tile && ((Tile)mv).Solid)
                    return Point.Zero;

                return null;
            };

            GameConfig config = Manager.Configuration;
            config.KeyMap.addAction(Keys.Up, () => ActiveMap.Slave.move(0, -1, teamCR));
            config.KeyMap.addAction(Keys.Right, () => ActiveMap.Slave.move(1, 0, teamCR));
            config.KeyMap.addAction(Keys.Down, () => ActiveMap.Slave.move(0, 1, teamCR));
            config.KeyMap.addAction(Keys.Left, () => ActiveMap.Slave.move(-1, 0, teamCR));

            ScreenManager.Game.ResetElapsedTime();
        }

        public override void UnloadContent() {
            content.Unload();
        }

        private float pauseAlpha;

        public override void Update(GameTime gameTime, bool otherFocused, bool covered) {
            base.Update(gameTime, otherFocused, false);

            if (covered) pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            if(IsActive) ActiveMap.update(gameTime);
        }

        public override void HandleInput(InputState input) {
            int playerIndex = (int)ControllingPlayer.Value;
            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
            GamePadState gamepadState = input.CurrentGamePadStates[playerIndex];

            bool padDisconnected = !gamepadState.IsConnected
                && input.GamePadWasConnected[playerIndex];

            if(input.IsPauseGame(ControllingPlayer) || padDisconnected)
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            else {
                Besmash game = (Besmash)ScreenManager.Game;
                KeyMap keyMap = game.Manager.Configuration.KeyMap;
                
                foreach(Keys key in keyMap.KeyStates.Keys) {
                    if(keyboardState.IsKeyDown(key))
                        keyMap.KeyStates[key]();
                }
            }
        }

        public override void Draw(GameTime gameTime) {
            ScreenManager.GraphicsDevice.Clear(Color.Black);
            ActiveMap.draw(ScreenManager.SpriteBatch);

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0) {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha/2);
                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }
    }
}