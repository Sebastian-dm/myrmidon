using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using clodd.Tiles;
using clodd.Geometry;

/// The random dungeon generator.
/// This dungeon generator is an implementation of the Hauberk dungeon Generator made by Robert Nystrom
/// https://github.com/munificent/hauberk/blob/db360d9efa714efb6d937c31953ef849c7394a39/lib/src/content/dungeon.dart
///
/// Starting with a stage of solid walls, it works like so:
///
/// 1. Place a number of randomly sized and positioned rooms. If a room
///    overlaps an existing room, it is discarded. Any remaining rooms are
///    carved out.
/// 2. Any remaining solid areas are filled in with mazes. The maze generator
///    will grow and fill in even odd-shaped areas, but will not touch any
///    rooms.
/// 3. The result of the previous two steps is a series of unconnected rooms
///    and mazes. We walk the stage and find every tile that can be a
///    "connector". This is a solid tile that is adjacent to two unconnected
///    regions.
/// 4. We randomly choose connectors and open them or place a door there until
///    all of the unconnected regions have been joined. There is also a slight
///    chance to carve a connector between two already-joined regions, so that
///    the dungeon isn't single connected.
/// 5. The mazes will have a lot of dead ends. Finally, we remove those by
///    repeatedly filling in any open tile that's closed on three sides. When
///    this is done, every corridor in a maze actually leads somewhere.
///
/// The end result of this is a multiply-connected dungeon with rooms and lots
/// of winding corridors.

namespace clodd {


    public class DungeonGenerator {

        Random rng = new Random();
        Map _map;


        private List<Vector> CardinalDirections = new List<Vector>() {
            new Vector(0,1), new Vector(0, -1), new Vector(1, 0), new Vector(-1, 0) };

        int numRoomTries;

        /// The inverse chance of adding a connector between two regions that have
        /// already been joined. Increasing this leads to more loosely connected
        /// dungeons.
        int extraConnectorChance = 20;

        /// Increasing this allows rooms to be larger.
        int roomExtraSize = 0;

        int windingPercent = 0;

        /// For each open position in the dungeon, the index of the connected region that that position is a part of.
        int[] _regions;

        /// The index of the current region being carved.
        int _currentRegion = -1;

        public Map GenerateMap(int mapWidth, int mapHeight, int maxRooms) {
            
            _map = new Map(mapWidth, mapHeight);
            numRoomTries = maxRooms;

            // Check if map is odd-sized
            if (_map.Width % 2 == 0 || _map.Height % 2 == 0) {
                throw new ArgumentException("The map must be odd-sized.");
            }

            // Fill map with walls
            for (int i = 0; i < _map.Tiles.Length; i++) {
                _map.Tiles[i] = new TileWall();
            }

            _regions = new int[mapWidth * mapHeight];

            _addRooms();

            // Fill in all of the empty space with mazes.
            for (var y = 1; y < _map.Height; y += 2) {
                for (var x = 1; x < _map.Width; x += 2) {
                    var pos = new Vector(x, y);
                    if (_map.GetTileAt<TileWall>(pos) != null) continue;
                    _growMaze(pos);
                }
            }

            //_connectRegions();
            _removeDeadEnds();

            _map.Rooms.ForEach(onDecorateRoom);

            return _map;
        }



        private void onDecorateRoom(Rect room) { }



        /// Implementation of the "growing tree" algorithm from here:
        /// http://www.astrolog.org/labyrnth/algrithm.htm.
        private void _growMaze(Vector start) {
            List<Vector> cells = new List<Vector>();
            Vector lastDir = new Vector(-1, -1);

            _startRegion();
            _carve(start);

            cells.Add(start);
            while (cells.Count > 0) {
                var cell = cells.Last();

                // See which adjacent cells are open.
                List<Vector> unmadeCells = new List<Vector>();

                foreach (var dir in CardinalDirections) {
                    if (_canCarve(cell, dir)) unmadeCells.Add(dir);
                }

                if (unmadeCells.Count > 0) {
                    // Based on how "windy" passages are, try to prefer carving in the same direction.
                    Vector dir;
                    if (unmadeCells.Contains(lastDir) && rng.Next(100) > windingPercent) {
                        dir = lastDir;
                    }
                    else {
                        dir = rng.Item(unmadeCells);
                    }

                    _carve(cell + dir);
                    _carve(cell + dir * 2);

                    cells.Add(cell + dir * 2);
                    lastDir = dir;
                }
                else {
                    // No adjacent uncarved cells.
                    cells.RemoveAt(cells.Count-1);

                    // This path has ended.
                    lastDir = new Vector(-1, -1); ;
                }
            }
        }



