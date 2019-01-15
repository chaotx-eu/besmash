// TODO deprecated
// namespace BesmashGame {
//     using GSMXtended;
//     using BesmashContent;
//     using Microsoft.Xna.Framework;
//     using System.Collections.Generic;
//     using System.Linq;
    
//     public class BattleOrderPane : BesmashMenuPane {
//         public BattleManager BattleManager {get; private set;}
//         public List<ImageItem> Thumbnails {get; protected set;}
//         public int MaxParticipants {get; protected set;} = 8; // TODO currently not in use

//         private HPane hpMain;

//         public BattleOrderPane(BattleManager battleManager) {
//             hpMain = new HPane();
//             hpMain.PercentWidth = hpMain.PercentHeight = 100;

//             BattleManager = battleManager;
//             Thumbnails = new List<ImageItem>();

//             HPane gapL = new HPane();
//             gapL.PercentWidth = 5;
//             hpMain.add(gapL);
//             add(hpMain);
//         }

//         // TODO test method
//         public void initThumbnails() {
//             hpMain.remove(hpMain.Children.ToArray());
//             BattleManager.TurnList.ForEach(c => {
//                 ImageItem thumb = new ImageItem(c.Thumbnail);
//                 thumb.HAlignment = HAlignment.Left;
//                 thumb.PercentHeight = 80;
//                 thumb.PercentWidth = 10;
//                 Thumbnails.Add(thumb);
//                 hpMain.add(thumb);
//             });
//         }

//         public override void show(bool giveFocus, float alpha) {
//             initThumbnails();
//             base.show(giveFocus, alpha);
//             Scale = 1;
//         }

//         public override void hide(bool takeFocus, float alpha) {
//             base.hide(takeFocus, alpha);
//             Scale = 0;
//         }
//     }
// } 