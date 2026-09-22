using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Myrmidon.Core.Parts;
using Myrmidon.Core.ECS;
using Myrmidon.Core.Signals;
using Myrmidon.Core.Systems;

namespace Myrmidon.Core.Actions {
    internal class OpenDoorAction : IAction {

        public bool IsImmediate { get; } = false;
        public readonly EntityId Performer;
        public readonly EntityId Door;

        public OpenDoorAction(EntityId performer, EntityId door) {
            Performer = performer;
            Door = door;
        }

        public ActionResult Perform(IWorldState context) {
            context.EcsWorld.TryGet<Identity>(Performer, out var identity);

            var doorIdentity = context.EcsZone.Get<Identity>(Door);
            var door = context.EcsZone.Get<Door>(Door);
            var doorSystem = new DoorSystem(context.EcsZone);

            try {
                if (door.IsLocked) {
                    doorSystem.Unlock(Door);
                    context.SignalQueue.Enqueue(new LogSignal(($"{identity.Name} unlocked {doorIdentity.Name}")));
                }
                else if (!door.IsLocked && door.IsClosed) {
                    doorSystem.Open(Door);
                    context.SignalQueue.Enqueue(new LogSignal(($"{identity.Name} opened {doorIdentity.Name}")));
                }
                return new ActionResult(succeeded: true);
            }
            catch (Exception) {
                return new ActionResult(succeeded: false,
                alternative: new SkipAction(Performer)
                );
            }
        }
    }
}
