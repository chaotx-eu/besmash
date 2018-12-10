namespace BesmashGame {
    using Config;
    using GSMXtended;
    using System.Text;
    using System.Linq;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    /// A horizontal pane to show info about the
    /// actions of dedicated keys/buttons
    public class InputInfoPane : HPane {
        private Dictionary<Keys, string> keyInfoMap;
        private Dictionary<Buttons, string> buttonInfoMap;
        private TextItem infoText;

        /// Creates a new input info pane object with
        /// each pair of strings within the passed params
        /// treated as an action/text pair. If the count
        /// of params is uneven the last text will be an
        /// empty string
        // public InputInfoPane(GameConfig config, params string[] args) {
        public InputInfoPane(Dictionary<Keys, string> keyInfoMap)
            : this(keyInfoMap, new Dictionary<Buttons, string>()) {}

        public InputInfoPane(
            Dictionary<Keys, string> keyInfoMap,
            Dictionary<Buttons, string> buttonInfoMap)
        {
            this.keyInfoMap = keyInfoMap;
            this.buttonInfoMap = buttonInfoMap;
            infoText = new TextItem("fwefers", "fonts/menu_font1");
            infoText.DefaultScale = 0.75f;

            add(infoText);
        }

        public override void update(GameTime time) {
            base.update(time);
            StringBuilder sb = new StringBuilder();

            if(GamePad.GetState(0).IsConnected)
                buttonInfoMap.Keys.ToList().ForEach(btn
                    => sb.Append((sb.Length > 0 ? ",  " : "")
                    + btn + ": " + buttonInfoMap[btn]));
            else {
                keyInfoMap.Keys.ToList().ForEach(key
                    => sb.Append((sb.Length > 0 ? ",  " : "")
                    + key + ": " + keyInfoMap[key]));
            }

            infoText.Text = sb.ToString();
        }
    }
}