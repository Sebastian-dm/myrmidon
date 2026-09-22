
using System.Collections.Generic;
using System.Threading;
using System.Reflection;
using System.Linq;

using Bramble.Core;
using Myrmidon.Core.Parts;
using Myrmidon.Core.Utilities.Random;
using Myrmidon.Core.Systems;
using Myrmidon.Core.ECS;



namespace Myrmidon.Core.Zones {

    public class TestMapGenerator : IMapGenerator {


        private RandomNumberGenerator rng = new();

        private EntityTileFactory _entityFactory;


        public TileMap Generate(TileMap map) {
            
            _entityFactory = new EntityTileFactory(map.Ecs);

            FillWithFloor(map);
            PlacePillarWall(map, new Vec(11, 8));
            PlacePillarWall(map, new Vec(12, 8));
            PlacePillarWall(map, new Vec(10, 18));

            TextureVariationSystem.RefineTileAdjacencyConnections(map, "Wall");

            return map;
        }

        private void PlacePillarWall(TileMap map, Vec position) {
            if (map.Bounds.Contains(position)) {
                map[position] = _entityFactory.CreateWall(map, position);
            }
        }


        private void FillWithFloor(TileMap map) {
            for (int i = 0; i < map.Tiles.Length; i++) {
                map[i] = _entityFactory.CreateFloor(map, new Vec(i % map.Width, i / map.Width));
            }
        }


    }
}
