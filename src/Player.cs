using System;
using SadConsole;
using Console = SadConsole.Console;
using Microsoft.Xna.Framework;
using clodd;

namespace clodd {
    internal class Player : SadConsole.Entities.Entity {


        /// <summary>
        /// Create a player using SadConsole's Entity class
        /// </summary>
        public Player() : base(1, 1) {
            Animation.CurrentFrame[0].Glyph = '@';
            Animation.CurrentFrame[0].Foreground = Color.HotPink;
            Position = new Point(5, 5);
        }
    }
}
