using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Runtime.InteropServices;

using Myrmidon.Core.Actions;

using SDL3;


namespace Myrmidon.App.Input;

public class InputController {
    
    public event EventHandler Quit;
    private IActionController _actionController;


    public InputController(IActionController actionController) {
        _actionController = actionController;
    }

    
    public void PollInput() {
        // Handle input events
        while (SDL.PollEvent(out SDL.Event e)) {
            switch (e.Type) {
                case (uint)SDL.EventType.Quit:
                    Quit?.Invoke(this, EventArgs.Empty);
                    break;
                case (uint)SDL.EventType.KeyDown:
                    //SDL.Log($"A key was pressed: {e.Key.Key}");
                    PollKeyboard();
                    break;
            }
        }
    }

    private void PollKeyboard() {
        var action = InputAction.None;
        var keys = SDL.GetKeyboardState(out var numKeys);


        if (keys[(int)SDL.Scancode.Escape])
            Quit?.Invoke(this, EventArgs.Empty);

        // Player directions
        if (keys[(int)SDL.Scancode.Kp8] || keys[(int)SDL.Scancode.Up])
            action = InputAction.MovePlayerN;
        if (keys[(int)SDL.Scancode.Kp9])
            action = InputAction.MovePlayerNE;
        if (keys[(int)SDL.Scancode.Kp6] || keys[(int)SDL.Scancode.Right])
            action = InputAction.MovePlayerE;
        if (keys[(int)SDL.Scancode.Kp3])
            action = InputAction.MovePlayerSE;
        if (keys[(int)SDL.Scancode.Kp2] || keys[(int)SDL.Scancode.Down])
            action = InputAction.MovePlayerS;
        if (keys[(int)SDL.Scancode.Kp1])
            action = InputAction.MovePlayerSW;
        if (keys[(int)SDL.Scancode.Kp4] || keys[(int)SDL.Scancode.Left])
            action = InputAction.MovePlayerW;
        if (keys[(int)SDL.Scancode.Kp7])
            action = InputAction.MovePlayerNW;

        // Other player actions
        if (keys[(int)SDL.Scancode.Kp5] || keys[(int)SDL.Scancode.Space])
            action = InputAction.SkipPlayerTurn;


        if (action != InputAction.None) {
            _actionController.AddFromPlayerInput(action);
            //SDL.Log($"The key press resulted in action: {action}");
        }
        else {
            //SDL.Log($"The key press resulted in no action.");
        }
    }


}
