
using Myrmidon.Simulation;
using Myrmidon.Core.Actors;
using Myrmidon.Core.Entities;
using Myrmidon.Core.Maps;

using Myrmidon.Core.GameState;
using Myrmidon.Core.UI;
using Myrmidon.Core.Rules;
using Myrmidon.App.UI;
using Myrmidon.Core.Actions;

namespace Myrmidon.App {
    public class MainGame {

        // World & Simulation
        public string Name { get; set; } = "Myrmidon";
        public IGameContext Context { get; private set; }
        private WorldController _worldController;
        private ActionController _actionController;
        private InputController _inputController;

        public bool WaitingForUserInput { get; set; } = true;


        public MainGame(string name) {
            Name = name;

            //Initialize core systems
            var fovSystem = new FovSystem();      // Field of View system
            var uiService = new UIService();      // User Interface service

            // Initialize game state
            var world = new World(91, 61); // Holds game state and entities
            Context = new GameContext(world); // Holds game state and context

            // Initialize controllers
            _worldController = new WorldController(Context, fovSystem, uiService); // Handles game state and logic
            _actionController = new ActionController(Context, fovSystem, uiService); // Handles actions and commands

            _worldController.Update(); // Initial update to set up the world

            //_turnManager = new TurnManager();   // Handles turn-based action resolution
            //_inputHandler = new MonoGameInputHandler(); // From Game.Input layer
        }

        private void Update() {

            while (!WaitingForUserInput) {
                // Update game state
                _worldController.Update();
                _actionController.Update();
            }

            WaitingForUserInput = true;
        }

        public void HandleKeyPress(KeyEventArgs e) {
            bool userTookAnAction = _inputController.HandleInput(e);
            if (userTookAnAction) {
                WaitingForUserInput = false; // User has taken an action, proceed with game update
            } else {
                // Handle other key presses or commands
                // This could include navigation, UI interactions, etc.
            }
        }
    }
}
