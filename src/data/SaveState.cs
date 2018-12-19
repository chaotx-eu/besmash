namespace BesmashGame {
    using BesmashGame.Config;
    using BesmashContent;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using GameStateManagement;
    using System;
    using System.Xml.Serialization;
    
    /// Holds the data of a single playthrough for
    /// one player.
    [DataContract(IsReference = true)]
    public class SaveState {
        /// Map to be loaded if no map has been specified yet.
        public static string DEFAULT_MAP {get;} = "maps/forestMap";

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
        }
        
        public TileMap loadBattleTestMap(ContentManager content) 
        {
            TileMap map = content.Load<TileMap>(DEFAULT_MAP);
            /*
            Player[] members = new Player[3];
            Player leader = new Player("images/entities/enemies/Rat_Spritesheet");

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
            

            //Das Teil stürzt sonst ab:
            BattleManager.newInstance().map = map;

            // some example enemies:
            Stats Enemy1Stats = new Stats(7, 11, 5, 8, 6, 14, 1.0f, 1.0f, 4);
            //Abilities to choose from:
            OffensiveAbility poisonBite = new OffensiveAbility(null, 25, "Poisonous_Bite", 35, 1, false);
            poisonBite.potentialStatus = new OffensiveAbility.PossibleStatus[1];
            poisonBite.potentialStatus[0] = new OffensiveAbility.PossibleStatus(Status.Type.poison, 30);
            OffensiveAbility bite = new OffensiveAbility(null, 15, "Bite", 25, 1, false);
            MovementAbility run = new MovementAbility(null, 0, "Run", null, null);
            HealAbility endure = new HealAbility(null, 40, "Endure_Pain", 0, 50);
            Ability[] Enemy1Abilities = {poisonBite, bite, run, endure};

            Enemy enemy1 = new Enemy(Enemy1Stats, Enemy1Abilities, NPC.FightingStyle.Meelee, 10);
            enemy1.SpriteSheet = "images/entities/enemies/Rat_Spritesheet";
            enemy1.SpriteRectangle = new Rectangle(0, 0, 16, 16);
            
            // EffectManager.newInstance().addEffect(new EffectAnimation("images/Effects/Status/Poison_Effect", new Microsoft.Xna.Framework.Rectangle(0, 16, 16, 16), 8, leader.Position));
            // Status.addStatus(leader, Status.Type.poison);

            // position auf der map
            enemy1.Position = new Vector2(10,10);
            foreach(Player member in members)
                map.addEntity(member);

            // direkter test
            EffectAnimation effect = new EffectAnimation("images/Effects/Status/Poison_Effect", new Microsoft.Xna.Framework.Rectangle(0, 0, 16, 16), 8, leader.Position);
            effect.startEffect();

            map.addEntity(effect);
            map.addEntity(enemy1);
            map.addEntity(leader);
            map.Slave = Team.Leader;
            map.BackgroundMusicFile = "super_smash_bros_remix";
            */
            return map;
        }
    }
}