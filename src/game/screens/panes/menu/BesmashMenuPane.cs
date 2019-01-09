namespace BesmashGame {
    using GSMXtended;
    using Microsoft.Xna.Framework;
    using System.Collections.Generic;
    using System;

    public class BesmashMenuPane : VPane {
        public event EventHandler FocusRequestEvent;
        public event EventHandler FocusLossEvent;
        public bool IsFocused {get; protected set;}
        public bool IsHidden {get {return isHidden && AlphaMod == TargetAlphaMod;}}
        private bool isHidden;

        /// Hides this pane and takes away any focus
        public void hide() {hide(true, 0);}
        public void hide(bool takeFocus) {hide(takeFocus, 0);}
        public void hide(float alpha) {hide(true, alpha);}
        public virtual void hide(bool takeFocus, float alpha) {
            isHidden = true;
            AlphaMod = alpha;
            if(takeFocus) {
                onFocusLoss(null);
                IsFocused = false;
            }
        }

        /// Shows this pane with the passed alpha set
        /// for all its children and sets its focus
        public void show() {show(true);}
        public void show(bool giveFocus) {show(giveFocus, 1);}
        public void show(float alpha) {show(true, alpha);}
        public virtual void show(bool giveFocus, float alpha) {
            isHidden = false;
            AlphaMod = alpha;
            PPSFactor = 1000; // very fast movement on first update
            if(giveFocus) {
                onFocusRequest(null);
                IsFocused = true;
            }
        }

        /// Update only when focused and not hidden
        public override void update(GameTime time) {
            if(!IsFocused && IsHidden) return;
            base.update(time);
            PPSFactor = 1;
        }

        /// Show this pane if not focused otherwise hides it
        public void toggle() {
            if(IsFocused) hide();
            else show();
        }

        /// Call this method in child classes whenever
        /// this pane should get the focus
        protected void onFocusRequest(EventArgs args) {
            EventHandler handler = FocusRequestEvent;
            if(handler != null) handler(this, args);
        }

        /// Call this method in child classes whenever
        /// this pane should lose the focus
        protected void onFocusLoss(EventArgs args) {
            EventHandler handler = FocusLossEvent;
            if(handler != null) handler(this, args);
        }
    }
}