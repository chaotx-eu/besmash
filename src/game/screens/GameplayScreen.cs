namespace BesmashGame {
    using GSMXtended;
    using BesmashGame.Config;
    using Microsoft.Xna.Framework;

    public class GameplayScreen : BesmashScreen {
        public GameplayScreen() {
            MainContainer.PercentWidth = 100;
            MainContainer.PercentHeight = 100;
            MainContainer.Color = Color.Black;
        }

        public override void LoadContent() {
            base.LoadContent();
            GameManager.ActiveSave.ActiveMap.load(Content);
        }

        public override void Update(GameTime gameTime,
        bool otherScreenHasFocus, bool coveredByOtherScreen) {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            GameManager.ActiveSave.ActiveMap.update(gameTime);
        }

        public override void Draw(GameTime gameTime) {
            base.Draw(gameTime);
            GameManager.ActiveSave.ActiveMap.draw(ImageBatch);
        }
    }
}