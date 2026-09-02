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

namespace Myrmidon.Core.Game {
    public class WorldManager {

        public IGameState GameState { get; private set; }
        public IFovSystem FovSystem { get; private set; }
        public ActionController ActionController;
        public Zone Zone { get; private set; }
        private Random _rng = new();

        public WorldManager(IGameState gamestate, IFovSystem fov, ActionController actionController) {
            GameState = gamestate;
            Zone = gamestate.Zone;
            FovSystem = fov;
            
            ActionController =  actionController;
        }

        public void Update() {
            if (Zone.GenerationState == Zone.ZoneGenState.NotStarted) {
                Zone.GenerationState = Zone.ZoneGenState.GeneratingTerrain;
                GenerateMap();
            }

            if (Zone.GenerationState == Zone.ZoneGenState.Populating) {
                CreatePlayer();
                CreateMonsters();
                CreateLoot();
                Zone.GenerationState = Zone.ZoneGenState.Ready;
                FovSystem.Recompute(GameState, GameState.Player.Position);
            }
        }

        private void GenerateMap() {
            var mapGen = new DungeonGenerator();
            mapGen.Generate(Zone.Map);
            Zone.GenerationState = Zone.ZoneGenState.Populating;
        }

        private void CreatePlayer() {
            var player = new Player(new Color(20, 255, 255), Color.Transparent);

            if (Zone.Map.Rooms.Count > 0) {
                int index = _rng.Next(Zone.Map.Rooms.Count);
                player.Position = Zone.Map.Rooms[index].Center;
            }
            else {
                player.Position = new Vec(10, 10);
            }

            GameState.Player = player;
            Zone.Map.Add(player);
        }

        private void CreateMonsters() {
            for (int i = 0; i < 30; i++) {
                var monster = new Monster(Color.Red, Color.Transparent, glyph: 2) {
                    AttackChance = _rng.Next(0, 50),
                    AttackStrength = _rng.Next(0, 10),
                    DefenseChance = _rng.Next(0, 50),
                    DefenseStrength = _rng.Next(0, 10),
                    Name = "a common troll"
                };

                PlaceEntityAtRandomWalkable(monster);
            }
        }

        private void CreateLoot() {
            for (int i = 0; i < 20; i++) {
                var loot = new Item(Color.Yellow, Color.Transparent, glyph: 36, name: "Loot");
                PlaceEntityAtRandomWalkable(loot);
            }
        }

        private void PlaceEntityAtRandomWalkable(Entity entity) {
            int pos;
            bool valid;
            do {
                pos = _rng.Next(0, Zone.Map.Width * Zone.Map.Height);
                valid = Zone.Map.Tiles[pos].IsWalkable;
            }
            while (!valid);

            entity.Position = new Vec(pos % Zone.Map.Width, pos / Zone.Map.Width);
            Zone.Map.Entities.Add(entity, new Coord(entity.Position.X, entity.Position.Y));
        }
    }
}
