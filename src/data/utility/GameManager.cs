namespace BesmashGame.Config {
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.IO;
    using System.Xml;
    using System;

    /// Manages the current state of the game.
    /// E.g. holds save states maps and entities.
    /// This class is serializable.
    [KnownType(typeof(BesmashContent.Player))]
    [DataContract]
    public class GameManager {
        public static string GAME_FOLDER {get;} = Environment.GetFolderPath(
            Environment.SpecialFolder.ApplicationData)
            + Path.DirectorySeparatorChar + "besmash";

        /// Default location of the game state file.
        public static string DEFAULT_GSF {get;} = GAME_FOLDER
            + Path.DirectorySeparatorChar + ".gamestate";

        /// References all save states in this manager.
        [DataMember]
        public List<SaveState> SaveStates {get {
            return (saveStates == null
                ? (saveStates = new List<SaveState>())
                : saveStates);
        }}

        private List<SaveState> saveStates;

        /// The currently active save played on.
        [DataMember]
        public SaveState ActiveSave {get; set;}

        /// The configuration of the game.
        [DataMember]
        public GameConfig Configuration {get; set;}

        /// Path to the file this manager will be
        /// saved to / loaded from.
        [DataMember]
        public string GameStateFile {get; set;}

        /// Loads and initializes a new GameManager from
        /// the file at the passed path. If the file doesnt
        /// exist a default game manager will be returned.
        public static GameManager newInstance(string gameStateFile) {
            if(File.Exists(gameStateFile)) {
                DataContractSerializer serializer = new DataContractSerializer(typeof(GameManager));
                using(var stream = File.Open(gameStateFile, FileMode.Open))
                using(XmlDictionaryReader reader = XmlDictionaryReader.CreateBinaryReader(stream, XmlDictionaryReaderQuotas.Max))
                    return (GameManager)serializer.ReadObject(reader);
            }

            return new GameManager(DEFAULT_GSF);
        }

        public static GameManager newInstance() {
            return newInstance(DEFAULT_GSF);
        }

        private GameManager(string gsf) {
            Directory.CreateDirectory(GAME_FOLDER);
            Configuration = new GameConfig();
            GameStateFile = gsf;
        }

        /// Saves the game and all currently set
        /// configurations to the game state file
        public void save() {
            ActiveSave.SavedDate = DateTime.Now;            
            DataContractSerializer serializer = new DataContractSerializer(typeof(GameManager));
            Stream stream = File.Open(GameStateFile, FileMode.Create);

            using(var writer = XmlDictionaryWriter.CreateBinaryWriter(stream))
                serializer.WriteObject(writer, this);
        }
    }
}