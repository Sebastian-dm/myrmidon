using Myrmidon.Core.Actions;
using Myrmidon.Core.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Myrmidon.Core.Signals;

namespace Myrmidon.Core.Game {

    public interface IGameState {
        Hectare Hectare { get; }
        FovSystem FovSystem { get; }
        SignalQueue SignalQueue { get; }
    }

    public class GameState : IGameState {
        
        public Hectare Hectare { get; private set; }
        public FovSystem FovSystem { get; private set; }
        public SignalQueue SignalQueue { get; private set; }

        public GameState(FovSystem fov) {
            Hectare = new Hectare(91, 61); // Holds game state and entities;
            FovSystem = fov;
            SignalQueue = new SignalQueue();
        }
    }
}
