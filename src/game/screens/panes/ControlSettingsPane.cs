namespace BesmashGame {
    using Config;
    using GSMXtended;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using System.Linq;
    using System.Collections.Generic;

    // TODO
    public class ControlSettingsPane : BesmashMenuPane {
        private List<HPane> hPanes = new List<HPane>();
        private List<VList> vLists = new List<VList>();
        private HList hlCategories = new HList();

        public ControlSettingsPane(GameConfig gameConfig) {
            StackPane spSettings = new StackPane();
            VPane vpMain = new VPane(hlCategories, spSettings);

            hlCategories.PercentWidth = 100;
            spSettings.PercentWidth = 100;

            hlCategories.PercentHeight = 10;
            spSettings.PercentHeight = 90;

            hlCategories.Color = Color.Black;
            hlCategories.EffectAlpha = 0.5f;

            gameConfig.KeyMaps.Keys.ToList().ForEach(key => {
                hlCategories.add(new TextItem(key, "fonts/menu_font1"));
                VList vlNames = new VList();
                VList vlKeys = new VList();
                VList vlButtons = new VList();
                HPane hpBorder = new HPane();
                HPane hpControls = new HPane(vlNames, vlKeys, hpBorder, vlButtons);

                hpControls.PercentWidth = 100;
                hpControls.PercentHeight = 100;

                vlNames.PercentWidth = 25;
                vlKeys.PercentWidth = 25;
                hpBorder.PercentWidth = 5;
                vlButtons.PercentWidth = 45;

                vlNames.PercentHeight = 100;
                vlKeys.PercentHeight = 100;
                hpBorder.PercentHeight = 100;
                vlButtons.PercentHeight = 100;

                vlNames.Color = Color.Black;
                vlKeys.Color = Color.Gray;
                hpBorder.Color = Color.Black;
                vlButtons.Color = Color.Gray;

                vlNames.EffectAlpha = 0.5f;
                vlKeys.EffectAlpha = 0.5f;
                hpBorder.EffectAlpha = 0.5f;
                vlButtons.EffectAlpha = 0.5f;

                List<UserInput> input_refs = new List<UserInput>();
                List<TextItem> key_refs = new List<TextItem>();
                List<TextItem> button_refs = new List<TextItem>();

                vlNames.ActionEvent += (sender, args) => {
                    ParentScreen.ScreenManager.AddScreen(new InputDialog(
                        (BesmashScreen)ParentScreen,
                        input_refs[args.SelectedIndex],
                        "Press a new Key or Button", 5,
                        a => initKeyTextItems(input_refs[args.SelectedIndex],
                            key_refs[args.SelectedIndex],
                            button_refs[args.SelectedIndex])
                        ), null);
                };

                spSettings.add(hpControls);
                hPanes.Add(hpControls);
                vLists.Add(vlNames);

                gameConfig.KeyMaps[key].Keys.ToList().ForEach(key2 => {
                    List<Keys> keys = gameConfig.KeyMaps[key][key2].TriggerKeys;
                    List<Buttons> buttons = gameConfig.KeyMaps[key][key2].TriggerButtons;
                    string title_key = "", title_button = "";
                    int i;

                    for(i = 0; i < keys.Count(); ++i)
                        title_key += (i > 0 ? ", " : "") + keys[i].ToString();

                    for(i = 0; i < buttons.Count(); ++i)
                        title_button += (i > 0 ? ", " : "") + buttons[i].ToString();

                    TextItem tiName = new TextItem(key2, "fonts/menu_font1");
                    TextItem tiKeys = new TextItem("", "fonts/menu_font1");
                    TextItem tiButtons = new TextItem("", "fonts/menu_font1");
                    initKeyTextItems(gameConfig.KeyMaps[key][key2], tiKeys, tiButtons);

                    vlNames.add(new TextItem(key2, "fonts/menu_font1"));
                    vlKeys.add(tiKeys);
                    vlButtons.add(tiButtons);

                    key_refs.Add(tiKeys);
                    button_refs.Add(tiButtons);
                    input_refs.Add(gameConfig.KeyMaps[key][key2]);
                });

                vlNames.SelectedIndex = 0;
            });

            hlCategories.SelectedEvent += (sender, args) => {
                if(hlCategories.IsFocused) {
                    vLists[args.SelectedIndex].IsFocused = true;
                    show(true, 1);
                }
            };

            hlCategories.DeselectedEvent += (sender, args)
                => vLists[args.SelectedIndex].IsFocused = false;

            hlCategories.CancelEvent += (sender, args) => {
                vLists[hlCategories.SelectedIndex].IsFocused = false;
                hide(0.5f);
            };

            FocusRequestEvent += (s, a) => {
                vLists[hlCategories.SelectedIndex].IsFocused = true;
                hlCategories.IsFocused = true;
            };

            FocusLossEvent += (s, a)
                => hlCategories.IsFocused = false;
            
            vpMain.PercentWidth = 100;
            vpMain.PercentHeight = 100;
            hlCategories.IsFocused = false;
            hlCategories.select(0);
            add(vpMain);
        }

        /// This pane only blends out the selected category
        public override void hide(bool takeFocus, float alpha) {
            base.hide(takeFocus, alpha);
            for(int i = 0; i < hPanes.Count; ++i) {
                if(i != hlCategories.SelectedIndex)
                    Container.applyAlpha(hPanes[i], 0);
            }
        }

        /// This pane only blends in the selected category
        public override void show(bool giveFocus, float alpha) {
            base.show(giveFocus, alpha);
            for(int i = 0; i < hPanes.Count; ++i) {
                if(i != hlCategories.SelectedIndex)
                    Container.applyAlpha(hPanes[i], 0);
            }
        }

        // helper to set the text of the key and button text items
        private void initKeyTextItems(UserInput input,
        TextItem tiKeys, TextItem tiButtons) {
            List<Keys> keys = input.TriggerKeys;
            List<Buttons> buttons = input.TriggerButtons;
            string text_keys = "", text_buttons = "";
            int i;

            for(i = 0; i < keys.Count(); ++i)
                text_keys += (i > 0 ? ", " : "") + keys[i].ToString();

            for(i = 0; i < buttons.Count(); ++i)
                text_buttons += (i > 0 ? ", " : "") + buttons[i].ToString();

            tiKeys.Text = text_keys.Length > 0 ? text_keys : "n. a.";
            tiButtons.Text = text_buttons.Length > 0 ? text_buttons : "n. a.";
        }
    }
}