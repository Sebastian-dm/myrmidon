using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Myrmidon.Core.Actors;

namespace Myrmidon.Core.Actions {
    internal class SkipAction : Action {

        public SkipAction(Actor performer) : base(performer) { }

        public override ActionResult Perform() {
            return new ActionResult();
        }
    }
}
