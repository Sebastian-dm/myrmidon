using System;
using SadConsole;
using Console = SadConsole.Console;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Clodd
{
    internal class MainLoop
    {
        public const int SCREEN_WIDTH = 80;
        public const int SCREEN_HEIGHT = 25;

        private static SadConsole.Entities.Entity player;

        private static void Main(string[] args)
        {
            SadConsole.Settings.UseDefaultExtendedFont = true;

            SadConsole.Game.Create(SCREEN_WIDTH, SCREEN_HEIGHT);
            SadConsole.Game.OnInitialize = Init;
            SadConsole.Game.OnUpdate = Update;
            SadConsole.Game.Instance.Run();

            CreatePlayer();


            // Code here will not run until the game window closes.
            SadConsole.Game.Instance.Dispose();
        }

        /// <summary>
        /// Any custom loading and prep. We will use a sample console for now
        /// </summary>
        private static void Init()
        {
            Console startingConsole = new Console(SCREEN_WIDTH, SCREEN_HEIGHT);
            //startingConsole.FillWithRandomGarbage();
            startingConsole.Fill(new Rectangle(3, 3, 27, 5), null, Color.Black, 0, SpriteEffects.None);
            //startingConsole.Print(6, 5, "Hello from SadConsole", ColorAnsi.CyanBright);

            CreatePlayer();
            startingConsole.Children.Add(player);

            // Set our new console as the thing to render and process
            SadConsole.Global.CurrentScreen = startingConsole;

        }

        /// <summary>
        /// Called each logic update.
        /// </summary>
        /// <param name="time"></param>
        private static void Update(GameTime time) {
            HandleInput();
            
        }


        /// <summary>
        /// Create a player using SadConsole's Entity class
        /// </summary>
        private static void CreatePlayer() {
            player = new SadConsole.Entities.Entity(1, 1);
            player.Animation.CurrentFrame[0].Glyph = '@';
            player.Animation.CurrentFrame[0].Foreground = Color.HotPink;
            player.Position = new Point(20, 10);
        }

        /// <summary>
        /// Handles input from the player's keyboard and mouse each game tick
        /// </summary>
        private static void HandleInput() {
            // As an example, we'll use the F5 key to make the game full screen
            if (SadConsole.Global.KeyboardState.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.F5)) {
                SadConsole.Settings.ToggleFullScreen();
            }

            // Keyboard movement for Player character: Up arrow
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Up)
                || SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.NumPad8)) {
                player.Position += new Point(0, -1);
            }
            // Keyboard movement for Player character: Down arrow
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Down)
                || SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.NumPad2)) {
                player.Position += new Point(0, 1);
            }
            // Keyboard movement for Player character: Left arrow
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Left)
                || SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.NumPad4)) {
                player.Position += new Point(-1, 0);
            }
            // Keyboard movement for Player character: Right arrow
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Right)
                || SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.NumPad6)) {
                player.Position += new Point(1, 0);
            }

            // Keyboard movement for Player character: Right+up
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.NumPad9)) {
                player.Position += new Point(1, -1);
            }
            // Keyboard movement for Player character: Left+Up
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.NumPad7)) {
                player.Position += new Point(-1, -1);
            }
            // Keyboard movement for Player character: Right+down
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.NumPad3)) {
                player.Position += new Point(1, 1);
            }
            // Keyboard movement for Player character: Left+down
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.NumPad1)) {
                player.Position += new Point(-1, 1);
            }


        }


    }
}