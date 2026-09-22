using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Myrmidon.Core.Parts {
    internal class Door : Part {


        public bool IsLocked = false; // Locked door = 1, unlocked = 0
        public bool IsClosed = true; // Closed door = 1, open = 0


        public Door() {
        }
        public Door(bool isLocked, bool isClosed) {

            IsLocked = isLocked;
            IsClosed = isClosed;
        }
    }
}
