using Bramble.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myrmidon.Core.Components {
    public class Position : Component {
        public Vec Coords { get; set; }
        public int ZoneId { get; set; }
        public uint? ContainerId { get; set; } = null; // ID of the entity that contains this entity, if any

    }
}
