using System;
using Microsoft.Xna.Framework;

namespace clodd.Entities {
    // Creates a new player
    // Default glyph is @
    public class Player : Actor {
        public Player(Color foreground, Color background) : base(foreground, background, glyph: 2) {
            AttackStrength = 10;
            AttackChance = 40;
            DefenseStrength = 5;
            DefenseChance = 20;
            Name = "Claude";
        }
    }
}
