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
        
        private Hectare _zone;
        private Random _rng = new();

        public WorldManager(IGameState gamestate, IFovSystem fov, ActionController actionController) {
            GameState = gamestate;
            _zone = gamestate.Hectare;
            FovSystem = fov;
            
            ActionController =  actionController;
        }

        public void Update() {
            if (_zone.IsMapGenRequested) {
                _zone.IsMapGenRequested = false;
                _zone.IsMapGenInProgress = true;
                GenerateMap();
            }

            if (_zone.IsEntityGenRequested) {
                CreatePlayer();
                CreateMonsters();
                CreateLoot();
                _zone.IsEntityGenRequested = false;
                FovSystem.Recompute(GameState, _zone.Player.Position);
            }
        }

        private void GenerateMap() {
            var mapGen = new DungeonGenerator();
            mapGen.Generate(_zone.Map);
            _zone.IsMapGenInProgress = false;
            _zone.IsEntityGenRequested = true;
        }

        private void CreatePlayer() {
            var player = new Player(new Color(20, 255, 255), Color.Transparent);

            if (_zone.Map.Rooms.Count > 0) {
                int index = _rng.Next(_zone.Map.Rooms.Count);
                player.Position = _zone.Map.Rooms[index].Center;
            }
            else {
                player.Position = new Vec(10, 10);
            }

            _zone.Player = player;
            _zone.Map.Add(player);
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
                pos = _rng.Next(0, _zone.Map.Width * _zone.Map.Height);
                valid = _zone.Map.Tiles[pos].IsWalkable;
            }
            while (!valid);

            entity.Position = new Vec(pos % _zone.Map.Width, pos / _zone.Map.Width);
            _zone.Map.Entities.Add(entity, new Coord(entity.Position.X, entity.Position.Y));
        }
    }
}
