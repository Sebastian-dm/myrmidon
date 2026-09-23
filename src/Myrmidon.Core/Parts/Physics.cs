using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myrmidon.Core.Parts {
    public class Physics : Part {
        public bool BlocksMovement { get; set; }
        public bool BlocksLineOfSight { get; set; }
    }
}
