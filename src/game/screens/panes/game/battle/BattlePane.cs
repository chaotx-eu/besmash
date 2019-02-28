namespace BesmashGame {
    using GSMXtended;
    using BesmashContent;
    using BesmashContent.Utility;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System.Collections.Generic;
    using System.Linq;
    using System;

    public class BattlePane : BesmashMenuPane {
        /// Reference to the battle manager of the game
        public BattleManager BattleManager {get; protected set;}

        /// The active team participating the battle
        public Team Team {get; set;}

        /// A list of thumbnails of creatures in the turn list
        private HList hlThumbs;

        /// Shows ap cost of currently selected move in skill list
        private HPane hpApCost;
        private TextItem tiApCost;

        /// A list of skills of the current player
        private VList vlSkills;
        private VPane skillPane;

        /// A list of player info of the current team
        private TeamInfoPane teamInfo;

        /// A list of object info beneath the cursor
        private MapObjectInfoPane objectInfo;

        /// Shows info of the currently selected ability
        private AbilityInfoPane abilityInfo;

        /// the font used within this pane
        private SpriteFont font;

        public BattlePane(BattleManager battleManager) {
            BattleManager = battleManager;

            StackPane spMain = new StackPane();
            spMain.PercentWidth = spMain.PercentHeight = 100;
            PercentWidth = PercentHeight = 100;

            hlThumbs = new HList();
            hlThumbs.HAlignment = HAlignment.Left;
            hlThumbs.VAlignment = VAlignment.Top;
            hlThumbs.PercentWidth = 15;
            hlThumbs.PercentHeight = 15;
            hlThumbs.VisibleRange = 5;
            hlThumbs.Color = Color.Black;
            hlThumbs.Alpha = 0.5f;

            hpApCost = new HPane();
            hpApCost.VAlignment = VAlignment.Top;
            hpApCost.PercentWidth = 100;
            hpApCost.Color = Color.Gray;
            hpApCost.Alpha = 0.5f;

            vlSkills = new VList();
            vlSkills.PercentWidth = 100;
            vlSkills.VisibleRange = 3;
            vlSkills.IsStatic = false;
            vlSkills.Color = Color.Gray;
            vlSkills.Alpha = 0.5f;

            skillPane = new VPane(hpApCost, vlSkills);
            skillPane.HAlignment = HAlignment.Left;
            // skillPane.HAlignment = HAlignment.Left;
            // skillPane.VAlignment = VAlignment.Bottom;
            // skillPane.PercentWidth = 15;
            // skillPane.PercentHeight = 30;
            skillPane.PercentWidth = 100;
            skillPane.PercentHeight = 100;
            skillPane.Color = Color.Black;
            skillPane.Alpha = 0.5f;

            teamInfo = new TeamInfoPane();
            teamInfo.HAlignment = HAlignment.Right;
            teamInfo.VAlignment = VAlignment.Bottom;
            teamInfo.PercentWidth = 40;
            teamInfo.PercentHeight = 20;
            teamInfo.Color = Color.Black;
            teamInfo.Alpha = 0.5f;

            objectInfo = new MapObjectInfoPane();
            objectInfo.HAlignment = HAlignment.Right;
            objectInfo.VAlignment = VAlignment.Bottom;
            objectInfo.PercentWidth = 25;
            objectInfo.PercentHeight = 20;
            objectInfo.Color = Color.Black;
            objectInfo.Alpha = 0.5f;

            abilityInfo = new AbilityInfoPane();
            abilityInfo.HAlignment = HAlignment.Left;
            abilityInfo.VAlignment = VAlignment.Bottom;
            // abilityInfo.HAlignment = HAlignment.Center;
            // abilityInfo.VAlignment = VAlignment.Bottom;
            abilityInfo.Color = Color.Black;
            abilityInfo.Alpha = 0.5f;

            HPane hpSkills = new HPane(skillPane, abilityInfo);
            hpSkills.HAlignment = HAlignment.Left;
            hpSkills.VAlignment = VAlignment.Bottom;
            hpSkills.PercentWidth = 15;
            hpSkills.PercentHeight = 30;

            vlSkills.ActionEvent += (sender, args) => {
                int s = args.SelectedIndex;

                if(player.ContainingMap.Slave is Cursor) {
                    if(s == 0) {
                        if(moveCost > player.AP || moveCost <= 0)
                            return;

                        movePath = new Queue<Point>(pathCache);
                        moveCost = getMoveCost(player, movePath.Count);
                    } else if(s > 0)
                        nextAbility = player.Abilities[s-1];

                    vlSkills.ControlLock = false;
                    player.ContainingMap.hideCursor();
                    cursor.MoveStartedEvent -= onCursorMove;
                    objectInfo.hide();
                    abilityInfo.hide();
                    hideSkillList();
                    // teamInfo.show(); // TODO looks to busy
                } else {
                    int cost = s == 0 ? 10 : player.Abilities[s-1].APCost; // TODO move cost
                    if(cost > player.AP) return;

                    playerPF = new Pathfinder(player);
                    player.ContainingMap.showCursor(player.Position);
                    cursor = player.ContainingMap.Cursor;
                    cursor.MoveStartedEvent -= onCursorMove;
                    cursor.MoveStartedEvent += onCursorMove;
                    objectInfo.MapCursor = cursor;
                    vlSkills.ControlLock = true;
                    objectInfo.show();
                    teamInfo.hide();
                    abilityInfo.hide();
                }
            };

            vlSkills.CancelEvent += (sender, args) => {
                if(cursor == null) return;
                vlSkills.ControlLock = false;
                player.ContainingMap.hideCursor();
                cursor.MoveStartedEvent -= onCursorMove;
                cursor = null;
                objectInfo.hide();
                teamInfo.show();
            };

            vlSkills.SelectedEvent += (sender, args) => {
                int sel = args.SelectedIndex;
                if(sel > 0) {
                    abilityInfo.Ability = player.Abilities[sel-1];
                    abilityInfo.show();
                } else abilityInfo.hide();
            };

            hlThumbs.SelectedEvent += (sender, args) => {
                int sel = args.SelectedIndex;
                int off = sel + hlThumbs.VisibleRange;
                if(off < hlThumbs.Children.Count)
                    (hlThumbs.Children[off] as MenuItem).Disabled = false;
            };

            // spMain.add(hlThumbs, skillPane, objectInfo, teamInfo, abilityInfo);
            spMain.add(hlThumbs, objectInfo, teamInfo, hpSkills);
            add(spMain);
            hide();
        }

        public override void load() {
            base.load();
            font = ParentScreen.Content.Load<SpriteFont>("fonts/menu_font1");
            tiApCost = new TextItem("", font);
            tiApCost.DefaultScale = 0.8f;
            hpApCost.remove(hpApCost.Children.ToArray());
            hpApCost.add(tiApCost);
        }

        private void initThumbs() {
            hlThumbs.remove(hlThumbs.Children.ToArray());
            BattleManager.TurnList.ToList().ForEach(addThumb);
            hlThumbs.select(0);
        }

        private void addThumb(Creature c) {
            ImageItem thumb = new ImageItem(c.Thumbnail);
            thumb.PercentWidth = 80;
            thumb.PercentHeight = 80;
            hlThumbs.add(thumb);

            int sel = hlThumbs.SelectedIndex;
            int off = sel - (hlThumbs.VisibleRange+2);

            if(hlThumbs.Children.Count - (sel+2) >= hlThumbs.VisibleRange)
                thumb.Disabled = true;

            if(off >= 0)
                (hlThumbs.Children[0] as MenuItem).Disabled = true;
        }

        private void showSkillList(Player player) {
            vlSkills.remove(vlSkills.Children.ToArray());

            TextItem textItem = new TextItem("Move", font);
            textItem.setPosition(vlSkills.X, vlSkills.Y);
            if(moveCost > player.AP) textItem.Color = Color.Gray; // TODO not really needed anymore

            vlSkills.add(textItem);
            player.Abilities.Sort((a1, a2) => a1.APCost.CompareTo(a2.APCost)); // order needs to be sync with order in vlist
            player.Abilities.ForEach(a => {                
                textItem = new TextItem(a.Title, font);
                textItem.setPosition(vlSkills.X, vlSkills.Y);
                if(a.APCost > player.AP) textItem.Color = Color.Gray;
                vlSkills.add(textItem);
            });

            vlSkills.select(0);
            vlSkills.IsFocused = true;
            skillPane.AlphaMod = 1;
            skillPane.Scale = 1;
        }

        private void hideSkillList() {
            if(vlSkills.IsFocused) {
                vlSkills.IsFocused = false;
                skillPane.AlphaMod = 0;
                skillPane.Scale = 0;
            }
        }

        private void updateSkillList() {
            if(!vlSkills.IsFocused) return;
            int sel = vlSkills.SelectedIndex;
            int cost = sel == 0 ? moveCost : player.Abilities[sel-1].APCost;
            if(sel == 0 && moveCost < 0) {
                tiApCost.Text = ("AP: ???");
                tiApCost.Color = Color.Orange;
            } else {
                tiApCost.Text = string.Format(
                    "AP: {0:000}/{1:000}",
                    Math.Abs(cost), player.AP);
                tiApCost.Color = cost < 0 ? Color.LightGreen
                    : cost > player.AP ? Color.Red
                    : cost == player.AP ? Color.Orange
                    : Color.White;
            }
        }

        private void updateFacing(Player p, Cursor c) {
            int xDiff = (int)(p.Position.X - c.Position.X);
            int yDiff = (int)(p.Position.Y - c.Position.Y);

            if(Math.Abs(xDiff) > Math.Abs(yDiff)) {
                if(xDiff < 0) p.Facing = Facing.East;
                else if(xDiff > 0) p.Facing = Facing.West;
            } else if(Math.Abs(xDiff) < Math.Abs(yDiff)) {
                if(yDiff < 0) p.Facing = Facing.South;
                else if(yDiff > 0) p.Facing = Facing.North;
            }
        }

        public override void show(bool giveFocus, float alpha) {
            reset();
            initThumbs();
            hideSkillList();
            objectInfo.hide();
            teamInfo.hide();
            hlThumbs.Scale = 1;
            hlThumbs.Alpha = 0.5f;
            // BattleManager.Participants.ForEach(c =>
            //     c.DamageEvent += onPlayerDamage);

            base.show(giveFocus, alpha);
        }

        public override void hide(bool takeFocus, float alpha) {
            base.hide(takeFocus, alpha);
            hideSkillList();
            objectInfo.hide();
            teamInfo.hide();
            abilityInfo.hide();
            hlThumbs.Scale = 0;
            hlThumbs.Alpha = 0;
            // BattleManager.Participants.ForEach(c =>
            //     c.DamageEvent -= onPlayerDamage);
        }

        private Player player = null;
        private Enemy enemy = null;
        private Creature next = null;
        private Cursor cursor = null;

        private bool moveFinished;
        private bool abilityRunning;
        private Point? nextMove = null;
        private Ability nextAbility = null;

        private int moveCost;
        private Pathfinder playerPF;
        private Queue<Point> movePath;
        private List<Point> pathCache = new List<Point>();

        public override void update(GameTime time) {
            if(!IsFocused && IsHidden) return;
            base.update(time);
            if(!IsFocused) return;

            if(next == null) {
                next = BattleManager.TurnList.First.Value;
                next.AP += next.APGain;
                next.applyEffects();

                if(next.HP <= 0) {
                    BattleManager.Participants.Remove(next);
                    BattleManager.TurnList.Remove(next);
                    hlThumbs.select();
                    next = null;
                    return;
                }

                if(next is Enemy) {
                    hideSkillList(); // TODO temp fix (should not be necessary)
                    enemy = (Enemy)next;
                    nextAbility = enemy.nextAbility();
                    if(nextAbility == null) enemy.nextMove(true);
                } else {
                    player = (Player)next;
                    showSkillList(player);
                    teamInfo.ActivePlayer = player;
                    teamInfo.Team = Team;
                    teamInfo.show();
                }
            }

            if(enemy != null && nextAbility == null
            && movePath == null && !enemy.Pathfinder.IsAtWork) {
                movePath = new Queue<Point>(enemy.Pathfinder.Path
                    .Where((p, i) => enemy.AP >= getMoveCost(enemy, i+1)));

                moveCost = getMoveCost(enemy, movePath.Count);
            }

            if(movePath != null) {
                if(movePath.Count > 0) {
                    if(!next.Moving)
                        next.moveTo(movePath.Dequeue());
                } else if(!moveFinished) {
                    next.AP -= moveCost;
                    moveFinished = true;
                    moveCost = 0;
                }
            }

            if(moveFinished && !next.Moving
            || nextAbility != null && !abilityRunning) {
                BattleManager.NextAction = nextAbility;
                BattleManager.nextTurn();
                abilityRunning = true;
            }

            if(moveFinished && !next.Moving
            || abilityRunning && !nextAbility.IsExecuting) {
                // TODO test
                next.ContainingMap.alignBattleMap();
                addThumb(next);
                hlThumbs.select();
                reset();
            }

            if(cursor != null && movePath == null)
                moveCost = getMoveCost(player, pathCache.Count);

            updateCursor();
            updateSkillList();
        }

        private int getMoveCost(Creature creature, int dist) {
            return dist == 0 ? 0 : creature.MoveAP*(2*dist - 1);
        }

        private void updateCursor() {
            if(cursor == null || player == null) return;
            if(!player.Moving) updateFacing(player, cursor);
            if(playerPF == null) return;

            if(playerPF.IsAtWork) {
                playerPF.update();
                cursor.Color = Microsoft.Xna.Framework.Color.Orange;
                moveCost = -1;

                if(!playerPF.IsAtWork) {
                    pathCache = new List<Point>(playerPF.Path);
                } else return;
            } else if(moveCost > player.AP || pathCache.Count == 0)
                cursor.Color = Microsoft.Xna.Framework.Color.Red;
            else cursor.Color = Microsoft.Xna.Framework.Color.Green;
        }

        private void reset() {
            pathCache.Clear();
            abilityRunning = false;
            moveFinished = false;
            nextAbility = null;
            nextMove = null;
            movePath = null;
            player = null;
            enemy = null;
            cursor = null;
            next = null;
        }

        public void onCursorMove(Movable sender, MoveEventArgs args) {
            if(sender.ContainingMap != null
            && player != null && playerPF != null) {
                int vw = player.ContainingMap.Viewport.X;
                int vh = player.ContainingMap.Viewport.Y;
                Point ct = player.ContainingMap.BattleMap.Position.ToPoint();
                Point tl = new Point(ct.X - vw, ct.Y - vh);
                Point br = new Point(ct.X + vw, ct.Y + vh);
                playerPF.getShortestPath(tl, br, p => p.Equals(args.Target));
            }
        }

        // moved to BattleManager
        // public void onPlayerDamage(Creature sender, DamageEventArgs args) {
        //     if(sender.ContainingMap == null) return;
        //     FloatingText damageText = new FloatingText(args.DamageAmount.ToString(), font);
        //     damageText.Position = sender.Position;
        //     damageText.ScaleMod = args.WasCritical ? 2 : 1.2f;
        //     damageText.Color = args.DamageElement == Element.Earth ? Color.Green
        //         : args.DamageElement == Element.Fire ? Color.Orange
        //         : args.DamageElement == Element.Thunder ? Color.Yellow
        //         : args.DamageElement == Element.Water ? Color.LightBlue : Color.White;

        //     sender.ContainingMap.addEntity(damageText);
        //     damageText.init();
        // }
    }
}