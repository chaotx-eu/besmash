namespace BesmashGame.Config {
    using System;
    using System.Runtime.Serialization;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    
    public class GameConfig {
        /// Commonly used resolutions
        public static Point[] CommonResolutions = {
            new Point(1280, 720),
            new Point(1920, 1028),
            new Point(2560, 1440)
        };

        /// Volume of sound effexts in percent
        public int SFXVolume {get; set;} = 70;

        /// Volume of music in percent
        public int MusicVolume {get; set;} = 60;

        /// Screen resolution
        public Point Resolution {get; set;} = CommonResolutions[0];

        /// If screen should be shown in fullscreen
        public bool IsFullscreen {get; set;} = false;

        /// Game language
        public string Language {get; set;} = "en";

        /// Key Map for user input
        [IgnoreDataMember] // TODO
        public KeyMap KeyMap {get; set;} = new KeyMap(); // Key Map

        public GameConfig() {
            // default keymap (TODO)
            // KeyMap.addAction(Keys.Up, () => {});
        }
    }
}