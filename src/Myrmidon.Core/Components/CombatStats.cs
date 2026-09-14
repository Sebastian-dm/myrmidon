using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myrmidon.Core.Components {
    internal class CombatStats : Component {
        public int AttackStrength { get; set; }
        public int AttackChance { get; set; }
        public int DefenseStrength { get; set; }
        public int DefenseChance { get; set; }
    }
}
