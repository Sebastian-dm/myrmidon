using System.Drawing;

namespace Myrmidon.App.Render;

public static class TerminalColor {
    
    public static Color ColFromString(string color) {
        color = color.ToLower();
        return color switch {
            "r" => Color.FromArgb(0xa6, 0x4a, 0x2e),
            "R" => Color.FromArgb(0xd7, 0x42, 0x00), // red / scarlet
            "o" => Color.FromArgb(0xf1, 0x5f, 0x22), // dark orange
            "O" => Color.FromArgb(0xe9, 0x9f, 0x10), // orange
            "w" => Color.FromArgb(0x98, 0x87, 0x5f), // brown
            "W" => Color.FromArgb(0xcf, 0xc0, 0x41), // gold / yellow
            "g" => Color.FromArgb(0x00, 0x94, 0x03), // dark green
            "G" => Color.FromArgb(0x00, 0xc4, 0x20), // green
            "b" => Color.FromArgb(0x00, 0x48, 0xbd), // dark blue
            "B" => Color.FromArgb(0x00, 0x96, 0xff), // blue / azure
            "c" => Color.FromArgb(0x40, 0xa4, 0xb9), // dark cyan / teal
            "C" => Color.FromArgb(0x77, 0xbf, 0xcf), // cyan
            "m" => Color.FromArgb(0xb1, 0x54, 0xcf), // dark magenta / purple
            "M" => Color.FromArgb(0xda, 0x5b, 0xd6), // magenta
            "k" => Color.FromArgb(0x0f, 0x3b, 0x3a), //
            "K" => Color.FromArgb(0x15, 0x53, 0x52), // dark grey / black
            "y" => Color.FromArgb(0xb1, 0xc9, 0xc3), // grey
            "Y" => Color.FromArgb(0xff, 0xff, 0xff), // white
            "black" => Color.Black,
            "white" => Color.White,
            "lightgray" => Color.FromArgb(192, 192, 192),
            "gray" => Color.FromArgb(128, 128, 128),
            "darkgray" => Color.FromArgb(48, 48, 48),
            "pink" => Color.FromArgb(255, 160, 160),
            "red" => Color.FromArgb(255, 0, 0),
            "darkred" => Color.FromArgb(100, 0, 0),
            "flesh" => Color.FromArgb(255, 200, 170),
            "orange" => Color.FromArgb(255, 128, 0),
            "darkorange" => Color.FromArgb(128, 64, 0),
            "lightgold" => Color.FromArgb(255, 230, 150),
            "gold" => Color.FromArgb(255, 192, 0),
            "darkgold" => Color.FromArgb(128, 96, 0),
            "lightyellow" => Color.FromArgb(255, 255, 150),
            "yellow" => Color.FromArgb(255, 255, 0),
            "darkyellow" => Color.FromArgb(128, 128, 0),
            "lightgreen" => Color.FromArgb(130, 255, 90),
            "green" => Color.FromArgb(0, 200, 0),
            "darkgreen" => Color.FromArgb(0, 100, 0),
            "lightcyan" => Color.FromArgb(200, 255, 255),
            "cyan" => Color.FromArgb(0, 255, 255),
            "darkcyan" => Color.FromArgb(0, 128, 128),
            "lightblue" => Color.FromArgb(128, 160, 255),
            "blue" => Color.FromArgb(0, 64, 255),
            "darkblue" => Color.FromArgb(0, 37, 168),
            "lightpurple" => Color.FromArgb(200, 140, 255),
            "purple" => Color.FromArgb(128, 0, 255),
            "darkpurple" => Color.FromArgb(64, 0, 128),
            "lightbrown" => Color.FromArgb(190, 150, 100),
            "brown" => Color.FromArgb(160, 110, 60),
            "darkbrown" => Color.FromArgb(100, 64, 32),
            _ => throw new  ArgumentOutOfRangeException(nameof(color), color, null)
        };
        
    }
}
    