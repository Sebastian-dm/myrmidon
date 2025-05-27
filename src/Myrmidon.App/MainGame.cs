
using Myrmidon.Simulation;
using Myrmidon.Core.Actors;
using Myrmidon.Core.Entities;
using Myrmidon.Core.Maps;

using Myrmidon.Core.GameState;
using Myrmidon.Core.UI;
using Myrmidon.Core.Simulation;
using Myrmidon.Core.Rules;
using Myrmidon.App.UI;

namespace Myrmidon.App {
    public class MainGame {

        // World & Simulation
        public string Name { get; set; } = "Myrmidon";
        private IGameContext _context;
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
            var world = new World(90, 60); // Holds game state and entities
            _context = new GameContext(world); // Holds game state and context

            // Initialize controllers
            _worldController = new WorldController(_context, fovSystem, uiService); // Handles game state and logic
            _actionController = new ActionController(_context, fovSystem, uiService); // Handles actions and commands

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
