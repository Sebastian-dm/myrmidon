using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myrmidon.Core.Utilities.Geometry {
    public readonly struct Point(int x, int y) {
        public int X { get; } = x;
        public int Y { get; } = y;

        public float DistanceTo(Point other)
        => MathF.Sqrt(MathF.Pow(X - other.X, 2) + MathF.Pow(Y - other.Y, 2));

        // === Conversion ===

        public Vector ToVector2() => new(X, Y);

        public static Point FromVector2(Vector v) => new(v.X, v.Y);

        // === Operators ===
        public static Point operator +(Point a, Point b)
            => new Point(a.X + b.X, a.Y + b.Y);

        public static Point operator -(Point a, Point b)
            => new Point(a.X - b.X, a.Y - b.Y);

        public static Point operator *(Point a, Point b)
            => new Point(a.X * b.X, a.Y * b.Y);


        public override string ToString() => $"({X}, {Y})";

    }
}
