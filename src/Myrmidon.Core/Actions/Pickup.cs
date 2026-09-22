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

internal class PickupAction : IAction {

    public bool IsImmediate { get; } = false;
    public readonly EntityId Performer;
    public readonly EntityId Item;

    public PickupAction(EntityId performer, EntityId item) {
        Performer = performer;
        Item = item;
    }

    public ActionResult Perform(IWorldState context) {
        
        context.EcsWorld.TryGet<Identity>(Performer, out var performerIdentity);
        context.EcsWorld.TryGet<Identity>(Performer, out var subjectIdentity);
        context.SignalQueue.Enqueue(new LogSignal(($"{performerIdentity.Name} wants to pickup {subjectIdentity.Name} but this function is not implemented.")));
        
        
        
        var posPerformer = context.EcsWorld.Get<Position>(Performer);
        var posItem = context.EcsWorld.Get<Position>(Item);
        
        if ((posItem.Coords-posPerformer.Coords).KingLength <= 1) {
            
            var invPerformer = context.EcsWorld.Get<Inventory>(Performer);
            invPerformer.Coins++;
            
            context.EcsWorld.DestroyEntity(Item);
            context.Zone.EntityIndex.Remove(Item, posPerformer.Coords);
            
            return new ActionResult(succeeded: true);
        }
        else {
            return new ActionResult(succeeded: false,
            alternative: new SkipAction(Performer)
            );
        }
    }
}
