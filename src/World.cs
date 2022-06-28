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


        public bool IsMapGenRequested = true;
        public bool IsMapGenInProgress = false;
        public bool IsEntityGenRequested = false;


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

        public void Update() {
            int i = 0;
            if (IsMapGenRequested) {
                IsMapGenRequested = false;
                IsMapGenInProgress = true;
                GenerateMapAsync();
            }
            if (IsEntityGenRequested) {
                CreatePlayer();
                CreateMonsters();
                CreateLoot();
                IsEntityGenRequested = false;
                GameLoop.FOV.Update();
            }
            GameLoop.UIManager.RefreshConsole();

        }


        private void GenerateMap() {
            DungeonGenerator mapGen = new DungeonGenerator();
            mapGen.Generate(CurrentMap);
            IsMapGenInProgress = false;
            IsEntityGenRequested = true;
        }

        private async Task GenerateMapAsync() {
            DungeonGenerator mapGen = new DungeonGenerator();
            await Task.Run(() => mapGen.Generate(CurrentMap));
            IsMapGenInProgress = false;
            IsEntityGenRequested = true;
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
        // and drop them all over the map in random places.
        private void CreateMonsters() {
            int numMonsters = 30;

            for (int i = 0; i < numMonsters; i++) {
                Monster newMonster = new Monster(Color.HotPink, Color.Transparent, glyph: 368);

                int monsterPosition = 0;
                bool isPositionValid = false;
                while (isPositionValid == false) {
                    monsterPosition = rng.Next(0, CurrentMap.Width * CurrentMap.Height);
                    isPositionValid = CurrentMap.Tiles[monsterPosition].IsWalkable;
                }

                newMonster.DefenseStrength = rng.Next(0, 10);
                newMonster.DefenseChance = rng.Next(0, 50);
                newMonster.AttackStrength = rng.Next(0, 10);
                newMonster.AttackChance = rng.Next(0, 50);
                newMonster.Name = "a common troll";

                newMonster.Position = new Point(monsterPosition % CurrentMap.Width, monsterPosition / CurrentMap.Width);
                CurrentMap.Add(newMonster);
            }
        }



        /// <summary>
        /// Create some sample treasure that can be picked up on the map
        /// </summary>
        private void CreateLoot() {
            int numLoot = 20;

            for (int i = 0; i < numLoot; i++) {
                Item newLoot = new Item(Color.Beige, Color.Transparent, glyph: 384, name: "Loot", 2);

                int lootPosition = 0;
                bool isPositionValid = false;
                while (isPositionValid == false) {
                    lootPosition = rng.Next(0, CurrentMap.Width * CurrentMap.Height);
                    isPositionValid = CurrentMap.Tiles[lootPosition].IsWalkable;
                }

                newLoot.Position = new Point(lootPosition % CurrentMap.Width, lootPosition / CurrentMap.Width);
                CurrentMap.Add(newLoot);
            }

        }

    }
}