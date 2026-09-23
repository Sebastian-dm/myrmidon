using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myrmidon.Core.Parts {
    public class Health : Part {
        public int Current { get; set; }
        public int Maximum { get; set; }

        public bool IsDead => Current <= 0;
    }
}
