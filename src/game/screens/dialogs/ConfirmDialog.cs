namespace BesmashGame {
    using System;
    using GSMXtended;
    using Microsoft.Xna.Framework;

    // 0:yes 1:no 2:cancel
    public delegate void Callback(int answer);

    public class ConfirmDialog : BesmashDialog {
        public ConfirmDialog(BesmashScreen parent,
        Callback onConfirm, params string[] messageLines) : base(parent) {
            VPane vpMessage = new VPane();
            foreach(string line in messageLines)
                vpMessage.add(new TextItem(line, "fonts/menu_font1"));

            HList hlAnswers = new HList(
                new TextItem("Yes", "fonts/menu_font1"),
                new TextItem("No", "fonts/menu_font1"),
                new TextItem("Cancel", "fonts/menu_font1")
            );

            hlAnswers.ActionEvent += (sender, args) => {
                onConfirm(args.SelectedIndex);
                Alpha = 0;
                ExitScreen();
            };

            VPane vpMain = new VPane(vpMessage, hlAnswers);
            vpMain.Color = Color.Black;
            vpMain.PercentWidth = 100;
            vpMain.Alpha = 0.7f;

            hlAnswers.select(0);
            hlAnswers.IsFocused = true;
            MainContainer.Alpha = 0.5f;
            MainContainer.Color = Color.Black;
            MainContainer.add(vpMain);
        }
    }
}