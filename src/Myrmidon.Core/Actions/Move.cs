using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bramble.Core;
using Myrmidon.Core.Actions;
using Myrmidon.Core.Parts;
using Myrmidon.Core.ECS;
using Myrmidon.Core.Signals;

namespace Myrmidon.Core.Actions;

internal class MoveAction : IAction {

    public bool IsImmediate { get; } = false;
    public readonly EntityId Performer;
    public readonly Vec Direction;
    
    private Vec _originalPosition;

    public MoveAction(EntityId performer, Vec direction) {
        Performer = performer;
        Direction = direction;
    }

    public ActionResult Perform(IWorldState context) {
        
        // Do nothing if performer has no position
        if (!context.EcsWorld.Has<Position>(Performer))
            return DoNothingResult();

        var pos = context.EcsWorld.Get<Position>(Performer);
        
        // store the actor's last move state
        _originalPosition = new Vec(pos.Coords.X, pos.Coords.Y);
        Vec newPosition = _originalPosition + Direction;
        
        // Check if it is possible to go there
        if (!context.Zone.TileMap.IsTileWalkable(newPosition))
            return DoNothingResult();
        
        
        context.Zone.EntityIndex.Remove(Performer, pos.Coords);
        context.Zone.EntityIndex.Add(Performer, newPosition);
        pos.Coords = newPosition;
        
        return new ActionResult(succeeded: true);
    }
    
    private ActionResult DoNothingResult() {
        return new ActionResult(
            succeeded: false,
            alternative: new SkipAction(Performer)
        );
    }
}
