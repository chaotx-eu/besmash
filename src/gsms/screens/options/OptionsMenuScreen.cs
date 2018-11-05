namespace BesmashGame.Screens {
    using GameStateManagement;
    using BesmashGame.Config;

    internal class OptionsMenuScreen : MenuScreen {
        public OptionsMenuScreen() : base("Settings") {
            MenuEntry mneVideo = new MenuEntry("Video");
            MenuEntry mneAudio = new MenuEntry("Audio");
            MenuEntry mneGameplay = new MenuEntry("Gameplay");
            MenuEntry mneControls = new MenuEntry("Controls");
            MenuEntry mneBack = new MenuEntry("Back");

            mneVideo.Selected += (obj, args) => ScreenManager.AddScreen(
                new VideoOptionsScreen(), args.PlayerIndex);

            mneAudio.Selected += (obj, args) => ScreenManager.AddScreen(
                new AudioOptionsScreen(), args.PlayerIndex);

            mneGameplay.Selected += (obj, args) => ScreenManager.AddScreen(
                new GameOptionsScreen(), args.PlayerIndex);

            // TODO implement ControlsOptionsScreen
            // mneControls += (obj, args) => ScreenManager.AddScreen(
            //     new ControlsOptionsScreen(), args.PlayerIndex);

            mneBack.Selected += OnCancel;

            MenuEntries.Add(mneVideo);
            MenuEntries.Add(mneAudio);
            MenuEntries.Add(mneGameplay);
            MenuEntries.Add(mneControls);
            MenuEntries.Add(mneBack);
        }
    }
}