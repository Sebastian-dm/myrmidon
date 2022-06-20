using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using clodd.Tiles;
using clodd.Geometry;

namespace clodd.Map {
    // based on tunnelling room generation algorithm from RogueSharp tutorial
    // https://roguesharp.wordpress.com/2016/03/26/roguesharp-v3-tutorial-simple-room-generation/
    public class MapGenerator {

        Random RandNumGenerator = new Random();
        Stage _workingStage;



        /// <summary>
        /// Empty constructor.
        /// </summary>
        public MapGenerator() {
        }


        /// <summary>
        /// Generates a map.
        /// </summary>
        /// <param name="mapWidth"></param>
        /// <param name="mapHeight"></param>
        /// <param name="maxRooms"></param>
        /// <param name="minRoomSize"></param>
        /// <param name="maxRoomSize"></param>
        /// <returns></returns>
        public Stage GenerateMap(int mapWidth, int mapHeight, int maxRooms, int minRoomSize, int maxRoomSize) {

            _workingStage = new Stage(mapWidth, mapHeight);

            // Fill map with empty
            for (int i = 0; i < _workingStage.Tiles.Length; i++) {
                _workingStage.Tiles[i] = new TileEmpty();
            }

            // create up to maxRooms non-overlapping rooms
            for (int i = 0; i < maxRooms; i++) {
                // set the room's (width, height) as a random size between (minRoomSize, maxRoomSize)
                int newRoomWidth = RandNumGenerator.Next(minRoomSize, maxRoomSize);
                int newRoomHeight = RandNumGenerator.Next(minRoomSize, maxRoomSize);

                // sets the room's X/Y Position at a random point between the edges of the map
                int newRoomX = RandNumGenerator.Next(0, mapWidth - newRoomWidth - 1);
                int newRoomY = RandNumGenerator.Next(0, mapHeight - newRoomHeight - 1);

                // create a Rectangle representing the room's perimeter
                Rect newRoom = new Rect(newRoomX, newRoomY, newRoomWidth, newRoomHeight);

                // Does the new room intersect with other rooms already generated?
                bool newRoomIntersects = _workingStage.Rooms.Any(room => newRoom.Intersects(room));

                if (!newRoomIntersects) {
                    _workingStage.Rooms.Add(newRoom);
                }
            }


            // carve out tunnels between all rooms based on the Positions of their centers
            for (int r = 1; r < _workingStage.Rooms.Count; r++) {
                //for all remaining rooms get the center of the room and the previous room
                Point previousRoomCenter = _workingStage.Rooms[r - 1].Center;
                Point currentRoomCenter = _workingStage.Rooms[r].Center;

                // give a 50/50 chance of which 'L' shaped connecting hallway to tunnel out
                if (RandNumGenerator.Next(1, 2) == 1) {
                    CreateHorizontalTunnel(previousRoomCenter.X, currentRoomCenter.X, previousRoomCenter.Y);
                    CreateVerticalTunnel(previousRoomCenter.Y, currentRoomCenter.Y, currentRoomCenter.X);
                }
                else {
                    CreateVerticalTunnel(previousRoomCenter.Y, currentRoomCenter.Y, previousRoomCenter.X);
                    CreateHorizontalTunnel(previousRoomCenter.X, currentRoomCenter.X, currentRoomCenter.Y);
                }
            }

            // Carve out rooms for every room in the Rooms list
            foreach (Rect room in _workingStage.Rooms) {
                CreateRoom(room);
            }

            // spit out the final map
            return _workingStage;
        }










        /// <summary>
        /// carve a tunnel out of the map parallel to the x-axis
        /// </summary>
        /// <param name="xStart"></param>
        /// <param name="xEnd"></param>
        /// <param name="yPosition"></param>
        private void CreateHorizontalTunnel(int xStart, int xEnd, int yPosition) {
            for (int x = Math.Min(xStart, xEnd); x <= Math.Max(xStart, xEnd); x++) {
                CreateTunnel(new Point(x, yPosition));
            }
        }


        /// <summary>
        /// carve a tunnel using the y-axis
        /// </summary>
        /// <param name="yStart"></param>
        /// <param name="yEnd"></param>
        /// <param name="xPosition"></param>
        private void CreateVerticalTunnel(int yStart, int yEnd, int xPosition) {
            for (int y = Math.Min(yStart, yEnd); y <= Math.Max(yStart, yEnd); y++) {
                CreateTunnel(new Point(xPosition, y));
            }
        }


        /// <summary>
        /// Builds a room composed of walls and floors using the supplied Rectangle which determines its size and position on the map
        /// Walls are placed at the perimeter of the room Floors are placed in the interior area of the room
        /// </summary>
        /// <param name="room"></param>
        private void CreateRoom(Rect room) {
            List<Point> RoomPerimeter = GetPerimeter(room);

            // Fill with floor
            for (int x = room.Left + 1; x < room.Right; x++) {
                for (int y = room.Top + 1; y < room.Bottom; y++) {
                    CreateFloor(new Point(x, y));
                }
            }

            foreach (Point location in RoomPerimeter) {
                if (_workingStage.GetTileAt<TileBase>(location) is not TileTunnel) {
                    CreateWall(location);
                }
            }

            foreach (Point location in RoomPerimeter) {
                if (IsPotentialDoor(location)) CreateDoor(location);
            }
        }


        /// <summary>
        /// Creates a Floor tile at the specified X/Y location
        /// </summary>
        /// <param name="location"></param>
        private void CreateFloor(Point location) {
            _workingStage.Tiles[location.ToIndex(_workingStage.Width)] = new TileFloor();
        }

        /// <summary>
        /// Creates a tunnel tile at the specified X/Y location
        /// </summary>
        /// <param name="location"></param>
        private void CreateTunnel(Point location) {
            _workingStage.Tiles[location.ToIndex(_workingStage.Width)] = new TileTunnel();
        }


