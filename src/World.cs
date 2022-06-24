using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using SadConsole.Components;
using Microsoft.Xna.Framework;
using myrmidon.Entities;
using myrmidon.Tiles;
using myrmidon.Map;

namespace myrmidon {

    /// <summary>
    /// Generates and stores all game state data.
    /// </summary>
    public class World {

        private Random rng = new Random();
        private int _mapWidth = 87;
        private int _mapHeight = 47;


        public bool IsMapGenDone = false;
        public bool IsMapGenStarted = false;


        public Map.Map CurrentMap { get; set; }
        public Player Player { get; set; }

        public GoRogue.MultiSpatialMap<Actor> Entities {
            get {
                return CurrentMap.Entities;
            }
        }


        public World() {
            CurrentMap = new Map.Map(_mapWidth, _mapHeight);
        }


        public void PopulateMap() {
            IsMapGenStarted = true;
            GenerateMap();
            CreatePlayer();
            CreateMonsters();
            CreateLoot();
            IsMapGenDone = true;
        }

        public async Task PopulateMapAsync() {
            await Task.Run(() => PopulateMap());
        }


        private void GenerateMap() {
            DungeonGenerator mapGen = new DungeonGenerator();
            mapGen.Populate(CurrentMap);
        }



        /// <summary>
        /// Create a player using the Player class and set its starting position
        /// </summary>
        private void CreatePlayer() {
            Player = new Player(new Color(20, 255, 255), Color.Transparent);

            // Place the player on the first non-movement-blocking tile on the map
            if (CurrentMap.Rooms.Count > 0) {
                int RoomIndex = rng.Next(0, CurrentMap.Rooms.Count);
                Player.Position = CurrentMap.Rooms[RoomIndex].Center;
            }
            else {
                Player.Position = new Point(10, 10);
            }
            

            // add the player to the Map's collection of Entities
            CurrentMap.Add(Player);
        }


        // Create some random monsters with random attack and defense values
        // and drop them all over the map in
        // random places.
        private void CreateMonsters() {
            // number of monsters to create
            int numMonsters = 10;

            // Create several monsters and 
            // pick a random position on the map to place them.
            // check if the placement spot is blocking (e.g. a wall)
            // and if it is, try a new position
            for (int i = 0; i < numMonsters; i++) {
                int monsterPosition = 0;
                Monster newMonster = new Monster(Color.HotPink, Color.Transparent);

                // pick a random spot on the map
                while (CurrentMap.Tiles[monsterPosition].IsBlockingMove) {
                    monsterPosition = rng.Next(0, CurrentMap.Width * CurrentMap.Height);
                }

                // plug in some magic numbers for attack and defense values
                newMonster.DefenseStrength = rng.Next(0, 10);
                newMonster.DefenseChance = rng.Next(0, 50);
                newMonster.AttackStrength = rng.Next(0, 10);
                newMonster.AttackChance = rng.Next(0, 50);
                newMonster.Name = "a common troll";

                // Set the monster's new position
                newMonster.Position = new Point(monsterPosition % CurrentMap.Width, monsterPosition / CurrentMap.Width);
                CurrentMap.Add(newMonster);
            }
        }



        /// <summary>
        /// Create some sample treasure that can be picked up on the map
        /// </summary>
        private void CreateLoot() {
            // number of treasure drops to create
            int numLoot = 20;

            // Produce lot up to a max of numLoot
            for (int i = 0; i < numLoot; i++) {
                // Create an Item with some standard attributes
                int lootPosition = 0;
                Item newLoot = new Item(Color.Beige, Color.Transparent, "Loot", glyph: 384, 2);

                // Try placing the Item at lootPosition; if this fails, try random positions on the map's tile array
                while (CurrentMap.Tiles[lootPosition].IsBlockingMove) {
                    // pick a random spot on the map
                    lootPosition = rng.Next(0, CurrentMap.Width * CurrentMap.Height);
                }

                // set the loot's new position
                newLoot.Position = new Point(lootPosition % CurrentMap.Width, lootPosition / CurrentMap.Width);

                // add the Item to the MultiSpatialMap
                CurrentMap.Add(newLoot);
            }

        }

    }
}