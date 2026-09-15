using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myrmidon.Core.Components {
    public class Health : Component {
        public int Current { get; set; }
        public int Maximum { get; set; }

        public bool IsDead => Current <= 0;
    }
}
