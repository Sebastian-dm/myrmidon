using System;
using System.Text;
using System.Collections.Generic;

using Myrmidon.Core.Actors;
using Myrmidon.Core.Maps.Tiles;
using Myrmidon.Core.GameState;

using Myrmidon.Core.Utilities.Geometry;
using Myrmidon.Core.Rules;
using Myrmidon.Core.UI;

namespace Myrmidon.Core.Actions {
    // Contains all generic actions performed on entities and tiles
    // including combat, movement, and so on.
    public interface IActionController {

        public bool Update();
        public void AddAction(InputAction command);
        public void AddAction(IAction action);

    }
}