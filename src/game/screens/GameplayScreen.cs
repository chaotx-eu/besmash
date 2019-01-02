namespace BesmashGame {
    using Config;
    using BesmashContent;
    using GSMXtended;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using GameStateManagement;
    using System.Linq;
    using Debug;

    using System.Collections.Generic;
    using Microsoft.Xna.Framework.Input;

    public class GameplayScreen : BesmashScreen {
        private BattleOverlayPane battleOverlay;

        // TODO test
        private DebugPane debugPane;

        public GameplayScreen(BesmashScreen parent)
        : base(parent) {
            MainContainer.PercentWidth = 100;
            MainContainer.PercentHeight = 100;
            // MainContainer.Color = Color.Black;
        }

        public override void LoadContent() {
            debugPane = new DebugPane();
            battleOverlay = new BattleOverlayPane(GameManager.ActiveSave);
            MainContainer.remove(MainContainer.Children.ToArray());
            MainContainer.add(battleOverlay);
            MainContainer.add(debugPane);
            base.LoadContent();
            TileMap.MapAlpha = 0;
            prevState = Keyboard.GetState();
        }
        
        private int millisPerAction = 160;
        private int actionTimer;

        public override void Update(GameTime gameTime,
        bool otherScreenHasFocus, bool coveredByOtherScreen) {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            TileMap.MapAlpha = MainContainer.Alpha;
            GameManager.ActiveSave.update(gameTime);

            // throttles input time for user actions
            if(actionTimer < millisPerAction)
                actionTimer += gameTime.ElapsedGameTime.Milliseconds;

            // update debug pane
            debugPane.Map = GameManager.ActiveSave.ActiveMap;
        }

        public override void Draw(GameTime gameTime) {
            GameManager.ActiveSave.ActiveMap.draw(ImageBatch);
            base.Draw(gameTime);
        }

        /// Handling user input, e.g. moving player, interacting
        /// with objects or opening menus (moving through menus
        /// is handled internally by the GSMXtended lib) (TODO)
        private KeyboardState prevState;
        public override void HandleInput(InputState inputState) {
            base.HandleInput(inputState);
            GameConfig config = GameManager.Configuration;
            Besmash game = (Besmash)ScreenManager.Game;
            TileMap map = GameManager.ActiveSave.ActiveMap;
            Team team = GameManager.ActiveSave.Team;

            if(map.Slave != null) {
                if(game.isActionTriggered("game", "move_up")) map.Slave.move(0, -1);
                if(game.isActionTriggered("game", "move_right")) map.Slave.move(1, 0);
                if(game.isActionTriggered("game", "move_down")) map.Slave.move(0, 1);
                if(game.isActionTriggered("game", "move_left")) map.Slave.move(-1, 0);
                if(prevState == null) prevState = Keyboard.GetState();

                if(Keyboard.GetState().IsKeyDown(config.KeyMaps["game"]["cancel"].TriggerKeys[0])
                && !prevState.IsKeyDown(config.KeyMaps["game"]["cancel"].TriggerKeys[0]))
                    GameManager.ActiveSave.Team.Player
                        .ForEach(p => p.StepTimeMultiplier = 0.7f);

                if(!Keyboard.GetState().IsKeyDown(config.KeyMaps["game"]["cancel"].TriggerKeys[0])
                && prevState.IsKeyDown(config.KeyMaps["game"]["cancel"].TriggerKeys[0]))
                    GameManager.ActiveSave.Team.Player
                        .ForEach(p => p.StepTimeMultiplier = 1f);

                if(Keyboard.GetState().IsKeyDown(config.KeyMaps["game"]["interact"].TriggerKeys[0])
                && !prevState.IsKeyDown(config.KeyMaps["game"]["interact"].TriggerKeys[0])) {
                    int x = map.Slave.Facing == Facing.EAST ? 1 :
                        map.Slave.Facing == Facing.WEST ? -1 : 0;

                    int y = map.Slave.Facing == Facing.SOUTH ? 1 :
                        map.Slave.Facing == Facing.NORTH ? -1 : 0;

                    map.getTiles((int)map.Slave.Position.X + x, (int)map.Slave.Position.Y + y)
                        .ForEach(tile => tile.trigger(map.Slave));

                    // TODO test abilities
                    if(map.Slave is Creature) {
                        Creature player = (Creature)map.Slave;
                        // player.BasicAttack.execute(new Point(x, y));
                        player.Abilities.Where(a => a.Title == "Fireball")
                            .ToList().ForEach(a => a.execute(new Point(x, y)));
                    }
                }

                if(Keyboard.GetState().IsKeyDown(config.KeyMaps["game"]["inspect"].TriggerKeys[0])
                && !prevState.IsKeyDown(config.KeyMaps["game"]["inspect"].TriggerKeys[0]))
                    debugPane.toggle();

                // if(game.isActionTriggered("game", "inspect")) {
                //     if(battleOverlay.IsActive) {
                //         battleOverlay.hide();
                //         GameManager.ActiveSave.ActiveMap
                //             .setRoamingState(GameManager.ActiveSave.Team);
                //     } else {
                //         battleOverlay.show();
                //         GameManager.ActiveSave.ActiveMap
                //             .setFightingState(GameManager.ActiveSave.Team.Leader.Target);
                //     }
                // }
            }

            if(Keyboard.GetState().IsKeyDown(config.KeyMaps["game"]["menu"].TriggerKeys[0])
            && !prevState.IsKeyDown(config.KeyMaps["game"]["menu"].TriggerKeys[0]))
                ScreenManager.AddScreen(new GameMenuScreen(this), null);

            prevState = Keyboard.GetState();
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