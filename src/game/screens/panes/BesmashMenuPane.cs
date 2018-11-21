namespace BesmashGame {
    using GSMXtended;
    using System;
    using System.Collections.Generic;

    public class BesmashMenuPane : VPane {
        public event EventHandler FocusRequestEvent;
        public event EventHandler FocusLossEvent;

        /// Hides this pane and takes away any focus
        public void hide() {hide(true, 0);}
        public void hide(bool takeFocus) {hide(takeFocus, 0);}
        public void hide(float alpha) {hide(true, alpha);}
        public virtual void hide(bool takeFocus, float alpha) {
            XtendedScreen.applyAlpha(this, alpha);
            if(takeFocus) onFocusLoss(null);
        }

        /// Shows this pane with the passed alpha set
        /// for all its children and sets its focus
        public void show() {show(true);}
        public void show(bool giveFocus) {show(giveFocus, 1);}
        public void show(float alpha) {show(true, alpha);}
        public virtual void show(bool giveFocus, float alpha) {
            XtendedScreen.applyAlpha(this, alpha);
            if(giveFocus) onFocusRequest(null);
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