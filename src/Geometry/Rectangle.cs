using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myrmidon.Geometry {
    public class Rect : IEnumerable<Vector> {


        public int X;
        public int Y;
        public int Width;
        public int Height;


        public Rect(int x, int y, int width, int height) {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public int Bottom { get { return Y + Height; } }
        public int Left { get { return X; } }
        public int Right { get { return X + Width; } }
        public int Top { get { return Y; } }
        public Rect Empty { get { return new Rect(0,0,0,0); } }
        public bool IsEmpty {
            get { if (X == 0 && Y == 0 && Width == 0 && Height == 0) {
                    return true;
                }
                return false;
            }
        }
        public int Area { get { return Width * Height; } }
        public Microsoft.Xna.Framework.Point Center {
            get { return new Microsoft.Xna.Framework.Point(X+Width/2,Y+Height/2); }
            }


        public bool Equals(Rect otherRect) {
            if (X==otherRect.X &&
                Y == otherRect.Y &&
                Width == otherRect.Width &&
                Height == otherRect.Height) {
                return true;
            }
            return false;
        }

        public override string ToString() {
            return $"Rect {{X={X}, Y={Y}, Width={Width}, Height={Height}}}";
        }


        public Rect Inflate(int distance) {
            return new Rect(X - distance, Y - distance, Width + (distance * 2),
                Height + (distance * 2));
        }

        public bool DoesContain(Vector point) {
            if (X <= point.X && point.X <= X + Width &&
                Y <= point.Y && point.Y <= Y + Height) {
                return true;
            }
            return false;
        }

        public int DistanceTo(Rect other) {
            int vertical;
            if (Top >= other.Bottom) {
                vertical = Top - other.Bottom;
            }
            else if (Bottom <= other.Top) {
                vertical = other.Top - Bottom;
            }
            else {
                vertical = -1;
            }

            int horizontal;
            if (Left >= other.Right) {
                horizontal = Left - other.Right;
            }
            else if (Right <= other.Left) {
                horizontal = other.Left - Right;
            }
            else {
                horizontal = -1;
            }

            if ((vertical == -1) && (horizontal == -1)) return -1;
            if (vertical == -1) return horizontal;
            if (horizontal == -1) return vertical;
            return horizontal + vertical;
        }

        public bool Intersects(Rect other) {
            int IntLeft = Math.Max(Left, other.Left);
            int IntRight = Math.Min(Right, other.Right);
            int IntTop = Math.Max(Top, other.Top);
            int IntBottom = Math.Min(Bottom, other.Bottom);

            int IntWidth = Math.Max(0, IntRight - IntLeft);
            int IntHeight = Math.Max(0, IntBottom - IntTop);

            if (IntWidth > 0 && IntHeight > 0) return true;
            else return false;
        }


        /// <summary>
        /// Creates a new rectangle that is the intersection of a and b.
        /// </summary>
        /// <param name="a">Rectangle A.</param>
        /// <param name="b">Rectangle B.</param>
        /// <returns>The intersection of rectangle A and B.</returns>
        public static Rect Intersect(Rect a, Rect b) {
            int IntLeft = Math.Max(a.Left, b.Left);
            int IntRight = Math.Min(a.Right, b.Right);
            int IntTop = Math.Max(a.Top, b.Top);
            int IntBottom = Math.Min(a.Bottom, b.Bottom);

            int IntWidth = Math.Max(0, IntRight - IntLeft);
            int IntHeight = Math.Max(0, IntBottom - IntTop);

            return new Rect(IntLeft, IntTop, IntWidth, IntHeight);
        }


        public IEnumerator<Vector> GetEnumerator() {
            for (int x = X; x < X+Width; x++) {
                for (int y = Y; y < Y+Height; y++) {
                    yield return new Vector(x, y);
                }
            }
        }
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
