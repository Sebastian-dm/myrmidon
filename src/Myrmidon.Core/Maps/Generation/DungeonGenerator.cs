
using System.Collections.Generic;
using System.Threading;
using System.Reflection;
using System.Linq;

using Myrmidon.Core.Maps.Tiles;
using Myrmidon.Core.Utilities.Geometry;
using Myrmidon.Core.Utilities.Random;

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
///    and mazes. We walk the map and find every tile that can be a
///    "connector". This is a wall tile that is adjacent to two unconnected
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

namespace Myrmidon.Core.Maps.Generation {

    public class DungeonGenerator {

        private TileMap _map;
        private int _maxRooms = 1000; // Number of attempts to create rooms
        private int _extraConnectorChance = 30; // Inverse chance of adding connector between two already joined regions.
        private int _roomExtraSize = 5; // Increasing this allows rooms to be larger.
        private int _windingPercent = 0;
        private int _currentRegion = -1; // Index of current region being carved.
        private int[] _regions; // For each open position in the dungeon, the index of the connected region that that position is a part of.
        private RandomNumberGenerator rng = new ();

        private readonly int _tileStepWaitMs = 0;


        private readonly List<Vector> CardinalDirections = new List<Vector>() {
            new Vector(0,1), new Vector(0, -1), new Vector(1, 0), new Vector(-1, 0)
        };


        public TileMap Generate(TileMap map) {
            _map = map;

            _regions = Enumerable.Repeat(-1, _map.Width * _map.Height).ToArray();



            // Check if map is odd-sized
            if (_map.Width % 2 == 0 || _map.Height % 2 == 0) {
                throw new ArgumentException("The map must be odd-sized.");
            }


            FillWithWalls();
            AddRooms();
            FillSpacesWithMazes();
            ConnectRegions();
            RemoveDeadEnds();
            RefineWallGlyphs();
            //_map.Rooms.ForEach(onDecorateRoom);

            return _map;
        }



        private void onDecorateRoom(Rect room) { }


        public void RefineWallGlyphs() {
            for (int y = 0; y < _map.Height; y++) {
                for (int x = 0; x < _map.Width; x++) {
                    Tile tileAtLocation = _map.GetTileAt<Tile>(x,y);
                    if (tileAtLocation is TileWall) {
                        Tile[] neighborTiles = _map.GetAdjacentTiles<Tile>(x,y);
                        ((TileWall)tileAtLocation).RefineTileGlyph(neighborTiles);
                    }
                }
            }
        }


        public void FillWithWalls() {
            for (int i = 0; i < _map.Tiles.Length; i++) {
                _map.Tiles[i] = new TileWall();
            }
        }


        /// <summary>
        /// Places rooms ignoring the existing maze corridors.
        /// </summary>
        private void AddRooms() {
            for (int i = 0; i < _maxRooms; i++) {
                // Pick a random room size. The funny math here does two things:
                // - It makes sure rooms are odd-sized to line up with maze.
                // - It avoids creating rooms that are too rectangular: too tall and
                //   narrow or too wide and flat.
                // TODO: This isn't very flexible or tunable. Do something better here.
                int size = rng.Range(1, 3 + _roomExtraSize) * 2 + 1;
                int rectangularity = rng.Range(0, 1 + size / 2) * 2;
                int width = size;
                int height = size;
                if (rng.OneIn(2)) {
                    width += rectangularity;
                }
                else {
                    height += rectangularity;
                }

                int x = rng.Range((_map.Width - width) / 2) * 2 + 1;
                int y = rng.Range((_map.Height - height) / 2) * 2 + 1;

                Rect Room = new Rect(x, y, width, height);

                bool overlaps = false;
                for (int r = 0; r < _map.Rooms.Count; r++) {
                    Rect other = _map.Rooms[r];
                    int dist = Room.DistanceTo(other);
                    if (dist <= 0) {
                        overlaps = true;
                        break;
                    }
                }

                if (overlaps) continue;

                _map.Rooms.Add(Room);

                StartRegion();
                foreach (Vector pos in new Rect(x, y, width, height)) {
                    Carve(pos);
                }

                Thread.Sleep(_tileStepWaitMs);
            }
        }


        private void FillSpacesWithMazes() {
            // Fill in all of the empty space with mazes.
            for (int y = 1; y < _map.Height; y += 2) {
                for (int x = 1; x < _map.Width; x += 2) {
                    Vector location = new Vector(x, y);
                    if (_map.GetTileAt<TileWall>(location) != null) {
                        GrowMaze(location);
                    }
                }
            }
        }


        /// Implementation of the "growing tree" algorithm from here:
        /// http://www.astrolog.org/labyrnth/algrithm.htm.
        private void GrowMaze(Vector start) {
            var cells = new List<Vector>();
            var lastDir = new Vector(-1, -1);

            StartRegion();
            Carve(start);

            cells.Add(start);
            while (cells.Count > 0) {
                Vector cell = cells.Last();

                // See which adjacent cells are open.
                List<Vector> unmadeCells = new List<Vector>();
                foreach (Vector dir in CardinalDirections) {
                    if (CanCarve(cell, dir)) unmadeCells.Add(dir);
                }

                if (unmadeCells.Count > 0) {
                    // Based on how "windy" passages are, try to prefer carving in the same direction.
                    Vector dir;
                    if (unmadeCells.Contains(lastDir) && rng.Next(100) > _windingPercent) {
                        dir = lastDir;
                    }
                    else {
                        dir = rng.Item(unmadeCells);
                    }

                    Carve(cell + dir);
                    Carve(cell + dir * 2);

                    cells.Add(cell + dir * 2);
                    lastDir = dir;
                }
                else {
                    // No adjacent uncarved cells.
                    cells.RemoveAt(cells.Count - 1);

                    // This path has ended.
                    lastDir = new Vector(-1, -1); ;
                }

                Thread.Sleep(_tileStepWaitMs/5);
            }
        }



