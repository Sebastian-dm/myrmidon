using System;

using Myrmidon.Core.Utilities.Graphics;
using Myrmidon.Core.Actors;

namespace Myrmidon.Core.Actors {
    // Creates a new player
    // Default glyph is @
    public class Player : Actor {
        public Player(Color foreground, Color background) : base(foreground, background, glyph: 352) {
            AttackStrength = 10;
            AttackChance = 40;
            DefenseStrength = 5;
            DefenseChance = 20;
            Name = "Player";
            isVisible = true;
        }
    }
}
