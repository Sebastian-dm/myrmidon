using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myrmidon.Core.GameState {

    public interface IGameContext {
        World World { get; }
    }


    public class GameContext : IGameContext {
        public World World { get; }

        public GameContext(World world) {
            World = world;
        }
    }
}
