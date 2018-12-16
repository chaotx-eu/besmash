namespace BesmashGame {
    using Config;
    using GSMXtended;
    using BesmashContent;
    using GameStateManagement;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    public class SaveMenuScreen : BesmashScreen {
        private static int MAX_SAVES {get;} = 5;

        private TextItem tiNewGame;
        private InputInfoPane inputInfoPane;
        private List<TextItem> saveInfos = new List<TextItem>();
        private List<TextItem> saveDetails = new List<TextItem>();
        private List<ImageItem> saveThumbnails = new List<ImageItem>();

        private Keys backKey;
        private Keys deleteKey = Keys.Tab; // TODO
        private Buttons backButton;
        private Buttons deleteButton = Buttons.X; // TODO

        public SaveMenuScreen(BesmashScreen parent)
        : base(parent) {
            tiNewGame = new TextItem("+ New Game", "fonts/menu_font1");
            backKey = GameManager.Configuration.KeyMaps["menu"]["menu_cancel"].TriggerKeys[0];
            backButton = GameManager.Configuration.KeyMaps["menu"]["menu_cancel"].TriggerButtons[0];

            Dictionary<Keys, string> keyInfoMap = new Dictionary<Keys, string>();
            Dictionary<Buttons, string> buttonInfoMap = new Dictionary<Buttons, string>();
            inputInfoPane = new InputInfoPane(keyInfoMap, buttonInfoMap);
            inputInfoPane.VAlignment = VAlignment.Bottom;
            inputInfoPane.HAlignment = HAlignment.Left;

            // TODO
            // config.KeyMaps["menu"]["menu_option"].TriggerKeys
            //     .ForEach(key => keyInfoMap.Add(key, "Delete Savegame"));

            // config.KeyMaps["menu"]["menu_option"].TriggerButtons
            //     .ForEach(btn => buttonInfoMap.Add(btn, "Delete Savegame"));

            // temp solution
            keyInfoMap.Add(deleteKey, "Delete Savegame");
            keyInfoMap.Add(backKey, "Return to Main Menu");
            buttonInfoMap.Add(deleteButton, "Delete Savegame");
            buttonInfoMap.Add(backButton, "Return to Main Menu");

            initMainContainer();
        }

        public void initMainContainer() {
            VList vlSaves = new VList();
            vlSaves.PercentWidth = vlSaves.PercentHeight = 100;

            VPane vpMain = new VPane(vlSaves, inputInfoPane);
            vpMain.PercentWidth = 50;
            vpMain.PercentHeight = 100;
            vpMain.Color = Color.Black;
            vpMain.Alpha = 0.5f;

            if(GameManager.SaveStates.Count < MAX_SAVES)
                vlSaves.add(tiNewGame);

            int i = 0;
            GameManager.SaveStates.Sort((s1, s2) => -DateTime.Compare(s1.SavedDate, s2.SavedDate));
            GameManager.SaveStates.ForEach(saveState => {
                TextItem saveInfo = new TextItem("", "fonts/menu_font1");
                TextItem saveDetail = new TextItem("", "fonts/menu_font2");
                ImageItem saveThumbnail = new ImageItem("images/entities/kevin_sheet", new Rectangle(0, 32, 16, 16));
                VPane textPane = new VPane(saveInfo, saveDetail);
                HPane saveEntry = new HPane(saveThumbnail, textPane);
                saveEntry.PercentWidth = 100;
                saveEntry.Color = i%2 == 0 ? Color.Black : Color.Gray;
                saveEntry.Alpha = 0.3f;
                textPane.PercentWidth = 70;

                saveDetail.SecondaryColor = Color.Yellow;
                saveInfo.SecondaryColor = Color.Yellow;
                saveThumbnail.SecondaryColor = Color.White;
                saveThumbnail.DefaultScale = 4;
                saveThumbnail.MillisPerScale = 128;

                saveInfo.Text = "" + saveState.SavedDate;
                saveDetail.Text = "Created at: "
                    + saveState.CreationDate + ", Total Playtime: "
                    + saveState.Playtime;

                saveInfos.Add(saveInfo);
                saveDetails.Add(saveDetail);
                saveThumbnails.Add(saveThumbnail);
                vlSaves.add(saveEntry);
                ++i;
            });

            vlSaves.SelectedEvent += (sender, args) => {
                if(!(args.SelectedItem is Container)) return;
                int selected = args.SelectedIndex - (vlSaves.Children.Contains(tiNewGame) ? 1 : 0);
                saveInfos[selected].IsSelected = true;
                saveDetails[selected].IsSelected = true;
                saveThumbnails[selected].IsSelected = true;
            };

            vlSaves.DeselectedEvent += (sender, args) => {
                if(!(args.SelectedItem is Container)) return;
                int selected = args.SelectedIndex - (vlSaves.Children.Contains(tiNewGame) ? 1 : 0);
                saveInfos[selected].IsSelected = false;
                saveDetails[selected].IsSelected = false;
                saveThumbnails[selected].IsSelected = false;
            };

            vlSaves.ActionKeys.Add(deleteKey);
            vlSaves.ActionButtons.Add(deleteButton);

            vlSaves.ActionEvent += (sender, args) => {
                if(Keyboard.GetState().IsKeyDown(deleteKey)
                || GamePad.GetState(0).IsButtonDown(deleteButton)) {
                    if(args.SelectedItem is Container) {
                        ScreenManager.AddScreen(new ConfirmDialog(ParentScreen, answer => {
                                if(answer == 0 || answer == 2) {
                                    if(answer == 0) {
                                        GameManager.SaveStates.RemoveAt(args.SelectedIndex - (vlSaves.Children.Contains(tiNewGame) ? 1 : 0));
                                        ScreenManager.AddScreen(new SaveMenuScreen(ParentScreen), null);
                                    }
                                    
                                    Alpha = 0;
                                    ExitScreen();
                                }
                            },

                            "Do you really want to delete this Savegame?",
                            "This action cannot be undone!"),
                        null);
                    }

                    return;
                }

                GameManager.ActiveSave = (args.SelectedItem is TextItem) ? new SaveState()
                    : GameManager.SaveStates[args.SelectedIndex - (vlSaves.Children.Contains(tiNewGame) ? 1 : 0)];

                ExitScreen();
                ParentScreen.ExitScreen();
                ((Besmash)ScreenManager.Game).loadSave();
                ScreenManager.AddScreen(new GameplayScreen(ParentScreen), null);
                // LoadingScreen.Load(ScreenManager, false, null,
                //     new GameplayScreen(this));
            };

            vlSaves.CancelEvent += (sender, args) => {
                Alpha = 0;
                ExitScreen();
            };

            vlSaves.select(0);
            vlSaves.IsFocused = true;
            MainContainer.add(vpMain);
        }
    }
}