using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myrmidon.Core.Maps.Generation {
    public interface IMapGenerator {
        TileMap Generate(TileMap map);
    }
}
