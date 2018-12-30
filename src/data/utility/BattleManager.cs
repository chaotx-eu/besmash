namespace BesmashGame {
    using BesmashContent;
    using System.Collections.Generic;
    using System.Linq;

    public class BattleManager {
        private static BattleManager instance;

        /// A list of all entities participating in
        /// the currently ongoing battle
        public List<Entity> Participants {get; protected set;}

        /// An ordered list of participants of the
        /// ongoing battle with the first entry
        /// beeing the next entity to act. Entities
        /// may occour multiple times in this list
        public List<Entity> TurnList {get; protected set;}

        /// The next action to execute
        public Ability NextAction {get; set;}

        private BattleManager() {}
        public static BattleManager newInstance() {
            if(instance == null)
                instance = new BattleManager();

            return instance;
        }

        public void startBattle(TileMap map, Team team, List<Enemy> enemies) {
            Participants = new List<Entity>();
            Participants.AddRange(team.Player.Cast<Entity>().Concat(enemies));

            // random order for now

        }

        /// Initializes the turn list. Entities
        /// may occour multiple times in the list if
        /// their agility is exceptional high relative
        /// to the other participants
        protected void initTurnList() {

        }

        /// Evaluates the next action or in case of a
        /// player waits for the user to choose one
        /// and executes it. Shifts the turn list
        /// afterwards to the left
        protected void nextTurn() {
            if(NextAction == null) return;
            
            // shift turn list
            TurnList.Add(TurnList[0]);
            TurnList.RemoveAt(0);
        }
    }
}