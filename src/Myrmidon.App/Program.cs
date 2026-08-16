using System;
using Myrmidon.App.Input;
using Myrmidon.App.Render;

using Myrmidon.Core.Actions;
using Myrmidon.Core.Game;
using Myrmidon.Core.Rules;
using Myrmidon.Terminal;


using SDL3;


namespace Myrmidon.App;

public static class Program {
    
    private static bool _running = true;
    
    private static FovSystem _fovSystem;
    private static HectareManager _hectareManager;
    private static ActionController _actionController;
    private static FpsCounter _fpsCounter = new FpsCounter(60);
    private static GameState _gameState;
    private static Renderer _renderer;
    private static InputController _inputController;

    [STAThread]
    public static void Main() {

        // Initialize world
        _fovSystem = new FovSystem();      // Field of View system
        _gameState = new GameState(_fovSystem); // Holds game state and context
        _hectareManager = new HectareManager(_gameState, _fovSystem); // Manages the game world and entities

        // Initialize controllers
        var worldController = new HectareManager(_gameState, _fovSystem); // Handles game state and logic
        _actionController = new ActionController(_gameState, _fovSystem); // Handles actions and commands
        _hectareManager.Update(); // Initial update to set up the world
        
        _inputController = new InputController(_actionController); // Handles user input;
        _inputController.Quit += (sender, e) => _running = false;
        
        // Initialize terminal
        var _terminal = new Renderer();

        MainLoop();
    }
    
    private static void MainLoop() {
        while (_running) {
            Tick();
            _fpsCounter.Update();
            SDL.Delay(16);
        }
    }

    private static void Tick() {

        PollInput();
        UpdateGameState();
        
        
    }

    private static void PollInput() {
        _inputController.PollInput();
        
        // Collect AI actions if it's not the player's turn
        if (!_actionController.IsPlayersTurn)
            _actionController.CollectEntityActions();
    }

    private static void UpdateGameState() {
        _actionController.ResolveAllActions();
        
        // Recalculate FOV if needed (not time-bound)
        _gameState.FovSystem.Recompute(_gameState, _gameState.Hectare.Player.Position);
    }

    private static void Render(GameState gameState) {
        if (!gameState.Hectare.IsMapGenInProgress)
            _renderer.Render(gameState);
        
    }
    
    
}