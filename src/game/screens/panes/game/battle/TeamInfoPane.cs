namespace BesmashGame {
    using System.Linq;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework;
    using BesmashContent;
    using GSMXtended;

    public class TeamInfoPane : BesmashMenuPane {
        private static int MAX_TITLE_LEN {get;} = 15;

        private Dictionary<Player, TextItem> tiNameMap;
        private Dictionary<Player, TextItem> tiStatMap;
        private Team team;
        private VPane vpMain = new VPane();
        private SpriteFont font;

        /// The player whose turn it is
        public Player ActivePlayer {get; set;}

        public Team Team {
            get {return team;}
            set {
                if(value != null) {
                    vpMain.remove(vpMain.Children.ToArray());
                    tiNameMap = new Dictionary<Player, TextItem>();
                    tiStatMap = new Dictionary<Player, TextItem>();
                    value.Player.ForEach(player => {
                        TextItem tiName = new TextItem("", font);
                        TextItem tiStat = new TextItem("", font);
                        HPane hpInfo = new HPane(tiName, tiStat);
                        tiName.HAlignment = HAlignment.Left;
                        tiStat.HAlignment = HAlignment.Right;
                        tiName.DefaultScale = 0.5f;
                        tiStat.DefaultScale = 0.5f;
                        hpInfo.PercentWidth = 100;
                        tiName.PPSFactor = 1000;
                        tiStat.PPSFactor = 1000;

                        tiNameMap.Add(player, tiName);
                        tiStatMap.Add(player, tiStat);
                        vpMain.add(hpInfo);
                    });
                }

                team = value;
            }
        }

        public TeamInfoPane() : this(null) {}
        public TeamInfoPane(Team team) {
            Team = team;
            MPSFactor = 1.5f; // scale a bit faster
            PPSFactor = 2;
            vpMain.PPSFactor = 2;
            vpMain.PercentWidth = vpMain.PercentHeight = 100;
            add(vpMain);
        }

        public override void load() {
            base.load();
            font = ParentScreen.Content.Load<SpriteFont>("fonts/game_font1");
        }

        public override void show(bool giveFocus, float alpha) {
            base.show(giveFocus, alpha);

            setPosition(ParentScreen.Width, ParentScreen.Height);
            vpMain.setPosition(ParentScreen.Width, ParentScreen.Height);
            tiNameMap.Values.ToList().ForEach(ti => ti.setPosition(ParentScreen.Width, ParentScreen.Height));
            tiStatMap.Values.ToList().ForEach(ti => ti.setPosition(ParentScreen.Width, ParentScreen.Height));

            Scale = 1;
        }

        public override void hide(bool takeFocus, float alpha) {
            base.hide(takeFocus, alpha);
            Scale = 0;
        }

        public override void update(GameTime gameTime) {
            if(!IsFocused && IsHidden) return;
            base.update(gameTime);
            if(Team == null) return;

            Team.Player.ForEach(player => {
                if(player.HP <= 0) return;

                string name;
                string stats;
                float percentHP = player.MaxHP > 0
                    ? player.HP/(float)player.MaxHP : 0;

                name = player.Name.Length > MAX_TITLE_LEN
                    ? string.Format("     {0, -15}: ", player.Name.Substring(0, MAX_TITLE_LEN-3) + "... ... ...")
                    : string.Format("     {0, -15}: ", player.Name);

                stats = string.Format("   Lvl: {0:000} | HP: {1:0000} / {2:0000}  |  AP: {3:000} / {4:000} |     ",
                    player.Level, player.HP, player.MaxHP,
                    player.AP, player.MaxAP);

                tiNameMap[player].Text = name;
                tiNameMap[player].Color = player == ActivePlayer
                    ? Color.Yellow : Color.White;

                tiStatMap[player].Text = stats;
                tiStatMap[player].Color = percentHP < 1/3f
                    ? Color.Red : percentHP < 1/2f
                    ? Color.Orange : Color.White;
            });
        }
    }
}