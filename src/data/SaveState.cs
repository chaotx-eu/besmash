namespace BesmashGame {
    using BesmashContent;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using GameStateManagement;
    using System;
    
    /// Holds the data of a single playthrough for
    /// one player.
    [DataContract(IsReference = true)]
    public class SaveState {
        /// Map to be loaded if no map has been specified yet.
        public static string DEFAULT_MAP {get;} = "objects/world/maps/forest1_ext";
        // public static string DEFAULT_MAP {get;} = "objects/world/maps/rainforest_0";

        /// Overall playtime in seconds.
        [DataMember]
        public long Playtime {
            get {return playtime;}
            private set {playtime = value;}
        }

        /// Date this save state was created.
        [DataMember]
        public DateTime CreationDate {
            get {return creationDate;}
            private set {creationDate = value;}
        }

        /// Date this save state was saved the last time.
        [DataMember]
        public DateTime SavedDate {get; set;}

        /// Team of players under user control
        /// while this state was saved.
        [DataMember]
        public Team Team {
            get {return team;}
            private set {team = value;}
        }

        /// Wether this save state is new i.e. the required
        /// ressources have not been loaded yet (ever).
        [DataMember]
        public bool IsNewGame {
            get {return isNewGame;}
            private set {isNewGame = value;}
        }

        /// Holds a reference to last active map
        /// of this save state. Will be initizialized
        /// on during the load method.
        [DataMember]
        public TileMap ActiveMap {get; set;}

        /// Last loaded map file
        [DataMember]
        public string LastMap {get; private set;}

        /// Conent manager of this save state
        public ContentManager Content {get; private set;}

        /// Reference to te game this save state is associated to
        public Besmash Game {get; private set;}

        /// General info of this save state.
        public string Info { get {
            return "Save: " + CreationDate.ToString();
        }}

        private DateTime creationDate;
        private List<string> maps;
        private bool isNewGame;
        private long playtime;
        private Team team;

        // default savestate
        public SaveState() {
            creationDate = DateTime.Now;
            LastMap = DEFAULT_MAP;
        }

        /// Loads the last map (or DEFAULT_MAP if none)
        /// and sets it as active map
        public void load(Besmash game) {
            load(game, LastMap);
        }

        /// Loads the passed mapFile and sets it as active map
        public void load(Besmash game, string mapFile) {
            if(Team == null)
                Team = new Team();
                
            if(Content == null)
                Content = new ContentManager(game.Services, "Content");

            TileMap prevMap = ActiveMap;
            ActiveMap = Content.Load<TileMap>(mapFile);
            if(!ActiveMap.Initialized)
                ActiveMap.init(game);
                
            ActiveMap.onLoad(prevMap, Team);
            ActiveMap.load(Content);
            LastMap = mapFile;
            Game = game;
        }

        /// Updates this save state and the active map
        public void update(GameTime gameTime) {
            string nextMap = ActiveMap.OtherMap;
            if(nextMap != null) load(Game, nextMap);
            ActiveMap.update(gameTime);
            Team.update(gameTime);
        }
    }
}