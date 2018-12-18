namespace BesmashGame {
    using GSMXtended;

    public class ActionInfoPane : StackPane {
        public TextItem ActionText {get; private set;}

        public ActionInfoPane(string actionName) {
            ActionText = new TextItem(actionName, "fonts/game_font1");
            ActionText.Scale = 1.3f;
            add(ActionText);
        }
    }
}