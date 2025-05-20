using System;

namespace Myrmidon.Core.Utilities.Graphics {
    public readonly struct Color {
        public byte R { get; }
        public byte G { get; }
        public byte B { get; }
        public byte A { get; }

        // === Constructors ===

        public Color(byte r, byte g, byte b, byte a = 255) {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public Color(int r, int g, int b, int a = 255) {
            R = ClampToByte(r);
            G = ClampToByte(g);
            B = ClampToByte(b);
            A = ClampToByte(a);
        }

        private static byte ClampToByte(int value)
            => (byte)Math.Clamp(value, 0, 255);


        // === Predefined Colors ===
        public static readonly Color Black = new(0, 0, 0);
        public static readonly Color White = new(255, 255, 255);
        public static readonly Color Red = new(255, 0, 0);
        public static readonly Color Green = new(0, 255, 0);
        public static readonly Color Blue = new(0, 0, 255);
        public static readonly Color Yellow = new(255, 255, 0);
        public static readonly Color Cyan = new(0, 255, 255);
        public static readonly Color Magenta = new(255, 0, 255);
        public static readonly Color Gray = new(128, 128, 128);
        public static readonly Color DarkGray = new(64, 64, 64);
        public static readonly Color LightGray = new(192, 192, 192);
        public static readonly Color Orange = new(255, 165, 0);
        public static readonly Color Pink = new(255, 192, 203);
        public static readonly Color Brown = new(139, 69, 19);
        public static readonly Color Purple = new(128, 0, 128);
        public static readonly Color Lime = new(0, 255, 0);
        public static readonly Color Olive = new(128, 128, 0);
        public static readonly Color Teal = new(0, 128, 128);
        public static readonly Color Navy = new(0, 0, 128);
        public static readonly Color Maroon = new(128, 0, 0);
        public static readonly Color Aqua = new(0, 255, 255);
        public static readonly Color Silver = new(192, 192, 192);
        public static readonly Color Gold = new(255, 215, 0);
        public static readonly Color Transparent = new(0, 0, 0, 0);

        // === Equality ===

        public override bool Equals(object obj)
            => obj is Color other && R == other.R && G == other.G && B == other.B && A == other.A;

        public override int GetHashCode()
            => HashCode.Combine(R, G, B, A);

        public static bool operator ==(Color a, Color b)
            => a.Equals(b);

        public static bool operator !=(Color a, Color b)
            => !a.Equals(b);

        // === Utility ===

        public override string ToString()
            => $"Color(R:{R}, G:{G}, B:{B}, A:{A})";
    }
}
