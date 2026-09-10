
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
                var rc = new RenderComponent("text/default", (byte)'#', "R");
                rc.Explored = true;
                rc.Dimfactor = 1;
                map.SetRenderComponent(position, rc);
                map[position] = new TileWall();
            }
        }


        private void FillWithFloor(TileMap map) {
            for (int i = 0; i < map.Tiles.Length; i++) {
                var rc = new RenderComponent("text/default", 7, "black");
                rc.Explored = true;
                rc.Dimfactor = 1;
                map.SetRenderComponent(i, rc);
                map[i] = new TileFloor();
            }
        }


    }
}
