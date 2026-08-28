using System;
using Bramble.Core;
using Myrmidon.App.Events;
using Myrmidon.App.Input;
using Myrmidon.App.Render;
using Myrmidon.App.UI;
using Myrmidon.Core.Actions;
using Myrmidon.Core.Game;
using Myrmidon.Core.Rules;


using SDL3;


namespace Myrmidon.App;

public static class Program {
    
    private static bool _running = true;
    public static double FPS {  get { return _fpsCounter.Fps; }}
    
    public static WorldManager WorldManager { get; private set; }
    public static UiManager? UiManager { get; private set; }
    
    private static ActionController _actionController;
    private static FpsCounter _fpsCounter = new FpsCounter(60);
    private static Terminal _terminal;
    private static InputController _inputController;
    private static SignalDispatcher _signalDispatcher;

    static Program() {
        // Initialize world
        var fovSystem = new FovSystem();      // Field of View system
        var gameState = new GameState(fovSystem); // Holds game state and context
        WorldManager = new WorldManager(gameState, fovSystem); // Manages the game world and entities
        WorldManager.Update(); // Initial update to set up the world
        
        // Initialize controllers
        _actionController = new ActionController(gameState, fovSystem); // Handles actions and commands
        _inputController = new InputController(_actionController); // Handles user input;
        _inputController.Quit += (sender, e) => _running = false;
        
        // Initialize terminal
        UiManager = new UiManager();
        _terminal = new Terminal(_fpsCounter, 80, 30);
        
        var scenePanel = new ScenePanel(_terminal, new Rect(0, 0, 80, 25), gameState);
        var statusPanel = new StatusPanel(_terminal, new Rect(0, 25, 80, 5), gameState);
        var logPanel = new LogPanel(_terminal, new Rect(0, 0, 80, 25));
        var profilingPanel = new ProfilingPanel(_terminal, new Vec(10, 10)) {FpsCounter =  _fpsCounter};
        _terminal.RegisterPanel(scenePanel);
        _terminal.RegisterPanel(statusPanel);
        _terminal.RegisterPanel(logPanel);
        _terminal.RegisterPanel(profilingPanel);
        
        // Signalling
        var signalHandlers = new SignalHandlers(logPanel);
        _signalDispatcher = new SignalDispatcher(gameState.SignalQueue, signalHandlers.HandleLogMessage);
    }
    
    [STAThread]
    private static void Main() {
        while (_running) {
            Tick();
            _fpsCounter.Update();
            var remainder = (uint)(_fpsCounter.GetTickRemainderMs());
            SDL.Delay(remainder);
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
        
        _signalDispatcher.ProcessSignals();
        
        // Recalculate FOV if needed (not time-bound)
        WorldManager.GameState.FovSystem.Recompute(_gameState, _gameState.Hectare.Player.Position);

        WorldManager.Update();
    }

    private static void Render() {
        _terminal.Render();
    }
    
    
}