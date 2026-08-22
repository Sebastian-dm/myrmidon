using Myrmidon.Core.Game;
using Bramble.Core;
using SDL3;

namespace Myrmidon.App.Render;

public class Panel : IPanel {
    
    public Rect UiArea;
    public readonly Terminal Terminal;

    public Panel(Terminal terminal, Rect uiArea) {
        UiArea = uiArea;
        Terminal = terminal;
    }
    
    public virtual void Render() {
        Terminal.SetPanelArea(UiArea);
    }


    public void RenderFillRect(Rect fillRect) {
        Terminal.RenderFillRect(UiArea, fillRect);
    }
    
    
    
}