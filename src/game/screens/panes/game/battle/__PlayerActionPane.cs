// TODO deprecated
// namespace BesmashGame {
//     using GSMXtended;
//     using BesmashContent;
//     using BesmashContent.Utility;
//     using Microsoft.Xna.Framework;
//     using Microsoft.Xna.Framework.Graphics;
//     using System.Collections.Generic;
//     using System.Linq;
    
//     public class PlayerActionPane : BesmashMenuPane {
//         private BattleOverlayPane battleOverlay;
//         private Player player;
//         private VList abilityList;
//         private VList vlSkills;
//         private SpriteFont font;

//         public VList ActionList {get; protected set;}
//         public BesmashMenuPane SkillList {get; protected set;}
//         public TargetSelectionPane TargetSelectionPane {get; protected set;}
//         public Player Player {
//             get {return player;}
//             set {
//                 player = value;
//                 SkillList.remove(SkillList.Children.ToArray());
                
//                 TextItem tiMove = new TextItem("Move", "fonts/menu_font1");
//                 TextItem tiDefend = new TextItem("Defend", "fonts/menu_font1");
//                 // tiMove.DefaultScale = 0.7f;
//                 // tiDefend.DefaultScale = 0.7f;
//                 tiMove.PPSFactor = 1000;
//                 tiDefend.PPSFactor = 1000;

//                 vlSkills = new VList(tiMove, tiDefend);
//                 vlSkills.IsStatic = false;
//                 vlSkills.VisibleRange = 3;
//                 // vlSkills.PPSFactor = 0;

//                 vlSkills.ActionEvent += (sender, args) => {
//                     int i = args.SelectedIndex;
//                     Ability ability = Player.Abilities[i-2];
//                     List<Point> targets = new List<Point>();
                    
//                     ability.getTargetSpots().ForEach(spot => targets.Add(
//                         player.Position.ToPoint()
//                         + MapUtils.rotatePoint(spot, player.Facing)));

//                     TargetSelectionPane.initGrid(
//                         player.ContainingMap,  targets);

//                     player.ContainingMap.showCursor();
//                     TargetSelectionPane.show();
//                     ((Besmash)ParentScreen.ScreenManager.Game).BattleManager.nextTurn();
//                     // ability.execute();

//                     SkillList.hide();
//                     teamInfoPane.hide();
//                     mapObjectInfoPane.show();
//                 };

//                 player.Abilities.ForEach(a => {
//                     TextItem tiSkill = new TextItem(a.Title, "fonts/menu_font1");
//                     // tiSkill.DefaultScale = 0.7f;
//                     tiSkill.PPSFactor = 1000;
//                     vlSkills.add(tiSkill);
//                 });

//                 SkillList.add(vlSkills);
//                 SkillList.load();
//             }
//         }


//         private TeamInfoPane teamInfoPane;
//         private ActionInfoPane actionInfoPane;
//         private MapObjectInfoPane mapObjectInfoPane;
//         private ActionListPane actionListPane;

//         public PlayerActionPane(SaveState activeSave) {
//             // target selection
//             TargetSelectionPane = new TargetSelectionPane(activeSave.ActiveMap);

//             // TODO
//             // TextItem tiAttack = new TextItem("Attack", "fonts/game_font1");
//             TextItem tiSkills = new TextItem("Skills", "fonts/game_font1");
//             TextItem tiMove = new TextItem("Move", "fonts/game_font1");
//             TextItem tiDefend = new TextItem("Defend", "fonts/game_font1");

//             // tiAttack.DefaultScale = 0.7f;
//             tiSkills.DefaultScale = 0.7f;
//             tiMove.DefaultScale = 0.7f;
//             tiDefend.DefaultScale = 0.7f;

//             SkillList = new BesmashMenuPane();
//             ActionList = new VList(tiSkills, tiMove, tiDefend);
//             ActionList.IsFocused = false;

//             SkillList.FocusRequestEvent += (sender, args) => {
//                 if(vlSkills != null) vlSkills.IsFocused = true;
//             };

//             SkillList.FocusLossEvent += (sender, args) => {
//                 if(vlSkills != null) vlSkills.IsFocused = false;
//             };

//             ActionList.ActionEvent += (sender, args) => {
//                 List<Point> targets = new List<Point>();
//                 // TODO (targets should be dependent on the used move)
//                 // targets.Add(new Point((int)player.Position.X+1, (int)player.Position.Y));
//                 // targets.Add(new Point((int)player.Position.X-1, (int)player.Position.Y));
//                 // targets.Add(new Point((int)player.Position.X, (int)player.Position.Y+1));
//                 // targets.Add(new Point((int)player.Position.X, (int)player.Position.Y-1));

