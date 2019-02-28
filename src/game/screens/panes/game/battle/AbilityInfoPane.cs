namespace BesmashGame {
    using Microsoft.Xna.Framework.Graphics;
    using System.Collections.Generic;
    using BesmashContent;
    using GSMXtended;
    using System.Linq;
    using System.Text;

    public class AbilityInfoPane : BesmashMenuPane {
        public int MaxColumns {get; set;}
        public int MaxRows {get; set;}
        public Ability Ability {get; set;}

        private List<TextItem> lines;

        public AbilityInfoPane() : this(28, 14) {}
        public AbilityInfoPane(int maxColumns, int maxRows) {
            lines = new List<TextItem>();
            MaxColumns = maxColumns;
            MaxRows = maxRows;

            for(int i = 0; i < MaxRows; ++i) {
                TextItem textItem = new TextItem("", "fonts/game_font1");
                textItem.Scale = 0.5f;
                lines.Add(textItem);
                add(textItem);
            }
        }

        public override void show(bool giveFocus, float alpha) {
            base.show(giveFocus, alpha);
            if(MaxColumns <= 0 || MaxRows <= 0 || Ability == null)
                return;

            StringBuilder word = new StringBuilder(MaxColumns);
            string text = Ability.Description;
            int column = 0, row = 0;
            bool spaceFlag = false;

            if(text != null) {
                lines[0].Text = "";
                text = text.Trim().Replace('\n', ' ');
                text.ToList().ForEach(c => {
                    if(c != ' ' || !spaceFlag) {
                        spaceFlag = c == ' ';
                        word.Append(c);
                        if(spaceFlag) { // word complete
                            lines[row].Text += word;
                            word = new StringBuilder(MaxColumns);
                        }

                        if(++column >= MaxColumns) {
                            if(++row >= MaxRows) return;
                            lines[row].Text = "";
                            column = word.Length;
                        }
                    }
                });

                if(word.Length > 0 && row < MaxRows)
                    lines[row].Text = word.ToString();
            } else hide();
        }
    }
}