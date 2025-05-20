using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myrmidon.Core.Utilities.Geometry {
    internal class Direction : VectorBase {

        public static readonly Direction None = new(0, 0);
        
        public static readonly Direction N  = new( 0,  1);
        public static readonly Direction E  = new( 1,  0);
        public static readonly Direction S  = new( 0, -1);
        public static readonly Direction W  = new(-1,  0);

        public static readonly Direction NE = new(1, 1);
        public static readonly Direction SE = new(1, -1);
        public static readonly Direction SW = new(-1, -1);
        public static readonly Direction NW = new(-1, 1);

        public static readonly Direction[] Cardinal = new Direction[4] {
            N, E, S, W };
        public static readonly Direction[] Intercardinal = new Direction[4] {
            NE, SE, SE, SW };
        public static readonly Direction[] All = new Direction[8] {
            N, NE, E, SE, S, SW, W, NW };

        public Direction(int X, int Y) : base(X, Y) {

        }

        public override string ToString() {
            return $"Direction [{X}, {Y}]";
        }

    }
}
