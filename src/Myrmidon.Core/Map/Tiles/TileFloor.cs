using System;
using Microsoft.Xna.Framework;

namespace Myrmidon.Core.Map.Tiles {
    // TileFloor is based on TileBase
    // Floor tiles to be used in maps.
    public class TileFloor : Tile {
        // Default constructor
        // Floors are set to allow movement and line of sight by default
        // and have a dark gray foreground and a transparent background
        // represented by the . symbol
        public TileFloor(bool walkable = true, bool blocksLOS = false) : 
            base(foreground: new Color(255, 255, 255), background: new Color(20, 10, 0), glyph: 0, walkable, blocksLOS) {
            Name = "Floor";
        }
    }
}