using Myrmidon.Core.Actions;
using Myrmidon.Core.Entities;
using Myrmidon.Core.Systems;
using Myrmidon.Core.Signals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Myrmidon.Core.Ecs;
using Myrmidon.Core.Zones;

namespace Myrmidon.Core;


public interface IWorldState {

    EcsWorld EcsWorld { get; }
    List<Zone> Zones { get; }
    Zone Zone { get; }
    Player Player { get; set; }
    SignalQueue SignalQueue { get; }
}


public class WorldState : IWorldState {
        
    public EcsWorld EcsWorld { get; private set; }
    public List<Zone> Zones { get; private set; } = new List<Zone>();
    public Zone Zone { get { return Zones[_currentZoneId]; } }
    public Player Player { get; set; }
    public SignalQueue SignalQueue { get; private set; }

    private int _currentZoneId { get; set; } = 0;

    public WorldState() {
        EcsWorld = new EcsWorld();
        Zones.Add(new Zone(0, 91, 61));
        SignalQueue = new SignalQueue();
    }
}
