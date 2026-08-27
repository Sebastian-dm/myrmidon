using SDL3;
using System.Drawing;
using System.Text.Json;
using static SDL3.SDL;


namespace Myrmidon.App.Render;

public class TextureSheet : IDisposable {

    public string Id { get; private set; }


    public nint ForegroundTexture { get; private set; }
    public nint AccentTexture { get; private set; }

    private int _textureWidth;
    private int _textureHeight;

    private readonly int _textureColumns;
    private readonly int _textureRows;
    private readonly int _asciiOffset;

    private readonly nint _renderer;
    
    
    public TextureSheet(string id, nint renderer) {

        string AssetsFolder = "../../../../../assets/";

        Id = id;
        _renderer = renderer;
        
        var configText = File.ReadAllText($"{AssetsFolder}/textures/{id}.json");
        var config = JsonSerializer.Deserialize<TextureSheetConfig>(configText)
            ?? throw new InvalidOperationException($"Couldn't load sprite sheet config for {id}");
        
        _textureColumns = config.TextureColumns;
        _textureRows = config.TextureRows;
        _asciiOffset = config.AsciiOffset;
        
        (ForegroundTexture, AccentTexture) = LoadAtlasTexturesFromFile($"{AssetsFolder}/textures/{id}.png");
    }

    public SDL.FRect GetRect(byte index) {
        // TODO: Figure the best size out to get exactly the tile pixel perfect
        int column = index % _textureColumns;
        int row = index / _textureRows;
        var frect = new SDL.FRect {
            X = _textureWidth * column,
            Y = _textureHeight * row,
            W = _textureWidth -0.4f,
            H = _textureHeight -0.4f
        };
        var bytearray = new byte[1]{index};
        var rectString = $"X: {frect.X}, Y: {frect.Y}, W: {frect.W}, H: {frect.H}, ASCII:{System.Text.Encoding.ASCII.GetString(bytearray)}";
        return frect;
    }



    private (nint, nint) LoadAtlasTexturesFromFile(string filename) {

        var srcSrfPtr = SDL.LoadPNG(filename);
        if (srcSrfPtr == IntPtr.Zero)
            throw new InvalidOperationException($"Couldn't load bitmap: {SDL.GetError()}");
        
        var srcSrf = SDL.PointerToStructure<SDL.Surface>(srcSrfPtr) ?? default;
        _textureWidth = (int)(srcSrf.Width / _textureColumns);
        _textureHeight = (int)(srcSrf.Height / _textureRows);

        var foregroundSrf = SDL.CreateSurface(
            srcSrf.Width,
            srcSrf.Height,
            SDL.PixelFormat.RGBA64);

        var accentSrf = SDL.CreateSurface(
            srcSrf.Width,
            srcSrf.Height,
            SDL.PixelFormat.RGBA64);

        for (int y = 0; y < srcSrf.Height; y++) {
            for (int x = 0; x < srcSrf.Width; x++) {
                byte r, g, b, a;

                if (!SDL.ReadSurfacePixel(
                        srcSrfPtr,
                        x,
                        y,
                        out r,
                        out g,
                        out b,
                        out a)) {
                    throw new Exception(SDL.GetError());
                }

                // Transparent source = transparent in both masks.
                if (a == 0) {
                    SDL.WriteSurfacePixel(
                        foregroundSrf, x, y,
                        255, 255, 255, 0);

                    SDL.WriteSurfacePixel(
                        accentSrf, x, y,
                        255, 255, 255, 0);

                    continue;
                }

                bool isAccent = r > 127;

                if (isAccent) {
                    // Original white pixel.
                    SDL.WriteSurfacePixel(
                        foregroundSrf, x, y,
                        255, 255, 255, 0);

                    SDL.WriteSurfacePixel(
                        accentSrf, x, y,
                        255, 255, 255, 255);
                }
                else {
                    // Original black pixel.
                    SDL.WriteSurfacePixel(
                        foregroundSrf, x, y,
                        255, 255, 255, 255);

                    SDL.WriteSurfacePixel(
                        accentSrf, x, y,
                        255, 255, 255, 0);
                }
            }
        }

        var foregroundTexture = SDL.CreateTextureFromSurface(_renderer, foregroundSrf);
        var accentTexture = SDL.CreateTextureFromSurface(_renderer, accentSrf);

        SDL.DestroySurface(foregroundSrf);
        SDL.DestroySurface(accentSrf);

        if (foregroundTexture == null || accentTexture == null)
            throw new Exception(SDL.GetError());

        return ((nint)foregroundTexture, (nint)accentTexture);
    }






    public void Dispose() {
        SDL.DestroyTexture(ForegroundTexture);
        SDL.DestroyTexture(AccentTexture);
    }
    
}


public class TextureSheetConfig {
    public int TextureColumns { get; set; } = 32;
    public int TextureRows { get; set; } = 6;
    public int AsciiOffset { get; set; } = 32; // default to ASCII offset

}