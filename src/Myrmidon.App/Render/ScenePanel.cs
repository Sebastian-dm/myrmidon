using Bramble.Core;
using Myrmidon.Core.Game;
using Myrmidon.Core.Maps;
using Myrmidon.Core.Maps.Tiles;
using Myrmidon.Core.Entities;

using SDL3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myrmidon.App.Render;


internal class ScenePanel : Panel {
    
    private GameState _gameState;

    public ScenePanel(Terminal terminal, Rect rect, GameState gameState) : base(terminal, rect) {
        _gameState = gameState;
    }

    public override void Render() {
        base.Render();
        if (!_gameState.Hectare.IsMapGenInProgress) {

            RenderHectare(_gameState.Hectare);
        }
        else {
            Terminal.SetRenderDrawColor("r");
            RenderFillRect(PanelRect);
        }
    }

    public void RenderHectare(Hectare hectare) {

        var map = hectare.Map;
        if (map == null) return;

        Vec center = new Vec(hectare.Player.Position.X, hectare.Player.Position.Y);
        Rect viewBounds = new Rect(center.X-PanelRect.X, center.Y-PanelRect.Y, PanelRect.Size.X, PanelRect.Size.Y);

        // Paint tiles
        for (int y = viewBounds.Top; y < viewBounds.Bottom; y++) {
            for (int x = viewBounds.Left; x < viewBounds.Right; x++) {
                if (!IsInMapBounds(x, y, map)) continue;

                var tile = map.GetTileAt<Tile>(x, y);

                var screenPos = new Vec(x - viewBounds.Left, y - viewBounds.Top);
                Terminal.SetRenderDrawColor("r");
                Terminal.DrawGlyph(screenPos, tile.Glyph);
                //terminal[screenPos.X, screenPos.Y][TerminalColor.Gray, backgroundColor].Write(tile.Glyph);

            }
        }

        //Paint entities
        foreach (var entity in map.Entities.Items) {

            if (entity is Actor actor) {
                if (!IsInMapBounds(actor.Position.X, actor.Position.Y, map)) continue;
                if (!IsInViewBounds(actor.Position.X, actor.Position.Y, viewBounds)) continue;
                Vec gridPos = new Vec(actor.Position.X - viewBounds.Left, actor.Position.Y - viewBounds.Top);
                Terminal.SetRenderDrawColor("r");
                Terminal.DrawGlyph(gridPos, actor.Glyph);
                //terminal[screenX, screenY][TerminalColor.ToSystemColor("LightRed"), backgroundColor].Write(actor.Glyph);
            }
        }

        // Paint player
        if (hectare.Player != null) {
            var playerPos = new Vec(hectare.Player.Position.X - viewBounds.Left, hectare.Player.Position.Y - viewBounds.Top);
            Terminal.SetRenderDrawColor("r");
            Terminal.DrawGlyph(playerPos, hectare.Player.Glyph);
            //terminal[playerPos.X, playerPos.Y][TerminalColor.LightGreen, backgroundColor].Write(hectare.Player.Glyph);
        }
    }

    private bool IsInMapBounds(int x, int y, TileMap map) {

        return x >= map.Bounds.Left && x < map.Bounds.Right && y >= map.Bounds.Top && y < map.Bounds.Bottom;
    }

    private bool IsInViewBounds(int x, int y, Rect viewBounds) {

        return x >= viewBounds.Left && x < viewBounds.Right && y >= viewBounds.Top && y < viewBounds.Bottom;
    }

}
