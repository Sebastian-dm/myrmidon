using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace myrmidon.Tiles {
    // TileWall is based on TileBase
    public class TileWall : Tile {

        private enum TileIndices {
            None = 272,
            North = 273,
            East = 274,
            South = 275,
            West = 276,
            NorthSouth = 277,
            EastWest = 278,
            NorthWest = 279,
            NorthEast = 280,
            SouthEast = 281,
            SouthWest = 282,
            NoSouth = 283,
            NoWest = 284,
            NoNorth = 285,
            NoEast = 286,
            All = 287,
            Unknown = 176,
        }

        [Flags]
        private enum dir {
            N = 1,
            E = 2,
            S = 4,
            W = 8,
        }

        [Flags]
        private enum dir8 {
            N = 1,
            E = 2,
            S = 4,
            W = 8,
            NW = 16,
            NE = 32,
            SW = 64,
            SE = 128,
        }

        // Walls are set to block movement and line of sight by default
        // and have a light gray foreground and a transparent background
        // represented by the # symbol
        public TileWall(bool walkable = false, bool blocksLOS = true) : base(new Color(68, 65, 59), new Color(38, 36, 34), glyph:272, walkable, blocksLOS) {
            Name = "Wall";
        }


        // 0 1 2
        // 7 x 3 This is the indices that the neighbor tiles must come in
        // 6 5 4
        public void RefineTileGlyph(Tile[] neighbors) {
            if (neighbors.Length != 8) {
                throw new ArgumentException("All 8 neighbors must be given in the clockwise order: " +
                    "[NW N NE E SE S SW W]");
            }

            byte n = 0;
            if (neighbors[1] is TileWall) n += (byte)dir.N;
            if (neighbors[3] is TileWall) n += (byte)dir.E;
            if (neighbors[5] is TileWall) n += (byte)dir.S;
            if (neighbors[7] is TileWall) n += (byte)dir.W;

            switch (n) {
                case (0b_1111):
                    _glyphWhenExplored = (int)TileIndices.None;
                    break;
                
                // 1 wall single side
                case (0b_1110):
                    _glyphWhenExplored = (int)TileIndices.North;
                    break;
                case (0b_1101):
                    _glyphWhenExplored = (int)TileIndices.East;
                    break;
                case (0b_1011):
                    _glyphWhenExplored = (int)TileIndices.South;
                    break;
                case (0b_0111):
                    _glyphWhenExplored = (int)TileIndices.West;
                    break;

                // 2 walls opposite sides
                case (0b_1010):
                    _glyphWhenExplored = (int)TileIndices.NorthSouth;
                    break;
                case (0b_0101):
                    _glyphWhenExplored = (int)TileIndices.EastWest;
                    break;

                // 2 walls corner
                case (0b_0001):
                    _glyphWhenExplored = (int)TileIndices.NoNorth;
                    break;
                case (0b_0010):
                    _glyphWhenExplored = (int)TileIndices.NoEast;
                    break;
                case (0b_0100):
                    _glyphWhenExplored = (int)TileIndices.NoSouth;
                    break;
                case (0b_1000):
                    _glyphWhenExplored = (int)TileIndices.NoWest;
                    break;

                // 3 walls end
                case (0b_1100):
                    _glyphWhenExplored = (int)TileIndices.NorthEast;
                    break;
                case (0b_1001):
                    _glyphWhenExplored = (int)TileIndices.SouthEast;
                    break;
                case (0b_0011):
                    _glyphWhenExplored = (int)TileIndices.SouthWest;
                    break;
                case (0b_0110):
                    _glyphWhenExplored = (int)TileIndices.NorthWest;
                    break;

                // 4 full walls all sides
                case (0b_0000):
                    _glyphWhenExplored = (int)TileIndices.All;
                    break;
                
                // Unfinished
                default:
                    _glyphWhenExplored = (int)TileIndices.Unknown;
                    break;
            }
            string gl = ((TileIndices)_glyphWhenExplored).ToString();
        }
        

    }
}
