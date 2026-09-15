using System;
using System.Collections.Generic;
using Myrmidon.Core.Components;
using Myrmidon.Core.Utilities.Graphics;

namespace Myrmidon.Core.Maps.Tiles {
    // TileWall is based on TileBase
    public class TileWall : Tile {

        // Walls are set to block movement and line of sight by default
        public TileWall(bool walkable = false, bool blocksLOS = true) :
            base(walkable, blocksLOS) {
            Name = "Wall";
        }
    }
}
