using Myrmidon.App.Events;
using Myrmidon.App.Input;
using Myrmidon.App.Render;
using Myrmidon.App.UI;
using Myrmidon.Core;
using SDL3;

namespace Myrmidon.App;

public interface IGameLoop {
    void Run();
}

internal sealed class GameLoop : IGameLoop {
    private readonly WorldManager _worldManager;
    private readonly InputController _inputController;
    private readonly SignalDispatcher _signalDispatcher;
    private readonly UiManager _uiManager;
    private readonly FpsCounter _fpsCounter;

    private bool _running = true;

    public bool UseStepMode { get; set; } = true;

    public GameLoop(
        WorldManager worldManager,
        InputController inputController,
        SignalDispatcher signalDispatcher,
        UiManager uiManager) {
        
        _worldManager = worldManager;
        _inputController = inputController;
        _signalDispatcher = signalDispatcher;
        _uiManager = uiManager;
        _fpsCounter = new FpsCounter(60);

        // Register event handlers
        _inputController.CommandRequested += OnCommandRequested;
    }

    public void Run() {

        UpdateGameState();
        Render();

        while (_running) {
            Tick();

            _fpsCounter.Update();

            var remainder = (uint)_fpsCounter.GetTickRemainderMs();
            if (!UseStepMode)
                SDL.Delay(remainder);
        }
        _inputController.CommandRequested -= OnCommandRequested;
    }


    private void Tick() {

        PollInput();

        if (!_running)
            return;

        if (!UseStepMode || !_worldManager.ActionController.CanAcceptInput) {
            UpdateGameState();
            Render();
        }
    }


    private void PollInput() {
        _inputController.PollInput();

        if (!_worldManager.ActionController.CanAcceptInput) {
            _worldManager.ActionController.CollectEntityActions();
        }
    }


    private void UpdateGameState() {
        _worldManager.ActionController.ResolveAllActions();

        _signalDispatcher.DispatchAllQueuedSignals();

        _worldManager.Update();
    }


    private void Render() {
        _uiManager.Render();
    }

    private void OnCommandRequested(
        object? sender,
        AppCommandEventArgs e) {
        switch (e.Command) {
            
            case AppCommand.ToggleStepMode:
                UseStepMode = !UseStepMode;
                break;
            case AppCommand.Quit:
                _running = false;
                break;
        }
    }


    private void OnQuit(object? sender, EventArgs e) {
        _running = false;
    }
}