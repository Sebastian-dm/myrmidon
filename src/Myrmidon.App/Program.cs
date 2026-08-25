using System;
using Bramble.Core;
using Myrmidon.App.Input;
using Myrmidon.App.Render;

using Myrmidon.Core.Actions;
using Myrmidon.Core.Game;
using Myrmidon.Core.Rules;


using SDL3;


namespace Myrmidon.App;

public static class Program {
    
    private static bool _running = true;
    public static double FPS {  get { return _fpsCounter.Fps; }}

    private static FovSystem _fovSystem;
    private static HectareManager _hectareManager;
    private static ActionController _actionController;
    private static FpsCounter _fpsCounter = new FpsCounter(60);
    private static GameState _gameState;
    private static Terminal _terminal;
    private static InputController _inputController;

    [STAThread]
    public static void Main() {

        // Initialize world
        _fovSystem = new FovSystem();      // Field of View system
        _gameState = new GameState(_fovSystem); // Holds game state and context
        _hectareManager = new HectareManager(_gameState, _fovSystem); // Manages the game world and entities

        // Initialize controllers
        _hectareManager = new HectareManager(_gameState, _fovSystem); // Handles game state and logic
        _actionController = new ActionController(_gameState, _fovSystem); // Handles actions and commands
        _hectareManager.Update(); // Initial update to set up the world
        
        _inputController = new InputController(_actionController); // Handles user input;
        _inputController.Quit += (sender, e) => _running = false;
        
        // Initialize terminal
        _terminal = new Terminal(_fpsCounter, 80, 30);
        var scenePanel = new ScenePanel(_terminal, new Rect(0, 0, 80, 25), _gameState);
        var statusPanel = new StatusPanel(_terminal, new Rect(0, 25, 80, 5), _gameState);
        _terminal.RegisterPanel(scenePanel);
        _terminal.RegisterPanel(statusPanel);

        MainLoop();
    }
    
    private static void MainLoop() {
        while (_running) {
            Tick();
            _fpsCounter.Update();
            var remainder = (uint)(_fpsCounter.GetTickRemainderMs());
            SDL.Delay(16);
        }
    }

    private static void Tick() {
        PollInput();
        UpdateGameState();
        Render();
    }

    private static void PollInput() {
        
        _inputController.PollInput();
        
        // Collect AI actions if it's not the player's turn
        if (!_actionController.CanAcceptInput)
            _actionController.CollectEntityActions();
    }

    private static void UpdateGameState() {
        _actionController.ResolveAllActions();
        
        // Recalculate FOV if needed (not time-bound)
        _gameState.FovSystem.Recompute(_gameState, _gameState.Hectare.Player.Position);

        _hectareManager.Update();
    }

    private static void Render() {
        _terminal.Render();
    }
    
    
}