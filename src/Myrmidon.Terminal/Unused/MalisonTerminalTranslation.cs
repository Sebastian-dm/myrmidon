using Bramble.Core;
using Malison.Core;
using SDL3;
using System;
using System.Drawing;
using System.Runtime.ConstrainedExecution;
using static System.Net.Mime.MediaTypeNames;


namespace Myrmidon.Terminal {
    public class MalisonTerminalTranslation {

        private IntPtr _window;
        private IntPtr _renderer;
        private bool _isRunning;
        public Vec Size { get { return mCharacters.Size; } }


        private Array2D<Character> mCharacters;
        private const int Padding = 2;

        //private GlyphSheet mGlyphSheet;

        private ITerminal mTerminal;




        public ITerminal Terminal {
            get { return mTerminal; }
            set {
                if (value == null) throw new ArgumentNullException("value");
                mTerminal = value;
                //ResizeToFitTerminal();
            }
        }


        public MalisonTerminalTranslation(int width, int height) {

            mTerminal = new Malison.Core.Terminal(width, height);

            mCharacters = new Array2D<Character>(mTerminal.Size.X, mTerminal.Size.Y);
            mCharacters.Fill(new Character(' '));

            //mGlyphSheet = GlyphSheet.GetGlyphSheet("Terminal6x10");


            Initialize("Myrmidon Terminal");

        }


        public void Initialize(string title) {


            if (!SDL.Init(SDL.InitFlags.Video)) {
                throw new Exception($"Failed to initialize SDL: {SDL.GetError()}");
            }
            

            _window = SDL.CreateWindow(
                title,
                mCharacters.Width,
                mCharacters.Height,
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


        //protected void OnLoad(EventArgs e) {
        //    ResizeToFitTerminal();
        //}

        //private void ResizeToFitTerminal() {
        //    Size terminalSize = mTerminalControl.PreferredSize;
        //    ClientSize = new Size(terminalSize.Width, terminalSize.Height);
        //}

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e) {
            Close();
        }


        //protected void OnPaint(PaintEventArgs e)
        //{
        //    e.Graphics.Clear(Color.Black);

        //    if (mTerminal != null)
        //    {
        //        // only refresh characters in the clip rect
        //        int left = Math.Max(0, (e.ClipRectangle.Left - Padding) / mGlyphSheet.Width);
        //        int top = Math.Max(0, (e.ClipRectangle.Top - Padding) / mGlyphSheet.Height);
        //        int right = Math.Min(mTerminal.Size.X, (e.ClipRectangle.Right - Padding) / mGlyphSheet.Width + 1);
        //        int bottom = Math.Min(mTerminal.Size.Y, (e.ClipRectangle.Bottom - Padding) / mGlyphSheet.Height + 1);

        //        for (int y = top; y < bottom; y++)
        //        {
        //            for (int x = left; x < right; x++)
        //            {
        //                Character character = mTerminal.Get(x, y);

        //                // fill the background if needed
        //                if (!character.BackColor.Equals(Color.Black))
        //                {
        //                    int fillLeft = (x * mGlyphSheet.Width) + Padding;
        //                    int fillTop = (y * mGlyphSheet.Height) + Padding;
        //                    int width = mGlyphSheet.Width;
        //                    int height = mGlyphSheet.Height;

        //                    // fill past the padding on the edges
        //                    if (x == 0)
        //                    {
        //                        fillLeft -= Padding;
        //                        width += Padding;
        //                    }
        //                    if (x == mTerminal.Size.X - 1)
        //                    {
        //                        width += Padding;
        //                    }
        //                    if (y == 0)
        //                    {
        //                        fillTop -= Padding;
        //                        height += Padding;
        //                    }
        //                    if (y == mTerminal.Size.Y - 1)
        //                    {
        //                        height += Padding;
        //                    }

        //                    e.Graphics.FillRectangle(new SolidBrush(character.BackColor.ToSystemColor()),
        //                        fillLeft, fillTop, width, height);
        //                }

        //                // draw the glyph
        //                mGlyphSheet.Draw(e.Graphics,
        //                    (x * mGlyphSheet.Width) + Padding,
        //                    (y * mGlyphSheet.Height) + Padding,
        //                    character);
        //            }
        //        }
        //    }
        //}


        private void InvalidateCharacter(Vec pos) {
            int width = 0;//mGlyphSheet.Width;
            int height = 0;//mGlyphSheet.Height;
            int left = (pos.X * width) + Padding;
            int top = (pos.Y * height) + Padding;

            // fill past the padding on the edges
            if (pos.X == 0) {
                left -= Padding;
                width += Padding;
            }
            if (pos.X == mTerminal.Size.X - 1) {
                width += Padding;
            }
            if (pos.Y == 0) {
                top -= Padding;
                height += Padding;
            }
            if (pos.Y == mTerminal.Size.Y - 1) {
                height += Padding;
            }

            // invalidate the rect under the character
            //Invalidate(new Rectangle(left, top, width, height));
        }

        private void Terminal_CharacterChanged(object sender, CharacterEventArgs e) {
            InvalidateCharacter(e.Position);
        }
    }
}
