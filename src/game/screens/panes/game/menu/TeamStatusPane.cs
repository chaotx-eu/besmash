namespace BesmashGame {
    using GSMXtended;
    using BesmashContent;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System.Collections.Generic;
    using System.Linq;

    public class TeamStatusPane : BesmashMenuPane {
        public Team Team {get; set;}
        private SpriteFont font;

        private HList hlThumbnails;
        private TextItem tiPlayerName;
        private TextItem tiPlayerLevel;
        private TextItem tiPlayerEXP;
        private TextItem tiPlayerHP;
        private TextItem tiPlayerAP;
        private TextItem[] tiStatTypes;
        private TextItem[] tiPlayerStats;

        public TeamStatusPane() {
            hlThumbnails = new HList();
            hlThumbnails.PercentHeight = 100;
            hlThumbnails.SelectedColor = Color.White;
            hlThumbnails.CancelEvent += (sender, args) => {
                hlThumbnails.IsFocused = false;
                hide();
            };

            FocusRequestEvent += (sender, args) => {
                hlThumbnails.IsFocused = true;
                hlThumbnails.select(0);
            };

            build();
            hide(); // TODO GSMXtended.ScreenComponent => targetAlphaMod = 0 initially
        }

        /// Initializes structural components
        protected void build() {
            remove(Children.ToArray());
            tiPlayerName = new TextItem();
            tiPlayerLevel = new TextItem();
            tiPlayerEXP = new TextItem();
            tiPlayerHP = new TextItem();
            tiPlayerAP = new TextItem();
            tiStatTypes = new TextItem[6];
            tiPlayerStats = new TextItem[6];

            tiPlayerName.DefaultScale = 0.7f;
            tiPlayerLevel.DefaultScale = 0.7f;
            tiPlayerEXP.DefaultScale = 0.7f;
            tiPlayerHP.DefaultScale = 0.7f;
            tiPlayerAP.DefaultScale = 0.7f;

            HPane hpThumbs = new HPane(hlThumbnails);
            VPane vpInfo = new VPane();
            VPane vpStats = new VPane();
            VPane vpMain = new VPane(hpThumbs, vpInfo, vpStats);
            add(vpMain);

            vpMain.PercentWidth = 100;
            vpMain.PercentHeight = 100;

            hpThumbs.PercentWidth = 100;
            hpThumbs.PercentHeight = 20;
            hpThumbs.Color = Color.Black;
            hpThumbs.EffectAlpha = 0.5f;

            vpInfo.PercentWidth = 100;
            vpInfo.PercentHeight = 10;
            vpInfo.Color = Color.Gray;
            vpInfo.EffectAlpha = 0.5f;

            vpStats.PercentWidth = 60;
            vpStats.PercentHeight = 70;
            vpStats.Color = Color.Black;
            vpStats.EffectAlpha = 0.5f;

            HPane hpName = new HPane(tiPlayerName);
            HPane hpLevel = new HPane(tiPlayerLevel);
            HPane hpExp = new HPane(tiPlayerEXP);
            HPane hpHP = new HPane(tiPlayerHP);
            HPane hpAP = new HPane(tiPlayerAP);
            HPane hpTop = new HPane(hpName, hpLevel, hpExp);
            HPane hpBot = new HPane(hpHP, hpAP);

            hpTop.PercentWidth = 100;
            hpBot.PercentWidth = 100;
            hpName.PercentWidth = 20;
            hpLevel.PercentWidth = 20;
            hpLevel.PercentWidth = 20;
            hpHP.PercentWidth = 20;
            hpAP.PercentWidth = 20;
            vpInfo.add(hpTop, hpBot);

            for(int i = 0; i < 6; ++i) {
                tiPlayerStats[i] = new TextItem();
                tiStatTypes[i] = new TextItem(((StatType)i).ToString() + ":");

                HPane hpL = new HPane(tiStatTypes[i]);
                HPane hpR = new HPane(tiPlayerStats[i]);
                HPane hpC = new HPane(hpL, hpR);

                hpL.PercentWidth = 40;
                hpR.PercentWidth = 40;
                hpC.PercentWidth = 100;
                vpStats.add(hpC);
            }
        }

        public override void load() {
            if(font == null) font =
                ParentScreen.Content
                .Load<SpriteFont>("fonts/menu_font1");

            tiPlayerName.Font = font;
            tiPlayerLevel.Font = font;
            tiPlayerEXP.Font = font;
            tiPlayerHP.Font = font;
            tiPlayerAP.Font = font;
            for(int i = 0; i < tiStatTypes.Length; ++i) {
                tiStatTypes[i].Font = font;
                tiPlayerStats[i].Font = font;
            }

            base.load();
        }

        /// Sets thumbnails to those from the team players
        public void initThumbnails() {
            hlThumbnails.remove(hlThumbnails.Children.ToArray());
            if(Team != null) Team.Player.ForEach(player => {
                ImageItem thumb = new ImageItem(player.Thumbnail);
                thumb.Width = thumb.Height = 128;
                hlThumbnails.add(thumb);
            });
        }

        /// Updates the textfields according to the
        /// properties of the seleccted player
        public override void update(GameTime time) {
            if(!IsFocused && IsHidden) return;
            base.update(time);

            int s = hlThumbnails.SelectedIndex;
            tiPlayerName.Text = Team == null ? "n/a" : (Team.Player[s].Name + " - " + Team.Player[s].Class.Title);
            tiPlayerLevel.Text = "Level " + (Team == null ? "n/a" : Team.Player[s].Level.ToString());
            tiPlayerEXP.Text = "EXP: " + (Team == null ? "n/a" : (Team.Player[s].Exp.ToString() + "/" + Team.Player[s].MaxExp.ToString()));
            tiPlayerHP.Text = "HP: " + (Team == null ? "n/a" : (Team.Player[s].HP.ToString() + "/" + Team.Player[s].MaxHP.ToString()));
            tiPlayerAP.Text = "AP: " + (Team == null ? "n/a" : (Team.Player[s].AP.ToString() + "/" + Team.Player[s].MaxAP.ToString()));
            for(int i = 0; i < 6; ++i) tiPlayerStats[i].Text = Team == null ? "n/a"
                : Team.Player[s].Stats.get((StatType)i).ToString();
        }

        public override void show(bool giveFocus, float alpha) {
            base.show(giveFocus, alpha);
            initThumbnails();
        }
    }
}