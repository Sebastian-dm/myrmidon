using System;
using Myrmidon.Core.Components;
using Myrmidon.Core.Utilities.Graphics;


namespace Myrmidon.Core.Maps.Tiles {
    /// <summary>
    /// Abstract and basic. TileBase is the simple form. Of all the tiles
    /// </summary>
    public abstract class Tile {

        public string Name;
        public bool IsWalkable;
        public bool IsBlockingLos;

        // TileBase is an abstract base class representing the most basic form of all Tiles used. Every TileBase has a Foreground Colour, Background Colour, and Glyph
        // IsBlockingMove and IsBlockingLOS are optional parameters, set to false by default
        public Tile(bool walkable = true, bool isBlockingLos = false, string name = "") {

            Name = name;
            
            IsWalkable = walkable;
            IsBlockingLos = isBlockingLos;
        }
    }

}
