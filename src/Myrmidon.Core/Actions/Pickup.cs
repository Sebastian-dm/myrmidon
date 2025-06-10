using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bramble.Core;
using Myrmidon.Core.Entities;
using Myrmidon.Core.Actions;
using Myrmidon.Core.Game;

namespace Myrmidon.Core.Actions {
    internal class PickupAction : IAction {

        public bool IsImmediate { get; } = false;
        public readonly Actor Performer;
        public readonly Item Item;

        public PickupAction(Actor performer, Item item) {
            Performer = performer;
            Item = item;
        }

        public ActionResult Perform(IGameState context) {

            if (Performer.Position.IsAdjacentTo(Item.Position)) {
                Performer.Inventory.Add(Item);
                //Program.UIManager.MessageLog.Add($"{Performer.Name} picked up {Item.Name}");
                context.World.Map.Remove(Item);
                return new ActionResult(succeeded: true);
            }
            else {
                return new ActionResult(succeeded: false,
                alternative: new SkipAction(Performer)
                );
            }
        }
    }
}
