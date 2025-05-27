using System;

namespace Myrmidon.App {
    public static class Program {

        [STAThread]
        public static void Main() {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());


            var GameInstance = new MainGame();
            //GameInstance.Run();
        }
    }
}