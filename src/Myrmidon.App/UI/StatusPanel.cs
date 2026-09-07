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
using Myrmidon.App.Render;

namespace Myrmidon.App.UI;


public class StatusPanel : GridPanel {
    
    private IGameState _gameState;

    public StatusPanel(TerminalRenderer terminal, Rect rect, IGameState gameState) : base(terminal, rect) {
        _gameState = gameState;
    }

    public override void Draw() {
        base.Draw();
        FillBackground("black");
        if (!_gameState.Hectare.IsMapGenInProgress)
            RenderStatus(_gameState.Hectare.Player);
    }

    public void RenderStatus(Player player) {
        DrawText(new Vec(1, 1), $"HP: {player.Health}/{player.MaxHealth}", "w");
        DrawText(new Vec(1, 3), $"Gold: {player.Gold}", "w");
    }

}
