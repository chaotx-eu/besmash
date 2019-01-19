namespace BesmashGame {
    using GSMXtended;
    using BesmashContent;
    using BesmashContent.Utility;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
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

        /// A list of player info of the current team
        private TeamInfoPane teamInfo;

        /// A list of object info beneath the cursor
        private MapObjectInfoPane objectInfo;

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

            VPane skillPane = new VPane(hpApCost, vlSkills);
            skillPane.HAlignment = HAlignment.Left;
            skillPane.VAlignment = VAlignment.Bottom;
            skillPane.PercentWidth = 15;
            skillPane.PercentHeight = 30;
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

            vlSkills.ActionEvent += (sender, args) => {
                int s = args.SelectedIndex;

                if(player.ContainingMap.Slave is Cursor) {
                    if(s == 0) move =
                        MapUtils.rotatePoint(new Point(0, -1), player.Facing);
                    else if(s > 0)
                        ability = player.Abilities[s-1];

                    vlSkills.ControlLock = false;
                    player.ContainingMap.hideCursor();
                    objectInfo.hide();
                    // teamInfo.show(); // TODO looks to busy
                } else {
                    int cost = s == 0 ? 10 : player.Abilities[s-1].APCost; // TODO move cost
                    if(cost > player.AP) return;

                    player.ContainingMap.showCursor();
                    cursor = player.ContainingMap.Cursor;
                    cursor.Position = player.Position;
                    objectInfo.MapCursor = cursor;
                    vlSkills.ControlLock = true;
                    objectInfo.show();
                    teamInfo.hide();
                }
            };

            vlSkills.CancelEvent += (sender, args) => {
                if(cursor == null) return;
                vlSkills.ControlLock = false;
                player.ContainingMap.hideCursor();
                cursor = null;
                objectInfo.hide();
                teamInfo.show();
            };

            vlSkills.SelectedEvent += (sender, args) => {
                int sel = args.SelectedIndex;
                int cost = sel == 0 ? 10 : player.Abilities[sel-1].APCost; // TODO move cost
                tiApCost.Text = string.Format("AP: {0:000}/{1:000}", Math.Abs(cost), player.AP);
                tiApCost.Color = cost < 0 ? Color.LightGreen
                    : cost > player.AP ? Color.Red
                    : cost == player.AP ? Color.Orange
                    : Color.White;
            };

            spMain.add(hlThumbs, skillPane, objectInfo, teamInfo);
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
            BattleManager.TurnList.ForEach(addThumb);
            hlThumbs.select(0);
        }

        private void addThumb(Creature c) {
            ImageItem thumb = new ImageItem(c.Thumbnail);
            thumb.PercentWidth = 80;
            thumb.PercentHeight = 80;
            hlThumbs.add(thumb);
        }

        private void showSkillList(Player player) {
            vlSkills.remove(vlSkills.Children.ToArray());

            TextItem textItem = new TextItem("Move", font);
            textItem.setPosition(vlSkills.X, vlSkills.Y);
            if(10 > player.AP) textItem.Color = Color.Gray; // TODO MoveCost property

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
            vlSkills.Scale = 1;
        }

        private void hideSkillList() {
            if(vlSkills.IsFocused) {
                vlSkills.IsFocused = false;
                vlSkills.Scale = 0;
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
            objectInfo.hide();
            teamInfo.hide();
            hlThumbs.Scale = 1;
            hlThumbs.Alpha = 0.5f;
            BattleManager.Participants.ForEach(c =>
                c.DamageEvent += onPlayerDamage);
            base.show(giveFocus, alpha);
        }

        public override void hide(bool takeFocus, float alpha) {
            base.hide(takeFocus, alpha);
            objectInfo.hide();
            teamInfo.hide();
            hlThumbs.Scale = 0;
            hlThumbs.Alpha = 0;
            BattleManager.Participants.ForEach(c =>
                c.DamageEvent -= onPlayerDamage);
        }

        private Player player = null;
        private Creature next = null;
        private Cursor cursor = null;
        private Ability ability = null;
        private Point? move = null;
        private bool started;

        public override void update(GameTime time) {
            if(!IsFocused && IsHidden) return;
            base.update(time);
            if(!IsFocused) return;

            if(next == null) {
                next = BattleManager.TurnList[0];
                next.AP += 10; // TODO ApPerTurn (APT) and cap add MaxAP
                next.applyEffects();

                if(next.HP <= 0) {
                    BattleManager.Participants.Remove(next);
                    BattleManager.TurnList.Remove(next);
                    hlThumbs.select();
                    next = null;
                    return;
                }

                if(next is Enemy)
                    hideSkillList();
                else player = (Player)next;
            }

            if(next is Enemy && ability == null && move == null) {
                ability = ((Enemy)next).nextAbility();
                if(ability == null)
                    move = ((Enemy)next).nextMove();
            }

            if(!started && cursor == null
            && player != null && !vlSkills.IsFocused) {
                showSkillList(player);
                teamInfo.ActivePlayer = player;
                teamInfo.Team = Team;
                teamInfo.show();
            }

            if(!started && (ability != null || move != null)) {
                if(move.HasValue) {
                    if(next is Enemy) next.moveTo(move.Value);
                    else next.move(move.Value);
                    next.AP -= 10; // TODO move cost
                }
                
                BattleManager.NextAction = ability;
                BattleManager.nextTurn();
                hideSkillList();
                started = true;
            }

            if(ability != null && !ability.IsExecuting
            || move != null && !next.Moving) {
                next.ContainingMap.hideCursor();
                addThumb(next);
                hlThumbs.select();
                reset();
            }

            if(cursor != null && !player.Moving)
                updateFacing(player, cursor);
        }

        private void reset() {
            cursor = null;
            ability = null;
            player = null;
            move = null;
            next = null;
            started = false;
        }

        public void onPlayerDamage(Creature sender, DamageEventArgs args) {
            if(sender.ContainingMap == null) return;
            FloatingText damageText = new FloatingText(args.DamageAmount.ToString(), font);
            damageText.Position = sender.Position;
            damageText.ScaleMod = args.WasCritical ? 2 : 1.2f;
            damageText.Color = args.DamageElement == Element.Earth ? Color.Green
                : args.DamageElement == Element.Fire ? Color.Orange
                : args.DamageElement == Element.Thunder ? Color.Yellow
                : args.DamageElement == Element.Water ? Color.LightBlue : Color.White;

            sender.ContainingMap.addEntity(damageText);
            damageText.init();
        }
    }
}