using System;
using SDL3;


namespace Myrmidon.Terminal {

    public interface ITerminal : IDisposable {

        // todo add properties for width, height, etc.
        

        void Initialize(int width, int height, string title);
        void Run();
        void Close();
    }

    public class SdlTerminal : ITerminal {

        private IntPtr _window;
        private IntPtr _renderer;
        private bool _isRunning;

        // Add properties for width, height to get them from window size
        public int Width {
            get {
                SDL.GetWindowSize(_window, out int width, out _);
                return width;
            }
        }
        public int Height {
            get {
                SDL.GetWindowSize(_window, out _, out int height);
                return height;
            }
        }


        public SdlTerminal() {
        }

        public void Initialize(int width, int height, string title) {
            if (!SDL.Init(SDL.InitFlags.Video)) {
                throw new Exception($"Failed to initialize SDL: {SDL.GetError()}");
            }

            _window = SDL.CreateWindow(
                title,
                width,
                height,
                SDL.WindowFlags.Resizable
            );

            if (_window == IntPtr.Zero) {
                throw new Exception($"Failed to create window: {SDL.GetError()}");
            }

            _renderer = SDL.CreateRenderer(_window, null);

            if (_renderer == IntPtr.Zero) {
                throw new Exception($"Failed to create renderer: {SDL.GetError()}");
            }
        }

        public void Run() {
            _isRunning = true;

            while (_isRunning) {
                HandleEvents();
                Render();
                SDL.Delay(16); // ~60 FPS
            }
        }

        private void HandleEvents() {
            while (SDL.PollEvent(out SDL.Event e)) {
                if (e.Type == (int)SDL.EventType.Quit) {
                    Close();
                }
            }
        }

        private void Render() {
            // Set background color (dark blue/terminal-like)
            SDL.SetRenderDrawColor(_renderer, 0, 43, 54, 255);
            SDL.RenderClear(_renderer);

            // Present the rendered frame
            SDL.RenderPresent(_renderer);
        }

        public void Close() {
            _isRunning = false;
        }

        public void Dispose() {
            if (_renderer != IntPtr.Zero) {
                SDL.DestroyRenderer(_renderer);
                _renderer = IntPtr.Zero;
            }

            if (_window != IntPtr.Zero) {
                SDL.DestroyWindow(_window);
                _window = IntPtr.Zero;
            }

            SDL.Quit();
        }
    }
}
