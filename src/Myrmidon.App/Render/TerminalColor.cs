using System.Drawing;
using System.Globalization;
using System.Text.Json;

namespace Myrmidon.App.Render;

public static class TerminalColor {
    private static Dictionary<string, RgbColor> _colors = new(StringComparer.Ordinal);

    public static void LoadColorsFromFile(string id) {
        string path = Path.Combine(
            "../../../../../assets",
            "colors",
            $"{id}.json");

        using JsonDocument document = JsonDocument.Parse(
            File.ReadAllText(path));

        JsonElement root = document.RootElement;

        string format = root.TryGetProperty("format", out JsonElement formatElement)
            ? formatElement.GetString()?.ToLowerInvariant()
                ?? throw new InvalidOperationException(
                    $"The color palette format is invalid: {id}")
            : "rgb";

        if (format is not ("rgb" or "hex")) {
            throw new InvalidOperationException(
                $"Unsupported color palette format '{format}'. " +
                "Expected 'rgb' or 'hex'.");
        }

        var colors = new Dictionary<string, RgbColor>(StringComparer.Ordinal);

        foreach (JsonProperty property in root.EnumerateObject()) {
            if (property.NameEquals("format")) {
                continue;
            }

            RgbColor color = format switch {
                "hex" => RgbColor.FromHex(property.Value),
                "rgb" => JsonSerializer.Deserialize<RgbColor>(
                    property.Value.GetRawText(),
                    new JsonSerializerOptions {
                        PropertyNameCaseInsensitive = true
                    }) ?? throw new InvalidOperationException(
                        $"Color '{property.Name}' is invalid."),
                _ => throw new InvalidOperationException(
                    $"Unsupported color palette format '{format}'.")
            };

            colors[property.Name] = color;
        }

        _colors = colors;
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


        public static RgbColor FromHex(JsonElement element) {
            if (element.ValueKind != JsonValueKind.String) {
                throw new InvalidOperationException(
                    "Hex colors must be JSON strings.");
            }

            string? value = element.GetString();

            if (value is null ||
                value.Length != 7 ||
                value[0] != '#') {
                throw new InvalidOperationException(
                    $"Invalid hex color '{value}'. Expected '#RRGGBB'.");
            }

            try {
                return new RgbColor {
                    Red = byte.Parse(
                        value.AsSpan(1, 2),
                        NumberStyles.HexNumber,
                        CultureInfo.InvariantCulture),
                    Green = byte.Parse(
                        value.AsSpan(3, 2),
                        NumberStyles.HexNumber,
                        CultureInfo.InvariantCulture),
                    Blue = byte.Parse(
                        value.AsSpan(5, 2),
                        NumberStyles.HexNumber,
                        CultureInfo.InvariantCulture)
                };
            }
            catch (FormatException exception) {
                throw new InvalidOperationException(
                    $"Invalid hex color '{value}'. Expected '#RRGGBB'.",
                    exception);
            }
        }
    }


    public static Color ColFromString(string color) {
        if (color == "") {
            color = "k";
        }

        ArgumentException.ThrowIfNullOrEmpty(color);

        if (!_colors.TryGetValue(color, out RgbColor? rgb)) {
            throw new ArgumentOutOfRangeException(
                nameof(color),
                color,
                $"Color '{color}' is not defined in the current palette.");
        }

        return rgb.ToColor();
    }
}