namespace BesmashGame {
    using GSMXtended;
    using BesmashContent;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System.Collections.Generic;
    using System.Linq;


    public class TeamFormationPane : BesmashMenuPane {
        public Team Team {get; set;}

        private VList vlGrid;
        private int width, height;

        public TeamFormationPane() : this(null, 5, 2) {}
        public TeamFormationPane(Team team) : this(team, 5, 2) {}
        public TeamFormationPane(Team team, int width, int height) {
            ConfirmDialog cd = new ConfirmDialog(ParentScreen as BesmashScreen, (a) => {
            }, "No Leader assigned", "Omit changes?");

            Team = team;
            this.width = width;
            this.height = height;
            initGrid();
            hide();
        }

        public override void show(bool giveFocus, float alpha) {
            initTeam();
            vlGrid.IsFocused = true;
            base.show(giveFocus, alpha);
        }

        public override void hide(bool takeFocus, float alpha) {
            vlGrid.IsFocused = false;
            base.hide(takeFocus, alpha);
        }

        private void initTeam() {
            playerMap = new Dictionary<ImageItem, Player>();
            Point p;

            for(int x, y = 0; y < height; ++y) {
                for(x = 0; x < width; ++x) {
                    Player player = null;

                    if(Team != null) {
                        p = new Point(x - width/2, y);

                        if(p.Equals(Point.Zero))
                            player = Team.Leader;
                        else {
                            KeyValuePair<Player, Point> pair = Team.Formation
                                .Where(kv => kv.Value.Equals(p))
                                .FirstOrDefault();

                            if(!pair.Equals(default(KeyValuePair<Player, Point>)))
                                player = pair.Key;
                        }
                    }

                    StackPane cell = grid[x, y];
                    cell.remove(cell.Children.ToArray());

                    if(player != null) {
                        ImageItem image = new ImageItem(player.Image);
                        image.SourceRectangle = new Rectangle(0, 0, 16, 16);
                        image.PercentWidth = image.PercentHeight = 80;
                        playerMap.Add(image, player);
                        cell.add(image);
                    }
                }
            }
        }

        private int columnIndex;
        private StackPane[,] grid;
        private ImageItem selected;
        private Dictionary<ImageItem, Player> playerMap;

        private void initGrid() {
            if(vlGrid != null) remove(vlGrid);
            grid = new StackPane[width, height];

            vlGrid = new VList();
            vlGrid.PercentWidth = 100;
            vlGrid.PercentHeight = 50;
            vlGrid.MillisPerInput = 128;
            vlGrid.InputSingleMode = true;

            vlGrid.CancelEvent += (sender, args) => {
                if(!setFormation()) ParentScreen.ScreenManager
                    .AddScreen(new ConfirmDialog(ParentScreen as BesmashScreen, (a) => {
                        if(a == 0) hide(); // yes -> hide()
                        if(a == 1) ; // no  -> do nothing
                        if(a == 2) ; // cancel -> do nothing
                    }, "No Leader assigned", "Omit changes?"), null);
                else hide();
            };

            vlGrid.SelectedEvent += (sender, args) => {
                if(args.SelectedItem is HList) {
                    HList row = args.SelectedItem as HList;
                    row.IsFocused = true;
                    row.select(columnIndex);
                }
            };

            vlGrid.DeselectedEvent += (sender, args) => {
                if(args.SelectedItem is HList) {
                    HList row = args.SelectedItem as HList;
                    row.IsFocused = false;
                    row.SelectedIndex = -1;
                }
            };

            for(int x, y = 0, c = 0; y < height; ++y) {
                HList row = new HList();
                row.PercentWidth = 100;
                row.PercentHeight = (int)(100f/height);
                row.IsStatic = true;
                row.MillisPerInput = 128;
                row.InputSingleMode = true;

                row.SelectedEvent += (sender, args) => {
                    Container cell = args.SelectedItem as Container;
                    columnIndex = args.SelectedIndex;

                    // if(cell.Children.Count > 0)
                    //     cell.Children[0].EffectScale = 1.2f;

                    if(cursor != null) {
                        // if(cursor.ParentContainer != null)
                        //     cursor.ParentContainer.remove(cursor);

                        if(cursor.ParentContainer != null) {
                            // image switching
                            if(selected != null && cell.Children.Count > 0) {
                                cursor.ParentContainer.add(cell.Children[0]);
                                cell.remove(cell.Children[0]);
                            }

                            cursor.ParentContainer.remove(cursor);
                        }

                        cell.add(cursor);
                    }
                };

                row.ActionEvent += (sender, args) => {
                    Container cell = args.SelectedItem as Container;
                    ImageItem pick = cell.Children.Count > 0
                        ? cell.Children[0] as ImageItem : null;

                    if(pick != null) {
                        cell.remove(pick);
                        cursor.prepend(pick);
                        pick.EffectScale = 1.2f;
                    }

                    if(selected != null) {
                        cell.prepend(selected);
                        cursor.remove(selected);
                        selected.EffectScale = 1;
                    }

                    selected = pick;
                };

                for(x = 0; x < width; ++x, ++c) {
                    StackPane cell = new StackPane();
                    cell.PercentWidth = (int)(100f/width);
                    cell.PercentHeight = 100;
                    cell.Color = (c%2) == 0 ? Color.Black : Color.Gray;
                    cell.Alpha = 0.5f;
                    grid[x, y] = cell;
                    row.add(cell);
                }

                vlGrid.add(row);
            }

            add(vlGrid);
        }

