using Myrmidon.Core.Actions;
using Myrmidon.Core.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myrmidon.Core.Game {

    public interface IGameState {
        Scene World { get; }

        FovSystem FovSystem { get; }

    }


    public class GameState : IGameState {
        public Scene World { get; private set; }
        public FovSystem FovSystem { get; private set; }
        public IActionController ActionController { get; set; }

        public GameState(Scene world, FovSystem fov) {
            World = world;
            FovSystem = fov;
        }
    }
}
