using System;
using Microsoft.Xna.Framework;
using clodd;

namespace clodd {
    internal static class Map {

        public static TileBase[] _tiles; // an array of TileBase that contains all of the tiles for a map
        private const int _roomWidth = 10; // demo room width
        private const int _roomHeight = 20; // demo room height


        public static void CreateMap() {
            CreateWalls();
            CreateFloors();
        }

        // Flood the map using the TileWall class
        public static void CreateWalls() {
            // Create an empty array of tiles that is equal to the map size
            _tiles = new TileBase[MainLoop.Width * MainLoop.Height];

            //Fill the entire tile array with floors
            for (int i = 0; i < _tiles.Length; i++) {
                _tiles[i] = new TileWall();
            }
        }

        public static void CreateFloors() {
            //Carve out a rectangle of floors in the tile array
            for (int x = 0; x < _roomWidth; x++) {
                for (int y = 0; y < _roomHeight; y++) {
                    // Calculates the appropriate position (index) in the array
                    // based on the y of tile, width of map, and x of tile
                    _tiles[y * MainLoop.Width + x] = new TileFloor();
                }
            }
        }

        
    }
}
