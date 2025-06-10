using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bramble.Core;
using Myrmidon.Core.Utilities.Geometry;
using Myrmidon.Core.Maps.Tiles;
using Myrmidon.Core.Entities;
using Myrmidon.Core.Maps;
using Myrmidon.Core.Game;

namespace Myrmidon.Core.Rules {

    public interface IFovSystem {
        public void Recompute(IGameState context, Vec origin);
    }


    public class FovSystem : IFovSystem {

        private int _viewDistance;

        public FovSystem(int viewDistance = 10) {
            _viewDistance = viewDistance;
        }


        // Recompute the visible area based on a given location.
        public void Recompute(IGameState context, Vec origin) {

            TileMap map = context.World.Map;

            // Update tiles visiblity
            for (int x = 0; x < map.Width; x++) {
                for (int y = 0; y < map.Height; y++) {
                    Tile tile = map[x, y];

                    if (Vec.IsDistanceWithin(origin, new Vec(x,y), _viewDistance)) {
                        tile.IsExplored = true;
                        tile.IsVisible = true;
                    }
                    else {
                        tile.IsVisible = false;
                    }
                }
            }

            // Update entity visibility
            foreach (Entity entity in map.Entities.Items) {
                if (Vec.IsDistanceWithin(origin, entity.Position, _viewDistance)) {
                    entity.isVisible = true;
                }
                else {
                    entity.isVisible = false;
                }
            }
        }
    }
}
