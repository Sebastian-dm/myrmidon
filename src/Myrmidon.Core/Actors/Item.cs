﻿using System;

using Myrmidon.Core.Utilities.Geometry;
using Myrmidon.Core.Utilities.Graphics;


namespace Myrmidon.Core.Actors {
    
    /// <summary>
    /// Things that can be picked up or used by actors.
    /// </summary>
    public class Item : Actor {


        private int _condition; 

        public int Weight { get; set; } // mass of the item

        /// <summary>
        /// physical condition of item (100 = undamaged, 0 = destroyed).
        /// </summary>
        public int Condition {
            get { return _condition; }
            set {
                _condition += value;
                if (_condition <= 0)
                    Destroy();
            }
        }

        // By default, a new Item is sized 1x1, with a weight of 1, and at 100% condition
        public Item(Color foreground, Color background, int glyph, string name, int weight = 1, int condition = 100, int width = 1, int height = 1) : base(foreground, background, glyph) {
            // assign the object's fields to the parameters set in the constructor
            Weight = weight;
            Condition = condition;
            Name = name;
        }


        /// <summary>
        /// Destroy this object by removing it from the MultiSpatialMap's list of entities
        /// and lets the garbage collector take it out of memory automatically.
        /// </summary>
        public void Destroy() {
            Program.World.CurrentMap.Remove(this);
        }
    }
}