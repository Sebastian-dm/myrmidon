using Bramble.Core;
using Myrmidon.Core.ECS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myrmidon.Core.Parts {
    public class Position : Part {
        public Vec Coords { get; set; }
        public int ZoneId { get; set; }
        public EntityId? Container { get; set; } = null; // ID of the entity that contains this entity, if any

    }
}
