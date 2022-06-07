using System;
using SadConsole;
using Console = SadConsole.Console;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace clodd
{
    internal class Program
    {
        public const int SCREEN_WIDTH = 80;
        public const int SCREEN_HEIGHT = 25;
        private static void Main(string[] args)
        {
            SadConsole.Settings.UseDefaultExtendedFont = true;

            SadConsole.Game.Create(SCREEN_WIDTH, SCREEN_HEIGHT);
            SadConsole.Game.OnInitialize = Init;
            SadConsole.Game.OnUpdate = Update;
            SadConsole.Game.Instance.Run();
            //
            // Code here will not run until the game window closes.
            //
            SadConsole.Game.Instance.Dispose();
        }

        private static void Init()
        {
            // Any custom loading and prep. We will use a sample console for now

            Console startingConsole = new Console(SCREEN_WIDTH, SCREEN_HEIGHT);
            startingConsole.FillWithRandomGarbage();
            startingConsole.Fill(new Rectangle(3, 3, 27, 5), null, Color.Black, 0, SpriteEffects.None);
            startingConsole.Print(6, 5, "Hello from SadConsole", ColorAnsi.CyanBright);

            // Set our new console as the thing to render and process
            SadConsole.Global.CurrentScreen = startingConsole;

        }

        private static void Update(GameTime time) {
            // Called each logic update.
            // As an example, we'll use the F5 key to make the game full screen
            if (SadConsole.Global.KeyboardState.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.F5)) {
                SadConsole.Settings.ToggleFullScreen();
            }
        }
    }
}