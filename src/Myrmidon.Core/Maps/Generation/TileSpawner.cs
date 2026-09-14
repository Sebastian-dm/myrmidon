using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Myrmidon.Core.Maps.Tiles;
using Myrmidon.Core.Components;
using Bramble.Core;

namespace Myrmidon.Core.Maps.Generation {
    public static class TileSpawner {

        private static Dictionary<string, Renderable> renderableBlueprints = new Dictionary<string, Renderable>();


        static TileSpawner() {
            renderableBlueprints["floor"] = new Renderable("text/default", (byte)' ', "g");
            renderableBlueprints["wall"] = new Renderable("tile/wall", (byte)0, "G");
            renderableBlueprints["door"] = new Renderable("text/default", (byte)'+', "Y");
        }

        public static void SpawnTile(TileMap map, int index, Tile tile, string blueprint) {
            if (index >= 0 && index < map.Tiles.Length) {
                map[index] = tile;
                if (renderableBlueprints.ContainsKey(blueprint)) {
                    map.SetRenderComponent(index, new Renderable(renderableBlueprints[blueprint]));
                }
                map.SetPerceptibleComponent(index, new Perceptible());
            }
        }

        public static void SpawnTile(TileMap map, Vec position, Tile tile, string blueprint) {
            if (map.Bounds.Contains(position)) {
                map[position] = tile;
                if (renderableBlueprints.ContainsKey(blueprint)) {
                    map.SetRenderComponent(position, new Renderable(renderableBlueprints[blueprint]));
                }
                map.SetPerceptibleComponent(position, new Perceptible());
            }
        }


    }
}
