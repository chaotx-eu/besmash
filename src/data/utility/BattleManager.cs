namespace BesmashGame {
    using BesmashContent;
    using System.Collections.Generic;
    using System.Linq;
    using System;

    public class BattleManager {
        private static BattleManager instance;

        /// A list of all creatures participating
        /// in the currently ongoing battle
        public List<Creature> Participants {get; protected set;}

        /// The map the current battle is fought on
        public TileMap Map {get; protected set;}

        /// An ordered list of participants of the
        /// ongoing battle with the first entry
        /// beeing the next creature to act. Creatures
        /// may occour multiple times in this list
        public List<Creature> TurnList {get; protected set;}

        /// The next action to execute
        public Ability NextAction {get; set;}

        private BattleManager() {}
        public static BattleManager newInstance() {
            if(instance == null) {
                instance = new BattleManager();
                instance.TurnList = new List<Creature>();
                instance.Participants = new List<Creature>();
            }

            return instance;
        }

        /// Initializes participants and turn
        /// list and starts a new battle (TODO)
        public void startBattle(TileMap map, List<Creature> participants) {
            Map = map;
            Participants = participants
                .OrderByDescending(c => c.Stats.AGI)
                .ToList();

            Participants.ForEach(p => {
                p.IsFighting = true;
                p.DeathEvent += onDeath;
            });

            expPool = 0;
            initTurnList();
        }

        /// Finishes the battle and grand
        /// gained exp to the surviving players
        public void finishBattle() {
            int exp = expPool/Participants.Where(p => p is Player).Count();
            Participants.ForEach(p => {
                p.IsFighting = false;
                p.DeathEvent -= onDeath;
                p.Exp += exp;
                p.levelUp(0.1f);
            });

            Participants.Clear();
        }

        /// Initializes the turn list. Entities
        /// may occour multiple times in the list if
        /// their agility is exceptional high relative
        /// to the other participants
        protected void initTurnList() {
            int i = 0;
            List<int> weights = new List<int>();
            TurnList = new List<Creature>();

            // reverse weights list so it can be
            // removed from it while traversing            
            for(i = 0; i < Participants.Count; ++i) {
                weights.Insert(0,
                    Participants[i].Stats.AGI /
                    Participants[Participants.Count-1].Stats.AGI
                );
            }

            while(weights.Sum() > 0) {
                for(i = weights.Count-1; i >= 0; --i) {
                    if(weights[i]-- > 0) TurnList
                        .Add(Participants[Participants.Count-1-i]);
                }
            }
        }

        /// Evaluates the next action or in case of a
        /// player waits for the user to choose one
        /// and executes it. Shifts the turn list
        /// afterwards to the left
        public void nextTurn() {
            if(NextAction != null) {
                TurnList[0].AP -= NextAction.APCost;
                NextAction.execute();
            }
            
            // shift turn list
            TurnList.Add(TurnList[0]);
            TurnList.RemoveAt(0);
        }

        private int expPool;
        protected void onDeath(object sender, EventArgs args) {
            if(sender is Enemy)
                expPool += ((Enemy)sender).ExpGrant;
        }
    }
}