namespace BesmashGame {
    using System;
    using GSMXtended;
    using BesmashGame.Config;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class BesmashScreen : XtendedScreen {
        /// Reference to parent screen
        public BesmashScreen ParentScreen {get; set;}

        /// Reference to background texture
        private Texture2D background;
        public Texture2D Background {
            get {
                return ParentScreen == null || background != null
                    ? background : ParentScreen.Background;
            }
            set {if(value != null) background = value;}
        }

        /// Reference to game manager
        private GameManager gameManager;
        public GameManager GameManager {
            get {
                return gameManager == null && ParentScreen != null
                    ? ParentScreen.GameManager : gameManager;
            }
            set {if(value != null) gameManager = value;}
        }

        public BesmashScreen() : this(null, null) {}
        public BesmashScreen(BesmashScreen parent) : this(parent, null) {}
        public BesmashScreen(GameManager gameManager) : this(null, gameManager) {}
        public BesmashScreen(BesmashScreen parent, GameManager gameManager) : base(new VPane()) {
            MainContainer.PercentWidth = 100;
            MainContainer.PercentHeight = 100;
            MainContainer.Color = Color.White;
            GameManager = gameManager == null && parent != null
                ?  parent.GameManager : null;
            ParentScreen = parent;
        }

        // make sure the background is set for the main container
        public override void LoadContent() {
            base.LoadContent();
            if(MainContainer.TextureFile == null)
                MainContainer.Texture = Background;
            else Background = MainContainer.Texture;
            
            TransitionOnTime = TimeSpan.FromMilliseconds(MainContainer.MillisPerAlpha);
            TransitionOffTime = TimeSpan.FromMilliseconds(MainContainer.MillisPerAlpha);
        }
    }
}