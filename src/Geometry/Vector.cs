using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace myrmidon.Geometry {
    public class Vector {


        public int X;
        public int Y;


        public Vector(int x, int y) {
            X = x;
            Y = y;
        }

        public double Length { get {
            var vec2 = new Vector2(X,Y);
            return vec2.Length();
            }
        }

        public override string ToString() {
            return $"Point [{X}, {Y}]";
        }


        public int ToIndex(int rowWidth) {
            return Y * rowWidth + X;
        }

        public static bool operator <(Vector vector, int number) {
            return vector.Length < number;
        }
        public static bool operator >(Vector vector, int number) {
            return !(vector<number);
        }

        public static Vector operator +(Vector vectorA, Vector vectorB) {
            return new Vector(vectorA.X + vectorB.X, vectorA.Y + vectorB.Y);
        }

        public static Vector operator -(Vector vectorA, Vector vectorB) {
            return new Vector(vectorA.X - vectorB.X, vectorA.Y - vectorB.Y);
        }

        public static Vector operator *(Vector vector, int scale) {
            return new Vector(vector.X * scale, vector.Y * scale);
        }

        public double DistanceTo(Vector vectorB) {
            return (vectorB - this).Length;
        }

        

    }
}
