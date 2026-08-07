using System;
using SDL3;


namespace Myrmidon.Terminal {


    public class SdlTerminal : IDisposable {

        private readonly int _width;
        private readonly int _height;
        private IntPtr _window;
        private IntPtr _renderer;
        private bool _isRunning;

        public SdlTerminal(int width, int height) {
            _width = width;
            _height = height;
        }

        public void Initialize() {
            if (!SDL.Init(SDL.InitFlags.Video)) {
                throw new Exception($"Failed to initialize SDL: {SDL.GetError()}");
            }

            _window = SDL.CreateWindow(
                "Myrmidon Terminal - Hello World",
                _width,
                _height,
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
                    _isRunning = false;
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
