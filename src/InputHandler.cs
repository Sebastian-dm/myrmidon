using System;
using SadConsole;
using Microsoft.Xna.Framework;
using clodd;

namespace clodd {
    internal static class InputHandler {

        /// <summary>
        /// Handles input from the player's keyboard and mouse each game tick
        /// </summary>
        public static void Update() {
            // As an example, we'll use the F5 key to make the game full screen
            if (SadConsole.Global.KeyboardState.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.F5)) {
                SadConsole.Settings.ToggleFullScreen();
            }

            // Keyboard movement for Player character: Up arrow
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Up)
                || SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.NumPad8)) {
                MainLoop.player.Position += new Point(0, -1);
            }
            // Keyboard movement for Player character: Down arrow
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Down)
                || SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.NumPad2)) {
                MainLoop.player.Position += new Point(0, 1);
            }
            // Keyboard movement for Player character: Left arrow
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Left)
                || SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.NumPad4)) {
                MainLoop.player.Position += new Point(-1, 0);
            }
            // Keyboard movement for Player character: Right arrow
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Right)
                || SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.NumPad6)) {
                MainLoop.player.Position += new Point(1, 0);
            }

            // Keyboard movement for Player character: Right+up
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.NumPad9)) {
                MainLoop.player.Position += new Point(1, -1);
            }
            // Keyboard movement for Player character: Left+Up
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.NumPad7)) {
                MainLoop.player.Position += new Point(-1, -1);
            }
            // Keyboard movement for Player character: Right+down
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.NumPad3)) {
                MainLoop.player.Position += new Point(1, 1);
            }
            // Keyboard movement for Player character: Left+down
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.NumPad1)) {
                MainLoop.player.Position += new Point(-1, 1);
            }


        }
    }
}
