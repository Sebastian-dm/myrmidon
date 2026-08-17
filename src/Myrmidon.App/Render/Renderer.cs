using Bramble.Core;
using Myrmidon.Core.Entities;
using Myrmidon.Core.Game;
using Myrmidon.Core.Maps.Tiles;
using SDL3;

namespace Myrmidon.App.Render;

public class Renderer : IDisposable {
    
    private static IntPtr _window;
    private static IntPtr _renderer;
    private static IntPtr _surface;
    private FpsCounter _fpsCounter;

    public Renderer(FpsCounter fpsCounter) {
        _fpsCounter = fpsCounter;
        if (!SDL.Init(SDL.InitFlags.Video))
            throw new InvalidOperationException("Failed to initialize SDL.");

        _window = SDL.CreateWindow("Myrmidon", 320, 240, SDL.WindowFlags.Resizable);

        //Check renderers available
        List<string> renderDrivers = new List<string>();
        SDL.Log("Available render drivers:");
        for (int i = 0; i < SDL.GetNumRenderDrivers(); i++) {
            renderDrivers.Add(SDL.GetRenderDriver(i));
            SDL.Log(SDL.GetRenderDriver(i));
        }

        // Create renderer with Direct3D if available, otherwise use default
        if (renderDrivers.Contains("opengl"))
            _renderer = SDL.CreateRenderer(_window, "opengl");
        else
            _renderer = SDL.CreateRenderer(_window, null);
        if (_renderer == IntPtr.Zero)
            throw new InvalidOperationException("Failed to create SDL renderer.");
        else
            SDL.Log("Chosen renderer: " + SDL.GetRendererName(_renderer));
        
        SDL.SetRenderLogicalPresentation(_renderer, 320, 240, SDL.RendererLogicalPresentation.Letterbox);
        SDL.SetRenderVSync(_renderer, 1);

        _surface = SDL.LoadPNG("Images/Default.png");
    }


    public void Render(GameState context) {
        string fpstext = $"FPS: {string.Format("{0:F1}", _fpsCounter.Fps)}";
        SDL.SetWindowTitle(_window, fpstext);

        // Clear
        SDL.SetRenderDrawColor(_renderer, 0xAA, 0xAA, 0xFF, 0xFF);
        SDL.RenderClear(_renderer);

        // Render the debug text
        //SDL.SetRenderScale(_renderer, 100, 100);
        SDL.SetRenderDrawColor(_renderer, 0x00, 0x00 , 0x00, 0xFF);
        SDL.RenderDebugText(_renderer, 0.1f, 0.1f, fpstext);


        // // Render the scene
        // var texture = mTexture ?? throw new InvalidOperationException("Texture was not created.");
        // var ticks = SDL.GetTicks();
        // var direction = (ticks % 2000) >= 1000 ? 1.0f : -1.0f;
        // var scale = ((((int)(ticks % 1000)) - 500) / 500.0f) * direction;
        //
        // SDL.SetRenderDrawColor(context.Renderer, 255, 255, 255, 255);
        // SDL.RenderClear(context.Renderer);
        //
        // var dstRect = new SDL.FRect {
        //     X = 100.0f * scale,
        //     Y = 0.0f,
        //     W = texture.Width,
        //     H = texture.Height
        // };
        // SDL.RenderTexture(context.Renderer, texture.Handle, IntPtr.Zero, in dstRect);
        //
        // dstRect.X = (context.Width - texture.Width) / 2.0f;
        // dstRect.Y = (context.Height - texture.Height) / 2.0f;
        // SDL.RenderTexture(context.Renderer, texture.Handle, IntPtr.Zero, in dstRect);
        //
        // dstRect.X = context.Width - texture.Width - (100.0f * scale);
        // dstRect.Y = context.Height - texture.Height;
        // SDL.RenderTexture(context.Renderer, texture.Handle, IntPtr.Zero, in dstRect);
        //
        // SDL.RenderPresent(context.Renderer);



        //
        // TerminalColor backgroundColor = TerminalColor.Black;
        //
        // //terminal.Clear();
        //
        // var map = context.Hectare.Map;
        // if (map == null) return;
        //
        // Vec center = new Vec(context.Hectare.Player.Position.X, context.Hectare.Player.Position.Y);
        // Rect viewBounds = new Rect(center - terminal.Size/2, terminal.Size);
        //
        // // Paint tiles
        // for (int y = viewBounds.Top; y < viewBounds.Bottom; y++) {
        //     for (int x = viewBounds.Left; x < viewBounds.Right; x++) {
        //         if (!IsInMapBounds(x, y, map)) continue;
        //
        //         var tile = map.GetTileAt<Tile>(x, y);
        //
        //         var screenPos = new Vec(x - viewBounds.Left, y - viewBounds.Top);
        //         terminal[screenPos.X, screenPos.Y][TerminalColor.Gray, backgroundColor].Write(tile.Glyph);
        //
        //     }
        // }
        //
        // //Paint entities
        // foreach (var entity in map.Entities.Items) {
        //
        //     if (entity is Actor actor) {
        //         if (!IsInMapBounds(actor.Position.X, actor.Position.Y, map)) continue;
        //         if (!IsInViewBounds(actor.Position.X, actor.Position.Y, viewBounds)) continue;
        //         int screenX = actor.Position.X - viewBounds.Left;
        //         int screenY = actor.Position.Y - viewBounds.Top;
        //         terminal[screenX, screenY][TerminalColor.ToSystemColor("LightRed"), backgroundColor].Write(actor.Glyph);
        //     }
        // }
        //
        // // Paint player
        // if (context.Hectare.Player != null) {
        //     var playerPos = new Vec(context.Hectare.Player.Position.X - viewBounds.Left, context.Hectare.Player.Position.Y - viewBounds.Top);
        //     terminal[playerPos.X, playerPos.Y][TerminalColor.LightGreen, backgroundColor].Write(context.Hectare.Player.Glyph);
        // }

        SDL.RenderPresent(_renderer);
    }

    private bool IsInViewBounds(int x, int y, Rect viewBounds) {

        return x >= viewBounds.Left && x < viewBounds.Right && y >= viewBounds.Top && y < viewBounds.Bottom;
    }


    public void Dispose() {
        SDL.DestroySurface(_surface);
        SDL.DestroyWindow(_window);
        SDL.Quit();
    }

}
