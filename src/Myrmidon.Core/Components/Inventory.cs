using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myrmidon.Core.Components {
    public class Inventory : Component {

        // List of item entity IDs in the inventory
        public List<uint> Items { get; } = new List<uint>();
        public int Coins { get; set; }
        public int Capacity { get; set; }
    }
}