        private void ConnectRegions() {

            // Find all connectors (tiles that can connect two or more regions).
            Dictionary<Vector, HashSet<int>> ConnectorRegions = new Dictionary<Vector, HashSet<int>>();

            for (int x = 1; x < _map.Width - 1; x++) {
                for (int y = 1; y < _map.Height - 1; y++) {
                    Vector pos = new Vector(x, y);

                    // Must be a wall tile to be a connector
                    if (_map.GetTileAt<TileWall>(pos) == null) continue;

                    // Can't already be part of a region.
                    HashSet<int> AdjacentRegions = new HashSet<int>();
                    foreach (Vector dir in CardinalDirections) { // NOTE: I don't know if this check is correct. Does it check element guid or value?
                        Vector PosAdjacent = pos + dir;
                        int PosAdjacentIndex = PosAdjacent.Y * _map.Width + PosAdjacent.X;
                        int RegionInDirection = _regions[PosAdjacentIndex];
                        if (RegionInDirection != -1) AdjacentRegions.Add(RegionInDirection);
                    }

                    if (AdjacentRegions.Count < 2) continue;
                    ConnectorRegions[pos] = AdjacentRegions;
                }
            }

            List<Vector> PossibleConnectors = ConnectorRegions.Keys.ToList();


            // Keep track of which regions have been merged. This maps an original
            // region index to the one it has been merged to.
            var Merged = new Dictionary<int, int>();
            var OpenRegions = new HashSet<int>();
            for (var i = 0; i <= _currentRegion; i++) {
                Merged[i] = i;
                OpenRegions.Add(i);
            }

            // Keep connecting regions until we're down to one.
            while (OpenRegions.Count > 1) {
                // Pick a connector
                Vector PickedConnector = rng.Item(PossibleConnectors);

                // Carve the connection.
                LinkRegions(PickedConnector);

                // Merge the connected regions. We'll pick one region (arbitrarily) and
                // map all of the other regions to its index.
                var RegionsToMerge = ConnectorRegions[PickedConnector].Select(r => Merged[r]);
                int DestinationRegion = RegionsToMerge.First();
                List<int> SourceRegions = RegionsToMerge.Skip(1).ToList();

                // Merge all of the affected regions. We have to look at *all* of the
                // regions because other regions may have previously been merged with
                // some of the ones we're merging now.
                for (var i = 0; i <= _currentRegion; i++) {
                    if (SourceRegions.Contains(Merged[i])) {
                        Merged[i] = DestinationRegion;
                    }
                }

                // Remove merged regions
                OpenRegions.RemoveWhere(r => SourceRegions.Contains(r));

                // Remove connectors from merged regions
                PossibleConnectors.RemoveAll(c => {
                    // Remove it right next to picked connector.
                    double ConnectorDistance = (PickedConnector - c).Length;
                    if (ConnectorDistance < 2) return true;

                    // If the connector connects unmerged regions, keep it.
                    List<int> Regions = ConnectorRegions[c].Select(r => Merged[r]).Distinct().ToList();
                    if (Regions.Count > 1) return false;

                    // This connecter isn't needed, but connect it occasionally so that the dungeon isn't singly-connected.
                    if (rng.OneIn(_extraConnectorChance)) LinkRegions(c);

                    return true;
                });

                Thread.Sleep(_tileStepWaitMs);
            }
        }

        private void LinkRegions(Vector pos) {
            if (rng.OneIn(4)) {
                _map[pos] = rng.OneIn(3) ? new TileDoor(locked: false, open: true) : new TileFloor();
            }
            else {
                _map[pos] = new TileDoor(locked: false, open: false);
            }
        }

        private void RemoveDeadEnds() {
            var done = false;

            while (!done) {
                done = true;
                
                for (int x = 1; x < _map.Width-1; x++) {
                    for (int y = 1; y < _map.Height - 1; y++) {
                        Vector pos = new Vector(x, y);
                        if (_map.GetTileAt<TileWall>(pos) != null) continue;

                        // If it only has one exit, it's a dead end.
                        var exits = 0;
                        foreach (var dir in CardinalDirections) {
                            if (_map.GetTileAt<TileWall>(pos + dir) == null) exits++;
                        }

                        if (exits != 1) continue;

                        done = false;
                        _map[pos] = new TileWall();

                        Thread.Sleep(_tileStepWaitMs / 5);
                    }
                }
            }
        }

        /// Gets whether or not an opening can be carved from the given starting
        /// [Cell] at [pos] to the adjacent Cell facing [direction]. Returns `true`
        /// if the starting Cell is in bounds and the destination Cell is filled
        /// (or out of bounds).</returns>
        private bool CanCarve(Vector pos, Vector direction) {
            // Must end in bounds.
            Vector NextCell = pos + direction * 3;
            bool InsideMapBoundary = _map.Bounds.DoesContain(NextCell);
            if (!InsideMapBoundary) return false;

            // Destination must not be open.
            Vector destination = pos + direction * 2;
            return _map.GetTileAt<TileWall>(destination) != null;
        }

        private void StartRegion() {
            _currentRegion++;
        }

        private void Carve(Vector location) {
            _map[location] = new TileFloor();
            int locationIndex = location.Y * _map.Width + location.X;
            _regions[locationIndex] = _currentRegion;
        }
    }
}
