using System;
using System.Text;
using System.Collections.Generic;

using Myrmidon.Core.Actors;
using Myrmidon.Core.Maps.Tiles;
using Myrmidon.Core.GameState;

using Myrmidon.Core.Utilities.Geometry;


namespace Myrmidon.Core.Actions {
    // Contains all generic actions performed on entities and tiles
    // including combat, movement, and so on.
    public class ActionManager {

        private Queue<IAction> _actions;
        private Queue<IAction> _reactions;
        private Queue<IAction> _actionsDone;

        private World _world;


        public ActionManager(World world) {
            _world = world;
            _actions = new Queue<IAction>();
            _actionsDone = new Queue<IAction>(100);
        }

        public bool Update() {
            bool keyPressed = CheckKeyboard(); 
            PerformActions();
            
            CollectEntityActions();

            return keyPressed;
        }


        private void PerformActions() {
            while (_actions.Count > 0) {
                IAction action = _actions.Dequeue();
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
            foreach (Actor actor in _world.Entities.Items) {
                _actions.Enqueue(actor.GetAction());
            }
        }


        public void AddAction(IAction action) {
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
        private bool CheckKeyboard() {
            //// F5 key to make the game full screen
            //if (SadConsole.Global.KeyboardState.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.F5)) {
            //    SadConsole.Settings.ToggleFullScreen();
            //    return true;
            //}

            //// Keyboard movement for Player character: Up arrow
            //// Decrement player's Y coordinate by 1
            //else if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Up)) {
            //    AddAction(new WalkAction(_world.Player, new Vector(0, -1)));
            //    return true;
            //}

            //// Keyboard movement for Player character: Down arrow
            //// Increment player's Y coordinate by 1
            //if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Down)) {
            //    AddAction(new WalkAction(_world.Player, new Vector(0, 1)));
            //    return true;
            //}

            //// Keyboard movement for Player character: Left arrow
            //// Decrement player's X coordinate by 1
            //if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Left)) {
            //    AddAction(new WalkAction(_world.Player, new Vector(-1, 0)));
            //    return true;
            //}

            //// Keyboard movement for Player character: Right arrow
            //// Increment player's X coordinate by 1
            //if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Right)) {
            //    AddAction(new WalkAction(_world.Player, new Vector(1, 0)));
            //    return true;
            //}

            //if (SadConsole.Global.KeyboardState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Space)) {
            //    return true;
            //}

            return false;
        }





    }
}