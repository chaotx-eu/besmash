namespace BesmashGame {
    using GSMXtended;
    using GameStateManagement;
    using Microsoft.Xna.Framework;
    using System.Linq;

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
                    ExitScreen();
                    parent.quit(true);
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

        public override void Update(GameTime gameTime,
        bool otherScreenHasFocus, bool coveredByOtherScreen) {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if(!ScreenManager.GetScreens().Contains(this))
                ScreenManager.AddScreen(new MainMenuScreen("images/blank", GameManager), null);
        }
    }
}