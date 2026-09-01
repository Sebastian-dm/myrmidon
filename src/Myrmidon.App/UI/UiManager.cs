using Bramble.Core;
using Myrmidon.App.Render;
using Myrmidon.Core.Game;
using Myrmidon.Core.Signals;

namespace Myrmidon.App.UI;

public class UiManager {
    public LogPanel Log { get; }
    public ScenePanel Scene { get; }
    public StatusPanel Status { get; }

    private readonly TerminalRenderer _terminal;

    public UiManager(IGameState gameState, TerminalRenderer terminal) {
        _terminal = terminal;

        int sceneHeight = terminal.WindowHeightTiles - 5; // Reserve 5 lines for the log
        int sceneWidth = terminal.WindowWidthTiles - 10;

        var leftTopRect = new Rect(0, 0, sceneWidth, sceneHeight);
        var rightColumnRect = new Rect(
            sceneWidth,
            0,
            terminal.WindowWidthTiles - sceneWidth,
            sceneHeight
        );
        var bottomRect = new Rect(
            0,
            sceneHeight,
            terminal.WindowWidthTiles,
            terminal.WindowHeightTiles - sceneHeight
        );

        Scene = new ScenePanel(terminal, leftTopRect, gameState);
        Log = new LogPanel(terminal, bottomRect);
        Status = new StatusPanel(terminal, rightColumnRect, gameState);
    }

    public void Render() {
        _terminal.BeginFrame();

        Scene.Draw();
        Status.Draw();
        Log.Draw();

        _terminal.Present();
    }

    public void HandleSignal(ISignal signal) {
        if (signal is LogSignal log)
            Log.AddEntry(log.Text);
    }
}