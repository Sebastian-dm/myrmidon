using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace clodd {
    // based on tunnelling room generation algorithm from RogueSharp tutorial
    // https://roguesharp.wordpress.com/2016/03/26/roguesharp-v3-tutorial-simple-room-generation/
    public class MapGenerator {
        // empty constructor
        public MapGenerator() {
        }

        Map _map; // Temporarily store the map currently worked on

        public Map GenerateMap(int mapWidth, int mapHeight, int maxRooms, int minRoomSize, int maxRoomSize) {

            
            Random randNum = new Random();

            _map = new Map(mapWidth, mapHeight);
            List<Rectangle> Rooms = new List<Rectangle>();

            // create up to maxRooms non-overlapping rooms
            for (int i = 0; i < maxRooms; i++) {
                // set the room's (width, height) as a random size between (minRoomSize, maxRoomSize)
                int newRoomWidth = randNum.Next(minRoomSize, maxRoomSize);
                int newRoomHeight = randNum.Next(minRoomSize, maxRoomSize);

                // sets the room's X/Y Position at a random point between the edges of the map
                int newRoomX = randNum.Next(0, mapWidth - newRoomWidth - 1);
                int newRoomY = randNum.Next(0, mapHeight - newRoomHeight - 1);

                // create a Rectangle representing the room's perimeter
                Rectangle newRoom = new Rectangle(newRoomX, newRoomY, newRoomWidth, newRoomHeight);

                // Does the new room intersect with other rooms already generated?
                bool newRoomIntersects = Rooms.Any(room => newRoom.Intersects(room));

                if (!newRoomIntersects) {
                    Rooms.Add(newRoom);
                }
            }

            // Fill the map with walls
            FloodWalls();

            // Carve out rooms for every room in the Rooms list
            foreach (Rectangle room in Rooms) {
                CreateRoom(room);
            }

            // carve out tunnels between all rooms based on the Positions of their centers
            for (int r = 1; r < Rooms.Count; r++) {
                //for all remaining rooms get the center of the room and the previous room
                Point previousRoomCenter = Rooms[r - 1].Center;
                Point currentRoomCenter = Rooms[r].Center;

                // give a 50/50 chance of which 'L' shaped connecting hallway to tunnel out
                if (randNum.Next(1, 2) == 1) {
                    CreateHorizontalTunnel(previousRoomCenter.X, currentRoomCenter.X, previousRoomCenter.Y);
                    CreateVerticalTunnel(previousRoomCenter.Y, currentRoomCenter.Y, currentRoomCenter.X);
                }
                else {
                    CreateVerticalTunnel(previousRoomCenter.Y, currentRoomCenter.Y, previousRoomCenter.X);
                    CreateHorizontalTunnel(previousRoomCenter.X, currentRoomCenter.X, currentRoomCenter.Y);
                }
            }

            // spit out the final map
            return _map;
        }

        /// <summary>
        /// carve a tunnel out of the map parallel to the x-axis
        /// </summary>
        /// <param name="xStart"></param>
        /// <param name="xEnd"></param>
        /// <param name="yPosition"></param>
        private void CreateHorizontalTunnel(int xStart, int xEnd, int yPosition) {
            for (int x = Math.Min(xStart, xEnd); x <= Math.Max(xStart, xEnd); x++) {
                CreateFloor(new Point(x, yPosition));
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
                CreateFloor(new Point(xPosition, y));
            }
        }

        // Builds a room composed of walls and floors using the supplied Rectangle
        // which determines its size and position on the map
        // Walls are placed at the perimeter of the room
        // Floors are placed in the interior area of the room
        private void CreateRoom(Rectangle room) {
            // Place floors in interior area
            for (int x = room.Left + 1; x < room.Right - 1; x++) {
                for (int y = room.Top + 1; y < room.Bottom - 1; y++) {
                    CreateFloor(new Point(x, y));
                }
            }

            // Place walls at perimeter
            List<Point> perimeter = GetBorderCellLocations(room);
            foreach (Point location in perimeter) {
                CreateWall(location);
            }
        }

        /// <summary>
        /// Creates a Floor tile at the specified X/Y location
        /// </summary>
        /// <param name="location"></param>
        private void CreateFloor(Point location) {
            _map.Tiles[location.ToIndex(_map.Width)] = new TileFloor();
        }

        /// <summary>
        /// Creates a Wall tile at the specified X/Y location
        /// </summary>
        /// <param name="location"></param>
        private void CreateWall(Point location) {
            _map.Tiles[location.ToIndex(_map.Width)] = new TileWall();
        }

        /// <summary>
        /// Fills the map with walls
        /// </summary>
        private void FloodWalls() {
            for (int i = 0; i < _map.Tiles.Length; i++) {
                _map.Tiles[i] = new TileWall();
            }
        }

        /// <summary>
        /// Returns a list of points expressing the perimeter of a rectangle
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        private List<Point> GetBorderCellLocations(Rectangle room) {
            //establish room boundaries
            int xMin = room.Left;
            int xMax = room.Right;
            int yMin = room.Top;
            int yMax = room.Bottom;

            // build a list of room border cells using a series of
            // straight lines
            List<Point> borderCells = GetTileLocationsAlongLine(xMin, yMin, xMax, yMin).ToList();
            borderCells.AddRange(GetTileLocationsAlongLine(xMin, yMin, xMin, yMax));
            borderCells.AddRange(GetTileLocationsAlongLine(xMin, yMax, xMax, yMax));
            borderCells.AddRange(GetTileLocationsAlongLine(xMax, yMin, xMax, yMax));

            return borderCells;
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
        /// Sets X coordinate between right and left edges of map to prevent any out-of-bounds errors
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <returns>Clamped X coordinate</returns>
        private int ClampX(int x) {
            return (x < 0) ? 0 : (x > _map.Width - 1) ? _map.Width - 1 : x;
        }

        /// <summary>
        /// Sets Y coordinate between top and bottom edges of map to prevent any out-of-bounds errors
        /// </summary>
        /// <param name="y">Y coordinate</param>
        /// <returns>Clamped Y coordinate</returns>
        private int ClampY(int y) {
            return (y < 0) ? 0 : (y > _map.Height - 1) ? _map.Height - 1 : y;
        }
    }
}