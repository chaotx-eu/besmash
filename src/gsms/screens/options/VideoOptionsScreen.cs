namespace BesmashGame.Screens {
    using GameStateManagement;

    internal class VideoOptionsScreen : MenuScreen {
        public VideoOptionsScreen() : base("Video Settings") {
            MenuEntry mneResolution = new MenuEntry("Resolution: < 1280x1024 >");
            MenuEntry mneBack = new MenuEntry("Back");

            mneBack.Selected += OnCancel;

            MenuEntries.Add(mneResolution);
            MenuEntries.Add(mneBack);
        }
    }
}