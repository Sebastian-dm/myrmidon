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

    public void RegisterPanel(Panel panel) {
        _panels.Add(panel);
    }
    

    public void SetRenderDrawColor(string color, byte? alpha = null) {
        Color c = TerminalColor.ToSystemColor(color);
        SDL.SetRenderDrawColor(_renderer, c.R, c.G, c.B, alpha ?? c.A);
    }

    public void SetPanelArea(Rect panelRect) {
        SDL.SetRenderViewport(_renderer, GetPixelRect(panelRect));
    }

    public void RenderFillRect(Rect panelRect, Rect fillRect) {
        
        var rect = new SDL.Rect {
            X = (panelRect.X + fillRect.X),
            Y = (panelRect.Y + fillRect.Y),
            W = fillRect.Width,
            H = fillRect.Height
        };
        SDL.RectToFRect(GetPixelRect(rect), out var frect);
        SDL.RenderFillRect(Renderer, frect);
    }
    
    
    private SDL.Rect GetPixelRect(SDL.Rect rect) {
        return new SDL.Rect {
            X = rect.X * _tileWidth,
            Y = rect.Y * _tileHeight,
            W = rect.W * _tileWidth,
            H = rect.W * _tileHeight
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
