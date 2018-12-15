namespace BesmashGame {
    using GSMXtended;
    using Microsoft.Xna.Framework;

    public class MessagePane : BesmashMenuPane {
        public MessagePane(BesmashScreen parent, string message) {
            Color = Color.Gray;
            EffectAlpha = 0.5f;

            add(new TextItem(parent.GameManager
                .Configuration.Language
                .translate(message),
                "fonts/menu_font1"));

            // TODO (test)
            PixelPerSecond = -1;
            Children[0].PixelPerSecond = -1;            
        }
    }
}