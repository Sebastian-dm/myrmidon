using System.Drawing;
using System.Text.Json;

using SDL3;


namespace Myrmidon.App.Render;

public class TextureSheet : IDisposable {

    public string Id { get; private set; }
    private readonly IntPtr _texture;


    public IntPtr Texture => _texture;

    private readonly int _spriteWidth;
    private readonly int _spriteHeight;

    private readonly int _spritesPerRow;
    private readonly int _spriteRows;
    private readonly int _asciiOffset;

    private readonly IntPtr _renderer;
    
    
    public TextureSheet(string id, IntPtr renderer) {

        string AssetsFolder = "../../../../../assets/";

        Id = id;
        _renderer = renderer;
        
        var configText = File.ReadAllText($"{AssetsFolder}/textures/{id}.json");
        var config = JsonSerializer.Deserialize<SpriteSheetConfig>(configText)
            ?? throw new InvalidOperationException($"Couldn't load sprite sheet config for {id}");
        
        _spritesPerRow = config.SpritesPerRow;
        _spriteRows = config.SpriteRows;
        _asciiOffset = config.AsciiOffset;
        
        
        _texture = LoadTextureFromFile($"{AssetsFolder}/textures/{id}.png");
        
        var textureProps = SDL.GetTextureProperties(_texture);
        _spriteWidth = (int)SDL.GetNumberProperty(textureProps,"SDL_PROP_TEXTURE_WIDTH_NUMBER", -1);
        _spriteHeight = (int)SDL.GetNumberProperty(textureProps,"SDL_PROP_TEXTURE_HEIGHT_NUMBER", -1);

        var a = 0;
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
        int column = index % _spritesPerRow;
        int row = index / _spritesPerRow;
        var frect = new SDL.FRect {
            X = _spriteWidth * column,
            Y = _spriteHeight * row,
            W = _spriteWidth,
            H = _spriteHeight
        };
        var rectString = $"X: {frect.X}, Y: {frect.Y}, W: {frect.W}, H: {frect.H}";
        return frect;
    }

    public void Dispose() {
        SDL.DestroySurface(_texture);
    }
    
}


public class SpriteSheetConfig {
    public int SpritesPerRow { get; set; } = 32;
    public int SpriteRows { get; set; } = 6;
    public int AsciiOffset { get; set; } = 32; // default to ASCII offset

}