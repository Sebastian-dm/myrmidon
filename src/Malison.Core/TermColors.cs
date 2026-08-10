using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Malison.Core
{
    /// <summary>
    /// Static class containing helper functions for dealing with <see cref="TerminalColor"/> values.
    /// </summary>
    public static class TermColors
    {
        public static TerminalColor FromName(string name)
        {
            return (TerminalColor)Enum.Parse(typeof(TerminalColor), name);
        }

        public static TerminalColor FromEscapeChar(char c)
        {
            switch (c)
            {
                case 'k': return TerminalColor.DarkGray;
                case 'K': return TerminalColor.Black;

                case 'm': return TerminalColor.Gray; // "m"edium

                case 'w': return TerminalColor.White;
                case 'W': return TerminalColor.LightGray;

                case 'r': return TerminalColor.Red;
                case 'R': return TerminalColor.DarkRed;

                case 'o': return TerminalColor.Orange;
                case 'O': return TerminalColor.DarkOrange;

                case 'l': return TerminalColor.Gold;
                case 'L': return TerminalColor.DarkGold;

                case 'y': return TerminalColor.Yellow;
                case 'Y': return TerminalColor.DarkYellow;

                case 'g': return TerminalColor.Green;
                case 'G': return TerminalColor.DarkGreen;

                case 'c': return TerminalColor.Cyan;
                case 'C': return TerminalColor.DarkCyan;

                case 'b': return TerminalColor.Blue;
                case 'B': return TerminalColor.DarkBlue;

                case 'p': return TerminalColor.Purple;
                case 'P': return TerminalColor.DarkPurple;

                case 'f': return TerminalColor.Flesh;
                case 'F': return TerminalColor.Brown;

                default: return TerminalColor.White;
            }
        }
    }
}
