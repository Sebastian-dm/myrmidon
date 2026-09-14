using Bramble.Core;
using Myrmidon.Core.Entities;
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
using Myrmidon.Core;
using Myrmidon.Core.Components;
using Myrmidon.Core.Ecs;
using Myrmidon.Core.Zones;

namespace Myrmidon.App.UI;


public class ScenePanel : GridPanel {
    
    private IWorldState _worldState;

    public ScenePanel(TerminalRenderer terminal, Rect rect, IWorldState worldState) : base(terminal, rect) {
        _worldState = worldState;
    }

    public override void Draw() {
        base.Draw();
        if (_worldState.Zone.GenerationState == ZoneGenState.Ready)
            DrawZone(_worldState.Zone, _worldState.Player, _worldState.PlayerEntity);
    }

    private void DrawZone(Zone zone, Player player, EntityId playerEntity) {

        var map = zone.Map;
        
        // Todo: Adapt draw center to entity based player
        
        Vec drawCenter = new Vec(player.Position.X, player.Position.Y);
        Rect viewBounds = new Rect(
            drawCenter.X - PanelRect.Size.X/2,
            drawCenter.Y - PanelRect.Size.Y/2,
            PanelRect.Size.X,
            PanelRect.Size.Y
        );

        // Paint tiles
        for (int y = viewBounds.Top; y < viewBounds.Bottom; y++) {
            for (int x = viewBounds.Left; x < viewBounds.Right; x++) {
                if (!IsInMapBounds(x, y, map)) continue;
                if (!IsInViewBounds(x, y, viewBounds)) continue;
                
                Vec mapPos = new Vec(x, y);
                Vec panelPos = new Vec(x - viewBounds.Left, y - viewBounds.Top);

                Renderable render = map.GetRenderComponent(mapPos);
                Perceptible percept = map.GetPerceptibleComponent(mapPos);

                if (render == null || !percept.Explored) continue;

                if (percept.LightLevel > 0.2f) {
                    // Draw lighted tiles with their respective colors and textures
                    var alpha = percept.LightLevel;
                    DrawTile(panelPos, render.TextureSheetName, render.TextureIndex, render.ColorBase, render.ColorAccent, render.ColorBackground, alpha);
                }
                else {
                    // Draw darkened tiles with their respective colors and textures
                    DrawTile(panelPos, render.TextureSheetName, render.TextureIndex, "K", alpha:0.2f);
                }
            }
        }

        //Paint entities old way
        foreach (var entity in map.Entities.Items) {
            if (entity is Actor actor) {
                if (!IsInMapBounds(actor.Position.X, actor.Position.Y, map)) continue;
                if (!IsInViewBounds(actor.Position.X, actor.Position.Y, viewBounds)) continue;
                Vec gridPos = new Vec(actor.Position.X - viewBounds.Left, actor.Position.Y - viewBounds.Top);
                string color = "W";
                if (actor is Monster monster) {
                    color = "g";
                }
                //DrawTile(gridPos, "text/default", actor.Glyph, color, "M");
            }
        }

        foreach(var entityId in zone.SpatialIndex.InBounds(viewBounds)) {
            if (!_worldState.EcsWorld.TryGet<Position>(entityId, out var position) && !(position.ZoneId == zone.Id))
                continue;
            if (!_worldState.EcsWorld.TryGet<Renderable>(entityId, out var renderable))
                continue;
            Vec gridPos = new Vec(position.Location.X - viewBounds.Left, position.Location.Y - viewBounds.Top);
            DrawTile(gridPos, renderable.TextureSheetName, renderable.TextureIndex, renderable.ColorBase, renderable.ColorAccent, renderable.ColorBackground);
        }

        // Paint player
        if (playerEntity != null) {
            if ((_worldState.EcsWorld.TryGet<Position>(playerEntity, out var pos) && pos.ZoneId == zone.Id) &&
            (_worldState.EcsWorld.TryGet<Renderable>(playerEntity, out var ren))) {
                var gridPosNew = new Vec(pos.Location.X - viewBounds.Left, pos.Location.Y - viewBounds.Top);
                var gridPos = new Vec(player.Position.X - viewBounds.Left, player.Position.Y - viewBounds.Top);
                //DrawTile(gridPos, ren.TextureSheetName, ren.TextureIndex, ren.ColorBase, ren.ColorAccent, ren.ColorBackground);
                DrawTile(gridPos, "text/default", player.Glyph, "o", "M");
            }
        }
    }

    private bool IsInMapBounds(int x, int y, TileMap map) {

        return x >= map.Bounds.Left && x < map.Bounds.Right && y >= map.Bounds.Top && y < map.Bounds.Bottom;
    }

    private bool IsInViewBounds(int x, int y, Rect viewBounds) {

        return x >= viewBounds.Left && x < viewBounds.Right && y >= viewBounds.Top && y < viewBounds.Bottom;
    }

}
