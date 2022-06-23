using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using clodd.Actions;
using clodd.Geometry;
using clodd.Tiles;

namespace clodd.Entities {
    public abstract class Actor : Entity {

        //private int _health; //current health
        //private int _maxHealth; //maximum possible health

        public int Health { get; set; } // current health
        public int MaxHealth { get; set; } // maximum health
        public int AttackStrength { get; set; } // attack strength
        public int AttackChance { get; set; } // percent chance of successful hit
        public int DefenseStrength { get; set; } // defensive strength
        public int DefenseChance { get; set; } // percent chance of successfully blocking a hit
        public int Gold { get; set; } // amount of gold carried

        public List<Item> Inventory = new List<Item>(); // the player's collection of items

        protected Actor(Color foreground, Color background, int glyph, int width = 1, int height = 1) : base(foreground, background, glyph, width, height) {
            
        }


        /// <summary>
        /// Moves the Actor TO newPosition location
        /// </summary>
        /// <param name="newPosition"></param>
        /// <returns>returns true if actor was able to move, false if failed to move</returns>
        public bool MoveTo(Vector newPosition) {
            Position = new Point(newPosition.X, newPosition.Y);
            return true;
        }

        public Action GetAction() {
            return new SkipAction(this);
        }

    }
}
