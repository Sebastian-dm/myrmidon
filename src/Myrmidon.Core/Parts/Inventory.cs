using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Myrmidon.Core.ECS;

namespace Myrmidon.Core.Parts {
    public class Inventory : Part {

        // List of item entity IDs in the inventory
        public List<EntityId> Items { get; set; } = new List<EntityId>();
        public int Coins { get; set; }
        public int Capacity { get; set; }
    }
}
