using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Myrmidon.App.UI;
using Myrmidon.Core.Actions;

namespace Myrmidon.App.Input {

    public interface IInputController {
        bool HandleInput(KeyEventArgs e);
    }

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

            var command = GetInputActionFromKey(e.KeyCode);

            if (command != InputAction.None) {
                _actionController.AddAction(command);
                return true;
            }

            if (e.KeyCode == Keys.Escape) {
                _uiController.Quit();
                return true;
            }

            return false;
        }



        private InputAction GetInputActionFromKey(Keys keys) {
            return keys switch {
                Keys.Up => InputAction.MovePlayerUp,
                Keys.Down => InputAction.MovePlayerDown,
                Keys.Left => InputAction.MovePlayerLeft,
                Keys.Right => InputAction.MovePlayerRight,
                Keys.Space => InputAction.SkipPlayerTurn,
                _ => InputAction.None
            };
        }

    }



    
}
