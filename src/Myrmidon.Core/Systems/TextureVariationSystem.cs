using GoRogue.GameFramework;
using Myrmidon.Core.ECS;
using Myrmidon.Core.Parts;
using Myrmidon.Core.Zones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;


namespace Myrmidon.Core.Systems {

    internal class TextureVariationSystem {

        public static void RefineTileAdjacencyConnections(TileMap map) {
            for (int y = 0; y < map.Height; y++) {
                for (int x = 0; x < map.Width; x++) {
                    
                    EntityId? tileAtLocation = map.GetTileAt(x, y);
                    if (tileAtLocation == null || !map.Ecs.Has<Renderable>(tileAtLocation.Value))
                        continue;

                    Renderable renderable = map.Ecs.Get<Renderable>(tileAtLocation.Value);
                    if (renderable.VariantType != (int)RenderableVariantType.OrthoAdjacent)
                        continue;


                    EntityId?[] orthoNeighborTiles = map.GetOrthoAdjacentTiles(x, y);
                    string[] orthoNeighborTypes = new string[4];
                    if (orthoNeighborTiles.Length != 4)
                        throw new ArgumentException("All 4 neighbors must be given in the clockwise order: [N E S W]");

                    for (int i = 0; i < orthoNeighborTiles.Length; i++) {
                        if (orthoNeighborTiles[i] == null)
                            orthoNeighborTypes[i] = "";
                        
                        if (!map.Ecs.Has<Renderable>(orthoNeighborTiles[i].Value)) {
                            orthoNeighborTypes[i] = "";
                        }
                        else {
                            Renderable neighborRenderable = map.Ecs.Get<Renderable>(orthoNeighborTiles[i].Value);
                            orthoNeighborTypes[i] = neighborRenderable.TextureSheetName + neighborRenderable.SpriteId;
                        }
                    }

                    byte n = 0;
                    for (int i = 0; i < orthoNeighborTiles.Length; i++)
                        n |= (byte)(((orthoNeighborTypes[i] == renderable.TextureSheetName + renderable.SpriteId) ? 1: 0) << i);

                    renderable.SetVariant(n);
                }
            }
        }
    }
}
