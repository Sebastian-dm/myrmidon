using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Myrmidon.Entities;

namespace Myrmidon.Core.Actions {
    internal class PickupAction : Action {

        public readonly Item Item;

        public PickupAction(Actor performer, Item item) : base(performer) {
            Item = item;
        }

        public override ActionResult Perform() {

            double Distance = (Performer.Position - Item.Position).ToVector2().LengthSquared();
            if (Distance < 2) {
                Performer.Inventory.Add(Item);
                Program.UIManager.MessageLog.Add($"{Performer.Name} picked up {Item.Name}");
                Item.Destroy();
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
