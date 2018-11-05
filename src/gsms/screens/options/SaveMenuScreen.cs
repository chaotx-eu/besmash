namespace BesmashGame.Screens {
    using BesmashGame.Config;
    using GameStateManagement;
    using System.Collections.Generic;
    using System.Linq;
    
    internal class SaveMenuScreen : MenuScreen {
        public static int MAX_SAVES = 3;

        public SaveMenuScreen() : base("Select a Savegame") {}

        public override void LoadContent() {
            base.LoadContent();

            // menu entries have to be initialized here since the
            // screen manager isnt accesible in the constructor
            Besmash game = (Besmash)ScreenManager.Game;
            GameManager manager = game.Manager;
            List<MenuEntry> menuEntries = new List<MenuEntry>();

            foreach(SaveState st in manager.SaveStates) if(st != null) {
                MenuEntry newEntry = new MenuEntry(st.Info);
                newEntry.Selected += (obj, args) => {
                    manager.ActiveSave = st;
                    LoadingScreen.Load(ScreenManager,
                        true, args.PlayerIndex,
                        new GameplayScreen());
                };

                MenuEntries.Add(newEntry);
            }

            if(menuEntries.Count < MAX_SAVES) {
                MenuEntry mneNew = new MenuEntry("+ New Game");
                MenuEntries.Add(mneNew);
                mneNew.Selected += (obj, args) => {
                    LoadingScreen.Load(ScreenManager,
                        true, args.PlayerIndex,
                        new GameplayScreen());

                    SaveState save = new SaveState();
                    manager.SaveStates.Add(save);
                    manager.ActiveSave = save;
                };
            }
            
            menuEntries.ForEach(MenuEntries.Add);
            MenuEntry mneBack = new MenuEntry("Back");
            mneBack.Selected += OnCancel;
            MenuEntries.Add(mneBack);
        }
    }
}