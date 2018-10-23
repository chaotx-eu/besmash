using System;

namespace BesmashGame {
    public static class Program {
        [STAThread]
        static void Main() {
            using (var game = new Besmash())
                game.Run();
        }
    }
}
