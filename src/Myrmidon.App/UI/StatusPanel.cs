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


internal class StatusPanel : GridPanel {
    
    private GameState _gameState;

    public StatusPanel(Terminal terminal, Rect rect, GameState gameState) : base(terminal, rect) {
        _gameState = gameState;
    }

    public override void Render() {
        base.Render();
        if (!_gameState.Hectare.IsMapGenInProgress)
            RenderStatus(_gameState.Hectare.Player);
    }

    public void RenderStatus(Player player) {
        DrawText(new Vec(1, 1), $"HP: {player.Health}/{player.MaxHealth}", "w");
        DrawText(new Vec(1, 3), $"Gold: {player.Gold}", "w");
    }

}
