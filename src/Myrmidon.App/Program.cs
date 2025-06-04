using Myrmidon.App.Input;
using Myrmidon.App.UI;
using Myrmidon.Core.Actions;
using Myrmidon.Core.Game;
using Myrmidon.Core.Rules;
using System;

namespace Myrmidon.App {
    public static class Program {

        [STAThread]
        public static void Main() {

            var fovSystem = new FovSystem();      // Field of View system
            var world = new Scene(91, 61); // Holds game state and entities
            var gameState = new GameState(world); // Holds game state and context

            // Initialize controllers
            var worldController = new SceneManager(gameState, fovSystem); // Handles game state and logic
            var actionController = new ActionController(gameState, fovSystem); // Handles actions and commands
            
            var uiController = new UiController(gameState);
            var inputController = new InputController(uiController, actionController); // Handles user input

            MainLoop mainGame = new MainLoop(gameState, inputController, uiController);

            uiController.Run();
        }
    }
}