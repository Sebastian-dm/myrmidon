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



    public void DrawGlyph(Vec gridLocation, byte asciiIndex, string color) {
        Terminal.DrawGlyph(new Vec(PanelRect.X + gridLocation.X, PanelRect.Y + gridLocation.Y), asciiIndex, color);
    }

    public void DrawText(Vec gridLocation, string text, string color) {
        Terminal.DrawText(new Vec(PanelRect.X + gridLocation.X, PanelRect.Y + gridLocation.Y), text, color);
    }

}