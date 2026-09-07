using Myrmidon.App.Events;
using Myrmidon.App.Input;
using Myrmidon.App.Render;
using Myrmidon.App.UI;
using Myrmidon.Core.Game;
using SDL3;

namespace Myrmidon.App;

internal sealed class GameLoop {
    private readonly WorldManager _worldManager;
    private readonly InputController _inputController;
    private readonly SignalDispatcher _signalDispatcher;
    private readonly UiManager _uiManager;
    private readonly FpsCounter _fpsCounter;

    private bool _running = true;

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

        _inputController.Quit += OnQuit;
    }

    public void Run() {
        while (_running) {
            Tick();

            _fpsCounter.Update();

            var remainder = (uint)_fpsCounter.GetTickRemainderMs();
            SDL.Delay(remainder);
        }

        _inputController.Quit -= OnQuit;
    }

    private void Tick() {
        PollInput();

        if (!_running)
            return;

        UpdateGameState();
        Render();
    }

    private void PollInput() {
        _inputController.PollInput();

        if (!_worldManager.ActionController.CanAcceptInput) {
            _worldManager.ActionController.CollectEntityActions();
        }
    }

    private void UpdateGameState() {
        _worldManager.ActionController.ResolveAllActions();

        _signalDispatcher.ProcessSignals();

        var gameState = _worldManager.GameState;

        gameState.FovSystem.Recompute(
            gameState,
            gameState.Hectare.Player.Position);

        _worldManager.Update();
    }

    private void Render() {
        _uiManager.Render();
    }

    private void OnQuit(object? sender, EventArgs e) {
        _running = false;
    }
}