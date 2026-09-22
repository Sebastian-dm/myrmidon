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

        public static void RefineTileAdjacencyConnections(TileMap map, string group) {
            for (int y = 0; y < map.Height; y++) {
                for (int x = 0; x < map.Width; x++) {
                    
                    // Check for correct tile and parts at the current location
                    EntityId? tileAtLocation = map.TryGetTile(x, y);
                    if (tileAtLocation == null ||
                        !map.Ecs.Has<Renderable>(tileAtLocation.Value) ||
                        !map.Ecs.Has<Identity>(tileAtLocation.Value))
                        continue;
                    Renderable renderable = map.Ecs.Get<Renderable>(tileAtLocation.Value);
                    Identity identity = map.Ecs.Get<Identity>(tileAtLocation.Value);

                    // Check for correct group and variant type
                    if (!identity.Groups.Contains(group) ||
                        renderable.VariantType != RenderableVariantType.OrthoAdjacent)
                        continue;


                    EntityId?[] neighbors = map.GetOrthoAdjacentTiles(x, y);
                    if (neighbors.Length != 4)
                        throw new ArgumentException("All 4 neighbors must be given in the clockwise order: [N E S W]");

                    List<string>[] neighborGroups = new List<string>[4];
                    for (int i = 0; i < neighbors.Length; i++) {
                        if (neighbors[i] != null && map.Ecs.Has<Identity>(neighbors[i].Value))
                            neighborGroups[i] = map.Ecs.Get<Identity>(neighbors[i].Value).Groups;
                        else
                            neighborGroups[i] = new List<string>();
                    }

                    byte n = 0;
                    for (int i = 0; i < neighbors.Length; i++)
                        n |= (byte)(((neighborGroups[i].Contains(group)) ? 1: 0) << i);

                    renderable.SetVariant(n);
                }
            }
        }
    }
}