        private bool setFormation() {
            int leaderX = width/2;
            int leaderY = 0;

            if(grid[leaderX, leaderY].Children.Count == 0
            || grid[leaderX, leaderY].Children[0] as ImageItem == null)
                return false;

            for(int x, y = 0; y < height; ++y) {
                for(x = 0; x < width; ++x) {
                    Player player = null;
                    ImageItem image = null;
                    StackPane cell = grid[x, y];

                    if(cell.Children.Count > 0) image = cell.Children[0] as ImageItem;
                    if(image != null) player = playerMap.GetValueOrDefault(image);

                    if(x == leaderX && y == leaderY) {
                        if(player != Team.Leader) {
                            player.ContainingMap.Slave = player;
                            Team.Formation.Remove(player);
                            Team.addMembers(Team.Leader);
                            Team.removeMember(player);
                            Team.Leader = player;
                        }
                    } else if(player != null)
                        Team.Formation[player] = new Point(x - leaderX, y - leaderY);
                }
            }

            return true;
        }

        private StackPane cursor;
        private ImageItem cursorItem;
        private Texture2D cursorSheet;
        private int cursorFPS = 4;
        private int spriteCount = 6;

        public override void load() {
            base.load();
            cursorSheet = ParentScreen.Content
                .Load<Texture2D>("images/world/entities/cursor/generic_cursor");

            cursorItem = new ImageItem(cursorSheet);
            cursorItem.PercentWidth = cursorItem.PercentHeight = 100;
            cursorItem.SourceRectangle = new Rectangle(0, 0, 16, 16);

            cursor = new StackPane(cursorItem);
            cursor.PercentWidth = cursor.PercentHeight = 100;
            cursor.PPSFactor = 4;
        }

        private int timer;
        public override void update(GameTime time) {
            if(!IsFocused && IsHidden) return;
            base.update(time);

            if(timer > 1000f/cursorFPS) {
                updateCursor();
                timer = 0;
            } else timer += time.ElapsedGameTime.Milliseconds;
        }

        private void updateCursor() {
            int x = cursorItem.SourceRectangle.Value.X;
            int w = cursorItem.SourceRectangle.Value.Width;
            x = selected != null ? 0 : (x + w)%(spriteCount*w);
            cursorItem.SourceRectangle = new Rectangle(x, 0, w, w);
        }
    }
}