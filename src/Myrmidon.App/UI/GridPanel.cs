using Myrmidon.Core.Game;
using Bramble.Core;
using Myrmidon.App.Render;
using Myrmidon.Core.Components;
using SDL3;

namespace Myrmidon.App.UI;

public class GridPanel : IPanel {
    
    public Rect PanelRect;
    public readonly Terminal Terminal;

    public GridPanel(Terminal terminal, Rect uiArea) {
        PanelRect = uiArea;
        Terminal = terminal;
    }
    
    public virtual void Draw() {
        //Terminal.SetPanelArea(PanelRect);
    }



    public void DrawGlyph(Vec gridLocation, byte asciiIndex, string color) {
        Terminal.DrawGlyph(new Vec(PanelRect.X + gridLocation.X, PanelRect.Y + gridLocation.Y), asciiIndex, color);
    }

    public void DrawText(Vec gridLocation, string text, string color) {
        Terminal.DrawText(new Vec(PanelRect.X + gridLocation.X, PanelRect.Y + gridLocation.Y), text, color);
    }

    public void DrawTile(Vec gridLocation, RenderComponent c) {
        DrawTile(gridLocation, c.TextureSheetName, c.TextureIndex,
            c.ColorBase, c.ColorAccent, c.ColorBackground);
    }

    public void DrawTile(Vec gridLocation, string textureSheetName, byte textureIndex,
    string foregroundColor, string accentColor, string backgroundColor = "") {
        Terminal.DrawTile(new Vec(PanelRect.X + gridLocation.X, PanelRect.Y + gridLocation.Y), textureSheetName, textureIndex,
        foregroundColor, accentColor, backgroundColor);
    }

}