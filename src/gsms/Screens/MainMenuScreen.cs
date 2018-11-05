namespace BesmashGame.Screens {
    using BesmashGame.Config;
    using GameStateManagement;
    using Microsoft.Xna.Framework;

    internal class MainMenuScreen : MenuScreen {
        public MainMenuScreen() : this("") {}
        public MainMenuScreen(string configFile) : base("BESMASH") {
            MenuEntry mnePlay = new MenuEntry("Play");
            MenuEntry mneOptions = new MenuEntry("Settings");
            MenuEntry mneQuit = new MenuEntry("Quit");

            mnePlay.Selected += (obj, args) => ScreenManager.AddScreen(
                    new SaveMenuScreen(), args.PlayerIndex);

            mneOptions.Selected += (obj, args) => ScreenManager.AddScreen(
                    new OptionsMenuScreen(), args.PlayerIndex);

            mneQuit.Selected += OnCancel;

            MenuEntries.Add(mnePlay);
            MenuEntries.Add(mneOptions);
            MenuEntries.Add(mneQuit);
        }

        protected override void OnCancel(PlayerIndex playerIndex) {
            // TODO (more stylish) CoonfirmMessageBox
            ScreenManager.Game.Exit();
        }
    }
}