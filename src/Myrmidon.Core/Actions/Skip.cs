using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Myrmidon.Core.Entities;
using Myrmidon.Core.Game;

namespace Myrmidon.Core.Actions {
    internal class SkipAction : IAction {

        public bool IsImmediate { get; } = false;

        public SkipAction(Actor performer) {

        }

        public ActionResult Perform(IGameState context) {
            return new ActionResult();
        }
    }
}
