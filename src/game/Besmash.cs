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

        private Player testplayer_0;
        private Player testplayer_1;

        private Entity testnpc_0;
        private Entity testnpc_1;

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

            testplayer_0 = new Player();
            testplayer_0.SpriteSheet = "game/texture/sheets/entity/kevin_sheet";
            testplayer_0.SpriteRectangle = new Rectangle(0, 32, 16, 16);
            testplayer_0.load(Content);

            testplayer_1 = new Player();
            testplayer_1.SpriteSheet = "game/texture/sheets/entity/kevin_sheet";
            testplayer_1.SpriteRectangle = new Rectangle(0, 32, 16, 16);
            testplayer_1.load(Content);

            testnpc_0 = new Entity();
            testnpc_0.SpriteSheet = "game/texture/sheets/entity/kevin_sheet";
            testnpc_0.SpriteRectangle = new Rectangle(0, 16, 16, 16);
            testnpc_0.load(Content);

            testnpc_1 = new Entity();
            testnpc_1.SpriteSheet = "game/texture/sheets/entity/kevin_sheet";
            testnpc_1.SpriteRectangle = new Rectangle(0, 0, 16, 16);
            testnpc_1.load(Content);

            testmap = Content.Load<TileMap>("xmlmap");
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

            testnpc_0.Position = new Point(2, 2);
            testnpc_1.Position = new Point(8, 8);
            
            testmap.init(this);
            testmap.Slave = testplayer_0;
            testmap.addEntity(testplayer_1);
            testmap.addEntity(testnpc_0);
            testmap.addEntity(testnpc_1);
        }

        // temporary (real update in ScreenManager)
        long timer = 0;
        protected override void Update(GameTime gameTime) {
            if(Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();
            if(timer < 200) timer += gameTime.ElapsedGameTime.Milliseconds;
            
            if(timer > 200) {
                if(Keyboard.GetState().IsKeyDown(Keys.Up)
                || Keyboard.GetState().IsKeyDown(Keys.Right)
                || Keyboard.GetState().IsKeyDown(Keys.Down)
                || Keyboard.GetState().IsKeyDown(Keys.Left)
                || Keyboard.GetState().IsKeyDown(Keys.Add)
                || Keyboard.GetState().IsKeyDown(Keys.OemMinus)
                || Keyboard.GetState().IsKeyDown(Keys.Space)) {
                    timer = 0;
                }

                if(Keyboard.GetState().IsKeyDown(Keys.Space)) {
                    testmap.Slave = testmap.Slave == testplayer_0
                        ? testplayer_1 : testplayer_0;
                }

                if(Keyboard.GetState().IsKeyDown(Keys.Add)) {
                    int vw = testmap.Viewport.X+1;
                    int vh = testmap.Viewport.Y+1;
                    testmap.Viewport = new Point(vw, vh);
                }

                if(Keyboard.GetState().IsKeyDown(Keys.OemMinus)) {
                    int vw = testmap.Viewport.X > 1 ? testmap.Viewport.X-1 : 1;
                    int vh = testmap.Viewport.Y > 1 ? testmap.Viewport.Y-1 : 1;
                    testmap.Viewport = new Point(vw, vh);
                }

                if(testmap.Slave != null) {
                    if(Keyboard.GetState().IsKeyDown(Keys.Up)) {
                        testmap.Slave.move(new Point(
                            testmap.Slave.Position.X,
                            testmap.Slave.Position.Y-1));
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
