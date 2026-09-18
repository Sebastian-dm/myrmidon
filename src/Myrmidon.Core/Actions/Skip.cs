using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Myrmidon.Core.ECS;

namespace Myrmidon.Core.Actions {
    internal class SkipAction : IAction {

        public bool IsImmediate { get; } = false;

        public SkipAction(EntityId performer) {

        }

        public ActionResult Perform(IWorldState context) {
            return new ActionResult();
        }
    }
}
