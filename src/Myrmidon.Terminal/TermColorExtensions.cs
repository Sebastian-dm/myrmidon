using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using Bramble.Core;
using Malison.Core;

namespace Myrmidon.Terminal
{
    public static class TermColorExtensions
    {
        public static Color ToSystemColor(this TerminalColor color)
        {
            switch (color)
            {
                case TerminalColor.Black: return Color.Black;
                case TerminalColor.White: return Color.White;

                case TerminalColor.LightGray: return Color.FromArgb(192, 192, 192);
                case TerminalColor.Gray: return Color.FromArgb(128, 128, 128);
                case TerminalColor.DarkGray: return Color.FromArgb(48, 48, 48);

                case TerminalColor.LightRed:
                case TerminalColor.Pink:
                    return Color.FromArgb(255, 160, 160);

                case TerminalColor.Red: return Color.FromArgb(220, 0, 0);
                case TerminalColor.DarkRed: return Color.FromArgb(100, 0, 0);

                case TerminalColor.LightOrange:
                case TerminalColor.Flesh:
                    return Color.FromArgb(255, 200, 170);

                case TerminalColor.Orange: return Color.FromArgb(255, 128, 0);
                case TerminalColor.DarkOrange: return Color.FromArgb(128, 64, 0);

                case TerminalColor.LightGold: return Color.FromArgb(255, 230, 150);
                case TerminalColor.Gold: return Color.FromArgb(255, 192, 0);
                case TerminalColor.DarkGold: return Color.FromArgb(128, 96, 0);

                case TerminalColor.LightYellow: return Color.FromArgb(255, 255, 150);
                case TerminalColor.Yellow: return Color.FromArgb(255, 255, 0);
                case TerminalColor.DarkYellow: return Color.FromArgb(128, 128, 0);

                case TerminalColor.LightGreen: return Color.FromArgb(130, 255, 90);
                case TerminalColor.Green: return Color.FromArgb(0, 200, 0);
                case TerminalColor.DarkGreen: return Color.FromArgb(0, 100, 0);

                case TerminalColor.LightCyan: return Color.FromArgb(200, 255, 255);
                case TerminalColor.Cyan: return Color.FromArgb(0, 255, 255);
                case TerminalColor.DarkCyan: return Color.FromArgb(0, 128, 128);

                case TerminalColor.LightBlue: return Color.FromArgb(128, 160, 255);
                case TerminalColor.Blue: return Color.FromArgb(0, 64, 255);
                case TerminalColor.DarkBlue: return Color.FromArgb(0, 37, 168);

                case TerminalColor.LightPurple: return Color.FromArgb(200, 140, 255);
                case TerminalColor.Purple: return Color.FromArgb(128, 0, 255);
                case TerminalColor.DarkPurple: return Color.FromArgb(64, 0, 128);

                case TerminalColor.LightBrown: return Color.FromArgb(190, 150, 100);
                case TerminalColor.Brown: return Color.FromArgb(160, 110, 60);
                case TerminalColor.DarkBrown: return Color.FromArgb(100, 64, 32);

                default: throw new UnexpectedEnumValueException(color);
            }
        }
    }
}
