using System;
using Microsoft.Xna.Framework;

namespace clodd {

    public class Map {

        private TileBase[] _tiles; // contain all tile objects
        private int _width;
        private int _height;

        public TileBase[] Tiles { get { return _tiles; } set { _tiles = value; } }
        public int Width { get { return _width; } set { _width = value; } }
        public int Height { get { return _height; } set { _height = value; } }


        /// <summary>
        /// Build a new map with a specified width and height
        /// </summary>
        /// <param name="width">Width of the map in tiles</param>
        /// <param name="height">Height of the map in tiles</param>
        public Map(int width, int height) {
            _width = width;
            _height = height;
            Tiles = new TileBase[width * height];
        }


        /// <summary>
        /// Checks the tile at the given location to see if it is walkable
        /// </summary>
        /// <param name="location"></param>
        /// <returns>false if tile location is not walkable or is off-map</returns>
        public bool IsTileWalkable(Point location) {
            // first make sure that actor isn't trying to move
            // off the limits of the map
            if (location.X < 0 || location.Y < 0 || location.X >= Width || location.Y >= Height)
                return false;
            // then return whether the tile is walkable
            return !_tiles[location.Y * Width + location.X].IsBlockingMove;
        }


    }
}
