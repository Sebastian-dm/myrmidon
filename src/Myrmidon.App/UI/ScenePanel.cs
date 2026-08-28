using Bramble.Core;
using Myrmidon.Core.Entities;
using Myrmidon.Core.Game;
using Myrmidon.Core.Maps;
using Myrmidon.Core.Maps.Tiles;
using SDL3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Myrmidon.App.Render;
using static System.Net.WebRequestMethods;

namespace Myrmidon.App.UI;


internal class ScenePanel : GridPanel {
    
    private GameState _gameState;

    public ScenePanel(Terminal terminal, Rect rect, GameState gameState) : base(terminal, rect) {
        _gameState = gameState;
    }

    public override void Draw() {
        base.Draw();
        if (!_gameState.Hectare.IsMapGenInProgress)
            DrawHectare(_gameState.Hectare);
    }

    private void DrawHectare(Hectare hectare) {

        var map = hectare.Map;

        Vec mapCenter = new Vec(hectare.Player.Position.X, hectare.Player.Position.Y);
        Rect viewBounds = new Rect(mapCenter.X-PanelRect.Size.X/2, mapCenter.Y-PanelRect.Size.Y/2, PanelRect.Size.X, PanelRect.Size.Y);

        // Paint tiles
        for (int y = viewBounds.Top; y < viewBounds.Bottom; y++) {
            for (int x = viewBounds.Left; x < viewBounds.Right; x++) {
                if (!IsInMapBounds(x, y, map)) continue;

                Tile? tile = map.GetTileAt<Tile>(x, y);
                if (tile == null) continue;
                    
                Vec gridPos = new Vec(x - viewBounds.Left, y - viewBounds.Top);
                DrawTile(gridPos, "text/default", tile.Glyph, "K", "w");

            }
        }

        //Paint entities
        foreach (var entity in map.Entities.Items) {

            if (entity is Actor actor) {
                if (!IsInMapBounds(actor.Position.X, actor.Position.Y, map)) continue;
                if (!IsInViewBounds(actor.Position.X, actor.Position.Y, viewBounds)) continue;
                Vec gridPos = new Vec(actor.Position.X - viewBounds.Left, actor.Position.Y - viewBounds.Top);
                string color = "W";
                if (actor is Monster monster) {
                    color = "g";
                }
                DrawTile(gridPos, "text/default", actor.Glyph, color, "black");
            }
        }

        // Paint player
        if (hectare.Player != null) {
            var gridPos = new Vec(hectare.Player.Position.X - viewBounds.Left, hectare.Player.Position.Y - viewBounds.Top);
            DrawTile(gridPos, "text/default", hectare.Player.Glyph, "o", "blue");
        }
    }

    private bool IsInMapBounds(int x, int y, TileMap map) {

        return x >= map.Bounds.Left && x < map.Bounds.Right && y >= map.Bounds.Top && y < map.Bounds.Bottom;
    }

    private bool IsInViewBounds(int x, int y, Rect viewBounds) {

        return x >= viewBounds.Left && x < viewBounds.Right && y >= viewBounds.Top && y < viewBounds.Bottom;
    }

}
