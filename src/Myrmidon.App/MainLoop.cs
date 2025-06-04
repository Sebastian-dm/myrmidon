
using Myrmidon.App.Input;
using Myrmidon.App.UI;
using Myrmidon.Core.Actions;
using Myrmidon.Core.Entities;
using Myrmidon.Core.Game;
using Myrmidon.Core.Maps;
using Myrmidon.Core.Rules;

namespace Myrmidon.App {
    public class MainLoop {

        // World & Simulation
        public string Name { get; set; } = "Myrmidon";

        private IGameState _gameState;
        private SceneManager _sceneManager;

        private IInputController _input;
        private IUiController _ui;
        private IActionController _actionController;
        

        public bool WaitingForUserInput { get; set; } = true;


        public MainLoop(IGameState gameState, IInputController inputController, IUiController uiController, IActionController actionController) {


            _gameState = gameState;
            _input = inputController;
            _ui = uiController;
            _actionController = actionController;


            _sceneManager.Update(); // Initial update to set up the world

        }

        private void Tick() {

            // 1. Check if there's a player action ready
            if (_gameState.ActionController.CanAcceptInput) {
                var action = _input.GetNextAction(_gameState);
                if (action != null)
                    _gameState.ActionController.Add(action);
            }

            // 2. Advance simulation only if actions exist
            if (_gameState.ActionController.HasPendingActions) {
                _gameState.ActionController.ResolveNextAction(_gameState);
            }

            // 3. Recalculate FOV if needed (not time-bound)
            if (_gameState.FovSystem.RequiresUpdate)
                _gameState.FovSystem.Recalculate(_gameState);

            // 4. Always update animations/effects/UI
            _ui.UpdateAnimations();   // optional
            _ui.Render(_gameState);
        }

        public void HandleKeyPress(KeyEventArgs e) {
            bool userTookAnAction = _input.HandleInput(e);
            if (userTookAnAction) {
                WaitingForUserInput = false; // User has taken an action, proceed with game update
            } else {
                // Handle other key presses or commands
                // This could include navigation, UI interactions, etc.
            }
        }
    }
}
