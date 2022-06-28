using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using myrmidon.Entities;
using myrmidon.Tiles;
using myrmidon.Geometry;
using SadConsole;

namespace myrmidon.Map {
    // Stores, manipulates and queries Tile data
    public class Map {

        private Tile[] _tiles; // contain all tile objects
        private int _width;
        private int _height;


        
        public Tile[] Tiles { get { return _tiles; } set { _tiles = value; } }
        public Tile this[int x, int y] {
            get => Tiles[x + y * Width];
            set => Tiles[x + y * Width] = value;
        }
        public Tile this[int i] {
            get => Tiles[i];
            set => Tiles[i] = value;
        }


        public List<Rect> Rooms { get; set; }
        public int Width { get { return _width; } set { _width = value; } }
        public int Height { get { return _height; } set { _height = value; } }

        public GoRogue.MultiSpatialMap<Actor> Entities; // Keeps track of all the Entities on the map
        public static GoRogue.IDGenerator IDGenerator = new GoRogue.IDGenerator(); // A static IDGenerator that all Entities can access


        public Rect Bounds {
            get {
                return new Rect(0, 0, Width, Height);
            }
        }


        // Build a new map with a specified width and height
        public Map(int width, int height) {
            _width = width;
            _height = height;
            Tiles = new Tile[width * height];
            for (int i = 0; i < width * height; i++) Tiles[i] = new TileEmpty();
            Rooms = new List<Rect>();
            Entities = new GoRogue.MultiSpatialMap<Actor>();
        }


        // Checks whether actor tried to walk off map or into solid tiles.
        public bool IsTileWalkable(Vector location) {
            // first make sure that actor isn't trying to move off the limits of the map
            if (location.X < 0 || location.Y < 0 || location.X >= Width || location.Y >= Height)
                return false;
            // then return whether the tile is walkable
            return _tiles[location.Y * Width + location.X].IsWalkable;
        }


        // Returns an entity if it exists at location. Return null otherwise.
        public T GetEntityAt<T>(Vector location) where T : Actor {
            return Entities.GetItems(new Point(location.X, location.Y)).OfType<T>().FirstOrDefault();
        }


        // Returns a tile if it exists at location. Return null otherwise.
        public T GetTileAt<T>(int x, int y) where T : Tile {
            int locationIndex = Helpers.GetIndexFromPoint(x, y, Width);
            // make sure the index is within the boundaries of the map!
            if (0 <= locationIndex && locationIndex < Width * Height) {
                if (Tiles[locationIndex] is T)
                    return (T)Tiles[locationIndex];
                else return null;
            }
            else return null;
        }
        public T GetTileAt<T>(Vector location) where T : Tile {
            return GetTileAt<T>(location.X, location.Y);
        }


        // Checks if a specific type of tile at a specified location is on the map. If it exists, returns that Tile.
        public T[] GetAdjacentTiles<T>(Vector loc) where T : Tile {
            int w = Width;
            int h = Height;

            T[] result = new T[8];
            result[0] = (loc.X <= 0   | loc.Y <= 0  ) ? null : GetTileAt<T>(loc.X-1, loc.Y-1);
            result[1] = (               loc.Y <= 0  ) ? null : GetTileAt<T>(loc.X  , loc.Y-1);
            result[2] = (loc.X >= w-1 | loc.Y <= 0  ) ? null : GetTileAt<T>(loc.X+1, loc.Y-1);
            result[3] = (loc.X >= w-1               ) ? null : GetTileAt<T>(loc.X+1, loc.Y  );
            result[4] = (loc.X >= w-1 | loc.Y >= h-1) ? null : GetTileAt<T>(loc.X+1, loc.Y+1);
            result[5] = (               loc.Y >= h-1) ? null : GetTileAt<T>(loc.X  , loc.Y+1);
            result[6] = (loc.X <= 0   | loc.Y >= h-1) ? null : GetTileAt<T>(loc.X-1, loc.Y+1);
            result[7] = (loc.X <= 0                 ) ? null : GetTileAt<T>(loc.X-1, loc.Y  );
            return result;
        }
        public T[] GetAdjacentTiles<T>(int x, int y) where T : Tile {
            return GetAdjacentTiles<T>(new Vector(x, y));
        }


        public void SetTileAt(Vector location, Tile tile) {
            Tiles[location.ToIndex(Width)] = tile;
        }



        // Removes an Entity from the Map
        public void Remove(Actor entity) {
            // remove from SpatialMap
            if (!Entities.Remove(entity))
                throw new Exception("Failed to remove entity from map");

            // De-link the entity's Moved event from the handler
            entity.Moved -= OnEntityMoved;
        }


        // Adds an Entity to the MultiSpatialMap
        public void Add(Actor entity) {
            if (!Entities.Add(entity, entity.Position))
                throw new Exception("Failed to add entity to map");

            entity.Moved += OnEntityMoved; // Link entity Moved event to new handler
        }


        // When the Entity's .Moved value changes, it triggers this event handler which updates the Entity's current position in the SpatialMap
        private void OnEntityMoved(object sender, SadConsole.Entities.Entity.EntityMovedEventArgs args) {
            if (!Entities.Move(args.Entity as Actor, args.Entity.Position))
                throw new Exception("Failed to move entity on map.");
        }
    }
}