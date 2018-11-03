namespace BesmashGame {
    using BesmashContent;
    using System.Collections.Generic;
    
    public class SaveState {
        /// Player under user control while GameState
        /// was saved.
        public Player ActivePlayer {get; set;}

        /// List of players to 
        public List<Player> Players {get;}

        /// Overall playtime in seconds.
        public long Playtime {
            get {
                return playtime;
            }
        }

        private long playtime;
    }
}