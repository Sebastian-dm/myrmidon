
using Microsoft.Xna.Framework;
using SadConsole.Components;
using myrmidon.Maps;
using myrmidon.Actions;
using myrmidon.Geometry;

namespace myrmidon.Entities {

    /// <summary>
    /// Extends the SadConsole.Entities.Entity class
    /// by adding an ID to it using GoRogue's ID system
    /// </summary>
    public abstract class Entity : SadConsole.Entities.Entity, GoRogue.IHasID {
        
        public uint ID { get; private set; } // stores the entity's unique identification number


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
        public Vector Location {
            get { return new Vector(Position.X, Position.Y); }
        }


        internal bool _isVisible = false;
        internal Color _foregroundVisible;
        internal Color _backgroundVisible;
        internal int _glyphVisible;
        internal Color _foregroundHidden;
        internal Color _backgroundHidden;
        internal int _glyphHidden;



        protected Entity(Color foreground, Color background, int glyph, int width = 1, int height = 1) : base(width, height) {

            // Set local variables
            _foregroundVisible = foreground;
            _backgroundVisible = background;
            _glyphVisible = glyph;

            _foregroundHidden = new Color(0, 0, 0);
            _backgroundHidden = new Color(0, 0, 0);
            _glyphHidden = 0;

            // Initial values
            Animation.CurrentFrame[0].Glyph = _glyphHidden;
            Animation.CurrentFrame[0].Foreground = _foregroundHidden;
            Animation.CurrentFrame[0].Background = _backgroundHidden;
            Animation.IsDirty = true;

            // Create a new unique identifier for this entity
            ID = Maps.Map.IDGenerator.UseID();

            // Ensure that the entity position/offset is tracked by scrollingconsoles
            Components.Add(new EntityViewSyncComponent());

        }

        private void OnVisible() {
            if (_isVisible) {
                Animation.CurrentFrame[0].Glyph = _glyphVisible;
                Animation.CurrentFrame[0].Foreground = _foregroundVisible;
                Animation.CurrentFrame[0].Background = _backgroundVisible;

            }
            else {
                Animation.CurrentFrame[0].Glyph = _glyphHidden;
                Animation.CurrentFrame[0].Foreground = _foregroundHidden;
                Animation.CurrentFrame[0].Background = _backgroundHidden;
            }
            Animation.IsDirty = true;
        }
    }
}