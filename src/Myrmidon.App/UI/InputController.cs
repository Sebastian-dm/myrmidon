using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Myrmidon.App.UI;
using Myrmidon.Core.Actions;

namespace Myrmidon.App.UI {
    public class InputController {

        private IUiController _uiController;
        private IActionController _actionController;


        public InputController(IUiController uiController, IActionController actionController) {
            // Initialize input handling logic here
            // This could include setting up event listeners, key bindings, etc.

            _uiController = uiController;
            _actionController = actionController;
        }

        public bool HandleInput(KeyEventArgs e) {
            // Implement input handling logic here
            // This could include reading keyboard or mouse input, processing commands, etc.
            // For example, you might check for key presses and execute corresponding actions
            // based on the current game state or UI context.

            switch (e.KeyCode) {

                case Keys.Up:
                    // Handle up arrow key to move player up
                    _actionController.AddAction(new WalkAction(_world.Player, new Vector(0, -1)));
                    return true;
                case Keys.Down:
                    // Handle down arrow key to move player down
                    _actionController.AddAction(new WalkAction(_world.Player, new Vector(0, 1)));
                    return true;
                case Keys.Left:
                // Handle left arrow key to move player left
                    _actionController.AddAction(new WalkAction(_world.Player, new Vector(-1, 0)));
                    return true;
                case Keys.Right:
                // Handle right arrow key to move player right
                    _actionController.AddAction(new WalkAction(_world.Player, new Vector(1, 0)));
                    return true;


                // UI Actions
                case Keys.Escape:
                    // Handle escape key to quit the game
                    _uiController.Quit();
                    return true;
                case Keys.F5:
                    // Toggle full screen mode
                    //_actionController.ToggleFullScreen();
                    return true;
                default:
                    break;
            }

            return false;
        }




    }
}
