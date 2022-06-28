using System;
using Microsoft.Xna.Framework;

namespace myrmidon.Tiles {
    // TileFloor is based on TileBase
    // Floor tiles to be used in maps.
    public class TileEmpty : Tile {

        public TileEmpty(bool blocksMovement = true, bool blocksLOS = true) :
            base(foreground: Color.DarkGray, background: Color.Transparent, glyph: 0, blocksMovement, blocksLOS) {
            Name = "Empty";
        }
    }
}