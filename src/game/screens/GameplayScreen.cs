namespace BesmashGame {
    using Config;
    using BesmashContent;
    using GSMXtended;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using GameStateManagement;
    using System.Collections.Generic;
    using System.Linq;
    using System;
    using Debug;


    public class GameplayScreen : BesmashScreen {
        /// Reference to the battle manager of the game
        public BattleManager BattleManager {get; protected set;}

        // TODO test
        private DebugPane debugPane;
        private BattlePane BattlePane {get; set;}

        public GameplayScreen(BesmashScreen parent)
        : base(parent) {
            MainContainer.PercentWidth = 100;
            MainContainer.PercentHeight = 100;
            // MainContainer.Color = Color.Black;
        }

        public override void LoadContent() {
            debugPane = new DebugPane();
            MainContainer.remove(MainContainer.Children.ToArray());
            MainContainer.add(debugPane);

            // TODO temporary solution?
            BattleManager = ((Besmash)ScreenManager.Game).BattleManager;
            BattlePane = new BattlePane(BattleManager);
            MainContainer.add(BattlePane);
            base.LoadContent();

            TileMap.MapAlpha = 0;
        }

        private TileMap.MapState lastState;
        public override void Update(GameTime gameTime,
        bool otherScreenHasFocus, bool coveredByOtherScreen) {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            TileMap.MapAlpha = MainContainer.Alpha;
            GameManager.ActiveSave.update(gameTime);

            // check if battle started
            TileMap map = GameManager.ActiveSave.ActiveMap;
            Team team = GameManager.ActiveSave.Team;

            if(lastState != TileMap.MapState.Fighting
            && map.State == TileMap.MapState.Fighting) {
                BattleManager.startBattle(map, map.BattleMap.Participants); // TODO
                BattlePane.Team = team;
                BattlePane.show();
            } else if(lastState != TileMap.MapState.Roaming
            && map.State == TileMap.MapState.Roaming) {
                BattlePane.hide();
                BattleManager.finishBattle(); // TODO
            }

            lastState = map.State;

            // update debug pane
            debugPane.Map = GameManager.ActiveSave.ActiveMap;
        }

        public override void Draw(GameTime gameTime) {
            GameManager.ActiveSave.ActiveMap.draw(ImageBatch);
            base.Draw(gameTime);
        }

        private bool running; // player running en-/disabled

        /// Handling user input, e.g. moving player, interacting
        /// with objects or opening menus (moving through menus
        /// is handled internally by the GSMXtended lib) (TODO)
        public override void HandleInput(InputState inputState) {
            base.HandleInput(inputState);
            GameConfig config = GameManager.Configuration;
            Besmash game = (Besmash)ScreenManager.Game;
            TileMap map = GameManager.ActiveSave.ActiveMap;
            Team team = GameManager.ActiveSave.Team;

            if(game.isActionTriggered("game", "menu"))
                ScreenManager.AddScreen(new GameMenuScreen(this), null);
                
            if(map.Slave != null) {
                bool inFight = map.State == TileMap.MapState.Fighting;

                if(!inFight) {
                    if(!running && team.Leader.AP >= 50 && game.isActionTriggered("game", "cancel", true)) {
                        running = true;
                        team.Player.ForEach(p => p.StepTimeMultiplier = 0.7f);
                    }
                } else running = false;

                if(running && !game.isActionTriggered("game", "cancel", true) || team.Leader.AP == 0) {
                    running = false;
                    team.Player.ForEach(p => p.StepTimeMultiplier = 1);
                }

                if(game.isActionTriggered("game", "move_up", true)) {
                    if(map.Slave.move(0, -1) && !inFight) team.Player.ForEach(
                        p => p.AP = Math.Max(0, Math.Min(
                            p.MaxAP, p.AP + (running ? -10 : 10))));
                }

                if(game.isActionTriggered("game", "move_right", true)) {
                    if(map.Slave.move(1, 0) && !inFight) team.Player.ForEach(
                        p => p.AP = Math.Max(0, Math.Min(
                            p.MaxAP, p.AP + (running ? -10 : 10))));
                }

                if(game.isActionTriggered("game", "move_down", true)) {
                    if(map.Slave.move(0, 1) && !inFight) team.Player.ForEach(
                        p => p.AP = Math.Max(0, Math.Min(
                            p.MaxAP, p.AP + (running ? -10 : 10))));
                }

                if(game.isActionTriggered("game", "move_left", true)) {
                    if(map.Slave.move(-1, 0) && !inFight) team.Player.ForEach(
                        p => p.AP = Math.Max(0, Math.Min(
                            p.MaxAP, p.AP + (running ? -10 : 10))));
                }

                int x = map.Slave.Facing == Facing.East ? 1 :
                    map.Slave.Facing == Facing.West ? -1 : 0;

                int y = map.Slave.Facing == Facing.South ? 1 :
                    map.Slave.Facing == Facing.North ? -1 : 0;

                if(game.isActionTriggered("game", "interact")) {
                    map.getTiles(
                        (int)map.Slave.Position.X + x,
                        (int)map.Slave.Position.Y + y
                    ).ForEach(tile
                        => tile.trigger(map.Slave));
                }

                if(game.isActionTriggered("game", "inspect")) {
                    // TODO
                }
            }

            // debug action: toggle debug pane
            if(game.isActionTriggered("debug", "action0")) {
                debugPane.toggle();
            }

            // debug action: give exp to (and level up) team leader
            if(game.isActionTriggered("debug", "action1")) {
                GameManager.ActiveSave.Team.Leader.Exp += 150;
                GameManager.ActiveSave.Team.Leader.levelUp();
            }

            // debug action: give exp to (and level up) first team member
            if(game.isActionTriggered("debug", "action2")) {
                GameManager.ActiveSave.Team.Members[0].Exp += 150;
                GameManager.ActiveSave.Team.Members[0].levelUp();
            }

            // debug action: teach ability to map slave
            if(game.isActionTriggered("debug", "action3")) {
                Player player = GameManager.ActiveSave.Team.Leader;
                if(player.Abilities.Where(a => a.Title == "Fireball").Count() == 0) {
                    player.addAbility(
                        "objects/battle/abilities/fireball_ability",
                        GameManager.ActiveSave.ActiveMap.Content);
                }
            }

            // debug action: use first ability of map slave
            if(game.isActionTriggered("debug", "action4")) {
                GameManager.ActiveSave.Team.Leader
                    .Abilities.Where(a => a.Title == "Whirl Strike")
                    .ToList().ForEach(a => a.execute());
            }

            // debug action: applay ability effects attached to creatures
            if(game.isActionTriggered("debug", "action5")) {
                map.Entities.Where(e => e is Creature).Cast<Creature>()
                    .Where(c => c.Effects.Count > 0).ToList()
                    .ForEach(c => c.applyEffects());
            }

            // debug action: kill all enemies on map
            if(game.isActionTriggered("debug", "action6")) {
                map.Entities.Where(e => e is Enemy)
                    .Cast<Enemy>().ToList()
                    .ForEach(e => e.die());
            }

            // debug action: spawn entities
            if(game.isActionTriggered("debug", "action7")) {
                map.spawnEntities();
            }

            // debug action: 'kill' all enemies participating the battle
            if(game.isActionTriggered("debug", "action8")) {
                map.BattleMap.Participants
                    .Where(e => e is Enemy).Cast<Enemy>()
                    .ToList().ForEach(e => e.die());
            }

            // debug action: toggle battle overlay
            if(game.isActionTriggered("debug", "action9")) {
                if(map.State == TileMap.MapState.Fighting) {
                    GameManager.ActiveSave.ActiveMap
                        .setRoamingState();
                } else {
                    GameManager.ActiveSave.ActiveMap
                        .setFightingState(GameManager.ActiveSave.Team.Leader.Target);
                }
            }
        }

        /// Closes the screen and may save the game
        public void quit(bool save) {
            if(save) GameManager.save();
            GameManager.ActiveSave.Content.Unload();
            ExitScreen();
            Alpha = 0;
        }
    }
}