namespace BesmashGame {
    using GSMXtended;
    using GameStateManagement;
    using Microsoft.Xna.Framework;

    public class GameMenuScreen : BesmashScreen {
        public GameMenuScreen(GameplayScreen parent) : base(parent) {
            TextItem entryInventory = new TextItem("Inventory", "fonts/menu_font1");
            TextItem entrySettings = new TextItem("Settings", "fonts/menu_font1");
            TextItem entryQuit = new TextItem("Quit", "fonts/menu_font1");
            VList menuEntries = new VList(entryInventory, entrySettings, entryQuit);
            menuEntries.IsFocused = true;
            IsPopup = true;

            menuEntries.ActionEvent += (sender, args) => {
                if(args.SelectedItem == entryInventory) {
                    // TODO
                }

                if(args.SelectedItem == entrySettings) {
                    // if(IsHidden) show(); else hide();
                    SettingsScreen settingsScreen = new SettingsScreen(this);
                    ScreenManager.AddScreen(settingsScreen, null);
                }

                if(args.SelectedItem == entryQuit) {
                    Alpha = 0;
                    ExitScreen();
                    parent.quit(true);
                    // ScreenManager.AddScreen(new MainMenuScreen(), null);
                    LoadingScreen.Load(ScreenManager, true, null,
                        new BackgroundScreen("images/menu/main_background"),
                        new MainMenuScreen("images/blank", GameManager));
                }
            };

            menuEntries.CancelEvent += (sender, args) => {
                Alpha = 0;
                ExitScreen();
            };

            HideParent = false;
            MainContainer.Alpha = 0.5f;
            MainContainer.Color = Color.Black;
            MainContainer.add(menuEntries);
        }
    }
}