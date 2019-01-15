// TODO deprecated
// namespace BesmashGame {
//     using GSMXtended;
//     using BesmashContent;
//     using Microsoft.Xna.Framework;
//     using System.Collections.Generic;
//     using System.Linq;

//     public class TargetSelectionPane : BesmashMenuPane {
//         public static float GRID_CELL_ALPHA {get;} = 0.25f;

//         public List<Point> Targets {get; private set;}
//         public HPane[] Grid {get; protected set;}
//         public TileMap Map {get; protected set;}
//         private int width, height;

//         public TargetSelectionPane(TileMap map) {
//             VPane vpMain = new VPane();
//             vpMain.PercentWidth = vpMain.PercentHeight = 100;

//             PercentWidth = PercentHeight = 100;
//             Targets = new List<Point>();
//             Map = map;

//             width = map.Viewport.X*2+1;
//             height = map.Viewport.Y*2+1;
//             Grid = new HPane[height];

//             for(int x, y = 0; y < height; ++y) {
//                 HPane hPane = new HPane();
//                 hPane.PercentWidth = 100;
//                 hPane.PercentHeight = (int)(100f/height + 0.5f);
//                 Grid[y] = hPane;
//                 vpMain.add(hPane);

//                 for(x = 0; x < width; ++x) {
//                     HPane square = new HPane();
//                     square.PercentWidth = (int)(100f/width + 0.5f);
//                     square.PercentHeight = 100;
//                     square.Alpha = 0.3f;
//                     hPane.add(square);
//                 }
//             }

//             add(vpMain);
//         }

//         // public override void hide() {
//         //     base.hide();
//         //     Container.applyAlpha(this, 0);
//         // }

//         // public override void show() {
//         //     base.show();
//         //     Container.applyAlpha(this, GRID_CELL_ALPHA);
//         // }

//         public void initGrid(TileMap map, List<Point> possibleTargets) {
//             int mapX = (int)(map.BattleMap.Position.X - width/2f);
//             int mapY = (int)(map.BattleMap.Position.Y - height/2f);
//             Targets = possibleTargets;
//             Map = map;

//             for(int x, y = 0; y < height; ++y) {
//                 HPane hPane = Grid[y];
//                 hPane.PixelPerSecond = -1;
//                 hPane.MillisPerScale = 0;

//                 for(x = 0; x < width; ++x) {
//                     Point tile = new Point(mapX+x, mapY+y);
//                     Color color = Targets.Contains(tile)
//                         ? Color.Green : Color.Black;

//                     HPane square = (HPane)hPane.Children[x];
//                     square.Color = color;
//                     square.PixelPerSecond = -1;
//                     square.MillisPerScale = 0;
//                 }
//             }
//         }

//         public override void update(GameTime gameTime) {
//             if(Alpha != TargetAlpha
//             || Scale != TargetScale
//             || X != TargetX || Y != TargetY)
//                 base.update(gameTime);
//         }
//     }
// }