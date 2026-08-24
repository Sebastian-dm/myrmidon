using System.Drawing;

using SDL3;

using Bramble.Core;
using Myrmidon.Core.Game;
using Myrmidon.Core.Entities;
using Myrmidon.Core.Maps.Tiles;

namespace Myrmidon.App.Render;

public class Terminal : IDisposable {
    
    
    public IntPtr Window => _window;
    public IntPtr Renderer => _renderer;

    private readonly TextureSheetManager _textureSheetManager = new TextureSheetManager();

    private static IntPtr _window;
    private static IntPtr _renderer;
    private FpsCounter _fpsCounter;

    private int _tileWidth = 16;
    private int _tileHeight = 24;
    private int _windowWidth;
    private int _windowHeight;


    private List<IPanel> _panels;
    
    
    public Terminal(FpsCounter fpsCounter,  int widthTiles, int heightTiles) {
        _fpsCounter = fpsCounter;
        if (!SDL.Init(SDL.InitFlags.Video))
            throw new InvalidOperationException("Failed to initialize SDL.");
        
        _windowWidth = widthTiles*_tileWidth;
        _windowHeight = heightTiles*_tileHeight;

        _window = SDL.CreateWindow("Myrmidon", _windowWidth, _windowHeight, SDL.WindowFlags.Resizable);

        //Check renderers available
        var renderDrivers = new List<string>();
        SDL.Log("Available render drivers:");
        for (int i = 0; i < SDL.GetNumRenderDrivers(); i++) {
            renderDrivers.Add(SDL.GetRenderDriver(i) ?? "N/A");
            SDL.Log(SDL.GetRenderDriver(i) ?? "N/A");
        }

        // Create renderer with Direct3D if available, otherwise use default
        _renderer = SDL.CreateRenderer(_window, renderDrivers.Contains("opengl") ? "opengl" : null);
        if (_renderer == IntPtr.Zero)
            throw new InvalidOperationException("Failed to create SDL renderer.");
        else
            SDL.Log("Chosen renderer: " + SDL.GetRendererName(_renderer));
        
        SDL.SetRenderLogicalPresentation(_renderer, _windowWidth, _windowHeight, SDL.RendererLogicalPresentation.Letterbox);
        SDL.SetRenderVSync(_renderer, 1);
        
        _panels = new List<IPanel>();
    }

    public void Render() {
        // Clear
        SetRenderDrawColor("DarkGray");
        SDL.RenderClear(_renderer);

        foreach (var panel in _panels) {
            panel.Render();
        }
        
        DrawFpsText(1f, 1f, 2);
        
        SDL.RenderPresent(_renderer);
    }

    public void RegisterPanel(GridPanel panel) {
        _panels.Add(panel);
    }
    

    public void SetRenderDrawColor(string color, byte? alpha = null) {
        Color c = TerminalColor.ColFromString(color);
        SDL.SetRenderDrawColor(_renderer, c.R, c.G, c.B, alpha ?? c.A);
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
        SDL.SetTextureColorMod(textTextureSheet.Texture, col.R, col.G, col.B);
        SDL.RenderTexture(_renderer, textTextureSheet.Texture, srcFRect, dstFRect);
    }

    public void DrawGlyph(Vec location, char character, string color) {
        DrawGlyph(location, (byte)character, color);
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
    
    

    private void DrawFpsText(float x, float y, int pad) {
        string fpsText = $"FPS: {_fpsCounter.Fps:F1}";
        SetRenderDrawColor("Black", 0x55);
        var rect = new SDL.FRect { X = x, Y = y, W = 2*pad+fpsText.Length*8-1, H = 2*pad+7 };
        SDL.RenderFillRect(_renderer, rect);
        SetRenderDrawColor("White");
        SDL.RenderDebugText(_renderer, x+pad, y+pad, fpsText);
    }


    public void Dispose() {
        SDL.DestroyWindow(_window);
        SDL.Quit();
    }

}
