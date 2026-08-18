using System.Drawing;

namespace Myrmidon.App.Render;

public static class TerminalColor {
    
    public static Color ToSystemColor(string color) {
        color = color.ToLower();
        return color switch {
            "black" => Color.Black,
            "white" => Color.White,
            "lightgray" => Color.FromArgb(192, 192, 192),
            "gray" => Color.FromArgb(128, 128, 128),
            "darkgray" => Color.FromArgb(48, 48, 48),
            "pink" => Color.FromArgb(255, 160, 160),
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
    