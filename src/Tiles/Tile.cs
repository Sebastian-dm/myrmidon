using System;
using Microsoft.Xna.Framework;
using SadConsole;

namespace myrmidon.Tiles {
    /// <summary>
    /// Abstract and basic. TileBase is the simple form. Of all the tiles
    /// </summary>
    public abstract class Tile : Cell {

        public string Name;
        public bool IsWalkable;
        public bool IsBlockingLineOfSigth;
        
        public bool isVisible {
            get { return _isVisible; }
            set { _isVisible = value; OnVisible(); }
        }
        public bool isExplored {
            get { return _isExplored; }
            set { _isExplored = value; OnExplored(); }
        }

        internal bool _isVisible;
        internal bool _isExplored;
        internal int _glyphWhenExplored;
        internal Color _foregroundVisible;
        internal Color _backgroundVisible;
        internal Color _foregroundHidden;
        internal Color _backgroundHidden;

        // TileBase is an abstract base class representing the most basic form of all Tiles used. Every TileBase has a Foreground Colour, Background Colour, and Glyph
        // IsBlockingMove and IsBlockingLOS are optional parameters, set to false by default
        public Tile(Color foreground, Color background, int glyph, bool walkable = true, bool blockingLOS = false, string name = "") : base(new Color(0, 0, 0), new Color(0, 0, 0), 0) {
            IsWalkable = walkable;
            IsBlockingLineOfSigth = blockingLOS;
            Name = name;

            _glyphWhenExplored = glyph;
            _foregroundVisible = foreground;
            _backgroundVisible = background;
            int dimFactor = 3;
            _foregroundHidden = new Color(_foregroundVisible.R / dimFactor, _foregroundVisible.G / dimFactor, _foregroundVisible.B / dimFactor);
            _backgroundHidden = new Color(_backgroundVisible.R / dimFactor, _backgroundVisible.G / dimFactor, _backgroundVisible.B / dimFactor);

            _isVisible = false;
            _isExplored = false;
        }


        private void OnExplored() {
            if (_isExplored) {
                Glyph = _glyphWhenExplored;
                Foreground = _foregroundHidden;
                Background = _backgroundHidden;
            }
        }


        private void OnVisible() {
            if (_isExplored) {
                if (_isVisible) {
                    Foreground = _foregroundVisible;
                    Background = _backgroundVisible;
                }
                else {
                    Foreground = _foregroundHidden;
                    Background = _backgroundHidden;
                }
            }
        }
    }

}
