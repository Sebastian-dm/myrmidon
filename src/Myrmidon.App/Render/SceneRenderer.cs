using Myrmidon.Core.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SDL3;

namespace Myrmidon.App.Render;


internal class SceneRenderer : ISubRenderer {

    private IntPtr _renderer;
    private SDL.Rect _uiArea;
    private GameState _gameState;

    public SceneRenderer(IntPtr renderer, SDL.Rect uiArea, GameState gameState) {
        _renderer = renderer;
        _uiArea = uiArea;
        _gameState = gameState;
    }


    public void Render() {
        SDL.SetRenderViewport(_renderer, _uiArea);
        RenderWorld();
    }

    public void RenderWorld() {
        Renderer.SetRenderDrawColor("green");
        SDL.RectToFRect(_uiArea, out var frect);
        SDL.RenderFillRect(_renderer, frect);
        //TerminalColor backgroundColor = TerminalColor.Black;
        //SDL.Clear
        ////terminal.Clear();

        //var map = context.Hectare.Map;
        //if (map == null) return;

        //Vec center = new Vec(context.Hectare.Player.Position.X, context.Hectare.Player.Position.Y);
        //Rect viewBounds = new Rect(center - terminal.Size / 2, terminal.Size);

        //// Paint tiles
        //for (int y = viewBounds.Top; y < viewBounds.Bottom; y++) {
        //    for (int x = viewBounds.Left; x < viewBounds.Right; x++) {
        //        if (!IsInMapBounds(x, y, map)) continue;

        //        var tile = map.GetTileAt<Tile>(x, y);

        //        var screenPos = new Vec(x - viewBounds.Left, y - viewBounds.Top);
        //        terminal[screenPos.X, screenPos.Y][TerminalColor.Gray, backgroundColor].Write(tile.Glyph);

        //    }
        //}

        ////Paint entities
        //foreach (var entity in map.Entities.Items) {

        //    if (entity is Actor actor) {
        //        if (!IsInMapBounds(actor.Position.X, actor.Position.Y, map)) continue;
        //        if (!IsInViewBounds(actor.Position.X, actor.Position.Y, viewBounds)) continue;
        //        int screenX = actor.Position.X - viewBounds.Left;
        //        int screenY = actor.Position.Y - viewBounds.Top;
        //        terminal[screenX, screenY][TerminalColor.ToSystemColor("LightRed"), backgroundColor].Write(actor.Glyph);
        //    }
        //}

        //// Paint player
        //if (context.Hectare.Player != null) {
        //    var playerPos = new Vec(context.Hectare.Player.Position.X - viewBounds.Left, context.Hectare.Player.Position.Y - viewBounds.Top);
        //    terminal[playerPos.X, playerPos.Y][TerminalColor.LightGreen, backgroundColor].Write(context.Hectare.Player.Glyph);
        //}
    }

    //private bool IsInViewBounds(int x, int y, Rect viewBounds) {
    //    return x >= viewBounds.Left && x < viewBounds.Right && y >= viewBounds.Top && y < viewBounds.Bottom;
    //}
}
