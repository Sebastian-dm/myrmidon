using System;
using Microsoft.Xna.Framework;

namespace myrmidon.Tiles {
    // TileFloor is based on TileBase
    // Floor tiles to be used in maps.
    public class TileTunnel : Tile {
        // Default constructor
        // Floors are set to allow movement and line of sight by default
        // and have a dark gray foreground and a transparent background
        // represented by the . symbol
        public TileTunnel(bool walkable = true, bool blocksLOS = false) :
            base(foreground: Color.DarkGray, background: Color.Transparent, glyph: 0, walkable, blocksLOS) {
            Name = "Floor";
        }
    }
}