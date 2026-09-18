using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Myrmidon.Core.Parts {
    internal class Door : Part {


        public bool IsLocked; // Locked door = 1, Unlocked = 0
        public bool IsClosed; // Open door = 1, closed = 0


        //Default constructor
        //A Door can be set locked/unlocked/open/closed using the constructor.
        public Door(bool isLocked, bool isClosed) {

            IsLocked = isLocked;
            IsClosed = isClosed;
        }
    }
}
