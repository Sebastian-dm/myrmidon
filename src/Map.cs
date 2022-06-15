using System;
using System.Linq;
using Microsoft.Xna.Framework;
using clodd.Entities;
using clodd.Tiles;
using SadConsole;

namespace clodd {
    // Stores, manipulates and queries Tile data
    public class Map {
        
        TileBase[] _tiles; // contain all tile objects
        private int _width;
        private int _height;

        public TileBase[] Tiles { get { return _tiles; } set { _tiles = value; } }
        public int Width { get { return _width; } set { _width = value; } }
        public int Height { get { return _height; } set { _height = value; } }

        public GoRogue.MultiSpatialMap<Entity> Entities; // Keeps track of all the Entities on the map
        public static GoRogue.IDGenerator IDGenerator = new GoRogue.IDGenerator(); // A static IDGenerator that all Entities can access



        /// <summary>
        /// Build a new map with a specified width and height
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Map(int width, int height) {
            _width = width;
            _height = height;
            Tiles = new TileBase[width * height];
            Entities = new GoRogue.MultiSpatialMap<Entity>();
        }
        


        /// <summary>
        /// Checks whether actor tried to walk off map or into solid tiles.
        /// </summary>
        /// <param name="location">Point coordinates of tile to check.</param>
        /// <returns>true if tile is walkable, false if not.</returns>
        public bool IsTileWalkable(Point location) {
            // first make sure that actor isn't trying to move
            // off the limits of the map
            if (location.X < 0 || location.Y < 0 || location.X >= Width || location.Y >= Height)
                return false;
            // then return whether the tile is walkable
            return !_tiles[location.Y * Width + location.X].IsBlockingMove;
        }



        /// <summary>
        /// Checking whether a certain type of entity is at a specified
        /// location the manager's list of entities and if it exists, return that Entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="location"></param>
        /// <returns></returns>
        public T GetEntityAt<T>(Point location) where T : Entity {
            return Entities.GetItems(location).OfType<T>().FirstOrDefault();
        }


        /// <summary>
        /// Checking whether a certain type of tile is at a specified location.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        // in the map's Tiles
        //and if it exists, return that Tile
        //accepts an x/y coordinate
        public T GetTileAt<T>(int x, int y) where T : TileBase {
            int locationIndex = Helpers.GetIndexFromPoint(x, y, Width);
            // make sure the index is within the boundaries of the map!
            if (0 <= locationIndex && locationIndex <= Width * Height) {
                if (Tiles[locationIndex] is T)
                    return (T)Tiles[locationIndex];
                else return null;
            }
            else return null;
        }



        /// <summary>
        /// Removes an Entity from the MultiSpatialMap
        /// </summary>
        /// <param name="entity"></param>
        public void Remove(Entity entity) {
            // remove from SpatialMap
            Entities.Remove(entity);

            // Link up the entity's Moved event to a new handler
            entity.Moved -= OnEntityMoved;
        }



        /// <summary>
        /// Adds an Entity to the MultiSpatialMap
        /// </summary>
        /// <param name="entity"></param>
        public void Add(Entity entity) {
            // add entity to the SpatialMap
            Entities.Add(entity, entity.Position);

            // Link up the entity's Moved event to a new handler
            entity.Moved += OnEntityMoved;
        }



        /// <summary>
        /// When the Entity's .Moved value changes, it triggers this event handler which updates the Entity's
        /// current position in the SpatialMap
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnEntityMoved(object sender, Entity.EntityMovedEventArgs args) {
            Entities.Move(args.Entity as Entity, args.Entity.Position);
        }
    }
}