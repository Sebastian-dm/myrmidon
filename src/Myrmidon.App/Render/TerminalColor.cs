using System.Drawing;

namespace Myrmidon.Terminal;

public static class TerminalColor {
    
    public static Color ToSystemColor(string color) {
        color = color.ToLower();
        return color switch {
            "black" => Color.Black,
            "white" => Color.White,
            "lightgray" => Color.FromArgb(192, 192, 192),
            "gray" => Color.FromArgb(128, 128, 128),
            "darkgray" => Color.FromArgb(48, 48, 48),
            "Pink" => Color.FromArgb(255, 160, 160),
            "yRed" => Color.FromArgb(220, 0, 0),
            "DarkRed" => Color.FromArgb(100, 0, 0),
            "Flesh" => Color.FromArgb(255, 200, 170),
            "Orange" => Color.FromArgb(255, 128, 0),
            "DarkOrange" => Color.FromArgb(128, 64, 0),
            "LightGold" => Color.FromArgb(255, 230, 150),
            "Gold" => Color.FromArgb(255, 192, 0),
            "DarkGold" => Color.FromArgb(128, 96, 0),
            "LightYellow" => Color.FromArgb(255, 255, 150),
            "Yellow" => Color.FromArgb(255, 255, 0),
            "DarkYellow" => Color.FromArgb(128, 128, 0),
            "LightGreen" => Color.FromArgb(130, 255, 90),
            "Green" => Color.FromArgb(0, 200, 0),
            "DarkGreen" => Color.FromArgb(0, 100, 0),
            "LightCyan" => Color.FromArgb(200, 255, 255),
            "Cyan" => Color.FromArgb(0, 255, 255),
            "DarkCyan" => Color.FromArgb(0, 128, 128),
            "LightBlue" => Color.FromArgb(128, 160, 255),
            "Blue" => Color.FromArgb(0, 64, 255),
            "DarkBlue" => Color.FromArgb(0, 37, 168),
            "LightPurple" => Color.FromArgb(200, 140, 255),
            "Purple" => Color.FromArgb(128, 0, 255),
            "DarkPurple" => Color.FromArgb(64, 0, 128),
            "LightBrown" => Color.FromArgb(190, 150, 100),
            "Brown" => Color.FromArgb(160, 110, 60),
            "DarkBrown" => Color.FromArgb(100, 64, 32),
            _ => throw new  ArgumentOutOfRangeException(nameof(color), color, null)
        };
    }
}
    