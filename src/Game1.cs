using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using GameStateManagement;

namespace RougeLikeDemo {
    public class Game1 : Game {
        private GraphicsDeviceManager graphics;
        private ScreenManager screenManager;

        private Dungeon0 MapD0 = new Dungeon0();
        private Kevin kevin = new Kevin();

        public Game1() {
            Content.RootDirectory = "Content";
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 853;
            graphics.PreferredBackBufferHeight = 853;

            screenManager = new ScreenManager(this);
            screenManager.AddScreen(new BackgroundScreen(), null);
            screenManager.AddScreen(new MainMenuScreen(), null);
            
            // Components.Add(screenManager);
        }

        protected override void Initialize() {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        protected override void LoadContent() {
            Content.Load<object>("gradient");

            kevin.MapX = kevin.MapY = 1;
            MapD0.addEntity(kevin);
            MapD0.Load(this);

            // TODO: use this.Content to load your game content here
        }

        // temporary (real update in ScreenManager)
        protected override void Update(GameTime gameTime) {
            if(Keyboard.GetState().IsKeyDown(Keys.Up))      kevin.move(Entity.Direction.UP);
            if(Keyboard.GetState().IsKeyDown(Keys.Right))   kevin.move(Entity.Direction.RIGHT);
            if(Keyboard.GetState().IsKeyDown(Keys.Down))    kevin.move(Entity.Direction.DOWN);
            if(Keyboard.GetState().IsKeyDown(Keys.Left))    kevin.move(Entity.Direction.LEFT);
            if(Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            MapD0.update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);
            MapD0.draw();

            // The real drawing happens inside the screen manager component.
            base.Draw(gameTime);
        }
    }
}