//                 int i = args.SelectedIndex;
//                 if(i < 4) {
//                     actionInfoPane.ActionText.Text = (
//                         i == 0 ? "Attack" :
//                         i == 1 ? "Skill" :
//                         i == 2 ? "Move" :
//                         i == 3 ? "Defend" : "Observe");

//                     TargetSelectionPane.initGrid(player.ContainingMap, targets);
//                     player.ContainingMap.showCursor();
//                     TargetSelectionPane.show();
//                     SkillList.show();

//                     teamInfoPane.hide();
//                     actionListPane.hide();
//                     mapObjectInfoPane.show();
//                     actionInfoPane.show();
//                 }
                
//                 ActionList.IsFocused = false;
//             };

//             // action pane // deprecated
//             actionListPane = new ActionListPane(ActionList);
//             // actionListPane.HAlignment = HAlignment.Left;
//             // actionListPane.VAlignment = VAlignment.Bottom;
//             // actionListPane.PercentWidth = 20;
//             // actionListPane.PercentHeight = 25;
//             // actionListPane.Color = Color.Black;
//             // actionListPane.Alpha = 0.5f;

//             // skill list
//             SkillList.HAlignment = HAlignment.Left;
//             SkillList.VAlignment = VAlignment.Bottom;
//             SkillList.PercentWidth = 20;
//             SkillList.PercentHeight = 25;
//             SkillList.Color = Color.Black;
//             SkillList.Alpha = 0.5f;
//             SkillList.PPSFactor = 1000;

//             // map object info pane
//             mapObjectInfoPane = new MapObjectInfoPane(activeSave.ActiveMap.Cursor);
//             mapObjectInfoPane.HAlignment = HAlignment.Right;
//             mapObjectInfoPane.VAlignment = VAlignment.Bottom;
//             mapObjectInfoPane.PercentWidth = 25;
//             mapObjectInfoPane.PercentHeight = 20;
//             mapObjectInfoPane.Color = Color.Black;
//             mapObjectInfoPane.Alpha = 0.5f;

//             // team info pane
//             teamInfoPane = new TeamInfoPane(activeSave.Team);
//             teamInfoPane.HAlignment = HAlignment.Right;
//             teamInfoPane.VAlignment = VAlignment.Bottom;
//             teamInfoPane.PercentWidth = 40;
//             teamInfoPane.PercentHeight = 25;
//             teamInfoPane.Color = Color.Black;
//             teamInfoPane.Alpha = 0.5f;

//             // action info pane
//             actionInfoPane = new ActionInfoPane("Observe");
//             actionInfoPane.HAlignment = HAlignment.Left;
//             actionInfoPane.VAlignment = VAlignment.Bottom;
//             actionInfoPane.PercentWidth = 40;
//             actionInfoPane.PercentHeight = 10;
//             actionInfoPane.Color = Color.Black;
//             actionInfoPane.Alpha = 0.5f;

//             HPane hp = new HPane(actionListPane, SkillList);

//             StackPane sp = new StackPane(
//                 // actionListPane,
//                 SkillList,
//                 teamInfoPane,
//                 actionInfoPane,
//                 mapObjectInfoPane
//             );

//             add(sp);
//             PercentWidth = PercentHeight = 100;
//             sp.PercentWidth = sp.PercentHeight = 100;
//         }

//         public override void show(bool giveFocus, float alpha) {
//             base.show(giveFocus, alpha);
//             teamInfoPane.show(giveFocus, alpha);
//             actionListPane.show(giveFocus, alpha);
//             actionInfoPane.hide();
//             mapObjectInfoPane.hide();
//             TargetSelectionPane.hide();
//             ActionList.IsFocused = true;
//             SkillList.show();
//         }

//         public override void hide(bool takeFocus, float alpha) {
//             base.hide(takeFocus, alpha);
//             teamInfoPane.hide(takeFocus, alpha);
//             actionListPane.hide(takeFocus, alpha);
//             actionInfoPane.hide(takeFocus, alpha);
//             mapObjectInfoPane.hide(takeFocus, alpha);
//             TargetSelectionPane.hide(takeFocus, alpha);
//             SkillList.hide();
//         }

//         public override void update(GameTime time) {
//             if(!IsFocused && IsHidden) return;
//             base.update(time);

//             if(TargetSelectionPane.IsFocused) {
//                 Besmash game = (Besmash)((BesmashScreen)ParentScreen).ScreenManager.Game;
//                 Point pos = player.ContainingMap.Cursor.Position.ToPoint();

//                 if(game.isActionTriggered("game", "cancel")) {
//                     player.ContainingMap.hideCursor();
//                     show();
//                 }

//                 if(game.isActionTriggered("game", "interact")
//                 && TargetSelectionPane.Targets.Contains(pos)) {
//                     // TODO trigger action
//                     // TargetSelectionPane.hide();
//                     int x = 42;
//                 }
//             }
//         }
//     }
// }