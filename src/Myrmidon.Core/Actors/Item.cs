using System;

using Myrmidon.Core.Utilities.Geometry;
using Myrmidon.Core.Utilities.Graphics;


namespace Myrmidon.Core.Actors {
    
    /// <summary>
    /// Things that can be picked up or used by actors.
    /// </summary>
    public class Item : Actor {

        public int Weight { get; set; } // mass of the item


        // By default, a new Item is sized 1x1, with a weight of 1, and at 100% condition
        public Item(Color foreground, Color background, int glyph, string name, int weight = 1, int condition = 100, int width = 1, int height = 1) : base(foreground, background, glyph) {
            // assign the object's fields to the parameters set in the constructor
            Weight = weight;
            Name = name;
        }

    }
}