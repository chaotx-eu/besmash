namespace BesmashGame {
    using BesmashContent;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
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
        public LinkedList<Creature> TurnList {get; protected set;}

        /// The next action to execute
        public Ability NextAction {get; set;}

        private BattleManager() {}
        public static BattleManager newInstance() {
            if(instance == null) {
                instance = new BattleManager();
                instance.TurnList = new LinkedList<Creature>();
                instance.Participants = new List<Creature>();
            }

            return instance;
        }

        // /// Initializes participants and turn
        // /// list and starts a new battle (TODO)
        // public void startBattle(TileMap map, List<Creature> participants) {
        //     Map = map;
        //     Participants = participants
        //         .OrderByDescending(c => c.Stats.AGI)
        //         .ToList();

        //     Participants.ForEach(p => {
        //         p.IsFighting = true;
        //         p.DeathEvent += onDeath;
        //         if(p is Npc) ((Npc)p).Pathfinder.Path.Clear();
        //     });

        //     expPool = 0;
        //     initTurnList();
        // }

        /// Initializes participants and turn
        /// list and starts a new battle (TODO)
        public void startBattle(TileMap map, List<Creature> participants) {
            addToBattle(participants);
            Map = map;
            expPool = 0;
        }

        /// Finishes the battle and grands
        /// gained exp to the surviving players
        public void finishBattle() {
            int exp = expPool/Participants.Where(p => p is Player).Count();
            Participants.ForEach(p => {
                p.IsFighting = false;
                p.DeathEvent -= onDeath;
                p.DamageEvent -= onDamage;
                p.Exp += exp;
                p.levelUp(0.1f);
            });

            Participants.Clear();
            TurnList.Clear();
        }

        // /// Initializes the turn list. Entities
        // /// may occour multiple times in the list if
        // /// their agility is exceptional high relative
        // /// to the other participants
        // protected void initTurnList() {
        //     int i = 0;
        //     List<int> weights = new List<int>();
        //     TurnList = new List<Creature>();

        //     // reverse weights list so it can be
        //     // removed from it while traversing            
        //     for(i = 0; i < Participants.Count; ++i) {
        //         weights.Insert(0,
        //             Participants[i].Stats.AGI /
        //             Participants[Participants.Count-1].Stats.AGI
        //         );
        //     }

        //     int j;
        //     int[] repeats = new int[weights.Count]; // TODO test
        //     while(weights.Sum() > 0) {
        //         repeats = new int[weights.Count];

        //         for(i = weights.Count-1; i >= 0; --i) {
        //             if(weights[i] > 0) {
        //                 ++repeats[i];
        //                 for(j = 0; j < repeats[i]; ++j) {
        //                     if(weights[i]-- > 0) TurnList
        //                         .Add(Participants[Participants.Count-1-i]);
        //                     else break;
        //                 }
        //             }
        //         }
        //     }
        // }

        /// Number of turns remaining from the old turn
        /// list if creatures have been added to an already
        /// ongoing battle
        private int oldTurns;

        /// Overload for convenience.
        /// Adds passed creatures to the battle
        public void addToBattle(params Creature[] participants) {
            addToBattle(participants.ToList());
        }

        /// Adds passed creatures to the battle. Creatures
        /// may occour multiple times in the turn list if
        /// their agility is exceptional high relative
        /// to the other participants
        public void addToBattle(List<Creature> participants) {
            if(Participants != null) participants = participants
                .Where(c => !Participants.Contains(c))
                .ToList();

            participants.ForEach(c => {
                c.IsFighting = true;
                c.DeathEvent += onDeath;
                c.DamageEvent += onDamage;
                if(c is Npc) (c as Npc).Pathfinder.Path.Clear();
                if(!c.ContainingMap.BattleMap.Participants.Contains(c))
                    c.ContainingMap.BattleMap.Participants.Add(c);
            });

            if(Participants != null) {
                Participants.AddRange(participants);
                oldTurns = TurnList.Count;
            } else {
                Participants = participants;
                TurnList = new LinkedList<Creature>();
                oldTurns = 0;
            }

            int i, n = 1, m = Participants.Count;
            int[] weights = new int[m];

            Participants = Participants
                .OrderByDescending(c => c.Stats.AGI)
                .ToList();

            for(i = 0; i < m; ++i) weights[i] =
                n*Participants[i].Stats.AGI
                /Participants[m-1].Stats.AGI;

            while(m > 0) {
                for(i = 0; i < m; ++i) {
                    if(weights[i] > 0) {
                        TurnList.AddLast(Participants[i]);
                        --weights[i];
                    } else --m;
                }
            }
        }

        /// Evaluates the next action or in case of a
        /// player waits for the user to choose one
        /// and executes it. Shifts the turn list
        /// afterwards to the left
        public void nextTurn() {
            if(NextAction != null) {
                TurnList.First.Value.AP -= NextAction.APCost;
                NextAction.execute();
            }
            
            // shift turn list
            if(oldTurns > 0) --oldTurns;
            else TurnList.AddLast(TurnList.First.Value);
            TurnList.RemoveFirst();
        }

        private int expPool;
        protected void onDeath(object sender, EventArgs args) {
            if(sender is Enemy)
                expPool += (sender as Enemy).ExpGrant;
        }

        protected void onDamage(Creature sender, DamageEventArgs args) {
            if(sender.ContainingMap == null) return;
            FloatingText damageText = new FloatingText(args.DamageAmount.ToString(), Besmash.GameFont);
            damageText.Position = sender.Position;
            damageText.ScaleMod = args.WasCritical ? 2 : 1.2f;
            damageText.Color = args.DamageElement == Element.Earth ? Color.Green
                : args.DamageElement == Element.Fire ? Color.Orange
                : args.DamageElement == Element.Thunder ? Color.Yellow
                : args.DamageElement == Element.Water ? Color.LightBlue : Color.White;

            sender.ContainingMap.addEntity(damageText);
            damageText.init();
        }
    }
}