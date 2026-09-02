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
        if (_gameState.Zone.GenerationState == Zone.ZoneGenState.Ready)
            DrawZone(_gameState.Zone, _gameState.Player);
    }

    private void DrawZone(Zone zone, Player player) {

        var map = zone.Map;

        Vec mapCenter = new Vec(player.Position.X, player.Position.Y);
        Rect viewBounds = new Rect(mapCenter.X-PanelRect.Size.X/2, mapCenter.Y-PanelRect.Size.Y/2, PanelRect.Size.X, PanelRect.Size.Y);

        // Paint tiles
        for (int y = viewBounds.Top; y < viewBounds.Bottom; y++) {
            for (int x = viewBounds.Left; x < viewBounds.Right; x++) {
                if (!IsInMapBounds(x, y, map)) continue;
                if (!IsInViewBounds(x, y, viewBounds)) continue;

                Tile? tile = map.GetTileAt<Tile>(x, y);
                if (tile == null) continue;
                    
                Vec gridPos = new Vec(x - viewBounds.Left, y - viewBounds.Top);
                DrawTile(gridPos, tile.RenderComponent);
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
                DrawTile(gridPos, "text/default", actor.Glyph, color, "M");
            }
        }

        // Paint player
        if (player != null) {
            var gridPos = new Vec(player.Position.X - viewBounds.Left, player.Position.Y - viewBounds.Top);
            DrawTile(gridPos, "text/default", player.Glyph, "o", "M");
        }
    }

    private bool IsInMapBounds(int x, int y, TileMap map) {

        return x >= map.Bounds.Left && x < map.Bounds.Right && y >= map.Bounds.Top && y < map.Bounds.Bottom;
    }

    private bool IsInViewBounds(int x, int y, Rect viewBounds) {

        return x >= viewBounds.Left && x < viewBounds.Right && y >= viewBounds.Top && y < viewBounds.Bottom;
    }

}
