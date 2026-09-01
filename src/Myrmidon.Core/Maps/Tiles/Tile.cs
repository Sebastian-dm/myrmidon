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
        
        public RenderComponent RenderComponent { get; set; }

        public bool IsVisible {
            get { return _isVisible; }
            set { _isVisible = value; OnVisible(); }
        }
        public bool IsExplored {
            get { return _isExplored; }
            set { _isExplored = value; OnExplored(); }
        }

        internal bool _isVisible;
        internal bool _isExplored;

        // TileBase is an abstract base class representing the most basic form of all Tiles used. Every TileBase has a Foreground Colour, Background Colour, and Glyph
        // IsBlockingMove and IsBlockingLOS are optional parameters, set to false by default
        public Tile(RenderComponent renderComponent, bool walkable = true, bool isBlockingLos = false, string name = "") {

            Name = name;
            
            RenderComponent = renderComponent;
            
            IsWalkable = walkable;
            IsBlockingLos = isBlockingLos;
            _isVisible = false;
            _isExplored = false;
        }


        private void OnExplored() {
            if (_isExplored) {
                //Glyph = _glyphWhenExplored;
                //ForegroundColor = _foregroundHidden;
                //BackgroundColor = _backgroundHidden;
            }
        }


        private void OnVisible() {
            if (_isExplored) {
                if (_isVisible) {
                    //ForegroundColor = _foregroundVisible;
                    //BackgroundColor = _backgroundVisible;
                }
                else {
                    //ForegroundColor = _foregroundHidden;
                    //BackgroundColor = _backgroundHidden;
                }
            }
        }
    }

}
