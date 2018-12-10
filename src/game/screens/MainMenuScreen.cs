namespace BesmashGame {
    using GSMXtended;
    using BesmashGame.Config;
    using GameStateManagement;
    using Microsoft.Xna.Framework;

    using System;

    public class MainMenuScreen : BesmashScreen {
        public static string DEFAULT_BACKGROUND = "images/menu/main_background";
        public static string PRIMARY_FONT = "fonts/menu_font1";
        public static string SECONDARY_FONT = "fonts/menu_font2";

        public MainMenuScreen() : this(null) {}
        public MainMenuScreen(GameManager gameManager) : this(DEFAULT_BACKGROUND, gameManager) {}
        public MainMenuScreen(string backgroundImage, GameManager gameManager) : base(null, gameManager) {
            TextItem itemSettings = new TextItem("Settings", PRIMARY_FONT);
            TextItem itemPlay = new TextItem("Play", PRIMARY_FONT);
            TextItem itemQuit = new TextItem("Quit", PRIMARY_FONT);
            VList vlItems = new VList(itemSettings, itemPlay, itemQuit);

            // debug
            vlItems.Color = Color.Gray;

            vlItems.IsFocused = true;
            vlItems.IsStatic = false;
            vlItems.SelectedIndex = 1;
            vlItems.PercentHeight = 50;
            vlItems.VAlignment = VAlignment.Bottom;
            vlItems.ActionEvent += (sender, args) => {
                if(args.SelectedItem == itemSettings)
                    ScreenManager.AddScreen(
                        new SettingsScreen(this), null);

                if(args.SelectedItem == itemPlay)
                    ScreenManager.AddScreen(
                        new SaveMenuScreen(this), null);

                if(args.SelectedItem == itemQuit)
                    ScreenManager.Game.Exit();
            };

            MainContainer.TextureFile = backgroundImage;
            MainContainer.add(vlItems);
        }
    }
}