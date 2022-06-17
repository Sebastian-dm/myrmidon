using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clodd.Geometry {
    public class Vector {


        public int X;
        public int Y;


        public Vector(int x, int y) {
            X = x;
            Y = y;
        }

        public override string ToString() {
            return $"Point [{X}, {Y}]";
        }


        public int ToIndex(int rowWidth) {
            return Y * rowWidth + X;
        }

        public static Vector operator +(Vector vectorA, Vector vectorB) {
            return new Vector(vectorA.X + vectorB.X, vectorA.Y + vectorB.Y);
        }

        public static Vector operator *(Vector vector, int scale) {
            return new Vector(vector.X * scale, vector.Y * scale);
        }

        

    }
}
