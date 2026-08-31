using System.Drawing;

namespace Myrmidon.App.Render;

public static class TerminalColor {
    
    public static Color ColFromString(string color) {
        color = color.ToLower();
        return color switch {
            "r" => Color.FromArgb(0xa6, 0x4a, 0x2e), // dark red
            "R" => Color.FromArgb(0xd7, 0x42, 0x00), // red
            "g" => Color.FromArgb(0x00, 0x94, 0x03), // dark green
            "G" => Color.FromArgb(0x00, 0xc4, 0x20), // green
            "b" => Color.FromArgb(0x00, 0x48, 0xbd), // dark blue
            "B" => Color.FromArgb(0x00, 0x96, 0xff), // blue

            "c" => Color.FromArgb(0x40, 0xa4, 0xb9), // dark cyan
            "C" => Color.FromArgb(0x77, 0xbf, 0xcf), // cyan
            "m" => Color.FromArgb(0xb1, 0x54, 0xcf), // dark magenta
            "M" => Color.FromArgb(0xda, 0x5b, 0xd6), // magenta
            "y" => Color.FromArgb(0xb1, 0xc9, 0xc3), // grey
            "Y" => Color.FromArgb(0xff, 0xff, 0xff), // white
            "k" => Color.FromArgb(59, 15, 59), // black
            "K" => Color.FromArgb(0x15, 0x53, 0x52), // dark grey

            "o" => Color.FromArgb(0xf1, 0x5f, 0x22), // dark orange
            "O" => Color.FromArgb(0xe9, 0x9f, 0x10), // orange
            "w" => Color.FromArgb(0x98, 0x87, 0x5f), // brown
            "W" => Color.FromArgb(0xcf, 0xc0, 0x41), // gold / yellow

            "black" => Color.Black,
            "white" => Color.White,
            _ => throw new  ArgumentOutOfRangeException(nameof(color), color, null)
        };
        
    }
}
    