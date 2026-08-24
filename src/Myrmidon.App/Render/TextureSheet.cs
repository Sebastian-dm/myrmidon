using System.Drawing;
using System.Text.Json;

using SDL3;


namespace Myrmidon.App.Render;

public class TextureSheet : IDisposable {

    public string Id { get; private set; }
    private readonly IntPtr _texture;


    public IntPtr Texture => _texture;

    private readonly int _textureWidth;
    private readonly int _textureHeight;

    private readonly int _textureColumns;
    private readonly int _textureRows;
    private readonly int _asciiOffset;

    private readonly IntPtr _renderer;
    
    
    public TextureSheet(string id, IntPtr renderer) {

        string AssetsFolder = "../../../../../assets/";

        Id = id;
        _renderer = renderer;
        
        var configText = File.ReadAllText($"{AssetsFolder}/textures/{id}.json");
        var config = JsonSerializer.Deserialize<TextureSheetConfig>(configText)
            ?? throw new InvalidOperationException($"Couldn't load sprite sheet config for {id}");
        
        _textureColumns = config.TextureColumns;
        _textureRows = config.TextureRows;
        _asciiOffset = config.AsciiOffset;
        
        
        _texture = LoadTextureFromFile($"{AssetsFolder}/textures/{id}.png");
        
        var textureProps = SDL.GetTextureProperties(_texture);
        var sheetWidth = SDL.GetNumberProperty(textureProps,SDL.Props.TextureWidthNumber, -1);
        var sheetHeight = SDL.GetNumberProperty(textureProps,SDL.Props.TextureHeightNumber, -1);
        
        _textureWidth = (int)(sheetWidth / _textureColumns);
        _textureHeight = (int)(sheetHeight / _textureRows);
    }

    
    private IntPtr LoadTextureFromFile(string filename) {
        var surface = SDL.LoadPNG(filename);
        if (surface == IntPtr.Zero)
            throw new InvalidOperationException($"Couldn't load bitmap: {SDL.GetError()}");
        var texture = SDL.CreateTextureFromSurface(_renderer, surface);
        SDL.DestroySurface(surface);
        SDL.SetTextureScaleMode(_texture, SDL.ScaleMode.Nearest);
        return texture;
    }

    public SDL.FRect GetRect(byte index) {
        int column = index % _textureColumns;
        int row = index / _textureRows;
        var frect = new SDL.FRect {
            X = _textureWidth * column,
            Y = _textureHeight * row,
            W = _textureWidth,
            H = _textureHeight
        };
        var bytearray = new byte[1]{index};
        var rectString = $"X: {frect.X}, Y: {frect.Y}, W: {frect.W}, H: {frect.H}, ASCII:{System.Text.Encoding.ASCII.GetString(bytearray)}";
        return frect;
    }

    public void Dispose() {
        SDL.DestroySurface(_texture);
    }
    
}


public class TextureSheetConfig {
    public int TextureColumns { get; set; } = 32;
    public int TextureRows { get; set; } = 6;
    public int AsciiOffset { get; set; } = 32; // default to ASCII offset

}