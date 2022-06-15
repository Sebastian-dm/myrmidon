using System;
using Microsoft.Xna.Framework;
using SadConsole;

namespace clodd.Tiles {
    /// <summary>
    /// Abstract and basic. TileBase is the simple form. Of all the tiles
    /// </summary>
    public abstract class TileBase : Cell {

        // Movement and Line of Sight Flags
        public bool IsBlockingMove;
        public bool IsBlockingLineOfSigth;

        // Tile's name
        public string Name;

        // TileBase is an abstract base class representing the most basic form of all Tiles used.
        // Every TileBase has a Foreground Colour, Background Colour, and Glyph
        // IsBlockingMove and IsBlockingLOS are optional parameters, set to false by default
        public TileBase(Color foreground, Color background, int glyph, bool blockingMove = false, bool blockingLOS = false, string name = "") : base(foreground, background, glyph) {
            IsBlockingMove = blockingMove;
            IsBlockingLineOfSigth = blockingLOS;
            Name = name;
        }
    }
}
