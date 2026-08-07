using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bramble.Core;

namespace Myrmidon.Core.Utilities.Geometry {
    internal class Direction {

        public static readonly Vec None = new(0, 0);
        
        public static readonly Vec N  = new( 0,  1);
        public static readonly Vec E  = new( 1,  0);
        public static readonly Vec S  = new( 0, -1);
        public static readonly Vec W  = new(-1,  0);

        public static readonly Vec NE = new(1, 1);
        public static readonly Vec SE = new(1, -1);
        public static readonly Vec SW = new(-1, -1);
        public static readonly Vec NW = new(-1, 1);

        public static readonly Vec[] Cardinal = new Vec[4] {
            N, E, S, W };
        public static readonly Vec[] Intercardinal = new Vec[4] {
            NE, SE, SE, SW };
        public static readonly Vec[] All = new Vec[8] {
            N, NE, E, SE, S, SW, W, NW };
    }
}
