using Bramble.Core;
using Myrmidon.Core.Maps;
using Myrmidon.Core.Actions;
using Myrmidon.Core.Utilities.Graphics;
using Myrmidon.Core.Utilities.Geometry;



namespace Myrmidon.Core.Entities {

    /// <summary>
    /// Extends the SadConsole.Entities.Entity class
    /// by adding an ID to it using GoRogue's ID system
    /// </summary>
    public abstract class Entity : GoRogue.IHasID {
        
        public uint ID { get; private set; } // stores the entity's unique identification number
        public string Name { get; set; } // stores the entity's name

        public int Glyph { get; set; } // the tile's glyph
        public Color Foreground { get; set; } // the tile's foreground colour
        public Color Background { get; set; } // the tile's background colour
        public bool IsDirty { get; set; } // indicates if the entity needs to be redrawn

        public bool isVisible {
            get {
                return _isVisible;
            }
            set {
                if (_isVisible != value) {
                    _isVisible = value;
                    OnVisible();
                }
            }
        }
        public Vec Position;
        public Vec Location {
            get { return new Vec(Position.X, Position.Y); }
        }


        internal bool _isVisible = false;
        internal Color _foregroundVisible;
        internal Color _backgroundVisible;
        internal int _glyphVisible;
        internal Color _foregroundHidden;
        internal Color _backgroundHidden;
        internal int _glyphHidden;

        protected Entity(Color foreground, Color background, int glyph, int width, int height) {

            // Set local variables
            _foregroundVisible = foreground;
            _backgroundVisible = background;
            _glyphVisible = glyph;

            _foregroundHidden = new Color(0, 0, 0);
            _backgroundHidden = new Color(0, 0, 0);
            _glyphHidden = 0;

            // Initial values
            Glyph = _glyphHidden;
            Foreground = _foregroundHidden;
            Background = _backgroundHidden;
            IsDirty = true;

            // Create a new unique identifier for this entity
            ID = Maps.TileMap.IDGenerator.UseID();

        }

        private void OnVisible() {
            if (_isVisible) {
                Glyph = _glyphVisible;
                Foreground = _foregroundVisible;
                Background = _backgroundVisible;

            }
            else {
                Glyph = _glyphHidden;
                Foreground = _foregroundHidden;
                Background = _backgroundHidden;
            }
            IsDirty = true;
        }
    }
}