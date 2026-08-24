using Myrmidon.Core.Game;
using Bramble.Core;
using SDL3;

namespace Myrmidon.App.Render;

public class GridPanel : IPanel {
    
    public Rect PanelRect;
    public readonly Terminal Terminal;

    public GridPanel(Terminal terminal, Rect uiArea) {
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