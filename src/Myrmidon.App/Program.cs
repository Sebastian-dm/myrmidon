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

            // Initialize world
            var fovSystem = new FovSystem();      // Field of View system
            var world = new Scene(91, 61); // Holds game state and entities
            var gameState = new GameState(world, fovSystem); // Holds game state and context
            var sceneManager = new SceneManager(gameState, fovSystem); // Manages the game world and entities

            // Initialize controllers
            var worldController = new SceneManager(gameState, fovSystem); // Handles game state and logic
            var actionController = new ActionController(gameState, fovSystem); // Handles actions and commands
            MainLoop mainLoop = new MainLoop(gameState, sceneManager, actionController);

            var uiController = new UiController(gameState, mainLoop, actionController);
            uiController.Run();
        }
    }
}