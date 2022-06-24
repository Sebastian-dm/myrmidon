using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using myrmidon.Actions;
using myrmidon.Entities;
using myrmidon.Geometry;
using myrmidon.Tiles;

namespace myrmidon.Actions {
    public class WalkAction : Action {

        // store the actor's last move state
        private Vector _originalPosition; 
        public readonly Vector Direction;

        public WalkAction(Actor performer, Vector direction) : base(performer) {
            Direction = direction;
        }

        public override ActionResult Perform() {

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
            Monster monster = GameLoop.World.CurrentMap.GetEntityAt<Monster>(newPosition);
            if (monster != null) {
                return new ActionResult( succeeded: false,
                alternative: new AttackAction(Performer, monster)
                );
            }

            // Check if there is an item on the new position
            Item item = GameLoop.World.CurrentMap.GetEntityAt<Item>(newPosition);
            if (item != null) {
                return new ActionResult( succeeded: false,
                alternative: new PickupAction(Performer, item)
                );
            }

            // Check for the presence of a door
            TileDoor door = GameLoop.World.CurrentMap.GetTileAt<TileDoor>(newPosition);
            if (door != null && !door.IsOpen) {
                return new ActionResult(succeeded: false,
                alternative: new OpenDoorAction(Performer, door)
                );
            }

            // Check if it is possible to go there
            if (GameLoop.World.CurrentMap.IsTileWalkable(newPosition)) {
                Performer.MoveTo(newPosition);
                return new ActionResult(succeeded: true);
            }

            // Handle situations where there are non-walkable tiles that CAN be used
            return new ActionResult(succeeded: false,
                alternative: new SkipAction(Performer)
                );
        }

    }
}
