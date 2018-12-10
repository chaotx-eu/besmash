namespace BesmashGame {
    using GSMXtended;
    using BesmashGame.Config;
    using GameStateManagement;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using System.Collections.Generic;

    // some testing
    using System.Threading;

    public class SettingsScreen : BesmashScreen {
        public static string DEFAULT_BACKGROUND = "images/menu/main_background";
        public static string PRIMARY_FONT = "fonts/menu_font1";
        public static string SECONDARY_FONT = "fonts/menu_font2";

        /// Local copy of the game configuration
        protected GameConfig Config {get; set;}
        
        public SettingsScreen(BesmashScreen parent)
        : base(parent) {
            IsPopup = true; // TODO test

            VList vlItems = new VList(
                new TextItem("Video", PRIMARY_FONT),
                new TextItem("Audio", PRIMARY_FONT),
                new TextItem("Controls", PRIMARY_FONT),
                new TextItem("Gameplay", PRIMARY_FONT),
                new TextItem("Back", PRIMARY_FONT)
            );

            Config = new GameConfig(GameManager.Configuration);
            VideoSettingsPane vsPane = new VideoSettingsPane(Config);
            AudioSettingsPane asPane = new AudioSettingsPane(Config);
            ControlSettingsPane csPane = new ControlSettingsPane(Config);
            GameplaySettingsPane gsPane = new GameplaySettingsPane(Config);
            MessagePane msPane = new MessagePane(this, "Return to Main Menu");
            StackPane spSettings = new StackPane(vsPane, asPane, csPane, gsPane, msPane);
            HPane hpMain = new HPane(vlItems, spSettings);

            hpMain.PercentWidth = 100;
            hpMain.PercentHeight = 100;
            spSettings.PercentWidth = 100;
            spSettings.PercentHeight = 100;
            spSettings.HAlignment = HAlignment.Left;

            vlItems.ActionEvent += (sender, args) => {
                showPane(vlItems.SelectedIndex, vsPane, asPane, csPane, gsPane);
                vlItems.IsFocused = args.SelectedIndex == 4;

                if(args.SelectedIndex == 4) { // Back
                    if(!Config.Equals(GameManager.Configuration)) {
                        ConfirmDialog dialog = new ConfirmDialog(this, (answer) =>  {
                            if(answer == 0) {
                                GameManager.Configuration = Config;
                                Thread thread = new Thread(() => {
                                    GameManager.save();
                                    ((Besmash)ScreenManager.Game).ConfigChanged = true;
                                });

                                thread.Start();
                            }

                            if(answer != 2) {
                                Alpha = 0;
                                ExitScreen();
                            }
                        }, "Some settings have changed!", "Overwrite them?");

                        // TODO => make dialog layout adjustable
                        // dialog.MainContainer.PercentWidth = 80;
                        // dialog.MainContainer.HAlignment = HAlignment.Right;
                        ScreenManager.AddScreen(dialog, null);
                    } else {
                        Alpha = 0;
                        ExitScreen();
                    }
                }
            };

            vsPane.PercentWidth = 80;
            vsPane.PercentHeight = 100;
            vsPane.HAlignment = HAlignment.Left;
            vsPane.FocusLossEvent += (sender, args)
                => vlItems.IsFocused = true;
            
            asPane.PercentWidth = 80;
            asPane.PercentHeight = 100;
            asPane.HAlignment = HAlignment.Left;
            asPane.FocusLossEvent += (sender, args)
                => vlItems.IsFocused = true;
            
            gsPane.PercentWidth = 80;
            gsPane.PercentHeight = 100;
            gsPane.HAlignment = HAlignment.Left;
            gsPane.FocusLossEvent += (sender, args)
                => vlItems.IsFocused = true;
            
            csPane.PercentWidth = 80;
            csPane.PercentHeight = 100;
            csPane.HAlignment = HAlignment.Left;
            csPane.FocusLossEvent += (sender, args)
                => vlItems.IsFocused = true;

            msPane.PercentWidth = 80;
            msPane.PercentHeight = 100;
            msPane.HAlignment = HAlignment.Left;

            vlItems.SelectedEvent += (sender, args) => {
                if(args.SelectedIndex == 0) vsPane.show(false, 0.5f);
                if(args.SelectedIndex == 1) asPane.show(false, 0.5f);
                if(args.SelectedIndex == 2) csPane.show(false, 0.5f);
                if(args.SelectedIndex == 3) gsPane.show(false, 0.5f);
                if(args.SelectedIndex == 4) msPane.show(false, 0.5f);
            };

            vlItems.DeselectedEvent += (sender, args) => {
                if(args.SelectedIndex == 0) vsPane.hide(false);
                if(args.SelectedIndex == 1) asPane.hide(false);
                if(args.SelectedIndex == 2) csPane.hide(false);
                if(args.SelectedIndex == 3) gsPane.hide(false);
                if(args.SelectedIndex == 4) msPane.hide(false);
            };

            vlItems.CancelEvent += (sender, args) => {
                Alpha = 0;
                ExitScreen();
            };

            vlItems.Color = Color.DarkSlateBlue;
            vlItems.Alpha = 0.3f;
            vlItems.IsFocused = true;
            vlItems.IsStatic = true;
            vlItems.PercentHeight = 100;
            vlItems.PercentWidth = 20;
            vlItems.HAlignment = HAlignment.Left;

            asPane.hide();
            csPane.hide();
            gsPane.hide();
            msPane.hide();
            vlItems.select(0);
            MainContainer.add(hpMain);
        }

        // helper to pick and show a selected pane
        // out of a collecion of panes
        protected void showPane(int i, params BesmashMenuPane[] panes) {
            if(i < panes.Length)
                panes[i].show(true, 1);
        }
    }
}