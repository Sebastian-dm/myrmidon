using System.Drawing;

using SDL3;

using Bramble.Core;
using Myrmidon.App.UI;
using Myrmidon.Core.Game;
using Myrmidon.Core.Entities;
using Myrmidon.Core.Maps.Tiles;

namespace Myrmidon.App.Render;

public class TerminalRenderer : IDisposable {
    
    
    public IntPtr Window => _window;
    public IntPtr Renderer => _renderer;

    private readonly TextureSheetManager _textureSheetManager = new TextureSheetManager();

    private static IntPtr _window;
    private static IntPtr _renderer;

    private int _tileWidth = 16;
    private int _tileHeight = 24;

    public int WindowWidtPix { get; private set;  }
    public int WindowHeightPix { get; private set;  }

    public int WindowWidthTiles => WindowWidtPix / _tileWidth;
    public int WindowHeightTiles => WindowHeightPix / _tileHeight;


    public TerminalRenderer(int widthTiles, int heightTiles) {
        if (!SDL.Init(SDL.InitFlags.Video))
            throw new InvalidOperationException("Failed to initialize SDL.");
        
        WindowWidtPix = widthTiles*_tileWidth;
        WindowHeightPix = heightTiles*_tileHeight;

        _window = SDL.CreateWindow("Myrmidon", WindowWidtPix, WindowHeightPix, SDL.WindowFlags.Resizable);

        //Check renderers available
        var renderDrivers = new List<string>();
        SDL.Log("Available render drivers:");
        for (int i = 0; i < SDL.GetNumRenderDrivers(); i++) {
            renderDrivers.Add(SDL.GetRenderDriver(i) ?? "N/A");
            SDL.Log(SDL.GetRenderDriver(i) ?? "N/A");
        }

        // Create renderer with Direct3D if available, otherwise use default
        _renderer = SDL.CreateRenderer(_window, null); //renderDrivers.Contains("opengl") ? "opengl" : null);
        if (_renderer == IntPtr.Zero)
            throw new InvalidOperationException("Failed to create SDL renderer.");
        else
            SDL.Log("Chosen renderer: " + SDL.GetRendererName(_renderer));
        
        SDL.SetRenderLogicalPresentation(_renderer, WindowWidtPix, WindowHeightPix, SDL.RendererLogicalPresentation.Letterbox);
        SDL.SetRenderVSync(_renderer, 1);


        TerminalColor.LoadColorsFromFile("qud");
    }

    public void BeginFrame() {
        SetRenderDrawColor("k");
        SDL.RenderClear(_renderer);
    }

    public void Present() {
        SDL.RenderPresent(_renderer);
    }

    public void SetRenderDrawColor(string color, byte? alpha = null) {
        Color c = TerminalColor.ColFromString(color);
        SDL.SetRenderDrawColor(_renderer, c.R, c.G, c.B, alpha ?? c.A);
    }

    public void FillRect(Rect rect, string color) {
        var background = TerminalColor.ColFromString(color);

        SDL.SetRenderDrawColor(
            Renderer,
            background.R,
            background.G,
            background.B,
            background.A);

        SDL.RenderFillRect(
            Renderer,
            GetPixelFRect(rect));
    }


    public void SetPanelArea(Rect panelRect) {
        var rect = GetPixelRect(panelRect);
        SDL.SetRenderViewport(_renderer, rect);
    }

    public void RenderFillRect(Rect panelRect, Rect fillRect) {
        
        var rect = new SDL.FRect {
            X = (panelRect.X + fillRect.X),
            Y = (panelRect.Y + fillRect.Y),
            W = fillRect.Width,
            H = fillRect.Height
        };
        var frect = GetPixelFRect(rect);
        SDL.RenderFillRect(Renderer, frect);
    }


    public void DrawText(Vec location, string text, string color) {
        for (int i = 0; i < text.Length; i++) {
            char ch = text[i];
            DrawGlyph(location.OffsetX(i), ch, color);
        }
    }

    public void DrawGlyph(Vec location, byte asciiIndex, string color) {
        if (asciiIndex == 0)
            return;
        TextureSheet textTextureSheet = _textureSheetManager.GetTextureSheet("text/default", Renderer);
        var srcFRect = textTextureSheet.GetRect(asciiIndex);
        var dstFRect = GetPixelFRect(new SDL.FRect { X = location.X, Y = location.Y, W = 1, H = 1 });
        Color col = TerminalColor.ColFromString(color);
        SDL.SetTextureColorMod(textTextureSheet.ForegroundTexture, col.R, col.G, col.B);
        SDL.RenderTexture(_renderer, textTextureSheet.ForegroundTexture, srcFRect, dstFRect);
    }

    public void DrawGlyph(Vec location, char character, string color) {
        DrawGlyph(location, (byte)character, color);
    }

    public void DrawTile(Vec location, string textureSheetName, byte textureIndex,
    string foregroundColor, string accentColor, string? backgroundColor = null) {

        TextureSheet textureSheet = _textureSheetManager.GetTextureSheet(textureSheetName, Renderer);
        var srcFRect = textureSheet.GetRect(textureIndex);
        var dstFRect = GetPixelFRect(new SDL.FRect { X = location.X, Y = location.Y, W = 1, H = 1 });


        // 1. Background
        if (backgroundColor != null) {
            Color background = TerminalColor.ColFromString(backgroundColor);
            SDL.SetRenderDrawColor(
                Renderer,
                background.R,
                background.G,
                background.B,
                background.A);
            SDL.RenderFillRect(Renderer, dstFRect);
        }

        // 2. Foreground
        Color foreground = TerminalColor.ColFromString(foregroundColor);
        SDL.SetTextureColorMod(
            textureSheet.ForegroundTexture,
            foreground.R,
            foreground.G,
            foreground.B);

        SDL.RenderTexture(
            Renderer,
            textureSheet.ForegroundTexture,
            srcFRect,
            dstFRect);

        // 3. Accent
        Color accent = TerminalColor.ColFromString(accentColor);
        SDL.SetTextureColorMod(
            textureSheet.AccentTexture,
            accent.R,
            accent.G,
            accent.B);

        SDL.RenderTexture(
            Renderer,
            textureSheet.AccentTexture,
            srcFRect,
            dstFRect);

    }




    private SDL.FRect GetPixelFRect(SDL.FRect rect) {
        return new SDL.FRect {
            X = rect.X * _tileWidth,
            Y = rect.Y * _tileHeight,
            W = rect.W * _tileWidth,
            H = rect.H * _tileHeight
        };
    }

    private SDL.FRect GetPixelFRect(Rect rect) {
        return new SDL.FRect {
            X = rect.X * _tileWidth,
            Y = rect.Y * _tileHeight,
            W = rect.Width * _tileWidth,
            H = rect.Height * _tileHeight
        };
    }

    private SDL.Rect GetPixelRect(Rect rect) {
        return new SDL.Rect {
            X = rect.X * _tileWidth,
            Y = rect.Y * _tileHeight,
            W = rect.Width * _tileWidth,
            H = rect.Height * _tileHeight
        };
    }


    public void Dispose() {
        SDL.DestroyWindow(_window);
        SDL.Quit();
    }

}
