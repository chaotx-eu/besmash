namespace BesmashGame {
    using GSMXtended;
    using GameStateManagement;
    using Microsoft.Xna.Framework;
    using System.Linq;

    public class GameMenuScreen : BesmashScreen {
        public GameMenuScreen(GameplayScreen parent) : base(parent) {
            TextItem entryStatus = new TextItem("Status", "fonts/menu_font1");
            TextItem entryFormation = new TextItem("Formation", "fonts/menu_font1");
            TextItem entryInventory = new TextItem("Inventory", "fonts/menu_font1");
            TextItem entrySettings = new TextItem("Settings", "fonts/menu_font1");
            TextItem entryQuit = new TextItem("Quit", "fonts/menu_font1");
            VList menuEntries = new VList(entryStatus, entryFormation, entryInventory, entrySettings, entryQuit);

            menuEntries.PercentWidth = 25;
            menuEntries.PercentHeight = 100;
            menuEntries.HAlignment = HAlignment.Left;
            menuEntries.Color = Color.Black;
            menuEntries.Alpha = 0.5f;
            menuEntries.IsFocused = true;
            IsPopup = true;

            TeamStatusPane teamStatus = new TeamStatusPane();
            teamStatus.PercentWidth = 75;
            teamStatus.PercentHeight = 100;
            teamStatus.HAlignment = HAlignment.Right;

            TeamFormationPane teamFormation = new TeamFormationPane();
            teamFormation.PercentWidth = 75;
            teamFormation.PercentHeight = 100;
            teamFormation.HAlignment = HAlignment.Right;

            teamStatus.FocusLossEvent += (sender, args)
                => menuEntries.IsFocused = true;

            teamFormation.FocusLossEvent += (sender, args)
                => menuEntries.IsFocused = true;

            menuEntries.ActionEvent += (sender, args) => {
                if(args.SelectedItem == entryStatus) {
                    menuEntries.IsFocused = false;
                    teamStatus.Team = GameManager.ActiveSave.Team;
                    teamStatus.show();
                }

                if(args.SelectedItem == entryFormation) {
                    menuEntries.IsFocused = false;
                    teamFormation.Team = GameManager.ActiveSave.Team;
                    teamFormation.show();
                }

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

            StackPane sp = new StackPane(menuEntries, teamStatus, teamFormation);
            sp.PercentWidth = sp.PercentHeight = 100;
            MainContainer.add(sp);
        }

        public override void Update(GameTime gameTime,
        bool otherScreenHasFocus, bool coveredByOtherScreen) {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if(!ScreenManager.GetScreens().Contains(ParentScreen))
                ScreenManager.AddScreen(new MainMenuScreen("images/blank", GameManager), null);
        }
    }
}