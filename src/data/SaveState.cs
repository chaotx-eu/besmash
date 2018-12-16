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

        /// Conent manager of this save state
        public ContentManager Content {get; private set;}

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
        }

        /// Loads Content required for the ActiveMap
        public void load(Game game) {
            if(Content == null)
                Content = new ContentManager(game.Services, "Content");

            if(ActiveMap == null)
                ActiveMap = loadDefaultMap(Content);

            ActiveMap.load(Content);
            ActiveMap.init(game);
        }

        /// Loads the default map of this game.
        public TileMap loadDefaultMap(ContentManager content) {
            TileMap map = content.Load<TileMap>(DEFAULT_MAP);
            Player[] members = new Player[3];
            Player leader = new Player("images/entities/kevin_sheet");

            for(int i = -1; i < members.Length; ++i) {
                Player player = i < 0 ? leader : new Player("images/entities/kevin_sheet");
                player.Position = new Vector2(2+i, 1);
                player.StepTime = 250;
                if(i >= 0) members[i] = player;
            };

            Team = new Team(leader, members);
            Team.Formation[members[0]] = new Point(1, 1);
            Team.Formation[members[1]] = new Point(-1, 1);
            Team.Formation[members[2]] = new Point(0, 1);
            // Team.Formation[members[3]] = new Point(2, 2);
            // Team.Formation[members[4]] = new Point(-2, 2);
            // Team.Formation[members[5]] = new Point(1, -2);
            // Team.Formation[members[6]] = new Point(-1, -2);

            // some example npcs
            Entity donald = new NeutralNPC();
            Entity dagobert = new NeutralNPC();
            donald.SpriteSheet = "images/entities/kevin_sheet";
            dagobert.SpriteSheet = "images/entities/kevin_sheet";
            donald.SpriteRectangle = new Rectangle(0, 16, 16, 16);
            dagobert.SpriteRectangle = new Rectangle(0, 48, 16, 16);

            // position auf der map
            donald.Position = new Vector2(8, 8);
            dagobert.Position = new Vector2(32, 7);

            foreach(Player member in members)
                map.addEntity(member);

            map.addEntity(leader);
            map.addEntity(donald);
            map.addEntity(dagobert);
            map.Slave = Team.Leader;
            map.BackgroundMusicFile = "super_smash_bros_remix";
            return map;
        }
    }
}