using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Myrmidon.Core.Parts;
using Myrmidon.Core.ECS;
using Myrmidon.Core.Maps.Tiles;
using Myrmidon.Core.Signals;

namespace Myrmidon.Core.Actions {
    internal class OpenDoorAction : IAction {

        public bool IsImmediate { get; } = false;
        public readonly EntityId Performer;
        public readonly TileDoor Door;

        public OpenDoorAction(EntityId performer, TileDoor door) {
            Performer = performer;
            Door = door;
        }

        public ActionResult Perform(IWorldState context) {
            context.EcsWorld.TryGet<Identity>(Performer, out var identity);
            
            try {
                if (Door.IsLocked) {
                    // TODO: Add a way to open a locked door.
                    
                    context.SignalQueue.Enqueue(new LogSignal(($"{identity.Name} could not open locked door {Door.Name}")));
                }
                else if (!Door.IsLocked && !Door.IsOpen) {
                    Door.Open();
                    context.SignalQueue.Enqueue(new LogSignal(($"{identity.Name} opened {Door.Name}")));
                }
                return new ActionResult(succeeded: true);
            }
            catch (Exception) {
                return new ActionResult(succeeded: false,
                alternative: new SkipAction(Performer)
                );
                throw;
            }
        }
    }
}
