using System.Runtime.InteropServices;
using System.Windows.Forms;

using Myrmidon.App.UI;
using Myrmidon.Core.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Myrmidon.App.Input {

    public interface IInputController {

        void HandleInput();
    }

    public class InputController : IInputController {

        private IUiController _uiController;
        private IActionController _actionController;


        public InputController(IUiController uiController, IActionController actionController) {
            _uiController = uiController;
            _actionController = actionController;
        }

        public void HandleInput() {
            // This method can be used to handle input in a loop or during the main game loop
            // For example, you might check for key presses or mouse clicks here

            var action = InputAction.None;

            if (NativeKeyboard.IsKeyDown(Keys.Up))          action = InputAction.MovePlayerUp;
            else if (NativeKeyboard.IsKeyDown(Keys.Down))   action = InputAction.MovePlayerDown;
            else if (NativeKeyboard.IsKeyDown(Keys.Left))   action = InputAction.MovePlayerLeft;
            else if (NativeKeyboard.IsKeyDown(Keys.Right))  action = InputAction.MovePlayerRight;
            else if (NativeKeyboard.IsKeyDown(Keys.Space))  action = InputAction.SkipPlayerTurn;

            if (action != InputAction.None) {
                _actionController.AddFromPlayerInput(action);
                return;
            }

            if (NativeKeyboard.IsKeyDown(Keys.Escape)) {
                _uiController.Quit();
                return;
            }

        }

        public static class NativeKeyboard {
            [DllImport("user32.dll")]
            private static extern short GetAsyncKeyState(Keys vKey);

            public static bool IsKeyDown(Keys key) {
                return (GetAsyncKeyState(key) & 0x8000) != 0;
            }
        }


    }
    
}
