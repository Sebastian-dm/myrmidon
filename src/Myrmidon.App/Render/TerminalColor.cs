using System.Drawing;
using System.Text.Json;

namespace Myrmidon.App.Render;

public static class TerminalColor {


    private static Dictionary<string, RgbColor> _colors = new(StringComparer.Ordinal);


    public static void LoadColorsFromFile(string id) {
        string path = Path.Combine(
            "../../../../../assets",
            "colors",
            $"{id}.json");

        string configText = File.ReadAllText(path);

        _colors = JsonSerializer.Deserialize<Dictionary<string, RgbColor>>(
            configText,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            ?? throw new InvalidOperationException(
                $"Couldn't load color palette: {id}");
        var a = "";
    }



    private sealed class RgbColor {
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }

        public Color ToColor() {
            if (Red is < 0 or > 255 ||
                Green is < 0 or > 255 ||
                Blue is < 0 or > 255) {
                throw new InvalidOperationException(
                    $"Invalid RGB value: ({Red}, {Green}, {Blue}).");
            }

            return Color.FromArgb(Red, Green, Blue);
        }
    }

    public static Color ColFromString(string color) {
        if (color == "") color = "k";
        ArgumentException.ThrowIfNullOrEmpty(color);
        if (!_colors.TryGetValue(color, out RgbColor? rgb))
            throw new ArgumentOutOfRangeException(nameof(color),color, $"Color '{color}' is not defined in the current palette.");

        return rgb.ToColor();
    }
}
    