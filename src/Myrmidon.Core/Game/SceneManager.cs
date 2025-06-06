using Myrmidon.Core.Entities;
using Myrmidon.Core.Maps;
using Myrmidon.Core.Rules;
using Myrmidon.Core.Utilities.Geometry;
using Myrmidon.Core.Utilities.Graphics;
using Myrmidon.Core.Maps.Generation;
using GoRogue;
using Myrmidon.Core.Game;

namespace Myrmidon.Core.Game {
    public class SceneManager {

        private readonly IGameState _context;
        private readonly IFovSystem _fov;
        private readonly Scene _world;
        private readonly Random _rng = new();

        public SceneManager(IGameState context, IFovSystem fov) {
            _context = context;
            _world = context.World;
            _fov = fov;
        }

        public void Update() {
            if (_world.IsMapGenRequested) {
                _world.IsMapGenRequested = false;
                _world.IsMapGenInProgress = true;
                GenerateMap();
            }

            if (_world.IsEntityGenRequested) {
                CreatePlayer();
                CreateMonsters();
                CreateLoot();
                _world.IsEntityGenRequested = false;
                _fov.Recompute(_context, _world.Player.Position);
            }

            //_ui.Refresh();
        }

        private void GenerateMap() {
            var mapGen = new DungeonGenerator();
            mapGen.Generate(_world.Map);
            _world.IsMapGenInProgress = false;
            _world.IsEntityGenRequested = true;
        }

        private void CreatePlayer() {
            var player = new Player(new Color(20, 255, 255), Color.Transparent);

            if (_world.Map.Rooms.Count > 0) {
                int index = _rng.Next(_world.Map.Rooms.Count);
                player.Position = _world.Map.Rooms[index].Center;
            }
            else {
                player.Position = new Point(10, 10);
            }

            _world.Player = player;
            _world.Map.Add(player);
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
                pos = _rng.Next(0, _world.Map.Width * _world.Map.Height);
                valid = _world.Map.Tiles[pos].IsWalkable;
            }
            while (!valid);

            entity.Position = new Point(pos % _world.Map.Width, pos / _world.Map.Width);
            _world.Map.Entities.Add(entity, new Coord(entity.Position.X, entity.Position.Y));
        }
    }
}
