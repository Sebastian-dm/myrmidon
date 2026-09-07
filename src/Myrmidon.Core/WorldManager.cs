using Bramble.Core;
using Myrmidon.Core.Entities;
using Myrmidon.Core.Maps;
using Myrmidon.Core.Rules;
using Myrmidon.Core.Utilities.Geometry;
using Myrmidon.Core.Utilities.Graphics;
using Myrmidon.Core.Maps.Generation;
using GoRogue;
using Myrmidon.Core.Actions;
using Myrmidon.Core.Game;

namespace Myrmidon.Core {
    public class WorldManager {

        public IGameState GameState { get; private set; }
        public IFovSystem FovSystem { get; private set; }
        public ActionController ActionController;


        private Random _rng = new();

        public WorldManager(IGameState gamestate, ActionController actionController) {
            GameState = gamestate;
            FovSystem = new FovSystem();
            
            ActionController =  actionController;
        }

        public void Update() {
            if (GameState.Zone.GenerationState != Zone.ZoneGenState.Ready) {
                GenerateZone(GameState.Zone);
                return;
            }

            FovSystem.Recompute(GameState, GameState.Player.Position);

        }

        private void GenerateZone(Zone zone)
        {
            if (zone.GenerationState == Zone.ZoneGenState.NotStarted) {
                zone.GenerationState = Zone.ZoneGenState.Terraforming;
                var mapGen = new DungeonGenerator();
                mapGen.Generate(zone.Map);
                zone.GenerationState = Zone.ZoneGenState.Unpopulated;
            }
            if (zone.GenerationState == Zone.ZoneGenState.Unpopulated) {
                zone.GenerationState = Zone.ZoneGenState.Populating;
                CreatePlayer(zone.Map);
                CreateMonsters(zone.Map);
                CreateLoot(zone.Map);
                zone.GenerationState = Zone.ZoneGenState.Ready;
            }
        }


        private void CreatePlayer(TileMap map) {
            var player = new Player(new Color(20, 255, 255), Color.Transparent);

            if (map.Rooms.Count > 0) {
                int index = _rng.Next(map.Rooms.Count);
                player.Position = map.Rooms[index].Center;
            }
            else {
                player.Position = new Vec(10, 10);
            }

            GameState.Player = player;
            map.AddEntity(player);
        }


        private void CreateMonsters(TileMap map) {
            for (int i = 0; i < 30; i++) {
                var monster = new Monster(Color.Red, Color.Transparent, glyph: 2) {
                    AttackChance = _rng.Next(0, 50),
                    AttackStrength = _rng.Next(0, 10),
                    DefenseChance = _rng.Next(0, 50),
                    DefenseStrength = _rng.Next(0, 10),
                    Name = "a common troll"
                };

                PlaceEntityAtRandomWalkable(map, monster);
            }
        }


        private void CreateLoot(TileMap map) {
            for (int i = 0; i < 20; i++) {
                var loot = new Item(Color.Yellow, Color.Transparent, glyph: 36, name: "Loot");
                PlaceEntityAtRandomWalkable(map, loot);
            }
        }


        private void PlaceEntityAtRandomWalkable(TileMap map, Entity entity) {
            int pos;
            bool valid;
            do {
                pos = _rng.Next(0, map.Width * map.Height);
                valid = map.Tiles[pos].IsWalkable;
            }
            while (!valid);

            entity.Position = new Vec(pos % map.Width, pos / map.Width);
            map.Entities.Add(entity, new Coord(entity.Position.X, entity.Position.Y));
        }
    }
}
