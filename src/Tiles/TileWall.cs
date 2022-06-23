using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace clodd.Tiles {
    // TileWall is based on TileBase
    public class TileWall : TileBase {

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
        public TileWall(bool blocksMovement = true, bool blocksLOS = true) : base(new Color(68, 65, 59), new Color(38, 36, 34), glyph:272, blocksMovement, blocksLOS) {
            Name = "Wall";
        }


        // 0 1 2
        // 7 x 3 This is the indices that the neighbor tiles must come in
        // 6 5 4
        public void RefineTileGlyph(TileBase[] neighbors) {
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
                    Glyph = (int)TileIndices.None;
                    break;
                
                // 1 wall single side
                case (0b_1110):
                    Glyph = (int)TileIndices.North;
                    break;
                case (0b_1101):
                    Glyph = (int)TileIndices.East;
                    break;
                case (0b_1011):
                    Glyph = (int)TileIndices.South;
                    break;
                case (0b_0111):
                    Glyph = (int)TileIndices.West;
                    break;

                // 2 walls opposite sides
                case (0b_1010):
                    Glyph = (int)TileIndices.NorthSouth;
                    break;
                case (0b_0101):
                    Glyph = (int)TileIndices.EastWest;
                    break;

                // 2 walls corner
                case (0b_0001):
                    Glyph = (int)TileIndices.NoNorth;
                    break;
                case (0b_0010):
                    Glyph = (int)TileIndices.NoEast;
                    break;
                case (0b_0100):
                    Glyph = (int)TileIndices.NoSouth;
                    break;
                case (0b_1000):
                    Glyph = (int)TileIndices.NoWest;
                    break;

                // 3 walls end
                case (0b_1100):
                    Glyph = (int)TileIndices.NorthEast;
                    break;
                case (0b_1001):
                    Glyph = (int)TileIndices.SouthEast;
                    break;
                case (0b_0011):
                    Glyph = (int)TileIndices.SouthWest;
                    break;
                case (0b_0110):
                    Glyph = (int)TileIndices.NorthWest;
                    break;

                // 4 full walls all sides
                case (0b_0000):
                    Glyph = (int)TileIndices.All;
                    break;
                
                // Unfinished
                default:
                    Glyph = (int)TileIndices.Unknown;
                    break;
            }
            int gg = Glyph;
        }


    }
}