        /// Places rooms ignoring the existing maze corridors.
        private void _addRooms() {
            for (var i = 0; i < numRoomTries; i++) {
                // Pick a random room size. The funny math here does two things:
                // - It makes sure rooms are odd-sized to line up with maze.
                // - It avoids creating rooms that are too rectangular: too tall and
                //   narrow or too wide and flat.
                // TODO: This isn't very flexible or tunable. Do something better here.
                int size = rng.Range(1, 3 + roomExtraSize) * 2 + 1;
                int rectangularity = rng.Range(0, 1 + size / 2) * 2;
                int width = size;
                int height = size;
                if (rng.OneIn(2)) {
                    width += rectangularity;
                }
                else {
                    height += rectangularity;
                }

                var x = rng.Range((_map.Width - width) / 2) * 2 + 1;
                var y = rng.Range((_map.Height - height) / 2) * 2 + 1;

                var room = new Rect(x, y, width, height);

                var overlaps = false;
                foreach (var other in _map.Rooms) {
                    if (room.DistanceTo(other) <= 0) {
                        overlaps = true;
                        break;
                    }
                }

                if (overlaps) continue;

                _map.Rooms.Add(room);

                _startRegion();
                foreach (Vector pos in new Rect(x, y, width, height)) {
                    _carve(pos);
                }
                int tiles = _map.Tiles.Count();
            }
        }

        //private void _connectRegions() {
        //    // Find all of the tiles that can connect two (or more) regions.
        //    Dictionary<Vector, HashSet<int>> connectorRegions = new Dictionary<Vector, HashSet<int>>();
        //    foreach (var pos in _map.Bounds.Inflate(-1)) {
        //        // Can't already be part of a region.
        //        if (_map.GetTileAt<TileWall>(pos) == null) continue;

        //        HashSet<int> regions = new HashSet<int>();
        //        foreach (var dir in Direction.CARDINAL) {
        //            var region = _regions[pos + dir];
        //            if (region != null) regions.Add(region);
        //        }

        //        if (regions.Count < 2) continue;

        //        connectorRegions[pos] = regions;
        //    }

        //    var connectors = connectorRegions.Keys.ToList();

        //    // Keep track of which regions have been merged. This maps an original
        //    // region index to the one it has been merged to.
        //    var merged = { };
        //    var openRegions = new HashSet<int>();
        //    for (var i = 0; i <= _currentRegion; i++) {
        //        merged[i] = i;
        //        openRegions.Add(i);
        //    }

        //    // Keep connecting regions until we're down to one.
        //    while (openRegions.Count > 1) {
        //        var connector = rng.Item(connectors);

        //        // Carve the connection.
        //        _addJunction(connector);

        //        // Merge the connected regions. We'll pick one region (arbitrarily) and
        //        // map all of the other regions to its index.
        //        var regions = connectorRegions[connector]
        //            .map((region) => merged[region]);
        //        var dest = regions.first;
        //        var sources = regions.skip(1).toList();

        //        // Merge all of the affected regions. We have to look at *all* of the
        //        // regions because other regions may have previously been merged with
        //        // some of the ones we're merging now.
        //        for (var i = 0; i <= _currentRegion; i++) {
        //            if (sources.contains(merged[i])) {
        //                merged[i] = dest;
        //            }
        //        }

        //        // The sources are no longer in use.
        //        openRegions.removeAll(sources);

        //        // Remove any connectors that aren't needed anymore.
        //        connectors.removeWhere(
        //            (pos) {
        //            // Don't allow connectors right next to each other.
        //            if (connector - pos < 2) return true;

        //            // If the connector no long spans different regions, we don't need it.
        //            var regions = connectorRegions[pos].map((region) => merged[region]).toSet();

        //            if (regions.length > 1) return false;

        //            // This connecter isn't needed, but connect it occasionally so that the
        //            // dungeon isn't singly-connected.
        //            if (rng.OneIn(extraConnectorChance)) _addJunction(pos);

        //            return true;
        //            }
        //        );
        //    }
        //}

        private void _addJunction(Vector pos) {
            if (rng.OneIn(4)) {
                _map.SetTileAt(pos, rng.OneIn(3) ? new TileDoor(locked: false, open: true) : new TileFloor());
            }
            else {
                _map.SetTileAt(pos, new TileDoor(locked: false, open: false));
            }
        }

        private void _removeDeadEnds() {
            var done = false;

            while (!done) {
                done = true;

                foreach (var pos in _map.Bounds.Inflate(-1)) {
                    if (_map.GetTileAt<TileWall>(pos) != null) continue;

                    // If it only has one exit, it's a dead end.
                    var exits = 0;
                    foreach (var dir in CardinalDirections) {
                        if (_map.GetTileAt<TileWall>(pos + dir) == null) exits++;
                    }

                    if (exits != 1) continue;

                    done = false;
                    _map.SetTileAt(pos, new TileWall());
                }
            }
        }

        /// Gets whether or not an opening can be carved from the given starting
        /// [Cell] at [pos] to the adjacent Cell facing [direction]. Returns `true`
        /// if the starting Cell is in bounds and the destination Cell is filled
        /// (or out of bounds).</returns>
        private bool _canCarve(Vector pos, Vector direction) {
            // Must end in bounds.
            if (!_map.Bounds.Contains(pos + direction * 3)) return false;

            // Destination must not be open.
            Vector destination = pos + direction * 2;
            return _map.GetTileAt<TileWall>(destination) != null;
        }

        private void _startRegion() {
            _currentRegion++;
        }

        private void _carve(Vector location) {
            _map.Tiles[location.ToIndex(_map.Width)] = new TileFloor();
            _regions[location.ToIndex(_map.Width)] = _currentRegion;
        }
    }
}
