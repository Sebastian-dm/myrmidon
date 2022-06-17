﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

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

        public static bool operator <(Vector vector, int number) {
            var vec2 = new Vector2(vector.X, vector.Y);
            return vec2.Length() < number;
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

        

    }
}
