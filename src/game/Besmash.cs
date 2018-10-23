using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using GameStateManagement;
using BesmashContent;

namespace BesmashGame {
    // Nur eine Demo bisheriger Funktionalitaeten
    public class Besmash : Game {
        private GraphicsDeviceManager graphics;
        private ScreenManager screenManager;
        private SpriteBatch batch;

        private TileMap testmap;
        private Player testplayer;

        public Besmash() {
            Content.RootDirectory = "Content";
            graphics = new GraphicsDeviceManager(this);

            // GameStateManagement-System (uncomment to unleash chaos)
            // screenManager = new ScreenManager(this);
            // screenManager.AddScreen(new BackgroundScreen(), null);
            // screenManager.AddScreen(new MainMenuScreen(), null);
            // Components.Add(screenManager);
        }

        protected override void LoadContent() {
            Content.Load<object>("menu/texture/gradient");

            testplayer = new Player();
            testplayer.SpriteSheet = "game/texture/sheets/entity/kevin_sheet";
            testplayer.SpriteRectangle = new Rectangle(0, 32, 16, 16);
            testplayer.load(Content);

            testmap = Content.Load<TileMap>("testmap");
            testmap.load(Content);
        }

        protected override void Initialize() {
            base.Initialize();

            // graphics.PreferredBackBufferWidth = graphics.GraphicsDevice.DisplayMode.Width;
            // graphics.PreferredBackBufferHeight = graphics.GraphicsDevice.DisplayMode.Height;
            // graphics.IsFullScreen = true;

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 768;

            graphics.ApplyChanges();
            batch = new SpriteBatch(graphics.GraphicsDevice);

            testmap.init(this);
            testmap.Slave = testplayer;
        }

        // temporary (real update in ScreenManager)
        long timer = 0;
        protected override void Update(GameTime gameTime) {
            if(Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();
            timer += gameTime.ElapsedGameTime.Milliseconds;
            
            if(timer > 200) {
                if(Keyboard.GetState().IsKeyDown(Keys.Up)
                || Keyboard.GetState().IsKeyDown(Keys.Right)
                || Keyboard.GetState().IsKeyDown(Keys.Down)
                || Keyboard.GetState().IsKeyDown(Keys.Left)) {
                    timer = 0;
                }

                if(testmap.Slave != null) {
                    if(Keyboard.GetState().IsKeyDown(Keys.Up)) {
                        testplayer.move(new Point(
                            testplayer.Position.X,
                            testplayer.Position.Y-1));
                    }

                    if(Keyboard.GetState().IsKeyDown(Keys.Right)) {
                        testmap.Slave.move(new Point(
                            testmap.Slave.Position.X+1,
                            testmap.Slave.Position.Y));
                    }

                    if(Keyboard.GetState().IsKeyDown(Keys.Down)) {
                        testmap.Slave.move(new Point(
                            testmap.Slave.Position.X,
                            testmap.Slave.Position.Y+1));
                    }

                    if(Keyboard.GetState().IsKeyDown(Keys.Left)) {
                        testmap.Slave.move(new Point(
                            testmap.Slave.Position.X-1,
                            testmap.Slave.Position.Y));
                    }
                }
            }

            base.Update(gameTime);
            testmap.update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            // The real drawing happens inside the screen manager component.
            base.Draw(gameTime);
            testmap.draw(batch);
        }
    }
}
