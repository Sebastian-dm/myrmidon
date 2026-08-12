using SDL3;

namespace SdlSandbox;

internal static class Program {

    private static bool mRunning = true;
    private static IntPtr mWindow;
    private static BitmapTexture? mTexture;
    private static RenderContext? mContext;


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

        mRunning = true;
        mContext = RenderContext.Create(
            "appName",
            "appIdentifier",
            "windowTitle",
            1200,
            900);
        mTexture = BitmapTexture.Create(mContext.Renderer, 128, 128, "Images/Default.png");
    }


    private static void MainLoop() {

        int fps = 0;
        ulong lastTime = 0;

        while (mRunning) {

            var currentTick = SDL.GetTicks();
            Tick();
            fps++;
            var deltatime = SDL.GetTicks() - currentTick;
            if (currentTick - lastTime >= 1000) {
                SDL.SetWindowTitle(mContext.Window, $"FPS: {fps}");
                fps = 0;
                lastTime = currentTick;
            }
            //SDL.Delay(16);
        }
    }

    private static void Tick() {
        // Do a frame
        Input();
        Update();
        Render(mContext);
    }

    private static void Input() {
        // Handle input events
        while (SDL.PollEvent(out SDL.Event e)) {
            switch (e.Type) {
                case (uint)SDL.EventType.Quit:
                    mRunning = false;
                    break;
                case (uint)SDL.EventType.KeyDown:
                    // Handle keyboard events here
                    break;
            }
        }
    }

    private static void Update() {
        // Update game logic
    }

    private static void Render(RenderContext context) {
        // Render the scene
        var texture = mTexture ?? throw new InvalidOperationException("Texture was not created.");
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
        mTexture?.Dispose();
        mTexture = null;
        mContext?.Dispose();
        mContext = null;
    }

}
