using System;

using SadConsole;
using Console = SadConsole.Console;
using Microsoft.Xna.Framework.Graphics;
using myrmidon;
using myrmidon.UI;
using myrmidon.Actions;

namespace myrmidon {
    public static class GameLoop {

        public const int GameWidth = 89;
        public const int GameHeight = 66;

        // Managers
        public static UIManager UIManager;
        public static ActionManager ActionManager;
        public static World World;

        public static Map.FieldOfView FOV;



        public static void Main(string[] args) {
            // Setup the engine and create the main window.
            Game.Create("fonts/Square_16x16.font", GameWidth, GameHeight);
            //SadConsole.Game.Create("res/fonts/Graphic_16x16.font", GameWidth, GameHeight);


            // Hook the start event so we can add consoles to the system.
            Game.OnInitialize = Init;

            // Hook the update event that happens each frame so we can trap keys and respond.
            Game.OnUpdate = Update;

            // Start the game.
            Game.Instance.Run();

            //
            // Code here will not run until the game window closes.
            //

            Game.Instance.Dispose();
        }


        private static void Init() {
            //Instantiate the managers
            UIManager = new UIManager();
            ActionManager = new ActionManager();
            FOV = new Map.FieldOfView(viewDistance: 10);

            // Build the world!
            World = new World();

            // Now let the UIManager create its consoles so they can use the World data
            UIManager.Init();

        }


        private static void Update(Microsoft.Xna.Framework.GameTime time) {
            World.Update();
            if (World.IsMapGenInProgress) UIManager.RefreshConsole();
            ActionManager.Update();
            

        }



    }
}