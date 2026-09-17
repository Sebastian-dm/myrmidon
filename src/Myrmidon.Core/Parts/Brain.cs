using Myrmidon.Core.Actions;
using Myrmidon.Core.Ecs;

namespace Myrmidon.Core.Parts;

public class Brain : Part {
    
    
    public IAction GetAction(EntityId entityId) {
        return new SkipAction(entityId);
    }
}