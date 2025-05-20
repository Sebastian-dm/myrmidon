using System;
using System.Drawing;

namespace Myrmidon.Core.Actors {

    // A generic monster capable of combat and interaction yields treasure upon death
    public class Monster : Actor {

        private Random Rng = new Random();

        public Monster(Color foreground, Color background, int glyph) : base(foreground, background, glyph) {
            Rng = new Random();

            //number of loot to spawn for monster
            int lootNum = Rng.Next(1, 4);

            for (int i = 0; i < lootNum; i++) {
                // monsters are made out of spork, obvs.
                Item newLoot = new Item(Color.Beige, Color.Transparent, glyph: 384, name: "Loot", 2);
                Inventory.Add(newLoot);
            }
        }
    }
}