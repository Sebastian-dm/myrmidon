using SDL3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bramble.Core;
using Myrmidon.App.Render;
using Myrmidon.Core;
using Myrmidon.Core.ECS;
using Myrmidon.Core.Zones;
using Myrmidon.Core.Parts;

namespace Myrmidon.App.UI;


public class StatusPanel : GridPanel {
    
    private IWorldState _gameState;

    public StatusPanel(TerminalRenderer terminal, Rect rect, IWorldState gameState) : base(terminal, rect) {
        _gameState = gameState;
    }

    public override void Draw() {
        base.Draw();
        FillBackground("black");
        if (_gameState.Zone.GenerationState == ZoneGenState.Ready)
            RenderStatus(_gameState.PlayerEntity);
    }

    public void RenderStatus(EntityId player) {
        var health = _gameState.EcsWorld.Get<Health>(player);
        DrawText(new Vec(1, 1), $"HP: {health.Current}/{health.Maximum}", "w");
        
        var inv = _gameState.EcsWorld.Get<Inventory>(player);
        DrawText(new Vec(1, 3), $"Gold: {inv.Coins}", "w");
    }

}
