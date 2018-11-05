namespace BesmashGame.Screens {
    using GameStateManagement;

    internal class PauseMenuScreen : MenuScreen {
        public PauseMenuScreen() : base("Game Paused") {
            MenuEntry mneResume = new MenuEntry("Resume");
            MenuEntry mneQuit = new MenuEntry("Quit");

            mneResume.Selected += OnCancel;
            mneQuit.Selected += (obj, args) => {
                LoadingScreen.Load(
                    ScreenManager, false, null,
                    new BackgroundScreen("menu/texture/background"),
                    new MainMenuScreen());

                ((Besmash)ScreenManager.Game).Manager.save();
            };

            MenuEntries.Add(mneResume);
            MenuEntries.Add(mneQuit);
        }
    }
}