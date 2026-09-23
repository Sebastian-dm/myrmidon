using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Myrmidon.Core.Parts {
    public class Door : Part {


        public bool IsLocked { get; set; } = false; // Locked door = 1, unlocked = 0
        public bool IsClosed { get; set; } = true; // Closed door = 1, open = 0


        public Door() {
        }
        public Door(bool isLocked, bool isClosed) {

            IsLocked = isLocked;
            IsClosed = isClosed;
        }
    }
}
