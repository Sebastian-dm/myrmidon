using Myrmidon.Core.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myrmidon.Core.Game {

    public interface IGameContext {
        Scene World { get; }

        public IActionController ActionController { get; }
    }


    public class GameState : IGameContext {
        public Scene World { get; }
        public IActionController ActionController { get; set; }

        public GameState(Scene world) {
            World = world;
        }
    }
}
