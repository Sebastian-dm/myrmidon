using SDL3;

namespace SdlSandbox;

internal static class Program {

    private static IntPtr _window;
    private static BitmapTexture? _texture;
    private static RenderContext? _context;

    [STAThread]
    private static int Main(string[] args) {
        
        if (!SDL.Init(SDL.InitFlags.Video))
            return -1;


        _context = RenderContext.Create(
            "appName",
            "appIdentifier",
            "windowTitle",
            1200,
            900);
        //_texture = BitmapTexture.Create(_context.Renderer, 128, 128, "Images/Default.png");
        
        var keys = SDL.GetKeyboardState(out var numKeys);

        var running = true;

        while (running) {
            // Input handling
            while (SDL.PollEvent(out var e)) {
                switch (e.Type) {
                    case (uint)SDL.EventType.Quit:
                        running = false;
                        break;
                    case (uint)SDL.EventType.KeyDown:
                        SDL.Log($"A key was pressed: {e.Key.Key}");
                        break;
                }
            }
            
            if (keys[(int)SDL.Scancode.L] && keys[(int)SDL.Scancode.K]) {
                SDL.Log("L+K was pressed");
            }
            
            // Rendering
            //RendertextureTestFrame(_context);
            
            // Game loop
            
            
            SDL.Delay(16);
        }
        
        
        _context.Dispose();
        Cleanup();
        SDL.Quit();

        return 0;
    }

    
    
    
    private static void RendertextureTestFrame(RenderContext context) {

        var texture = _texture ?? throw new InvalidOperationException("Texture was not created.");
        var ticks = SDL.GetTicks();
        var direction = (ticks % 2000) >= 1000 ? 1.0f : -1.0f;
        var scale = ((((int)(ticks % 1000)) - 500) / 500.0f) * direction;

        SDL.SetRenderDrawColor(context.Renderer, 255, 255, 255, 255);
        SDL.RenderClear(context.Renderer);

        var dstRect = new SDL.FRect {
            X = 100.0f * scale,
            Y = 0.0f,
            W = texture.Width,
            H = texture.Height
        };
        SDL.RenderTexture(context.Renderer, texture.Handle, IntPtr.Zero, in dstRect);

        dstRect.X = (context.Width - texture.Width) / 2.0f;
        dstRect.Y = (context.Height - texture.Height) / 2.0f;
        SDL.RenderTexture(context.Renderer, texture.Handle, IntPtr.Zero, in dstRect);

        dstRect.X = context.Width - texture.Width - (100.0f * scale);
        dstRect.Y = context.Height - texture.Height;
        SDL.RenderTexture(context.Renderer, texture.Handle, IntPtr.Zero, in dstRect);

        SDL.RenderPresent(context.Renderer);
    }

    private static void Cleanup() {
        _texture?.Dispose();
        _texture = null;
    }

}
