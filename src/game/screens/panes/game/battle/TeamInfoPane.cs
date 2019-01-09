namespace BesmashGame {
    using System.Linq;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Microsoft.Xna.Framework;
    using BesmashContent;
    using GSMXtended;

    public class TeamInfoPane : BesmashMenuPane {
        private static int MAX_TITLE_LEN {get;} = 15;

        private Dictionary<Player, TextItem> tiNameMap;
        private Dictionary<Player, TextItem> tiStatMap;
        private Team team;
        private VPane vpMain = new VPane();

        public Team Team {
            get {return team;}
            set {
                if(value != null) {
                    vpMain.remove(vpMain.Children.ToArray());
                    tiNameMap = new Dictionary<Player, TextItem>();
                    tiStatMap = new Dictionary<Player, TextItem>();
                    value.Player.ForEach(player => {
                        TextItem tiName = new TextItem("", "fonts/game_font1");
                        TextItem tiStat = new TextItem("", "fonts/game_font1");
                        HPane hpInfo = new HPane(tiName, tiStat);
                        tiName.HAlignment = HAlignment.Left;
                        tiStat.HAlignment = HAlignment.Right;
                        tiName.DefaultScale = 0.5f;
                        tiStat.DefaultScale = 0.5f;
                        hpInfo.PercentWidth = 100;

                        tiNameMap.Add(player, tiName);
                        tiStatMap.Add(player, tiStat);
                        vpMain.add(hpInfo);
                    });
                }

                team = value;
            }
        }

        public TeamInfoPane(Team team) {
            Team = team;
            vpMain.PercentWidth = vpMain.PercentHeight = 100;
            add(vpMain);
        }

        public override void show(bool giveFocus, float alpha) {
            base.show(giveFocus, alpha);
            Scale = 1;
        }

        public override void hide(bool takeFocus, float alpha) {
            base.hide(takeFocus, alpha);
            Scale = 0;
        }

        public override void update(GameTime gameTime) {
            if(!IsFocused && IsHidden) return;
            base.update(gameTime);
            Team.Player.ForEach(player => {
                string name;
                string stats;
                float percentHP = player.MaxHP > 0
                    ? player.HP/player.MaxHP : 0;

                name = player.Name.Length > MAX_TITLE_LEN
                    ? string.Format("     {0, -15}: ", player.Name.Substring(0, MAX_TITLE_LEN-3) + "... ... ...")
                    : string.Format("     {0, -15}: ", player.Name);

                stats = string.Format("   Lvl: {0:000} | HP: {1:0000} / {2:0000}  |  AP: {3:000} / {4:000} |     ",
                    player.Level, player.HP, player.MaxHP,
                    player.AP, player.MaxAP);

                tiNameMap[player].Text = name;
                tiNameMap[player].Color = player.ContainingMap.Cursor != null
                    && player.ContainingMap.Cursor.Position.Equals(player.Position)
                    ? Color.Yellow : Color.White;

                tiStatMap[player].Text = stats;
                tiStatMap[player].Color = percentHP < 1/3f
                    ? Color.Red : percentHP < 1/2f
                    ? Color.Orange : Color.White;
            });
        }
    }
}