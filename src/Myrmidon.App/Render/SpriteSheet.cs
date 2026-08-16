using System.Drawing;
using System.Text.Json;

using SDL3;
using Malison.Core;


namespace Myrmidon.App.Render;

public class SpriteSheet : IDisposable {

    public string Id { get; private set; }

    private readonly int _spriteWidth;
    private readonly int _spriteHeight;

    private readonly int _spritesPerRow;
    private readonly int _spriteRows;
    private readonly int _asciiOffset;

    private readonly IntPtr _renderer;
    private readonly IntPtr _texture;
    
    
    public SpriteSheet(string id, IntPtr renderer) {
        Id = id;
        _renderer = renderer;
        
        var configText = File.ReadAllText($"images/{id}.json");
        var config = JsonSerializer.Deserialize<SpriteSheetConfig>(configText)
            ?? throw new InvalidOperationException($"Couldn't load sprite sheet config for {id}");
        
        _spritesPerRow = config.SpritesPerRow;
        _spriteRows = config.SpriteRows;
        _asciiOffset = config.AsciiOffset;
        
        
        _texture = LoadTextureFromFile($"images/{id}.png");
        
        var textureProps = SDL.GetTextureProperties(_texture);
        _spriteWidth = (int)SDL.GetNumberProperty(textureProps,"SDL_PROP_TEXTURE_WIDTH_NUMBER", -1);
        _spriteHeight = (int)SDL.GetNumberProperty(textureProps,"SDL_PROP_TEXTURE_HEIGHT_NUMBER", -1);
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

    
    public void Draw(IntPtr renderer, int x, int y, Character character) {
        // don't draw if it's a blank Sprite
        if (character.Glyph == Glyph.Space) return;
        
        byte glyph = (byte)character.Glyph;
        int column = glyph % _spritesPerRow;
        int row = glyph / _spritesPerRow;
        var srcRect = new SDL.FRect {
            X = _spriteWidth*column,
            Y =_spriteHeight*row,
            W = column,
            H = row
        };
        
        //Remap colors
        // SDL.Surface frame = SDL.PointerToStructure<SDL.Surface>(_sheetSurface) ?? default;
        // SDL.SetSurfaceColorKey(_sheetSurface, true, SDL.MapRGB(SDL.GetPixelFormatDetails(frame.Format), IntPtr.Zero, 255, 0, 255));
        // Todo: Implement color remapping based on character.ForeColor and character.BackColor
        
        var destRect = new SDL.FRect {
            X = x,
            Y = y,
            W = _spriteWidth,
            H = _spriteHeight
        };

        SDL.RenderTexture(renderer, _texture, srcRect, destRect);
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