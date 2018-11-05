namespace BesmashGame.Screens {
    using GameStateManagement;

    internal class GameOptionsScreen : MenuScreen {
        public GameOptionsScreen() : base("Gameplay Settings") {
            MenuEntry mneLang = new MenuEntry("Language: < English >");
            MenuEntry mneBack = new MenuEntry("Back");

            mneBack.Selected += OnCancel;
            
            MenuEntries.Add(mneLang);
            MenuEntries.Add(mneBack);
        }
    }
}