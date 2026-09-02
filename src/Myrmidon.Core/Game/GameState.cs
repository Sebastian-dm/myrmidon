using Myrmidon.Core.Actions;
using Myrmidon.Core.Entities;
using Myrmidon.Core.Rules;
using Myrmidon.Core.Signals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myrmidon.Core.Game {

    public interface IGameState {
        Zone Zone { get; }

        Player Player { get; set; }

        FovSystem FovSystem { get; }

        SignalQueue SignalQueue { get; }
    }

    public class GameState : IGameState {
        
        public Zone Zone { get; private set; }
        public Player Player { get; set; }
        public FovSystem FovSystem { get; private set; }
        public SignalQueue SignalQueue { get; private set; }

        public GameState(FovSystem fov) {
            Zone = new Zone(91, 61); // Holds game state and entities;
            FovSystem = fov;
            SignalQueue = new SignalQueue();
        }
    }
}
