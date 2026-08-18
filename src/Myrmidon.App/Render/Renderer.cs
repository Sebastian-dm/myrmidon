using System.Drawing;

using SDL3;

using Bramble.Core;
using Myrmidon.Core.Game;
using Myrmidon.Core.Entities;
using Myrmidon.Core.Maps.Tiles;

namespace Myrmidon.App.Render;

public class Renderer : IDisposable {
    
    private static IntPtr _window;
    private static IntPtr _renderer;
    private FpsCounter _fpsCounter;

    private List<ISubRenderer> _subRenderers = new List<ISubRenderer>();
        
    private GameState _gameState;
    
    public Renderer(FpsCounter fpsCounter,GameState gameState) {
        _fpsCounter = fpsCounter;
        _gameState = gameState;
        if (!SDL.Init(SDL.InitFlags.Video))
            throw new InvalidOperationException("Failed to initialize SDL.");

        _window = SDL.CreateWindow("Myrmidon", 320, 240, SDL.WindowFlags.Resizable);

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
        
        SDL.SetRenderLogicalPresentation(_renderer, 320, 240, SDL.RendererLogicalPresentation.Letterbox);
        SDL.SetRenderVSync(_renderer, 1);

        RegisterSubRenderer();
    }

    private void RegisterSubRenderer() {
        _subRenderers.Add(new SceneRenderer(_renderer, new SDL.Rect { X = 0, Y = 0, W = 280, H = 200 }, _gameState));
    }


    public void Render() {
        // Clear
        SDL.SetRenderViewport(_renderer, IntPtr.Zero);
        SetRenderDrawColor("DarkGray");
        SDL.RenderClear(_renderer);

        
        foreach (var subRenderer in _subRenderers) {
            subRenderer.Render();
        }

        DrawFpsText(1f, 1f, 2);
        
        SDL.RenderPresent(_renderer);
    }

    public static void SetRenderDrawColor(string color, byte? alpha = null) {
        Color c = TerminalColor.ToSystemColor(color);
        SDL.SetRenderDrawColor(_renderer, c.R, c.G, c.B, alpha ?? c.A);
    }
    

    private void DrawFpsText(float x, float y, int pad) {
        string fpsText = $"FPS: {_fpsCounter.Fps:F1}";
        SetRenderDrawColor("Black", 0x55);
        var rect = new SDL.FRect { X = x, Y = y, W = 2*pad+fpsText.Length*8-1, H = 2*pad+7 };
        SDL.RenderFillRect(_renderer, rect);
        SetRenderDrawColor("White");
        SDL.RenderDebugText(_renderer, x+pad, y+pad, fpsText);
    }
    
    
    private void DrawTestScreen(GameState context) {
        // _testSurface = SDL.LoadPNG("Images/Default.png");
        // // Render the scene
        // var texture = mTexture ?? throw new InvalidOperationException("Texture was not created.");
        // var ticks = SDL.GetTicks();
        // var direction = (ticks % 2000) >= 1000 ? 1.0f : -1.0f;
        // var scale = ((((int)(ticks % 1000)) - 500) / 500.0f) * direction;
        //
        // SDL.SetRenderDrawColor(_renderer, 255, 255, 255, 255);
        // SDL.RenderClear(_renderer);
        //
        // var dstRect = new SDL.FRect {
        //     X = 100.0f * scale,
        //     Y = 0.0f,
        //     W = texture.Width,
        //     H = texture.Height
        // };
        // SDL.RenderTexture(_renderer, texture.Handle, IntPtr.Zero, in dstRect);
        //
        // dstRect.X = (context.Width - texture.Width) / 2.0f;
        // dstRect.Y = (context.Height - texture.Height) / 2.0f;
        // SDL.RenderTexture(_renderer, texture.Handle, IntPtr.Zero, in dstRect);
        //
        // dstRect.X = context.Width - texture.Width - (100.0f * scale);
        // dstRect.Y = context.Height - texture.Height;
        // SDL.RenderTexture(_renderer, texture.Handle, IntPtr.Zero, in dstRect);
        //
        // SDL.RenderPresent(_renderer);
    }

    public void Dispose() {
        SDL.DestroyWindow(_window);
        SDL.Quit();
    }

}
