
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
        private IActionController _actionController;


        public MainLoop(IGameState gameState, SceneManager sceneManager,IActionController actionController) {

            _gameState = gameState;
            _sceneManager = sceneManager;
            _actionController = actionController;


            _sceneManager.Update(); // Initial update to set up the world

        }

        public void Tick() {

            // Collect AI actions if it's not the player's turn
            if (!_actionController.IsPlayersTurn) {
                _actionController.CollectEntityActions();
            }

            // Process all actions
            _actionController.ResolveAllActions();

            // 3. Recalculate FOV if needed (not time-bound)
            //if (_gameState.FovSystem.RequiresUpdate)
            //    _gameState.FovSystem.Recalculate(_gameState);
        }
    }
}
