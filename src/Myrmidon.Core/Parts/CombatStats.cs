using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myrmidon.Core.Parts {
    internal class CombatStats : Part {
        public int NoAttacks { get; set; }
        public int AttackChance { get; set; }
        public int NoBlocks { get; set; }
        public int BlockChance { get; set; }
    }
}
