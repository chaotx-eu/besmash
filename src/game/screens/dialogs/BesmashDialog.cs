namespace BesmashGame {
    using Microsoft.Xna.Framework;
    using Config;

    public class BesmashDialog : BesmashScreen {
        public BesmashDialog(BesmashScreen parent) : this(parent, null) {}
        public BesmashDialog(BesmashScreen parent, GameManager gameManager)
        : base(parent, gameManager) {
            IsPopup = true;
        }
    }
}