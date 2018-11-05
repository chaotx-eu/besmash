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
        public static string DEFAULT_MAP {get;} = "testmap_50x50";

        /// List of maps ordered by the last time they
        /// were visited with the active map being at index 0.
        [DataMember]
        public List<string> Maps {get {
            return maps == null
                ? (maps = new List<string>()) : maps;
        }}

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
            Maps.Add(DEFAULT_MAP);
            Team = new Team();
        }

        /// Loads Content required for the ActiveMap
        public void load(ContentManager content, Game game) {
            if(ActiveMap == null)
                ActiveMap = loadDefaultMap(content);

            ActiveMap.load(content);
            ActiveMap.init(game);
        }

        /// Loads the default map of this game.
        public TileMap loadDefaultMap(ContentManager content) {
            TileMap map = content.Load<TileMap>(DEFAULT_MAP);
            Player tick = new Player();
            Player trick = new Player();
            Player track = new Player();
            Player[] member = {trick, track};

            Team.Leader = tick;
            Team.addMember(trick);
            Team.addMember(track);
            tick.StepTime = 250;

            Entity donald = new Entity();
            Entity dagobert = new Entity();

            tick.Position = new Vector2(1, 1);
            donald.Position = new Vector2(8, 8);
            dagobert.Position = new Vector2(32, 7);

            tick.SpriteSheet = "images/entities/kevin_sheet";
            trick.SpriteSheet = "images/entities/kevin_sheet";
            track.SpriteSheet = "images/entities/kevin_sheet";
            tick.SpriteRectangle = new Rectangle(0, 32, 16, 16);
            trick.SpriteRectangle = new Rectangle(0, 32, 16, 16);
            track.SpriteRectangle = new Rectangle(0, 32, 16, 16);

            donald.SpriteSheet = "images/entities/kevin_sheet";
            dagobert.SpriteSheet = "images/entities/kevin_sheet";
            donald.SpriteRectangle = new Rectangle(0, 16, 16, 16);
            dagobert.SpriteRectangle = new Rectangle(0, 48, 16, 16);

            map.addEntity(tick);
            map.addEntity(trick);
            map.addEntity(track);
            map.addEntity(donald);
            map.addEntity(dagobert);
            map.Slave = Team.Leader;
            return map;
        }
    }
}