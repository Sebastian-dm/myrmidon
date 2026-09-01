using System;
using Myrmidon.Core.Components;
using Myrmidon.Core.Utilities.Graphics;

namespace Myrmidon.Core.Maps.Tiles {
    // TileFloor is based on TileBase
    // Floor tiles to be used in maps.
    public class TileEmpty : Tile {

        public TileEmpty(RenderComponent renderComponent, bool blocksMovement = true, bool blocksLOS = true) :
            base(renderComponent, blocksMovement, blocksLOS) {
            Name = "Empty";
        }
    }
}