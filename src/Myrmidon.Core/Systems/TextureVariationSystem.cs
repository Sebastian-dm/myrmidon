using GoRogue.GameFramework;
using Myrmidon.Core.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Myrmidon.Core.Maps.Tiles;
using Myrmidon.Core.Components;


namespace Myrmidon.Core.Systems {

    internal class TextureVariationSystem {

        public static void RefineTileAdjacencyConnections<T>(TileMap map) where T : Tile {
            for (int y = 0; y < map.Height; y++) {
                for (int x = 0; x < map.Width; x++) {
                    var tileAtLocation = map.GetTileAt<Tile>(x, y);
                    if (!(tileAtLocation is T))
                        continue;

                    var orthoNeighborTiles = map.GetOrthoAdjacentTiles<T>(x, y);
                    if (orthoNeighborTiles.Length != 4)
                        throw new ArgumentException("All 4 neighbors must be given in the clockwise order: [N E S W]");

                    byte n = 0;
                    for (int i = 0; i < orthoNeighborTiles.Length; i++)
                        n |= (byte)(((orthoNeighborTiles[i] is T) ? 1: 0) << i);

                    RenderComponent renderComponent = map.GetRenderComponent(x, y);
                    if (renderComponent != null)
                        renderComponent.SetVariant(n);
                }
            }
        }
    }
}
