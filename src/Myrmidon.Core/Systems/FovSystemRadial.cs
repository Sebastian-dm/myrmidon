using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bramble.Core;
using Myrmidon.Core.Parts;
using Myrmidon.Core.Utilities.Geometry;
using Myrmidon.Core.Zones;

namespace Myrmidon.Core.Systems {

    public interface IFovSystem {
        public void Recompute(Zone zone, Vec origin);
    }


    public class FovSystemRadial : IFovSystem {

        private int _range;
        private float _rangeSqrt;

        public FovSystemRadial(int range = 8) {
            _range = range;
            _rangeSqrt = _range * _range;
        }


        // Recompute the visible area based on a given location.
        public void Recompute(Zone zone, Vec origin) {
            ComputeRadialFov(zone, origin);
        }
        
        
        // Computes the visibility and dimness values based on a simple radius
        private void ComputeRadialFov(Zone zone, Vec origin) {

            var map = zone.TileMap;

            int margin = 1;
            int left = Math.Max(0, origin.X - _range - margin);
            int top = Math.Max(0, origin.Y - _range  - margin);
            int right = Math.Min(map.Width, origin.X + _range + margin);
            int bottom = Math.Min(map.Height, origin.Y + _range + margin);

            // Update tile visiblity
            for (int x = left; x < right; x++) {
                for (int y = top; y < bottom; y++) {
                    var perceptible = map.Ecs.Get<Perceptible>(map[x, y]);
                    UpdatePerceptibleLightFromDistance(zone, origin, perceptible);
                }
            }

            // Update entity visibility
            // TODO: Refactor for ECS
            
            // foreach (Entity entity in map.Entities.Items) {
            //     if (Vec.IsDistanceWithin(origin, entity.Position, _range)) {
            //         entity.isVisible = true;
            //     }
            //     else {
            //         entity.isVisible = false;
            //     }
            // }
        }

        private void UpdatePerceptibleLightFromDistance(Zone zone, Vec distance, Perceptible perceptible) {

            int distSqrt = distance.LengthSquared;
            if (distSqrt <= _rangeSqrt)
                perceptible.Explored = true;

            float clampedDist = (float)Math.Clamp(Math.Pow(distSqrt / _rangeSqrt, 1.0f), 0.0f, 1.0f);
            perceptible.LightLevel = 1f - clampedDist;
        }
    }
}
