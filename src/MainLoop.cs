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
        public const int Width = 80;
        public const int Height = 25;

        public static SadConsole.Entities.Entity player;
        public static Console MainConsole;

        private static void Main(string[] args)
        {
            SadConsole.Settings.UseDefaultExtendedFont = true;

            SadConsole.Game.Create(Width, Height);
            SadConsole.Game.OnInitialize = Init;
            SadConsole.Game.OnUpdate = Update;
            SadConsole.Game.Instance.Run();

            // Code here will not run until the game window closes.
            SadConsole.Game.Instance.Dispose();
        }

        /// <summary>
        /// Any custom loading and prep. We will use a sample console for now
        /// </summary>
        private static void Init()
        {
            // Create map
            Map.CreateMap();
            MainConsole = new ScrollingConsole(Width, Height, Global.FontDefault, new Rectangle(0, 0, Width, Height), Map._tiles);

            //Create player
            player = new Player();
            MainConsole.Children.Add(player);

            // Set our new console as the thing to render and process
            SadConsole.Global.CurrentScreen = MainConsole;

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