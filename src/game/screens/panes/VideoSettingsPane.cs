namespace BesmashGame {
    using Config;
    using GSMXtended;
    using Microsoft.Xna.Framework;

    public class VideoSettingsPane : BesmashMenuPane {
        private HList hlResolutions, hlFullscreen;
        // private GameConfig gameConfig;

        public VideoSettingsPane(GameConfig gameConfig) {
            hlResolutions = new HList();
            hlFullscreen = new HList(
                new TextItem("Enabled", "fonts/menu_font1"),
                new TextItem("Disabled", "fonts/menu_font1")
            );

            foreach(Point res in GameConfig.CommonResolutions)
                hlResolutions.add(new TextItem(resToStr(res), "fonts/menu_font1"));

            VList vlNames = new VList(
                new TextItem("Resolution", "fonts/menu_font1"),
                new TextItem("Fullscreen", "fonts/menu_font1")
            );

            VPane vpSettings = new VPane(hlResolutions, hlFullscreen);

            vlNames.SelectedEvent += (sender, args) => {
                hlResolutions.IsFocused = args.SelectedIndex == 0;
                hlFullscreen.IsFocused = args.SelectedIndex == 1;
            };

            vlNames.CancelEvent += (sender, args) => {
                hlResolutions.IsFocused = false;
                hlFullscreen.IsFocused = false;
                vlNames.select(0);
                hide(0.5f);
            };

            hlResolutions.SelectedEvent += (sender, args)
                => gameConfig.Resolution = GameConfig.CommonResolutions[args.SelectedIndex];

            hlFullscreen.SelectedEvent += (sender, args)
                => gameConfig.IsFullscreen = args.SelectedIndex == 0;

            int selectedFullscreen = gameConfig.IsFullscreen ? 0 : 1;
            int selectedResolution = 0;
            for(int i = 0; i < GameConfig.CommonResolutions.Length; ++i) {
                if(GameConfig.CommonResolutions[i].Equals(gameConfig.Resolution)) {
                    selectedResolution = i;
                    break;
                }
            }

            hlResolutions.select(selectedResolution);
            hlFullscreen.select(selectedFullscreen);
            hlResolutions.SelectedColor = Color.White;
            hlFullscreen.SelectedColor = Color.White;

            vlNames.PercentWidth = 30;
            vlNames.PercentHeight = 100;
            vlNames.HAlignment = HAlignment.Left;
            vlNames.EffectAlpha = 0.5f;
            vlNames.Color = Color.Black;

            vpSettings.PercentWidth = 70;
            vpSettings.PercentHeight = 100;
            vpSettings.HAlignment = HAlignment.Left;
            vpSettings.EffectAlpha = 0.5f;
            vpSettings.Color = Color.Gray;

            FocusRequestEvent += (s, a) => {
                vlNames.select(0);
                vlNames.IsFocused = true;
            };

            FocusLossEvent += (s, a) => {
                vlNames.IsFocused = false;
                vlNames.SelectedIndex = -1;
            };

            HPane hpMain = new HPane(vlNames, vpSettings);
            hpMain.PercentWidth = 100;
            hpMain.PercentHeight = 100;
            add(hpMain);
        }

        /// Helper to create a readable string out of
        /// a point which gets interpreted as resolution
        protected string resToStr(Point res) {
            return res.X + "x" + res.Y;
        }
    }
}