using SDL3;

namespace SdlSandbox;

internal static class Program {

    private static bool _running = true;
    private static IntPtr _window;
    private static IntPtr _renderer;
    private static IntPtr _surface;
    private static FpsCounter _fpsCounter;


    [STAThread]
    private static int Main(string[] args) {
        
        Initialize();
        MainLoop();
        Cleanup();
        SDL.Quit();

        return 0;
    }

    private static void Initialize() {
        if (!SDL.Init(SDL.InitFlags.Video))
            throw new InvalidOperationException("Failed to initialize SDL.");

        _running = true;
        _fpsCounter = new FpsCounter(60);
        SDL.CreateWindowAndRenderer("windowTitle",
            1200, 900,
            SDL.WindowFlags.Resizable,
            out _window, out _renderer);
        
        _surface = SDL.LoadPNG("Images/Default.png");
        
    }


    private static void MainLoop() {
        while (_running) {
            Tick();
            _fpsCounter.Update();
            var remainder = (uint)_fpsCounter.GetTickRemainderMs();
            SDL.Delay(remainder);
        }
    }

    private static void Tick() {
        // Do a frame
        Input();
        Update();
        Render();
    }

    private static void Input() {
        // Handle input events
        while (SDL.PollEvent(out SDL.Event e)) {
            switch (e.Type) {
                case (uint)SDL.EventType.Quit:
                    _running = false;
                    break;
                case (uint)SDL.EventType.KeyDown:
                    SDL.Log($"A key was pressed: {e.Key.Key}");
                    break;
            }
        }
    }

    private static void InputKeyboard() {
        var keys = SDL.GetKeyboardState(out var numKeys);

        if (keys[(int)SDL.Scancode.Escape])
            _running = false;
        if (keys[(int)SDL.Scancode.L] && keys[(int)SDL.Scancode.K]) {
            SDL.Log("L+K was pressed");
        }
    }

    private static void Update() {
        // Update game logic
    }

    private static void Render() {
        SDL.SetWindowTitle(_window, $"FPS: {(_fpsCounter.Fps).ToString()}");
        SDL.SetRenderDrawColor(_renderer, 200,200,200,255);
        SDL.RenderClear(_renderer);
        SDL.RenderPresent(_renderer);
        SDL.SetRenderScale(_renderer,100,100);
        SDL.SetRenderDrawColor(_renderer, 0,0,0,255);
        SDL.RenderDebugText(_renderer, 50,50,$"FPS: {_fpsCounter.Fps.ToString()}");
        SDL.RenderPresent(_renderer);
        
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
    }


    private static void Cleanup() {
        SDL.DestroySurface(_surface);
        SDL.DestroyWindow(_window);
    }

}
