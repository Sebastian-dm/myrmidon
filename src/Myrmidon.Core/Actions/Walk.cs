using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Myrmidon.Core.Entities;
using Myrmidon.Core.Utilities.Geometry;
using Myrmidon.Core.Maps.Tiles;
using Myrmidon.Core.Game;

namespace Myrmidon.Core.Actions {
    public class WalkAction : IAction {

        public bool IsImmediate { get; } = false;
        public readonly Actor Performer;
        public readonly Vector Direction;

        private Vector _originalPosition;

        public WalkAction(Actor performer, Vector direction) {
            Performer = performer;
            Direction = direction;
            _originalPosition = performer.Position.ToVector2();
        }

        public ActionResult Perform(IGameState context) {

            // Do nothing if no length given
            if (Direction.X == 0 && Direction.Y == 0) {
                return new ActionResult(
                succeeded: false,
                alternative: new SkipAction(Performer)
                );
            }

            // store the actor's last move state
            _originalPosition = new Vector(Performer.Position.X, Performer.Position.Y);
            Vector newPosition = _originalPosition + Direction;

            // Check if there is an actor on new position
            Monster monster = context.World.Map.GetEntityAt<Monster>(newPosition);
            if (monster != null) {
                return new ActionResult( succeeded: false,
                alternative: new AttackAction(Performer, monster)
                );
            }

            // Check if there is an item on the new position
            Item item = context.World.Map.GetEntityAt<Item>(newPosition);
            if (item != null) {
                return new ActionResult( succeeded: false,
                alternative: new PickupAction(Performer, item)
                );
            }

            // Check for the presence of a door
            TileDoor door = context.World.Map.GetTileAt<TileDoor>(newPosition);
            if (door != null && !door.IsOpen) {
                return new ActionResult(succeeded: false,
                alternative: new OpenDoorAction(Performer, door)
                );
            }

            // Check if it is possible to go there
            if (context.World.Map.IsTileWalkable(newPosition)) {
                Performer.MoveTo(newPosition, context.World.Map);
                return new ActionResult(succeeded: true);
            }

            // Handle situations where there are non-walkable tiles that CAN be used
            return new ActionResult(succeeded: false,
                alternative: new SkipAction(Performer)
                );
        }



    }
}
