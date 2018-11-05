namespace BesmashGame.Screens {
    using GameStateManagement;

    internal class AudioOptionsScreen : MenuScreen {
        public AudioOptionsScreen() : base("Audio Settings") {
            MenuEntry mneSFX = new MenuEntry("SFX:   <  50% >");
            MenuEntry mneMusic = new MenuEntry("Music: <  75% >");
            MenuEntry mneBack = new MenuEntry("Back");
            
            mneBack.Selected += OnCancel;

            MenuEntries.Add(mneSFX);
            MenuEntries.Add(mneMusic);
            MenuEntries.Add(mneBack);
        }
    }
}