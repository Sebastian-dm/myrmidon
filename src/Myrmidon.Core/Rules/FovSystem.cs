using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Myrmidon.Core.Utilities.Geometry;
using Myrmidon.Core.Maps.Tiles;
using Myrmidon.Core.Entities;
using Myrmidon.Core.Maps;
using Myrmidon.Core.GameState;

namespace Myrmidon.Core.Rules {

    public interface IFovSystem {
        public void Update(IGameContext context, Point origin);
    }


    public class FovSystem : IFovSystem {

        private int _viewDistance;

        public FovSystem(int viewDistance = 10) {
            _viewDistance = viewDistance;
        }


        // Recompute the visible area based on a given location.
        public void Update(IGameContext context, Point origin) {

            Map map = context.World.Map;

            // Update tiles visiblity
            for (int x = 0; x < map.Width; x++) {
                for (int y = 0; y < map.Height; y++) {
                    Tile tile = map[x, y];
                    if (origin.DistanceTo(new Point(x, y)) <= _viewDistance) {
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
                if (origin.DistanceTo(entity.Position) <= _viewDistance) {
                    entity.isVisible = true;
                }
                else {
                    entity.isVisible = false;
                }
            }
        }
    }
}
