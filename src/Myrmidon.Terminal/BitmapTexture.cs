using SDL3;

namespace SdlSandbox;

internal public class BitmapTexture : IDisposable {
    private bool _disposed;

    private BitmapTexture(IntPtr handle, int width, int height) {
        Handle = handle;
        Width = width;
        Height = height;
    }

    public IntPtr Handle { get; }

    public int Width { get; }

    public int Height { get; }

    public static BitmapTexture Create(IntPtr renderer, int width = 128, int height = 128, string path = "Images/Default.png") {
        var surface = SDL.LoadPNG(path);
        if (surface == IntPtr.Zero)
            throw new InvalidOperationException($"Couldn't load bitmap: {SDL.GetError()}");

        var texture = SDL.CreateTextureFromSurface(renderer, surface);
        SDL.DestroySurface(surface);

        SDL.SetTextureScaleMode(texture, SDL.ScaleMode.Nearest);
        return new BitmapTexture(texture, width, height);
    }

    public void Dispose() {
        if (_disposed) {
            return;
        }

        if (Handle != IntPtr.Zero) {
            SDL.DestroyTexture(Handle);
        }

        _disposed = true;
    }
}
