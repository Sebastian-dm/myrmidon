using Myrmidon.Core.Game;
using Bramble.Core;
using SDL3;

namespace Myrmidon.App.Render;

public class Panel : IPanel {
    
    public Rect PanelRect;
    public readonly Terminal Terminal;

    public Panel(Terminal terminal, Rect uiArea) {
        PanelRect = uiArea;
        Terminal = terminal;
    }
    
    public virtual void Render() {
        //Terminal.SetPanelArea(PanelRect);
    }


    public void RenderFillRect(Rect fillRect) {
        Terminal.RenderFillRect(PanelRect, fillRect);
    }
    
    
    
}