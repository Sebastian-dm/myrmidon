using System;
using SadConsole;
using Console = SadConsole.Console;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using clodd;

namespace clodd
{
    internal class MainLoop
    {
        public static Console MainConsole;

        public const int Width = 80;
        public const int Height = 25;
        
        public static Player player;

        public static Map GameMap;
        private static int _mapWidth = 100;
        private static int _mapHeight = 100;
        private static int _maxRooms = 500;
        private static int _minRoomSize = 4;
        private static int _maxRoomSize = 15;
               

        private static void Main(string[] args)
        {
            // Setup the engine and create the main window.
            SadConsole.Game.Create(Width, Height);
            // Hook the start event so we can add consoles to the system.
            SadConsole.Game.OnInitialize = Init;
            // Hook the update event that happens each frame so we can trap keys and respond.
            SadConsole.Game.OnUpdate = Update;
            // Start the game.
            SadConsole.Game.Instance.Run();

            // Code here will not run until the game window closes.
            
            SadConsole.Game.Instance.Dispose();
        }

        /// <summary>
        /// Any custom loading and prep. We will use a sample console for now
        /// </summary>
        private static void Init()
        {
            // Initialize an empty map
            GameMap = new Map(_mapWidth, _mapHeight);

            // Instantiate a new map generator and populate the map with rooms and tunnels
            MapGenerator mapGen = new MapGenerator();
            GameMap = mapGen.GenerateMap(_mapWidth, _mapHeight, _maxRooms, _minRoomSize, _maxRoomSize);

            // Create a console using gameMap's tiles
            Console MainConsole = new ScrollingConsole(GameMap.Width, GameMap.Height, Global.FontDefault, new Rectangle(0, 0, Width, Height), GameMap.Tiles);

            // Set our new console as the thing to render and process
            SadConsole.Global.CurrentScreen = MainConsole;

            // Create player
            player = new Player(Color.Yellow, Color.Transparent);
            player.Position = new Point(5, 5);
            MainConsole.Children.Add(player);

        }

        /// <summary>
        /// Called each logic update.
        /// </summary>
        /// <param name="time"></param>
        private static void Update(GameTime time) {
            InputHandler.Update();
            
        }




    }
}