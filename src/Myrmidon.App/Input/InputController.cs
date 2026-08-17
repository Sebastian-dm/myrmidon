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

    public void HandleInput() {
        // This method can be used to handle input in a loop or during the main game loop
        // For example, you might check for key presses or mouse clicks here

        var action = InputAction.None;
        
        //NativeKeyboard.

        // if (NativeKeyboard.IsKeyDown())          action = InputAction.MovePlayerUp;
        // else if (NativeKeyboard.IsKeyDown(Keys.Down))   action = InputAction.MovePlayerDown;
        // else if (NativeKeyboard.IsKeyDown(Keys.Left))   action = InputAction.MovePlayerLeft;
        //  else if (NativeKeyboard.IsKeyDown(Keys.Right))  action = InputAction.MovePlayerRight;
        //  else if (NativeKeyboard.IsKeyDown(Keys.Space))  action = InputAction.SkipPlayerTurn;
        //
        //  if (action != InputAction.None) {
        //      _actionController.AddFromPlayerInput(action);
        //      return;
        //  }
        //
        //  if (NativeKeyboard.IsKeyDown(Keys.Escape)) {
        //      _uiController.Quit();
        //      return;
        //  }

    }
    
    public void PollInput() {
        // Handle input events
        while (SDL.PollEvent(out SDL.Event e)) {
            switch (e.Type) {
                case (uint)SDL.EventType.Quit:
                    Quit?.Invoke(this, EventArgs.Empty);
                    break;
                case (uint)SDL.EventType.KeyDown:
                    PollKeyboard();
                    SDL.Log($"A key was pressed: {e.Key.Key}");
                    break;
            }
        }
    }

    private void PollKeyboard() {
        var keys = SDL.GetKeyboardState(out var numKeys);

        if (keys[(int)SDL.Scancode.Escape])
            Quit?.Invoke(this, EventArgs.Empty);
        if (keys[(int)SDL.Scancode.L] && keys[(int)SDL.Scancode.K]) {
            SDL.Log("L+K was pressed");
        }
        else {
            SDL.Log($"An unknown key was pressed");
        }
    }


}
