using Myrmidon.Core.Actions;
using Myrmidon.Core.Ecs;

namespace Myrmidon.Core.Components;

public class Brain : Component {
    
    
    public IAction GetAction(EntityId entityId) {
        return new SkipAction(entityId);
    }
}