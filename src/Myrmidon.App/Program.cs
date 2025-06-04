using System;

using Myrmidon.App.UI;

namespace Myrmidon.App {
    public static class Program {

        [STAThread]
        public static void Main() {


            MainGame mainGame = new MainGame("Myrmidon");
            UiController uiController = new UiController(mainGame.Context);
            uiController.Run();
        }
    }
}