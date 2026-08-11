using System.Drawing;
using System.Text.Json;

using Malison.Core;
using SDL3;
using SdlSandbox;

namespace Myrmidon.Terminal;

public class SpriteSheet : IDisposable {

    public string Id { get; private set; }
    public int Width { get { return _bitmap.Width / _spritesPerRow; } }
    public int Height { get { return _bitmap.Height / _spriteRows; } }

    private int _spritesPerRow;
    private int _spriteRows;
    private int _asciiOffset;
    
    private IntPtr _sheetSurface;
    private Dictionary<Character, BitmapTexture> _characterCache;
    
    
    public SpriteSheet(string id) {
        Id = id;
        var configText = File.ReadAllText($"images/{id}.json");
        var config = JsonSerializer.Deserialize<SpriteSheetConfig>(configText)
            ?? throw new InvalidOperationException($"Couldn't load sprite sheet config for {id}");
        
        _spritesPerRow = config.SpritesPerRow;
        _spriteRows = config.SpriteRows;
        _asciiOffset = config.AsciiOffset;

        var bitMapFileName = $"images/{id}.png";
        _sheetSurface = SDL.LoadPNG(bitMapFileName);

        _characterCache = new Dictionary<Character, BitmapTexture>();
    }


    public BitmapTexture GetTexture(Character character) {

        // use the previously cached one if there
        BitmapTexture characterTexture;
        if (_characterCache.TryGetValue(character, out var texture)) {
            return texture;
        }

        // not there, so create it
        byte glyph = (byte)character.Glyph;
        int column = glyph % _spritesPerRow;
        int row = glyph / _spritesPerRow;

        Rectangle destRect = new Rectangle(0, 0, Width, Height);

        //Remap colors
        SDL.Surface frame = SDL.PointerToStructure<SDL.Surface>(_sheetSurface) ?? default;
        SDL.SetSurfaceColorKey(_sheetSurface, true, SDL.MapRGB(SDL.GetPixelFormatDetails(frame.Format), IntPtr.Zero, 255, 0, 255));
        // Todo: Implement color remapping based on character.ForeColor and character.BackColor

        characterTexture = BitmapTexture.Create(SDL.GetRenderer(SDL.GetWindowFromSurface(_sheetSurface)), Width, Height);
        g.DrawImage(_bitmap, destRect,
            column * Width, row * Height, Width, Height,
            GraphicsUnit.Pixel, attributes);

        // cache it
        _characterCache[character] = characterTexture;

        return characterTexture;
    }

    public void Draw(RenderContext context, int x, int y, Character character) {
        // don't draw if it's a blank Sprite
        if (character.Glyph == Glyph.Space) return;
        
        var characterTexture = GetTexture(character);
        
        var destRect = new SDL.FRect {
            X = x,
            Y = y,
            W = characterTexture.Width,
            H = characterTexture.Height
        };

        SDL.RenderTexture(context.Renderer, characterTexture.Handle, IntPtr.Zero, destRect);
    }

    public void Dispose() {
        SDL.DestroySurface(_sheetSurface);
    }
    
}


public class SpriteSheetConfig {
    
    public int SpriteWidth { get; set; }
    public int SpriteHeight { get; set; }
    public int SpritesPerRow { get; set; } = 32;
    public int SpriteRows { get; set; } = 6;
    public int AsciiOffset { get; set; } = 32; // default to ASCII offset

}