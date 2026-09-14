using Myrmidon.App.Events;
using Myrmidon.App.Input;
using Myrmidon.App.Render;
using Myrmidon.App.UI;

using Myrmidon.Core;
using Myrmidon.Core.Actions;
using Myrmidon.Core.Zones;
using Myrmidon.Core.Maps.Generation;
using Myrmidon.Core.Systems;

namespace Myrmidon.App;

public sealed class AppHost : IDisposable {
    private readonly IGameLoop _gameLoop;
    private readonly TerminalRenderer _terminal;

    private AppHost(
        IGameLoop gameLoop,
        TerminalRenderer terminal) {
        _gameLoop = gameLoop;
        _terminal = terminal;
    }

    public static AppHost Create() {
        var terminal = new TerminalRenderer(80, 30);

        var gameState = new WorldState();
        var actionController = new ActionController(gameState);
        var zoneGenerator = new ZoneGenerator(gameState.EcsWorld, new DungeonGenerator());
        var worldManager = new WorldManager(
            gameState,
            actionController,
            zoneGenerator);

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