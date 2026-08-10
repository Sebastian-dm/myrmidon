using SDL3;

namespace SdlSandbox;

internal class RenderContext : IDisposable {

    private bool _disposed;

    public RenderContext(IntPtr window, IntPtr renderer, int width, int height) {
        Window = window;
        Renderer = renderer;
        Width = width;
        Height = height;
    }

    public IntPtr Window { get; }

    public IntPtr Renderer { get; }

    public int Width { get; }

    public int Height { get; }

    public static RenderContext Create(
        string appName,
        string appIdentifier,
        string windowTitle,
        int width,
        int height,
        SDL.InitFlags initFlags = SDL.InitFlags.Video,
        SDL.RendererLogicalPresentation presentation = SDL.RendererLogicalPresentation.Letterbox) {
        SDL.SetAppMetadata(appName, "1.0", appIdentifier);

        if (!SDL.Init(initFlags)) {
            throw new InvalidOperationException($"Couldn't initialize SDL: {SDL.GetError()}");
        }

        if (!SDL.CreateWindowAndRenderer(windowTitle, width, height, SDL.WindowFlags.Resizable, out var window, out var renderer)) {
            SDL.Quit();
            throw new InvalidOperationException($"Couldn't create window/renderer: {SDL.GetError()}");
        }

        SDL.SetRenderLogicalPresentation(renderer, width, height, presentation);

        return new RenderContext(window, renderer, width, height);
    }

    public bool PollEvents(Func<SDL.Event, bool>? handleEvent = null) {
        while (SDL.PollEvent(out var sdlEvent)) {
            if (sdlEvent.Type == (uint)SDL.EventType.Quit) {
                return false;
            }

            if (handleEvent?.Invoke(sdlEvent) == false) {
                return false;
            }
        }

        return true;
    }

    public void Dispose() {
        if (_disposed) {
            return;
        }

        if (Renderer != IntPtr.Zero) {
            SDL.DestroyRenderer(Renderer);
        }

        if (Window != IntPtr.Zero) {
            SDL.DestroyWindow(Window);
        }

        SDL.Quit();
        _disposed = true;
    }
}
