namespace BesmashGame {
    using Config;
    using GSMXtended;
    using BesmashContent;
    using Microsoft.Xna.Framework;

    public class GameplaySettingsPane : BesmashMenuPane {
        private HList hlLang;

        public GameplaySettingsPane(GameConfig gameConfig) {
            hlLang = new HList();
            foreach(Language lang in GameConfig.SupportedLanguages)
                hlLang.add(new TextItem(lang.Title, "fonts/menu_font1"));

            VList vlNames = new VList(new TextItem(gameConfig.Language.translate("Language"), "fonts/menu_font1"));
            VPane vpSettings = new VPane(hlLang);
            hlLang.VisibleRange = 2;

            vlNames.SelectedEvent += (sender, args)
                => hlLang.IsFocused = args.SelectedIndex == 0;

            vlNames.CancelEvent += (sender, args) => {
                hlLang.IsFocused = false;
                hide(0.5f);
            };

            hlLang.SelectedEvent += (sender, args)
                => gameConfig.Language = GameConfig.SupportedLanguages[args.SelectedIndex];

            Language[] langs = GameConfig.SupportedLanguages;
            int index = langs.Length-1;
            for(; index >= 0 && gameConfig.Language.ID != langs[index].ID; --index);
            hlLang.select(index < 0 ? 0 : index);
            hlLang.SelectedColor = Color.White;

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