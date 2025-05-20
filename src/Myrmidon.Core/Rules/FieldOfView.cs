using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Myrmidon.Core.Utilities.Geometry;
using Myrmidon.Core.Map.Tiles;
using Myrmidon.Entities;
using Myrmidon.Maps;

namespace Myrmidon.Rules {
    public class FieldOfView {

        private int _viewDistance;

        public FieldOfView(int viewDistance = 10) {
            _viewDistance = viewDistance;
        }


        // Recompute the visible area based on a given location.
        public void Update(Vector origin) {
            Map map = Program.World.CurrentMap;

            // Update tiles visiblity
            for (int x = 0; x < map.Width; x++) {
                for (int y = 0; y < map.Height; y++) {
                    Tile tile = map[x, y];
                    if (origin.DistanceTo(new Vector(x, y)) <= _viewDistance) {
                        tile.IsExplored = true;
                        tile.isVisible = true;
                    }
                    else {
                        tile.isVisible = false;
                    }
                }
            }

            // Update entity visibility
            foreach (Entity entity in map.Entities.Items) {
                if (origin.DistanceTo(entity.Location) <= _viewDistance) {
                    entity.isVisible = true;
                }
                else {
                    entity.isVisible = false;
                }
            }
        }

        public void Update() {
            Vector PlayerPos = new Vector(
                Program.World.Player.Position.X,
                Program.World.Player.Position.Y
                );
            Update(PlayerPos);

        }
    }
}
