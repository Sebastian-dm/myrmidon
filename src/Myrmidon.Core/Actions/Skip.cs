using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Myrmidon.Core.Actors;
using Myrmidon.Core.GameState;

namespace Myrmidon.Core.Actions {
    internal class SkipAction : IAction {

        public bool IsImmediate { get; } = false;

        public SkipAction(Actor performer) {

        }

        public ActionResult Perform(IGameContext context) {
            return new ActionResult();
        }
    }
}
