using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Myrmidon.Core.Game;

namespace Myrmidon.Core.Actions {
    public interface IAction {

        bool IsImmediate { get; }
        ActionResult Perform(IGameState context);
    }

    public class ActionResult {
        public bool Succeeded;
        public IAction? Alternative;

        public ActionResult() { 

        }

        public ActionResult(bool succeeded) {
            Succeeded = succeeded;
        }

        public ActionResult(bool succeeded, IAction alternative) {
            Succeeded = succeeded;
            Alternative = alternative;
        }

    }
}
