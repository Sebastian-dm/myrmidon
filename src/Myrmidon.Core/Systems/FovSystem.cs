using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bramble.Core;
using Myrmidon.Core.Components;
using Myrmidon.Core.Utilities.Geometry;
using Myrmidon.Core.Maps.Tiles;
using Myrmidon.Core.Entities;
using Myrmidon.Core.Maps;

namespace Myrmidon.Core.Systems {

    public interface IFovSystem {
        public void Recompute(TileMap map, Vec origin);
    }


    public class FovSystem : IFovSystem {

        private int _range;
        private float _rangeSqrt;

        public FovSystem(int range = 8) {
            _range = range;
            _rangeSqrt = _range * _range;
        }


        // Recompute the visible area based on a given location.
        public void Recompute(TileMap map, Vec origin) {

            int margin = 1;
            int left = Math.Max(0, origin.X - _range - margin);
            int top = Math.Max(0, origin.Y - _range  - margin);
            int right = Math.Min(map.Width, origin.X + _range + margin);
            int bottom = Math.Min(map.Height, origin.Y + _range + margin);

            // Update tile visiblity
            for (int x = left; x < right; x++) {
                for (int y = top; y < bottom; y++) {
                    Vec loc = new Vec(x, y);
                    var renderComp = map.GetRenderComponent(loc);
                    
                    int distSqrt = (loc - origin).LengthSquared;
                    if (distSqrt <= _rangeSqrt)
                        renderComp.Explored = true;

                    float dim = 1f - (float)Math.Pow(distSqrt / _rangeSqrt, 1.0f);
                    renderComp.Dimfactor = Math.Clamp(dim, 0.1f, 1);
                }
            }

            // Update entity visibility
            foreach (Entity entity in map.Entities.Items) {
                if (Vec.IsDistanceWithin(origin, entity.Position, _range)) {
                    entity.isVisible = true;
                }
                else {
                    entity.isVisible = false;
                }
            }
        }
    }
}
