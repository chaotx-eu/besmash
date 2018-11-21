namespace BesmashGame {
    using Config;
    using GSMXtended;
    using Microsoft.Xna.Framework;

    public class AudioSettingsPane : BesmashMenuPane {
        private HList hlMusic, hlSFX;

        public AudioSettingsPane(GameConfig gameConfig) {
            hlMusic = new HList(new TextItem("Off", "fonts/menu_font1"));
            hlSFX = new HList(new TextItem("Off", "fonts/menu_font1"));

            for(int i = 10; i <= 100; i += 10) {
                hlMusic.add(new TextItem(i.ToString(), "fonts/menu_font1"));
                hlSFX.add(new TextItem(i.ToString(), "fonts/menu_font1"));
            }

            VList vlNames = new VList(
                new TextItem("Music Volume", "fonts/menu_font1"),
                new TextItem("Audio Volume", "fonts/menu_font1")
            );

            hlSFX.VisibleRange = 4;
            hlMusic.VisibleRange = 4;
            VPane vpSettings = new VPane(hlMusic, hlSFX);

            vlNames.SelectedEvent += (sender, args) => {
                hlMusic.IsFocused = args.SelectedIndex == 0;
                hlSFX.IsFocused = args.SelectedIndex == 1;
            };

            vlNames.CancelEvent += (sender, args) => {
                hlMusic.IsFocused = false;
                hlSFX.IsFocused = false;
                hide(0.5f);
            };

            hlMusic.SelectedEvent += (sender, args)
                => gameConfig.MusicVolume = args.SelectedIndex*10;

            hlSFX.SelectedEvent += (sender, args)
                => gameConfig.SFXVolume = args.SelectedIndex*10;

            hlMusic.select(gameConfig.MusicVolume/10);
            hlSFX.select(gameConfig.SFXVolume/10);
            hlMusic.SelectedColor = Color.White;
            hlSFX.SelectedColor = Color.White;

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
    }
}