// TODO deprecated
// namespace BesmashGame {
//     using Microsoft.Xna.Framework;
//     using GSMXtended;

//     public class ActionInfoPane : BesmashMenuPane {
//         public TextItem ActionText {get; private set;}

//         public ActionInfoPane(string actionName) {
//             ActionText = new TextItem(actionName, "fonts/game_font1");
//             ActionText.Scale = 1.3f;
//             add(ActionText);
//         }

//         public override void show(bool giveFocus, float alpha) {
//             base.show(giveFocus, alpha);
//             Scale = 1;
//         }

//         public override void hide(bool takeFocus, float alpha) {
//             base.hide(takeFocus, alpha);
//             Scale = 0;
//         }
//     }
// }