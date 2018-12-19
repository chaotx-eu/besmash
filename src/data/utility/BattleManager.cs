namespace BesmashGame.Config
{
    using BesmashContent;
    using System;
    using System.Collections.Generic;
    public class BattleManager
    {
        private static BattleManager instance = null;
        public Random random;
        private BattleUtils BattleUtils = BattleUtils.newInstance();
        private bool battleIsHappening;
        public TileMap map {get{return BattleUtils.map;}set{BattleUtils.map = value;}}
        public static BattleManager newInstance()
        {
            if(instance == null)
                instance = new BattleManager();
            return instance;
        }        
        private BattleManager()
        {
            random = new Random();
            battleIsHappening = false;
        }
        public void update()
        {
            //Eventuelll aus BattleUtil hinzuzufügenes Zeug adden
            if(BattleUtils.newInstance().AddTheeseGroups != null)
            foreach(BattleUtils.groupToAdd g in BattleUtils.newInstance().AddTheeseGroups)
            {
                this.addGroupOfEnemies(g.group, g.Alignment);
            }
            if(BattleUtils.FightingEntities != null && BattleUtils.FightingEntities.Exists(x=> x.Creature is Player))
            {
                FightingInfo Player = BattleUtils.FightingEntities.Find(x=> x.Creature is Player);
                if(BattleUtils.FightingEntities.Exists(x=> !FightingInfo.IsFriendlyTo(Player, x)))
                {
                    if(!battleIsHappening)
                    {
                        battleIsHappening = true;
                        this.startBattle();
                    }
                    this.ManageRound();
                }
                else
                    if(battleIsHappening)
                    {
                        battleIsHappening = false;
                        this.endBattle();
                    }
            }
            if(battleIsHappening)
            {
                battleIsHappening = false;
                this.endBattle();
                //todo: Gameover?
            }
        }
        private void startBattle()
        {
            BattleUtils.map.setFightingState(null);

        }
        private void endBattle()
        {
            BattleUtils.map.setRoamingState(GameManager.newInstance().ActiveSave.Team);
        }

        //Berechnet die Reihenfolge in der die Kämpfenden Creatures agieren dürfen und arbeitet sie der Reihe nach ab
        private void ManageRound()
        {
            //Der Anfang der Runde
            for(int i = 0; i < BattleUtils.FightingEntities.Count; i++)  
            {
                FightingInfo FightingInfo = BattleUtils.FightingEntities[i];

                if (FightingInfo.Creature.status.poisoned)    //Arbeitet den Schaden durch gift ab
                    FightingInfo.Creature.isDealtDamage(12);
                
                foreach(Buff b in FightingInfo.battleBuffs) //prüft ob buffs/debuffs ablaufen
                {
                    if (!b.Over)
                        b.updateRound();
                }
            }

            //Die einzelnen Aktionen der Kämpfer
            List<FightingInfo> fightingOrder = calculateFightingOrder(); //Berechnet die Reihenfolge in der die Entities
            for(int i = 0; i < fightingOrder.Count; i++)
            {
                if(!fightingOrder[i].Creature.status.dead)
                    ManageTurn(fightingOrder[i]);
            }

            //Das Ende der Runde
        }

        private void ManageTurn(FightingInfo FightingInfo) //Ein einzelner Zug
        {
            foreach(Buff b in FightingInfo.battleBuffs) //prüft ob buffs/debuffs ablaufen
            {
                if (!b.Over)
                    b.updateTurn();
            }
            
            if(FightingInfo.Creature.status.stunned)
            {
                FightingInfo.Creature.status.stunned = false;
            }
            else if (FightingInfo.Creature.status.asleep)
            {
                FightingInfo.Creature.status.roundsAsleep++;
                if (random.Next(100) <= 10 * FightingInfo.Creature.status.roundsAsleep)
                {
                    FightingInfo.Creature.status.asleep = false;
                    FightingInfo.Creature.status.roundsAsleep = 0;
                }
            }
            else
                FightingInfo.Creature.nextTurn();
        }

        public void addToBattle(Creature Creature, FightingInfo.Faction alignment)
        {
            if(Creature is Enemy && ((Enemy)Creature).Group.getMember() != null)
            {
                this.addGroupOfEnemies(((Enemy)Creature).Group.getMember(), alignment);
            }
            FightingInfo FightingInfo = new FightingInfo(Creature, alignment);     //Erstellt eine FightingInfo, die auf die angegebene Creature verweist

            BattleUtils.FightingEntities.Add(FightingInfo);
        }
        public void addGroupOfEnemies(List<Enemy> enemies,  FightingInfo.Faction alignment)
        {
            foreach(Enemy e in enemies)
            {
                FightingInfo FightingInfo = new FightingInfo(e, alignment);     //Erstellt eine FightingInfo, die auf die angegebene Creature verweist
                BattleUtils.FightingEntities.Add(FightingInfo);
            }
        }
        public void addGroupOfEnemies(List<Creature> creatures,  FightingInfo.Faction alignment)
        {
            foreach(Enemy c in creatures)
            {
                FightingInfo FightingInfo = new FightingInfo(c, alignment);     //Erstellt eine FightingInfo, die auf die angegebene Creature verweist
                BattleUtils.FightingEntities.Add(FightingInfo);
            }
        }

        public void removeFromBattle(Creature Creature)
        {
            if(BattleUtils.FightingEntities.Count > 0)
            {
                int i = 0;
                bool removed = false;
                do
                {
                    if(BattleUtils.FightingEntities[i].Creature == Creature)
                    {
                        BattleUtils.FightingEntities.RemoveAt(i);
                        removed = true;
                    }
                    i++;
                } while(!removed && i < BattleUtils.FightingEntities.Count -1);
            }
        }

        public List<FightingInfo> calculateFightingOrder()
        {
            //Rechnet übriggebliebende agilität aus letzter Runde dazu
            for(int i = 0; i < BattleUtils.FightingEntities.Count; i++)
            {
                FightingInfo FightingInfo = BattleUtils.FightingEntities[i];
                FightingInfo.temporalAgility += FightingInfo.stats.AGI;
            }

            //Die Priorität wird auf 0 gesetzt
            for(int i = 0; i < BattleUtils.FightingEntities.Count; i++)
            {
                FightingInfo Creature = BattleUtils.FightingEntities[i];
                Creature.priority = 0;
            }

            //Bestimmt die Priorität
            for(int i = 0; i < BattleUtils.FightingEntities.Count; i++)
            {
                FightingInfo entitiyI = BattleUtils.FightingEntities[i];
                entitiyI.priority = 1;
                for(int j = 0; j < BattleUtils.FightingEntities.Count; j++)
                {
                    FightingInfo entitiyJ = BattleUtils.FightingEntities[j];

                    if(i != j)
                    {
                        if(entitiyI.temporalAgility < entitiyJ.temporalAgility)
                        {
                            entitiyI.priority++;
                        }
                        else if(entitiyI.temporalAgility == entitiyJ.temporalAgility)
                        {
                            if (entitiyJ.priority == 0)
                                entitiyI.priority++;
                        }
                    }
                }
            }

            //Nimmt sich die geschwindigkeit des niedrigsten
            int lowestAgility = BattleUtils.FightingEntities.Find(x => x.priority == BattleUtils.FightingEntities.Count).priority;

            //Es wird berechnet wie oft jeder angreifen kann (n mal so oft, wie die eigene AGI höher ist als die des langsamsten)
            for(int i = 0; i < BattleUtils.FightingEntities.Count; i++)
            {
                FightingInfo Creature = BattleUtils.FightingEntities[i];
                Creature.times = 0;
                while(Creature.temporalAgility >= lowestAgility)
                {
                    Creature.temporalAgility -= lowestAgility;
                    Creature.times++;
                }
            }

            //Eine Liste wird erstellt, die der Reihenfolge in der die Entities nächste Runde agieren dürfen entspricht
            List<FightingInfo> fightingOrder = new List<FightingInfo>();
            bool wasAdded;
            do
            {
                wasAdded = false;                
                for (int i = 0; i < BattleUtils.FightingEntities.Count; i++)
                {
                    for (int j = 0; j < BattleUtils.FightingEntities.Count; j++)
                    {
                        FightingInfo Creature = BattleUtils.FightingEntities[j];
                        if(Creature.priority == i && Creature.times > 0)
                        {
                            fightingOrder.Add(Creature);
                            Creature.times--;
                            wasAdded = true;
                        }
                    }
                }
            } while (wasAdded);

            return fightingOrder;
        }
    }
}