using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clodd {
    internal class Random : System.Random {

        /// <summary>
        /// Gets a random int within a given range.
        /// If max is given, then it is in the range [minOrMax, max). Otherwise, it is [0, minOrMax).
        /// In other words, range(3) returns a 0, 1, or 2, and range(2, 5) returns 2, 3, or 4. 
        /// </summary>
        /// <returns></returns>
        public int Range(int minOrMax, int max=0) {
            if (max <= 0) {
                return Next(minOrMax);
            }
            else {
                return Next(minOrMax, max);
            }
        }


        /// <summary>
        /// Picks a random item from a list of items.
        /// </summary>
        /// <typeparam name="T">Type of items in list.</typeparam>
        /// <param name="list">List to pick from.</param>
        /// <returns>Random element from list.</returns>
        public T Item<T>(List<T> list) {
            int index = Next(0, list.Count);
            return list[index];
        }
        /// <summary>
        /// Picks a random item from an array of items.
        /// </summary>
        /// <typeparam name="T">Type of items in array.</typeparam>
        /// <param name="list">Array to pick from.</param>
        /// <returns>Random element from array.</returns>
        public T Item<T>(T[] array) {
            return Item<T>(array.ToList());
        }



        /// <summary>
        /// Rolls a 1 in x chance roll.
        /// </summary>
        /// <param name="chance">Fraction chance</param>
        /// <returns>True if a random int chosen between 1 and <b>chance</b> was 1. Otherwise false.</returns>
        public bool OneIn(int chance) {
            if (Next(1, chance) == 1) return true;
            else return false;
        }

        /// <summary>
        /// Rolls a percentage roll.
        /// </summary>
        /// <param name="">Percentage probability to be true.</param>
        /// <returns>True <b>chance</b> percent of the time.</returns>
        public bool Percent(int chance) {
            if (Next(1, 100) <= chance) return true;
            else return false;
        }


    }
}
