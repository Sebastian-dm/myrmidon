using System;
using System.Text;
using Microsoft.Xna.Framework;
using GoRogue.DiceNotation;
using clodd.Entities;
using clodd.Tiles;
using System.Collections.Generic;

using clodd.Geometry;

namespace clodd.Actions {
    // Contains all generic actions performed on entities and tiles
    // including combat, movement, and so on.
    public class ActionManager {

        private Queue<Action> _actions;
        private Queue<Action> _reactions;
        private Queue<Action> _actionsDone;

        public ActionManager() {
            _actions = new Queue<Action>();
            _actionsDone = new Queue<Action>(100);
        }

        public void Update() {
            CheckKeyboard(); 
            PerformActions();
            
            //CollectEntityActions();
        }


        private void PerformActions() {
            while (_actions.Count > 0) {
                Action action = _actions.Dequeue();
                ActionResult result = action.Perform();
                
                // Try alternatives
                while (result.Alternative != null) {
                    action = result.Alternative;
                    result = action.Perform();
                }

                _actionsDone.Enqueue(action);

            }
        }

        private void CollectEntityActions() {
            foreach (Actor actor in GameLoop.World.Entities.Items) {
                _actions.Enqueue(actor.GetAction());
            }
        }


        public void AddAction(Action action) {
            if (action.IsImmediate) {
                _reactions.Enqueue(action);
            }
            else {
                _actions.Enqueue(action);
            }
        }


        /// <summary>
        /// Checks input from the player's keyboard and mouse.
        /// </summary>
        private void CheckKeyboard() {
            // F5 key to make the game full screen
            if (SadConsole.Global.KeyboardState.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.F5)) {
                SadConsole.Settings.ToggleFullScreen();
            }

            // Keyboard movement for Player character: Up arrow
            // Decrement player's Y coordinate by 1
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Up)) {
                AddAction(new WalkAction(GameLoop.World.Player, new Vector(0, -1)));
                GameLoop.UIManager.CenterOnActor(GameLoop.World.Player);
            }

            // Keyboard movement for Player character: Down arrow
            // Increment player's Y coordinate by 1
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Down)) {
                AddAction(new WalkAction(GameLoop.World.Player, new Vector(0, 1)));
                GameLoop.UIManager.CenterOnActor(GameLoop.World.Player);
            }

            // Keyboard movement for Player character: Left arrow
            // Decrement player's X coordinate by 1
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Left)) {
                AddAction(new WalkAction(GameLoop.World.Player, new Vector(-1, 0)));
                GameLoop.UIManager.CenterOnActor(GameLoop.World.Player);
            }

            // Keyboard movement for Player character: Right arrow
            // Increment player's X coordinate by 1
            if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Right)) {
                AddAction(new WalkAction(GameLoop.World.Player, new Vector(1, 0)));
                GameLoop.UIManager.CenterOnActor(GameLoop.World.Player);
            }
        }





    }
}