        /// <summary>
        /// Creates a Wall tile at the specified X/Y location
        /// </summary>
        /// <param name="location"></param>
        private void CreateWall(Point location) {
            _workingStage.Tiles[location.ToIndex(_workingStage.Width)] = new TileWall();
        }


        /// <summary>
        /// Creates a Door tile at the specified X/Y location
        /// </summary>
        /// <param name="location"></param>
        private void CreateDoor(Point location) {
            _workingStage.Tiles[location.ToIndex(_workingStage.Width)] = new TileDoor(false, false);
        }


        /// <summary>
        /// Returns a list of points expressing the perimeter of a rectangle
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        private List<Point> GetPerimeter(Rect room) {
            //establish room boundaries
            int xMin = room.Left;
            int xMax = room.Right;
            int yMin = room.Top;
            int yMax = room.Bottom;

            // build a list of room border cells using a series of
            // straight lines
            List<Point> Perimeter = GetTileLocationsAlongLine(xMin, yMin, xMax, yMin).ToList();
            Perimeter.AddRange(GetTileLocationsAlongLine(xMin, yMin, xMin, yMax));
            Perimeter.AddRange(GetTileLocationsAlongLine(xMin, yMax, xMax, yMax));
            Perimeter.AddRange(GetTileLocationsAlongLine(xMax, yMin, xMax, yMax));

            return Perimeter;
        }


        /// <summary>
        /// Returns a collection of Points which represent locations along a line
        /// </summary>
        /// <param name="xOrigin"></param>
        /// <param name="yOrigin"></param>
        /// <param name="xDestination"></param>
        /// <param name="yDestination"></param>
        /// <returns></returns>
        public IEnumerable<Point> GetTileLocationsAlongLine(int xOrigin, int yOrigin, int xDestination, int yDestination) {
            // prevent line from overflowing
            // boundaries of the map
            xOrigin = ClampX(xOrigin);
            yOrigin = ClampY(yOrigin);
            xDestination = ClampX(xDestination);
            yDestination = ClampY(yDestination);

            int dx = Math.Abs(xDestination - xOrigin);
            int dy = Math.Abs(yDestination - yOrigin);

            int sx = xOrigin < xDestination ? 1 : -1;
            int sy = yOrigin < yDestination ? 1 : -1;
            int err = dx - dy;

            while (true) {

                yield return new Point(xOrigin, yOrigin);
                if (xOrigin == xDestination && yOrigin == yDestination) {
                    break;
                }
                int e2 = 2 * err;
                if (e2 > -dy) {
                    err = err - dy;
                    xOrigin = xOrigin + sx;
                }
                if (e2 < dx) {
                    err = err + dx;
                    yOrigin = yOrigin + sy;
                }
            }
        }


        /// <summary>
        /// Determines if a Point on the map is a good candidate for a door.
        /// </summary>
        /// <param name="location"></param>
        /// <returns>True if location is good for a door.</returns>
        private bool IsPotentialDoor(Point location) {

            // Is tile walkable?
            int locationIndex = location.ToIndex(_workingStage.Width);
            if (_workingStage.Tiles[locationIndex] != null && _workingStage.Tiles[locationIndex] is TileWall) {
                return false;
            }

            // Is there no door here or adjacent already?
            Point right = new Point(location.X + 1, location.Y);
            Point left = new Point(location.X - 1, location.Y);
            Point top = new Point(location.X, location.Y - 1);
            Point bottom = new Point(location.X, location.Y + 1);
            if (_workingStage.GetTileAt<TileDoor>(location.X, location.Y) != null ||
                _workingStage.GetTileAt<TileDoor>(right.X, right.Y) != null ||
                _workingStage.GetTileAt<TileDoor>(left.X, left.Y) != null ||
                _workingStage.GetTileAt<TileDoor>(top.X, top.Y) != null ||
                _workingStage.GetTileAt<TileDoor>(bottom.X, bottom.Y) != null
               ) {
                return false;
            }

            // Is tile placed in a horizonral wall?
            if (!_workingStage.Tiles[right.ToIndex(_workingStage.Width)].IsBlockingMove
                && !_workingStage.Tiles[left.ToIndex(_workingStage.Width)].IsBlockingMove
                && _workingStage.Tiles[top.ToIndex(_workingStage.Width)].IsBlockingMove
                && _workingStage.Tiles[bottom.ToIndex(_workingStage.Width)].IsBlockingMove) {
                return true;
            }
            // Is tile placed in a vertical wall?
            if (_workingStage.Tiles[right.ToIndex(_workingStage.Width)].IsBlockingMove
                && _workingStage.Tiles[left.ToIndex(_workingStage.Width)].IsBlockingMove
                && !_workingStage.Tiles[top.ToIndex(_workingStage.Width)].IsBlockingMove
                && !_workingStage.Tiles[bottom.ToIndex(_workingStage.Width)].IsBlockingMove) {
                return true;
            }
            return false;
        }


        /// <summary>
        /// Sets X coordinate between right and left edges of map to prevent any out-of-bounds errors
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <returns>Clamped X coordinate</returns>
        private int ClampX(int x) {
            return x < 0 ? 0 : x > _workingStage.Width - 1 ? _workingStage.Width - 1 : x;
        }


        /// <summary>
        /// Sets Y coordinate between top and bottom edges of map to prevent any out-of-bounds errors
        /// </summary>
        /// <param name="y">Y coordinate</param>
        /// <returns>Clamped Y coordinate</returns>
        private int ClampY(int y) {
            return y < 0 ? 0 : y > _workingStage.Height - 1 ? _workingStage.Height - 1 : y;
        }
    }
}