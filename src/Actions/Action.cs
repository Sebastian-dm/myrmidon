using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using clodd.Entities;

namespace clodd.Actions {
    public abstract class Action {
        public readonly Actor Performer;

        public bool IsImmediate;

        public Action(Actor performer) {
            Performer = performer;
        }

        public abstract ActionResult Perform();
    }

    public class ActionResult {
        public bool Succeeded;
        public Action Alternative;

        public ActionResult() { 

        }

        public ActionResult(bool succeeded) {
            Succeeded = succeeded;
        }

        public ActionResult(bool succeeded, Action alternative) {
            Succeeded = succeeded;
            Alternative = alternative;
        }

    }
}
