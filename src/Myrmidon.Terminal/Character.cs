using System;
using System.Collections.Generic;
using System.Text;

namespace Myrmidon.Terminal
{
    public struct Character : IEquatable<Character>
    {
        private Glyph mGlyph;
        private TermColor mForeColor;
        private TermColor mBackColor;
        
        public static TermColor DefaultForeColor { get { return TermColor.White; } }
        public static TermColor DefaultBackColor { get { return TermColor.Black; } }
        public static Glyph ToGlyph(char ascii) { return (Glyph)(ascii); }
        
        public Glyph Glyph { get { return mGlyph; } }
        public TermColor ForeColor { get { return mForeColor; } }
        public TermColor BackColor { get { return mBackColor; } }
        public bool IsWhitespace { get { return mGlyph == Glyph.Space; } }
        
        
        public Character(Glyph glyph, TermColor foreColor, TermColor backColor) {
            mGlyph = glyph;
            mBackColor = backColor;
            mForeColor = foreColor;
        }
        public Character(Glyph glyph, TermColor foreColor)
            : this(glyph, foreColor, DefaultBackColor) {
        }
        public Character(Glyph glyph)
            : this(glyph, DefaultForeColor) {
        }
        public Character(char ascii, TermColor foreColor, TermColor backColor)
            : this(Character.ToGlyph(ascii), foreColor, backColor) {
        }
        public Character(char ascii, TermColor foreColor)
            : this(Character.ToGlyph(ascii), foreColor, DefaultBackColor) {
        }
        public Character(char ascii)
            : this(Character.ToGlyph(ascii), DefaultForeColor) {
        }
        
        public static Character Parse(string text) {
            if (text == null) throw new ArgumentNullException("text");
            if (text.Length == 0) throw new ArgumentException("Argument 'text' cannot be empty.");

            text = text.Trim();

            // separate out the colors and glyph
            string[] parts = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // only supports three parts (max)
            if (parts.Length > 3) throw new ArgumentException("Character.Parse() should be formatted \"Glyph\", \"ForeColor Glyph\", or \"ForeColor BackColor Glyph\".");

            Glyph glyph;
            TermColor foreColor = DefaultForeColor;
            TermColor backColor = DefaultBackColor;

            // parse the glyph
            glyph = ParseGlyph(parts[parts.Length - 1]);

            // parse the fore color
            if (parts.Length > 1)
            {
                foreColor = TermColors.FromName(parts[0]);
            }

            // parse the back color
            if (parts.Length > 2)
            {
                backColor = TermColors.FromName(parts[1]);
            }

            return new Character(glyph, foreColor, backColor);
        }
        
        
        
        public static Glyph ParseGlyph(string text)
        {
            if (text.Length == 1)
            {
                // a single character is assumed to be ascii
                return ToGlyph(text[0]);
            }
            else
            {
                // multiple characters are the glyph enum names
                return (Glyph)Enum.Parse(typeof(Glyph), text, true);
            }
        }
        
        
        public override string ToString() {
            return mGlyph.ToString();
        }
        
        public override bool Equals(object obj) {
            if (obj is Character) return Equals((Character)obj);

            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return mGlyph.GetHashCode() + mBackColor.GetHashCode() + mForeColor.GetHashCode();
        }

        #region IEquatable<Character> Members
        public bool Equals(Character other) {
            return (mGlyph == other.mGlyph) && mBackColor.Equals(other.mBackColor) && mForeColor.Equals(other.mForeColor);
        }
        #endregion
    }
}