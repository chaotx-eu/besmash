namespace BesmashGame {
    using Config;
    using GSMXtended;
    using BesmashContent;
    using GameStateManagement;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using System.Collections.Generic;
    using System;

    public class InputDialog : BesmashDialog {
        private float time;
        private string message;
        private UserInput input;
        private Language lang;
        private TextItem tiTime;
        private TextItem tiMessage;
        private Callback callback;
        private int ignTime = 500;  // time in millis any input
                                    // will be ignored initially

        public InputDialog(BesmashScreen parent, UserInput input)
        : this(parent, input, "Press a new Key or Button") {}

        public InputDialog(BesmashScreen parent, UserInput input,
        string message) : this(parent, input, message, 4) {}

        public InputDialog(BesmashScreen parent, UserInput input,
        string message, int time) : this(parent, input, message, time, a => {}) {}

        public InputDialog(BesmashScreen parent, UserInput input,
        string message, int time, Callback callback) : base(parent) {
            this.time = time;
            this.input = input;
            this.message = message;
            this.callback = callback;

            tiTime = new TextItem(time + "", "fonts/menu_font1");
            tiMessage = new TextItem("", "fonts/menu_font1");

            // TODO make this somehow accessible from outside
            VPane vpMain = new VPane(tiMessage, tiTime);
            vpMain.PercentWidth = 100; // TODO default should be 100
            vpMain.PercentHeight = 100;
            vpMain.HAlignment = HAlignment.Right; // TODO default should be centered

            MainContainer.Color = Color.Black;
            MainContainer.Alpha = 0.7f;
            MainContainer.add(vpMain);
        }

        public override void LoadContent() {
            base.LoadContent();
            tiMessage.Text = GameManager
                .Configuration.Language
                .translate(message, true);
        }

        /// Updates the timer text, decrements the time
        /// and closes the screen in case it reaches 0
        public override void Update(GameTime gameTime,
        bool otherScreenHasFocus, bool coveredByOtherScreen) {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            if(ignTime > 0) ignTime -= gameTime.ElapsedGameTime.Milliseconds;
            if(ignTime <= 0) { // we start the time as soon as the ignTime runs out
                time -= gameTime.ElapsedGameTime.Milliseconds/1000f;
                if(time <= 0) {
                    Alpha = 0;
                    ExitScreen();
                    callback(0);
                }

                tiTime.Text = (int)(time+0.5f) + "";
            }
        }

        /// Sets the pressed key or button for the input object
        /// and exits the screen on success
        public override void HandleInput(InputState inputState) {
            if(ignTime > 0) return; // input will be ignored until ignTime runs out
            if(inputState.CurrentKeyboardStates.Length > 0
            && inputState.CurrentKeyboardStates[0].GetPressedKeys().Length > 0) {
                input.TriggerKeys = new List<Keys>(); // current only 1 key supported (TODO)
                input.TriggerKeys.Add(inputState
                    .CurrentKeyboardStates[0]
                    .GetPressedKeys()[0]);
                Alpha = 0;
                ExitScreen();
                callback(0);
            } else if(inputState.CurrentGamePadStates.Length > 0) {
                GamePadState gamePadeState = inputState.CurrentGamePadStates[0];
                foreach(Buttons b in Enum.GetValues(typeof(Buttons))) {
                    if(gamePadeState.IsButtonDown(b)) {
                        input.TriggerButtons = new List<Buttons>(); // currently only 1 button supported (TODO)
                        input.TriggerButtons.Add(b);
                        Alpha = 0;
                        ExitScreen();
                        callback(0);
                        break;
                    }
                }
            }
        }
    }
}