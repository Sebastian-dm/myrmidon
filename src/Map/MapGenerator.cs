﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using myrmidon.Tiles;
using myrmidon.Geometry;

namespace myrmidon.Map {
    // based on tunnelling room generation algorithm from RogueSharp tutorial
    // https://roguesharp.wordpress.com/2016/03/26/roguesharp-v3-tutorial-simple-room-generation/
    public class MapGenerator {

        Random RandNumGenerator = new Random();
        Map _workingStage;



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
        public Map GenerateMap(int mapWidth, int mapHeight, int maxRooms, int minRoomSize, int maxRoomSize) {

            _workingStage = new Map(mapWidth, mapHeight);

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


        // Carve a tunnel out of the map parallel to the x-axis
        private void CreateHorizontalTunnel(int xStart, int xEnd, int yPosition) {
            for (int x = Math.Min(xStart, xEnd); x <= Math.Max(xStart, xEnd); x++) {
                CreateTunnel(new Vector(x, yPosition));
            }
        }


        // Carve a tunnel using the y-axis
        private void CreateVerticalTunnel(int yStart, int yEnd, int xPosition) {
            for (int y = Math.Min(yStart, yEnd); y <= Math.Max(yStart, yEnd); y++) {
                CreateTunnel(new Vector(xPosition, y));
            }
        }


        // Builds a room composed of walls and floors using the supplied Rectangle which determines its size and position on the map
        // Walls are placed at the perimeter of the room Floors are placed in the interior area of the room
        private void CreateRoom(Rect room) {
            List<Vector> RoomPerimeter = GetPerimeter(room);

            // Fill with floor
            for (int x = room.Left + 1; x < room.Right; x++) {
                for (int y = room.Top + 1; y < room.Bottom; y++) {
                    CreateFloor(new Vector(x, y));
                }
            }

            foreach (Vector location in RoomPerimeter) {
                if (_workingStage.GetTileAt<TileBase>(location) is not TileTunnel) {
                    CreateWall(location);
                }
            }

            foreach (Vector location in RoomPerimeter) {
                if (IsDoorCandidate(location)) CreateDoor(location);
            }
        }


        // Creates a specific tile at a specified location.
        private void CreateFloor(Vector location) {
            _workingStage.Tiles[location.ToIndex(_workingStage.Width)] = new TileFloor();
        }
        private void CreateTunnel(Vector location) {
            _workingStage.Tiles[location.ToIndex(_workingStage.Width)] = new TileTunnel();
        }
        private void CreateWall(Vector location) {
            _workingStage.Tiles[location.ToIndex(_workingStage.Width)] = new TileWall();
        }
        private void CreateDoor(Vector location) {
            _workingStage.Tiles[location.ToIndex(_workingStage.Width)] = new TileDoor(false, false);
        }


        // Returns a list of points expressing the perimeter of a rectangle
        private List<Vector> GetPerimeter(Rect room) {
            //establish room boundaries
            int xMin = room.Left;
            int xMax = room.Right;
            int yMin = room.Top;
            int yMax = room.Bottom;

            // build a list of room border cells using a series of straight lines
            List<Vector> Perimeter = GetTileLocationsAlongLine(xMin, yMin, xMax, yMin).ToList();
            Perimeter.AddRange(GetTileLocationsAlongLine(xMin, yMin, xMin, yMax));
            Perimeter.AddRange(GetTileLocationsAlongLine(xMin, yMax, xMax, yMax));
            Perimeter.AddRange(GetTileLocationsAlongLine(xMax, yMin, xMax, yMax));

            return Perimeter;
        }


        // Returns a collection of Points which represent locations along a line
        public IEnumerable<Vector> GetTileLocationsAlongLine(int xOrigin, int yOrigin, int xDestination, int yDestination) {
            // prevent line from overflowing boundaries of the map
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
                yield return new Vector(xOrigin, yOrigin);
                if (xOrigin == xDestination && yOrigin == yDestination) {
                    break;
                }
                int e2 = 2 * err;
                if (e2 > -dy) {
                    err -= dy;
                    xOrigin += sx;
                }
                if (e2 < dx) {
                    err += dx;
                    yOrigin += sy;
                }
            }
        }


        // Determines if a Point on the map is a good candidate for a door.
        private bool IsDoorCandidate(Vector location) {

            // Is tile walkable?
            int locationIndex = location.ToIndex(_workingStage.Width);
            if (_workingStage.Tiles[locationIndex] != null && _workingStage.Tiles[locationIndex] is TileWall) {
                return false;
            }

            // Is there no door here or adjacent already?
            Vector right = new Vector(location.X + 1, location.Y);
            Vector left = new Vector(location.X - 1, location.Y);
            Vector top = new Vector(location.X, location.Y - 1);
            Vector bottom = new Vector(location.X, location.Y + 1);
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


        // Sets X coordinate between right and left edges of map to prevent any out-of-bounds errors
        private int ClampX(int x) {
            return x < 0 ? 0 : x > _workingStage.Width - 1 ? _workingStage.Width - 1 : x;
        }


        // Sets Y coordinate between top and bottom edges of map to prevent any out-of-bounds errors
        private int ClampY(int y) {
            return y < 0 ? 0 : y > _workingStage.Height - 1 ? _workingStage.Height - 1 : y;
        }
    }
}