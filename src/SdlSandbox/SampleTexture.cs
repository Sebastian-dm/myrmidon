using SDL3;

namespace SdlSandbox;

internal sealed class SampleTexture : IDisposable {
    private bool _disposed;

    private SampleTexture(IntPtr handle, int width, int height) {
        Handle = handle;
        Width = width;
        Height = height;
    }

    public IntPtr Handle { get; }

    public int Width { get; }

    public int Height { get; }

    public static SampleTexture Create(IntPtr renderer, int width = 128, int height = 128) {
        var texture = SDL.CreateTexture(renderer, SDL.PixelFormat.RGBA8888, SDL.TextureAccess.Static, width, height);

        if (texture == IntPtr.Zero) {
            throw new InvalidOperationException($"Couldn't create static texture: {SDL.GetError()}");
        }

        var pixels = CreatePixels(width, height);
        if (!SDL.UpdateTexture(texture, IntPtr.Zero, pixels, width * 4)) {
            SDL.DestroyTexture(texture);
            throw new InvalidOperationException($"Couldn't upload static texture: {SDL.GetError()}");
        }

        SDL.SetTextureScaleMode(texture, SDL.ScaleMode.Nearest);
        return new SampleTexture(texture, width, height);
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

    private static byte[] CreatePixels(int width, int height) {
        var pixels = new byte[width * height * 4];

        for (var y = 0; y < height; y++) {
            for (var x = 0; x < width; x++) {
                var offset = ((y * width) + x) * 4;
                var checker = (((x / 16) + (y / 16)) & 1) == 0;
                var diagonal = Math.Abs(x - y) < 5 || Math.Abs((width - x) - y) < 5;
                var edge = x < 4 || y < 4 || x >= width - 4 || y >= height - 4;

                byte r = edge ? (byte)255 : checker ? (byte)40 : (byte)230;
                byte g = edge ? (byte)255 : diagonal ? (byte)210 : checker ? (byte)170 : (byte)60;
                byte b = edge ? (byte)255 : diagonal ? (byte)45 : checker ? (byte)255 : (byte)95;

                var pixel = ((uint)r << 24) | ((uint)g << 16) | ((uint)b << 8) | SDL.AlphaOpaque;
                BitConverter.TryWriteBytes(pixels.AsSpan(offset, 4), pixel);
            }
        }

        return pixels;
    }
}
