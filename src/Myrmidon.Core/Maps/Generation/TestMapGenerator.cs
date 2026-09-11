
using System.Collections.Generic;
using System.Threading;
using System.Reflection;
using System.Linq;

using Bramble.Core;
using Myrmidon.Core.Components;
using Myrmidon.Core.Maps.Tiles;
using Myrmidon.Core.Utilities.Random;
using Myrmidon.Core.Systems;

/// This generator fills the whole map with floor tiles.

namespace Myrmidon.Core.Maps.Generation {

    public class TestMapGenerator : IMapGenerator {

        public TileMap Generate(TileMap map) {
            FillWithFloor(map);
            PlacePillarWall(map, new Vec(11, 8));
            PlacePillarWall(map, new Vec(12, 8));
            PlacePillarWall(map, new Vec(10, 18));

            //TextureVariationSystem.RefineTileAdjacencyConnections<TileWall>(map);

            return map;
        }

        private void PlacePillarWall(TileMap map, Vec position) {
            if (map.Bounds.Contains(position)) {
                map.SetRenderComponent(position, new Renderable("text/default", (byte)'#', "R"));
                map.SetPerceptibleComponent(position, new Perceptible());
                map[position] = new TileWall();
            }
        }


        private void FillWithFloor(TileMap map) {
            for (int i = 0; i < map.Tiles.Length; i++) {
                var render = new Renderable("text/default", 7, "black");
                map.SetRenderComponent(i, render);
                var percept = new Perceptible();
                map.SetPerceptibleComponent(i, percept);
                map[i] = new TileFloor();
            }
        }


    }
}
