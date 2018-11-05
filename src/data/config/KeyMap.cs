namespace BesmashGame.Config {
    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    /// The context in which user input may be interpreted
    public enum KeyContext {MAIN_MENU, GAME_MENU, GAME_MAP}

    /// Defines a callback action when a specific key is pressed
    public delegate void KeyCallback();

    public class KeyMap {
        private Dictionary<KeyContext, Dictionary<Keys, KeyCallback>> keys
            = new Dictionary<KeyContext, Dictionary<Keys, KeyCallback>>();

        /// Currently set context trigger keys will
        /// be retreived for.
        public KeyContext Context {get; set;}

        /// Dictionary of keys and their callback function
        /// within the current KeyContext.
        public ReadOnlyDictionary<Keys, KeyCallback> KeyStates {
            get {
                if(!keys.ContainsKey(Context))
                    keys[Context] = new Dictionary<Keys, KeyCallback>();

                return new ReadOnlyDictionary<Keys, KeyCallback>(keys[Context]);
            }
        }

        /// Adds an action to this KeyMap which is riggered when
        /// the passed key is pressed in the currently set context.
        public void addAction(Keys key, KeyCallback callback) {
            if(!keys.ContainsKey(Context))
                keys[Context] = new Dictionary<Keys, KeyCallback>();

            keys[Context][key] = callback;
        }
    }
}