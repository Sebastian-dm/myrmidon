using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bramble.Core;
using Myrmidon.Core.Parts;
using Myrmidon.Core.ECS;
using Myrmidon.Core.Utilities.Geometry;
using Myrmidon.Core.Zones;

namespace Myrmidon.Core.Actions {
    public class DirectionAction : IAction {

        public bool IsImmediate { get; } = false;
        public readonly EntityId Performer;
        public readonly Vec Direction;

        private Vec _originalPosition;

        public DirectionAction(EntityId performer, Vec direction) {
            
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
            var entitiesInFront = context.Zone.EntityIndex.At(newPosition);
            
            // There is an entity in front of the player
            if (entitiesInFront.Count != 0) {
                
                var entityInFront = entitiesInFront.First();
                
                // Fight if the entity has combat stats
                if (context.EcsWorld.TryGet<CombatStats>(entityInFront, out var combatStats)) {
                    return new ActionResult( succeeded: false,
                        alternative: new AttackAction(Performer, entityInFront)
                    );
                }
                
                // Pick up if entity has no brain
                // TODO: Create a better way to check if something is an item
                if (context.EcsWorld.Has<Identity>(entityInFront) &&
                    context.EcsWorld.Get<Identity>(entityInFront).Groups.Contains("Item"))
                {
                    return new ActionResult( succeeded: false,
                        alternative: new PickupAction(Performer, entityInFront)
                    );
                }
            }
            
            // Check for the presence of a door
            var tile = context.Zone.TileMap[newPosition];
            context.EcsZone.TryGet<Door>(tile, out var doorComponent);
            
            if (doorComponent != null && doorComponent.IsClosed) {
                return new ActionResult(succeeded: false,
                    alternative: new OpenDoorAction(Performer, tile)
                );
            }
            
            // Check if it is possible to go there
            if (context.Zone.TileMap.IsTileWalkable(newPosition)) {
                context.Zone.EntityIndex.Remove(Performer, pos.Coords);
                context.Zone.EntityIndex.Add(Performer, newPosition);
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
