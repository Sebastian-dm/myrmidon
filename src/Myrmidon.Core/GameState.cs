using Myrmidon.Core.Actions;
using Myrmidon.Core.Entities;
using Myrmidon.Core.Game;
using Myrmidon.Core.Rules;
using Myrmidon.Core.Signals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myrmidon.Core {

    public interface IGameState {
        Zone Zone { get; }
        int CurrentZone { get; set; }
        Player Player { get; set; }

        SignalQueue SignalQueue { get; }
    }

    public class GameState : IGameState {
        
        public Zone Zone { get; private set; }
        public int CurrentZone { get; set; } = 0;
        public Player Player { get; set; }
        public SignalQueue SignalQueue { get; private set; }

        public GameState() {
            Zone = new Zone(91, 61); // Holds game state and entities;
            SignalQueue = new SignalQueue();
        }
    }
}
