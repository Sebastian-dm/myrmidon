using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myrmidon.Core.Utilities.Geometry {

    public class VectorBase {

        public int X;
        public int Y;


        public VectorBase(int x, int y) {
            X = x;
            Y = y;
        }

        /// Gets the area of a [Rect] whose corners are (0, 0) and this Vec.
        /// Returns a negative area if one of the Vec's coordinates are negative.
        public int Area {
            get { return X * Y; }
        }

        /// Gets the rook length of the Vec, which is the number of squares a rook on
        /// a chessboard would need to move from (0, 0) to reach the endpoint of the
        /// Vec. Also known as Manhattan or taxicab distance. <summary>
        /// Gets the rook length of the Vec, which is the number of squares a rook on
        public int RookLength {
            get { return Math.Abs(X) + Math.Abs(Y); }
        }

        /// Gets the king length of the Vec, which is the number of squares a king on
        /// a chessboard would need to move from (0, 0) to reach the endpoint of the
        /// Vec. Also known as Chebyshev distance.
        public int KingLength {
            get { return X + Y; }
        }


        public int LengthSquared {
            get { return X * X + Y * Y; }
        }

        /// The Cartesian length of the vector.
        /// If you just need to compare the magnitude of two vectors, prefer using
        /// the comparison operators or [lengthSquared], both of which are faster.
        public double Length {
            get { return Math.Sqrt(LengthSquared); }
        }

        /// The eight Vecs surrounding this one to the north, south, east, and west
        /// and points in between.
        public Vector[] Neighbors {
            get {
                Vector[] neighbors = new Vector[8];
                for (int i = 0; i < 8; i++)
                    neighbors[i] = (Vector)(this + Direction.All[i]);
                return neighbors;
            }
        }

        /// The four Vecs surrounding this one to the north, south, east, and west..
        public Vector[] NeighborsCardinal {
            get {
                Vector[] neighbors = new Vector[4];
                for (int i = 0; i < 4; i++)
                    neighbors[i] = (Vector)(this + Direction.Cardinal[i]);
                return neighbors;
            }
        }

        /// The four Vecs surrounding this one to the north, south, east, and west..
        public Vector[] NeighborsIntercardinal {
            get {
                Vector[] neighbors = new Vector[4];
                for (int i = 0; i < 4; i++)
                    neighbors[i] = (Vector)(this + Direction.Intercardinal[i]);
                return neighbors;
            }
        }






        public static Vector operator +(VectorBase vector, Object other) {
            if (other is VectorBase) {
                VectorBase otherVector = (VectorBase)other;
                return new Vector(vector.X + otherVector.X, vector.Y + otherVector.Y);
            }
            else if (other is int) {
                return new Vector(vector.X + (int)other, vector.Y + (int)other);
            }
            throw new ArgumentException("Operand must be an int or VectorBase.");
        }

        public static Vector operator -(VectorBase vector, Object other) {
            if (other is VectorBase) {
                VectorBase otherVector = (VectorBase)other;
                return new Vector(vector.X - otherVector.X, vector.Y - otherVector.Y);
            }
            else if (other is int) {
                return new Vector(vector.X - (int)other, vector.Y - (int)other);
            }
            throw new ArgumentException("Operand must be an int or VectorBase.");
        }

        public static Vector operator *(VectorBase vector, int other) {
            return new Vector(vector.X * other, vector.Y * other);
        }



        public static bool operator <(VectorBase vector, Object other) {
            return !(vector >= other);
        }
        public static bool operator >(VectorBase vector, Object other) {
            return !(vector <= other);
        }
        public static bool operator <=(VectorBase vector, Object other) {
            if (other is VectorBase) {
                VectorBase otherVector = (VectorBase)other;
                return vector.LengthSquared <= otherVector.LengthSquared;
            }
            else if (IsNumber(other)) {
                return vector.LengthSquared <= (dynamic)other * (dynamic)other;
            }
            throw new ArgumentException("Operand must be an int or VectorBase.");
        }
        public static bool operator >=(VectorBase vector, Object other) {
            if (other is VectorBase) {
                VectorBase otherVector = (VectorBase)other;
                return vector.LengthSquared >= otherVector.LengthSquared;
            }
            else if (IsNumber(other)) {
                return vector.LengthSquared >= (dynamic)other * (dynamic)other;
            }
            throw new ArgumentException("Operand must be an int or VectorBase.");
        }

        


        public double DistanceTo(VectorBase vectorB) {
            return (vectorB - this).Length;
        }


        private static bool IsNumber(Object o) {
            if (o is sbyte  || o is byte  || o is short ||
                o is ushort || o is int   || o is uint  ||
                o is long   || o is ulong || o is float ||
                o is double || o is decimal)
                return true;
            else
                return false;
        }
    }


    public class Vector : VectorBase{


        public Vector(int X, int Y) : base(X, Y) {

        }

        public override int GetHashCode() {
            // Map negative coordinates to positive and spread
            // out the positive ones to make room for them.
            var a =  X>=0 ? 2*X : -2*X-1;
            var b =  Y>=0 ? 2*Y : -2*Y-1;

            // Cantor pairing function.
            // https://en.wikipedia.org/wiki/Pairing_function
            return (a+b) * (a+b+1) / 2 + b;
        }


        public override bool Equals(object obj) {
            return this == obj;
        }

        public static bool operator ==(Vector vector, Object other) {
            if (other is VectorBase)
                return vector.X == ((VectorBase)other).X &&
                    vector.Y == ((VectorBase)other).Y;
            else
                return false;
        }

        public static bool operator !=(Vector vector, Object other) {
            return !(vector == other);
        }

        public override string ToString() {
            return $"Vector [{X}, {Y}]";
        }

    }
}
