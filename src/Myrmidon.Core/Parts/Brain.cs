using Myrmidon.Core.Actions;
using Myrmidon.Core.ECS;

namespace Myrmidon.Core.Parts;

public class Brain : Part {
    
    
    public IAction GetAction(EntityId entityId) {
        return new SkipAction(entityId);
    }
}