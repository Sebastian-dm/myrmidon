using Myrmidon.Core.Maps.Tiles;
using SDL3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

using Bramble.Core;
using Myrmidon.Core;
using Myrmidon.Core.Maps;
using Myrmidon.Core.Parts;
using Myrmidon.Core.Ecs;
using Myrmidon.Core.Zones;
using Myrmidon.App.Render;

namespace Myrmidon.App.UI;


public class ScenePanel : GridPanel {
    
    private IWorldState _worldState;

    public ScenePanel(TerminalRenderer terminal, Rect rect, IWorldState worldState) : base(terminal, rect) {
        _worldState = worldState;
    }

    public override void Draw() {
        base.Draw();
        if (_worldState.Zone.GenerationState == ZoneGenState.Ready)
            DrawZone(_worldState.Zone, _worldState.PlayerEntity);
    }

    private void DrawZone(Zone zone, EntityId player) {

        var map = zone.Map;
        
        // Center on player
        if (!_worldState.EcsWorld.TryGet<Position>(player, out var pPos))
            return;
            
        Vec drawCenter = pPos.Coords;
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
                    DrawTile(panelPos, render.TextureSheetName, render.TextureIndex, "K", alpha:0.5f);
                }
            }
        }
        
        // Paint entities
        foreach(var entityId in zone.SpatialIndex.InBounds(viewBounds)) {
            if (!_worldState.EcsWorld.TryGet<Position>(entityId, out var mpos) ||
                !_worldState.EcsWorld.TryGet<Renderable>(entityId, out var mren) ||
                !_worldState.EcsWorld.TryGet<Perceptible>(entityId, out var mperc))
                continue;
            if (mperc.LightLevel > 0.2f) {
                Vec gridPos = new Vec(mpos.Coords.X - viewBounds.Left, mpos.Coords.Y - viewBounds.Top);
                DrawTile(gridPos, mren.TextureSheetName, mren.TextureIndex, mren.ColorBase, mren.ColorAccent,
                    mren.ColorBackground);
            }
        }

        // Paint player
        if (_worldState.EcsWorld.TryGet<Position>(player, out var ppos) &&
            _worldState.EcsWorld.TryGet<Renderable>(player, out var pren))
        {
            var gridPos = new Vec(ppos.Coords.X - viewBounds.Left, ppos.Coords.Y - viewBounds.Top);
            DrawTile(gridPos, pren.TextureSheetName, pren.TextureIndex, pren.ColorBase, pren.ColorAccent, pren.ColorBackground);
        }
    }

    private bool IsInMapBounds(int x, int y, TileMap map) {

        return x >= map.Bounds.Left && x < map.Bounds.Right && y >= map.Bounds.Top && y < map.Bounds.Bottom;
    }

    private bool IsInViewBounds(int x, int y, Rect viewBounds) {

        return x >= viewBounds.Left && x < viewBounds.Right && y >= viewBounds.Top && y < viewBounds.Bottom;
    }

}
