using Myrmidon.Core.Game;

using Bramble.Core;
using Myrmidon.App.Render;
using Myrmidon.App.UI;
using SDL3;

namespace Myrmidon.App.UI;

public class ProfilingPanel : IPanel {
    
    private readonly TerminalRenderer _terminal;
    public FpsCounter FpsCounter;
    public Vec Location;

    private readonly int _padding = 4;

    public ProfilingPanel(TerminalRenderer terminal, Vec location, FpsCounter fpsCounter) {
        _terminal = terminal;
        Location = location;
        FpsCounter = fpsCounter;
    }

    public void Draw() {
        DrawFpsText();
    }


    private void DrawFpsText() {
        string fpsText = $"FPS: {FpsCounter.Fps:F1}";
        _terminal.SetRenderDrawColor("black", 0x55);
        var rect = new SDL.FRect {
            X = Location.X,
            Y = Location.Y,
            W = 2*_padding+fpsText.Length*8-1,
            H = 2*_padding+7
        };
        SDL.RenderFillRect(_terminal.Renderer, rect);
        _terminal.SetRenderDrawColor("white");
        SDL.RenderDebugText(_terminal.Renderer, Location.X+_padding, Location.Y+_padding, fpsText);
    }
}