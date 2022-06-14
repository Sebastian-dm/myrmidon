using System;
using SadConsole.Components;
using Microsoft.Xna.Framework;
using clodd.Entities;

namespace clodd {

    /// <summary>
    /// Generates and stores all game state data.
    /// </summary>
    public class World {
        
        // map creation and storage data
        private int _mapWidth = 100;
        private int _mapHeight = 100;
        private int _maxRooms = 100;
        private int _minRoomSize = 4;
        private int _maxRoomSize = 15;
        private TileBase[] _mapTiles;

        public Map CurrentMap { get; set; }
        public Player Player { get; set; }



        /// <summary>
        /// Creates a new game world and stores it in publicly accessible
        /// </summary>
        public World() {
            // Build a map
            CreateMap();

            // create an instance of player
            CreatePlayer();
        }



        /// <summary>
        /// Create a new map using the Map class and a map generator.
        /// Uses several parameters to determine .
        /// </summary>
        private void CreateMap() {
            _mapTiles = new TileBase[_mapWidth * _mapHeight];
            CurrentMap = new Map(_mapWidth, _mapHeight);
            MapGenerator mapGen = new MapGenerator();
            CurrentMap = mapGen.GenerateMap(_mapWidth, _mapHeight, _maxRooms, _minRoomSize, _maxRoomSize);
        }



        /// <summary>
        /// Create a player using the Player class and set its starting position
        /// </summary>
        private void CreatePlayer() {
            Player = new Player(Color.Yellow, Color.Transparent);

            // Place the player on the first non-movement-blocking tile on the map
            for (int i = 0; i < CurrentMap.Tiles.Length; i++) {
                if (!CurrentMap.Tiles[i].IsBlockingMove) {
                    // Set the player's position to the index of the current map position
                    Player.Position = SadConsole.Helpers.GetPointFromIndex(i, CurrentMap.Width);
                }
            }

            // Add the ViewPort sync Component to the player
            Player.Components.Add(new EntityViewSyncComponent());
        }
    }
}