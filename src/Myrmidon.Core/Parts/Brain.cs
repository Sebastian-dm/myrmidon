using Bramble.Core;
using Myrmidon.Core.Actions;
using Myrmidon.Core.ECS;

namespace Myrmidon.Core.Parts;

public class Brain : Part {
    
    
    public IAction GetAction(IWorldState context, EntityId entity)
    {
        var map = context.Zone.TileMap;
        var pos = context.EcsWorld.Get<Position>(entity);

        foreach (Vec? loc in map.GetAdjacentPositions(pos.Coords)) {
            if (loc == null) continue;
            
            var entities = context.Zone.EntityIndex.At(loc.Value);
            if (entities.Count > 0) {
                foreach (var opponent in entities) {
                    if (context.EcsWorld.Has<CombatStats>(opponent)) {
                        return new AttackAction(entity, opponent);
                    }
                }
            }
        }

        return new SkipAction(entity);
    }
    
    
    
}