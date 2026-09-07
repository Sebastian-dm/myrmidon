using Myrmidon.App.Events;
using Myrmidon.App.Input;
using Myrmidon.App.Render;
using Myrmidon.App.UI;
using Myrmidon.Core.Actions;
using Myrmidon.Core.Game;
using Myrmidon.Core.Rules;

namespace Myrmidon.App;

public sealed class AppHost : IDisposable {
    private readonly GameLoop _gameLoop;
    private readonly TerminalRenderer _terminal;

    private AppHost(
        GameLoop gameLoop,
        TerminalRenderer terminal) {
        _gameLoop = gameLoop;
        _terminal = terminal;
    }

    public static AppHost Create() {
        var terminal = new TerminalRenderer(80, 30);

        var fovSystem = new FovSystem();
        var gameState = new GameState(fovSystem);
        var actionController = new ActionController(gameState, fovSystem);
        var worldManager = new WorldManager(
            gameState,
            fovSystem,
            actionController);

        worldManager.Update();

        var input = new InputController(actionController);
        var ui = new UiManager(gameState, terminal);

        var signals = new SignalDispatcher(
            gameState.SignalQueue,
            ui.HandleSignal);

        var gameLoop = new GameLoop(
            worldManager,
            input,
            signals,
            ui);

        return new AppHost(gameLoop, terminal);
    }

    public void Run() {
        _gameLoop.Run();
    }

    public void Dispose() {
        _terminal.Dispose();
    }
}