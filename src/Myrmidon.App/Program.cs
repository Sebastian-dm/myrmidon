using System;

namespace Myrmidon.MonoGame {
    public static class Program {

        [STAThread]
        public static void Main() {
            var GameInstance = new Game1();
            GameInstance.Run();
        }
    }
}