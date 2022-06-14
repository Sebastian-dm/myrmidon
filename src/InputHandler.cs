using System;
using SadConsole;
using Microsoft.Xna.Framework;
using clodd;
using clodd.UI;

namespace clodd {
    internal static class InputHandler {

        /// <summary>
        /// Handles input from the player each game tick
        /// </summary>
        public static void Update() {
            CheckKeyboard();

        }

        /// <summary>
        /// Checks input from the player's keyboard and mouse.
        /// </summary>
        private static void CheckKeyboard() {
            // F5 key to make the game full screen
            if (SadConsole.Global.KeyboardState.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.F5)) {
                SadConsole.Settings.ToggleFullScreen();
            }

            // Keyboard movement for Player character: Up arrow
            // Decrement player's Y coordinate by 1
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Up)) {
                GameLoop.CommandManager.MoveActorBy(GameLoop.World.Player, new Point(0, -1));
                GameLoop.UIManager.CenterOnActor(GameLoop.World.Player);
            }

            // Keyboard movement for Player character: Down arrow
            // Increment player's Y coordinate by 1
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Down)) {
                GameLoop.CommandManager.MoveActorBy(GameLoop.World.Player, new Point(0, 1));
                GameLoop.UIManager.CenterOnActor(GameLoop.World.Player);
            }

            // Keyboard movement for Player character: Left arrow
            // Decrement player's X coordinate by 1
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Left)) {
                GameLoop.CommandManager.MoveActorBy(GameLoop.World.Player, new Point(-1, 0));
                GameLoop.UIManager.CenterOnActor(GameLoop.World.Player);
            }

            // Keyboard movement for Player character: Right arrow
            // Increment player's X coordinate by 1
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Right)) {
                GameLoop.CommandManager.MoveActorBy(GameLoop.World.Player, new Point(1, 0));
                GameLoop.UIManager.CenterOnActor(GameLoop.World.Player);
            }


            // Undo last command: Z
            if (SadConsole.Global.KeyboardState.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.Z)) {
                GameLoop.CommandManager.UndoMoveActorBy();
                GameLoop.UIManager.CenterOnActor(GameLoop.World.Player);
            }

            // Repeat last command: X
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.X)) {
                GameLoop.CommandManager.RedoMoveActorBy();
                GameLoop.UIManager.CenterOnActor(GameLoop.World.Player);
            }
        }


    }
}
