namespace BesmashGame {
    using System;
    using GSMXtended;
    using BesmashGame.Config;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System.Collections.Generic;
    using System.Linq;

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

        /// Wether the parent screen should be hidden while this
        /// screen is shown
        public bool HideParent {get; set;} = true;

        /// Wether the parent screen should still be updated
        /// while this screen is shown
        public bool UpdateParent {get; set;} = false;

        /// Wether this screen is currently hidden
        private bool isHidden;
        public bool IsHidden {get {return isHidden;}}

        public BesmashScreen() : this(null, null) {}
        public BesmashScreen(BesmashScreen parent) : this(parent, null) {}
        public BesmashScreen(GameManager gameManager) : this(null, gameManager) {}
        public BesmashScreen(BesmashScreen parent, GameManager gameManager) : base(new VPane()) {
            MainContainer.PercentWidth = 100;
            MainContainer.PercentHeight = 100;
            ParentScreen = parent;
            Alpha = 1;

            GameManager = gameManager == null && parent != null
                ?  parent.GameManager : gameManager;
        }

        // make sure the background is set for the main container
        public override void LoadContent() {
            base.LoadContent();
            if(MainContainer.TextureFile == null)
                MainContainer.Texture = Background;
            else Background = MainContainer.Texture;
            
            TransitionOnTime = TimeSpan.FromMilliseconds(MainContainer.MillisPerAlpha);
            TransitionOffTime = TimeSpan.FromMilliseconds(MainContainer.MillisPerAlpha);

            toggleFocus(MainContainer, false);
            toggleMoveSpeed(MainContainer, false);
            toggletScaleSpeed(MainContainer, false);
        }

        private int initialTime = 256; // time this screen uses for initializing of its components (no input)
        private bool initialized = false;

        public override void Update(GameTime gameTime,
        bool otherScreenHasFocus, bool coveredByOtherScreen) {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            if(!initialized) {
                if(initialTime <= 0) {
                    toggleFocus(MainContainer, true);
                    toggleMoveSpeed(MainContainer, true);
                    toggletScaleSpeed(MainContainer, true);
                    initialized = true;
                } else initialTime -= gameTime.ElapsedGameTime.Milliseconds;
            }

            if(ParentScreen != null && HideParent) {
                if(IsExiting && ParentScreen.IsHidden)
                    ParentScreen.show();

                if(!IsExiting && !ParentScreen.IsHidden)
                    ParentScreen.hide();
            }

            // updates parent screen while its fading off
            // (or if UpdateParent is true)
            // TODO: temporary solution until I figured out
            // a good wa to determine wether a screen is
            // currently fading off (i. e. if(ParentScreen.IsFading))
            if(!IsExiting/* && (UpdateParent || ParentScreen.IsFading) */)
                if(ParentScreen != null)
                    ParentScreen.Update(gameTime, false, true);
        }

        /// Hides this screen and all its components
        private float defaultAlpha = 0.5f;
        public void hide() {
            isHidden = true;
            defaultAlpha = Alpha;
            Alpha = 0;
        }

        /// Shows a previously hidden screen
        public void show() {
            isHidden = false;
            Alpha = defaultAlpha;
            resetInputTimer(MainContainer);
        }

        private void resetInputTimer(ScreenComponent c) {
            if(c is MenuList)
                ((MenuList)c).InputTimer = -256;    // (TODO property) time input will be ignored initialy
                                                    // (plus the MenuLists MillisPerInput value)

            if(c is Container)
                ((Container)c).Children.ToList()
                    .ForEach(resetInputTimer);
        }

        /// Sets the focus of this component and all its children
        /// to either false if reset is false or to their original
        /// value otherwise
        private List<MenuList> focusedMenus = new List<MenuList>();
        private void toggleFocus(ScreenComponent c, bool reset) {
            if(reset) {
                focusedMenus.ForEach(m => m.IsFocused = true);
                focusedMenus = null;
                return;
            } else if(focusedMenus == null)
                focusedMenus = new List<MenuList>();

            if(c is MenuList) {
                MenuList m = (MenuList)c;

                if(m.IsFocused) {
                    focusedMenus.Add(m);
                    m.IsFocused = false;
                }
            }

            if(c is Container) ((Container)c).Children.ToList()
                .ForEach(child => toggleFocus(child, reset));
        }

        /// Sets the PixelPerSecond property of this component and
        /// all its children to either -1 if reset is false or to
        /// their original value otherwise
        private Dictionary<ScreenComponent, int> speedMap = new Dictionary<ScreenComponent, int>();
        private void toggleMoveSpeed(ScreenComponent component, bool reset) {
            if(!speedMap.ContainsKey(component))
                speedMap.Add(component, component.MillisPerScale);

            component.PixelPerSecond = reset ? speedMap[component] : -1;
            if(component is Container)
                ((Container)component).Children
                    .ToList().ForEach(child => toggleMoveSpeed(child, reset));
        }

        /// Sets the MillisPerScale property of this component and
        /// all its children to either -1 if reset is false or to
        /// their original value otherwise
        private Dictionary<ScreenComponent, int> scaleMap = new Dictionary<ScreenComponent, int>();
        private void toggletScaleSpeed(ScreenComponent component, bool reset) {
            if(!scaleMap.ContainsKey(component))
                scaleMap.Add(component, component.MillisPerScale);

            component.MillisPerScale = reset ? scaleMap[component] : -1;
            if(component is Container)
                ((Container)component).Children
                    .ToList().ForEach(child => toggletScaleSpeed(child, reset));
        }
    }
}