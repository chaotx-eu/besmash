namespace BesmashGame.Config {
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Content;
    using BesmashContent;
    
    public class GameConfig {
        /// Commonly used resolutions
        public static Point[] CommonResolutions = {
            new Point(1280, 720),
            new Point(1920, 1028),
            new Point(2560, 1440)
        };

        /// Languages supported by the game
        public static Language[] SupportedLanguages = {
            new Language("en", "English", "lang/en"),
            new Language("de", "Deutsch", "lang/de")
        };

        /// Volume of sound effexts in percent
        public int SFXVolume {get; set;} = 70;

        /// Volume of music in percent
        public int MusicVolume {get; set;} = 60;

        /// Screen resolution
        public Point Resolution {get; set;} = CommonResolutions[0];

        /// If screen should be shown in fullscreen
        public bool IsFullscreen {get; set;} = false;

        /// Active Game language
        public Language Language {get; set;} = SupportedLanguages[0];

        /// Holds keys and buttons for user actions
        public Dictionary<string ,Dictionary<string, UserInput>> KeyMaps {get;}

        /// Creates a new game config object with a
        /// default key map configuration
        public static GameConfig createDefault() {
            return new GameConfig(true);
        }

        /// Creates an empty GameConfig object intended
        /// to be used for desirialization. Rather use
        /// createDefault for creating new instances
        public GameConfig() {
            KeyMaps = new Dictionary<string, Dictionary<string, UserInput>>();
        }

        /// Creates a new game config instance with its
        /// properties set to the ones from other
        public GameConfig(GameConfig other) : this() {
            SFXVolume = other.SFXVolume;
            MusicVolume = other.MusicVolume;
            Resolution = other.Resolution;
            IsFullscreen = other.IsFullscreen;
            Language = other.Language;

            foreach(string key in other.KeyMaps.Keys) {
                KeyMaps.Add(key, new Dictionary<string, UserInput>());
                foreach(string key2 in other.KeyMaps[key].Keys)
                    KeyMaps[key].Add(key2, new UserInput(other.KeyMaps[key][key2]));
            }
        }

        /// No public access, use createDefault instead
        private GameConfig(bool not_used) : this() {
            // Default menu action- keys and buttons
            KeyMaps.Add("menu", new Dictionary<string, UserInput>());
            KeyMaps["menu"].Add("menu_up", new UserInput("menu.menu_up"));
            KeyMaps["menu"].Add("menu_right", new UserInput("menu.menu_right"));
            KeyMaps["menu"].Add("menu_down", new UserInput("menu.menu_down"));
            KeyMaps["menu"].Add("menu_left", new UserInput("menu.menu_left"));
            KeyMaps["menu"].Add("menu_confirm", new UserInput("menu.menu_confirm"));
            KeyMaps["menu"].Add("menu_cancel", new UserInput("menu.menu_cancel"));
            KeyMaps["menu"].Add("menu_close", new UserInput("menu.menu_close"));

            KeyMaps["menu"]["menu_up"].TriggerKeys.Add(Keys.Up);
            KeyMaps["menu"]["menu_right"].TriggerKeys.Add(Keys.Right);
            KeyMaps["menu"]["menu_down"].TriggerKeys.Add(Keys.Down);
            KeyMaps["menu"]["menu_left"].TriggerKeys.Add(Keys.Left);
            KeyMaps["menu"]["menu_confirm"].TriggerKeys.Add(Keys.Enter);
            KeyMaps["menu"]["menu_cancel"].TriggerKeys.Add(Keys.Back);
            KeyMaps["menu"]["menu_close"].TriggerKeys.Add(Keys.Escape);

            KeyMaps["menu"]["menu_up"].TriggerButtons.Add(Buttons.DPadUp);
            KeyMaps["menu"]["menu_up"].TriggerButtons.Add(Buttons.LeftThumbstickUp);
            KeyMaps["menu"]["menu_right"].TriggerButtons.Add(Buttons.DPadRight);
            KeyMaps["menu"]["menu_right"].TriggerButtons.Add(Buttons.LeftThumbstickRight);
            KeyMaps["menu"]["menu_down"].TriggerButtons.Add(Buttons.DPadDown);
            KeyMaps["menu"]["menu_down"].TriggerButtons.Add(Buttons.LeftThumbstickDown);
            KeyMaps["menu"]["menu_left"].TriggerButtons.Add(Buttons.DPadLeft);
            KeyMaps["menu"]["menu_left"].TriggerButtons.Add(Buttons.LeftThumbstickLeft);
            KeyMaps["menu"]["menu_confirm"].TriggerButtons.Add(Buttons.A);
            KeyMaps["menu"]["menu_cancel"].TriggerButtons.Add(Buttons.B);
            // KeyMaps["menu"]["menu_close"].TriggerButtons.Add(Buttons.X);

            // Default game action- keys and buttons
            KeyMaps.Add("game", new Dictionary<string, UserInput>());
            KeyMaps["game"].Add("move_up", new UserInput("game.move_up"));
            KeyMaps["game"].Add("move_down", new UserInput("game.move_down"));
            KeyMaps["game"].Add("move_left", new UserInput("game.move_left"));
            KeyMaps["game"].Add("move_right", new UserInput("game.move_right"));
            KeyMaps["game"].Add("interact", new UserInput("game.interact"));
            KeyMaps["game"].Add("cancel", new UserInput("game.cancel"));
            KeyMaps["game"].Add("menu", new UserInput("game.menu"));
            // KeyMaps["game"].Add("pause", new UserInput("game.pause"));

            // Default game menu action- keys and buttons
            KeyMaps.Add("game_menu", new Dictionary<string, UserInput>());
            // TODO
        }

        /// Loads required content for configurations (e.g. language)
        public void load(ContentManager content) {
            if(Language.Words.Values.Count == 0) {
                string file = Language.File;
                Language = content.Load<Language>(file);
                Language.File = file;
            }
        }

        public override bool Equals(object obj) {
            if(obj == null || obj.GetType() != this.GetType())
                return false;

            GameConfig conf = (GameConfig)obj;
            return conf.IsFullscreen == IsFullscreen
                && conf.SFXVolume == SFXVolume
                && conf.MusicVolume == MusicVolume
                && conf.Resolution == Resolution
                && conf.Language.Equals(Language)
                && compareKeyMaps(conf.KeyMaps, KeyMaps);
        }

        protected static bool compareKeyMaps(
        Dictionary<string, Dictionary<string, UserInput>> keyMap1,
        Dictionary<string, Dictionary<string, UserInput>> keyMap2) {
            if(keyMap1.Count != keyMap2.Count)
                return false;

            foreach(string key in keyMap1.Keys) {
                if(!keyMap2.ContainsKey(key)
                || keyMap1[key].Count != keyMap2[key].Count)
                    return false;
   
                foreach(string key2 in keyMap1[key].Keys) {
                    if(!keyMap2[key].ContainsKey(key2)
                    || !keyMap1[key][key2].Equals(keyMap2[key][key2]))
                        return false;
                }
            }

            return true;
        }
    }
}