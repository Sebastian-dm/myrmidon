using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myrmidon.Core.Actions;


public interface IAction {

    bool IsImmediate { get; }
    ActionResult Perform(IWorldState context);
}
