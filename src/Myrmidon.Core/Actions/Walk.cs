using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bramble.Core;
using Myrmidon.Core.Components;
using Myrmidon.Core.Ecs;
using Myrmidon.Core.Entities;
using Myrmidon.Core.Utilities.Geometry;
using Myrmidon.Core.Maps.Tiles;
using Myrmidon.Core.Zones;

namespace Myrmidon.Core.Actions {
    public class WalkAction : IAction {

        public bool IsImmediate { get; } = false;
        public readonly EntityId Performer;
        public readonly Vec Direction;

        private Vec _originalPosition;

        public WalkAction(EntityId performer, Vec direction) {
            
            Performer = performer;
            Direction = direction;
            
        }

        public ActionResult Perform(IWorldState context) {

            // Do nothing if no length given
            if (Direction.X == 0 && Direction.Y == 0)
                return GetDoNothingResult();
            
            // Do nothing if performer has no position
            if (!context.EcsWorld.TryGet<Position>(Performer, out var pos))
                return GetDoNothingResult();

            // store the actor's last move state
            _originalPosition = new Vec(pos.Coords.X, pos.Coords.Y);
            Vec newPosition = _originalPosition + Direction;

            // Check if there is an actor on new position
            var entityInFront = context.Zone.SpatialIndex.At(newPosition).FirstOrDefault();
            
            // There is an entity in front of the player
            if (entityInFront != null) {
                
                // Fight if the entity has combat stats
                if (context.EcsWorld.TryGet<CombatStats>(entityInFront, out var combatStats)) {
                    return new ActionResult( succeeded: false,
                        alternative: new AttackAction(Performer, entityInFront)
                    );
                }
                
                // Pick up if entity has no brain
                // TODO: Create a better way to check if something is an item
                if (!context.EcsWorld.TryGet<Brain>(entityInFront, out var brain)) {
                    return new ActionResult( succeeded: false,
                        alternative: new PickupAction(Performer, entityInFront)
                    );
                }
            }
            
            // Check for the presence of a door
            TileDoor door = context.Zone.Map.GetTileAt<TileDoor>(newPosition);
            if (door != null && !door.IsOpen) {
                return new ActionResult(succeeded: false,
                    alternative: new OpenDoorAction(Performer, door)
                );
            }
            
            // Check if it is possible to go there
            if (context.Zone.Map.IsTileWalkable(newPosition)) {
                context.Zone.SpatialIndex.Remove(Performer, pos.Coords);
                context.Zone.SpatialIndex.Add(Performer, newPosition);
                pos.Coords = newPosition;
                return new ActionResult(succeeded: true);
            }

            // Handle situations where there are non-walkable tiles that CAN be used
            return new ActionResult(succeeded: false,
                alternative: new SkipAction(Performer)
            );


        }

        private ActionResult GetDoNothingResult() {
            return new ActionResult(
                succeeded: false,
                alternative: new SkipAction(Performer)
            );
        }



    }
}
