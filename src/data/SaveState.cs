namespace BesmashGame {
    using BesmashContent;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using GameStateManagement;
    using System.Linq;
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
            if(Content == null) Content = new ContentManager(
                game.Content.ServiceProvider,
                game.Content.RootDirectory);

            ContentManager mapContent = new ContentManager(
                Content.ServiceProvider,
                Content.RootDirectory
            );


            bool newGame;
            if((newGame = Team == null)) {
                Team = new Team(
                    (mapContent.Load<Player>("objects/world/entities/player/grey").clone() as Player),
                    (mapContent.Load<Player>("objects/world/entities/player/pink").clone() as Player)
                );

                Team.Formation.Add(Team.Members[0], new Point(0, 1));
            }

            TileMap prevMap = ActiveMap;
            ActiveMap = Content.Load<TileMap>(mapFile);
            if(!ActiveMap.Initialized)
                ActiveMap.init(game);

            ActiveMap.onLoad(prevMap, Team);
            if(prevMap != null) prevMap.unload(); // TODO test!
            ActiveMap.load(mapContent);
            ActiveMap.spawnEntities();
            LastMap = mapFile;
            Game = game;

            // TODO testcode (battle start)
            ActiveMap.Entities.Where(e => e is Creature)
                .Cast<Creature>().ToList().ForEach(c => {
                    if(c is Enemy) (c as Enemy)
                        .PlayerInRangeEvent += onPlayerInEnemyRange;
                });

            if(newGame) {
                int i;
                Team.Player.ForEach(pl => {
                    for(i = 0; i < 3; ++i) { // TODO init level
                        pl.Exp = pl.MaxExp;
                        pl.levelUp();
                    }

                    pl.HP = pl.MaxHP;
                    pl.AP = pl.MaxAP;
                });
            }
        }

        /// Updates this save state and the active map
        public void update(GameTime gameTime) {
            string nextMap = ActiveMap.OtherMap;
            if(nextMap != null) load(Game, nextMap);
            ActiveMap.update(gameTime);
            Team.update(gameTime);
        }

        protected void onPlayerInEnemyRange(Creature sender, PlayerEventArgs args) {
            if(sender.IsFighting) return;

            TileMap map = sender.ContainingMap;
            if(map.State == TileMap.MapState.Fighting)
                Game.BattleManager.addToBattle(sender);
            else {
                map.setFightingState();
                Game.BattleManager.startBattle(
                    map, map.BattleMap.Participants);
            }
        }
    }
}