using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Myrmidon.Core.Entities;
using Myrmidon.Core.Actors;
using Myrmidon.Core.Utilities.Geometry;
using Myrmidon.Core.Actions;
using Myrmidon.Core.GameState;

namespace Myrmidon.Core.Actions {
    internal class PickupAction : IAction {

        public bool IsImmediate { get; } = false;
        public readonly Actor Performer;
        public readonly Item Item;

        public PickupAction(Actor performer, Item item) {
            Performer = performer;
            Item = item;
        }

        public ActionResult Perform(IGameContext context) {

            double Distance = (Performer.Position - Item.Position).ToVector2().LengthSquared();
            if (Distance < 2) {
                Performer.Inventory.Add(Item);
                //Program.UIManager.MessageLog.Add($"{Performer.Name} picked up {Item.Name}");
                context.World.CurrentMap.Remove(Item);
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
