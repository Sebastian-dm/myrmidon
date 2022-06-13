using System;
using SadConsole;
using Console = SadConsole.Console;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using clodd;
using clodd.UI;

namespace clodd {
    class GameLoop {

        public const int GameWidth = 160;
        public const int GameHeight = 80;

        // Managers
        public static UIManager UIManager;
        public static World World;

        static void Main(string[] args) {
            // Setup the engine and create the main window.
            SadConsole.Game.Create("res/fonts/Square_12x12.font", GameWidth, GameHeight);


            // Hook the start event so we can add consoles to the system.
            SadConsole.Game.OnInitialize = Init;

            // Hook the update event that happens each frame so we can trap keys and respond.
            SadConsole.Game.OnUpdate = Update;

            // Start the game.
            SadConsole.Game.Instance.Run();

            //
            // Code here will not run until the game window closes.
            //

            SadConsole.Game.Instance.Dispose();
        }


        private static void Init() {
            //Instantiate the UIManager
            UIManager = new UIManager();

            // Build the world!
            World = new World();

            // Now let the UIManager create its consoles so they can use the World data
            UIManager.Init();
        }


        private static void Update(GameTime time) {

        }



    }
}