using Myrmidon.Core.Actions;
using Myrmidon.Core.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myrmidon.Core.Game {

    public interface IGameState {
        Hectare Hectare { get; }
        FovSystem FovSystem { get; }
    }

    public class GameState : IGameState {
        
        public FovSystem FovSystem { get; private set; }
        public IActionController ActionController { get; set; }
        
        public Hectare Hectare { get; private set; }

        public GameState(FovSystem fov) {
            Hectare = new Hectare(91, 61); // Holds game state and entities;
            FovSystem = fov;
        }
    }
}
