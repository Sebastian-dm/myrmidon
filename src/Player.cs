using System;
using Microsoft.Xna.Framework;

namespace clodd {
    // Creates a new player
    // Default glyph is @
    public class Player : Actor {
        public Player(Color foreground, Color background) : base(foreground, background, '@') {

        }
    }
}
