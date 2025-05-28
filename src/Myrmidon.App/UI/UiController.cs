using Bramble.Core;
using Malison.Core;
using Malison.WinForms;
using Myrmidon.App.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Myrmidon.App.UI {
    public class UiController {

        public string Title { get { return _form.Text; } }

        public Vec Size { get { return _form.Terminal.Size; } }

        private TerminalForm _form;
        private MainGame _game;
        private TileRenderer _renderer;

        public UiController(MainGame game) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            _game = game;
            _form = new TerminalForm(_game.Name, 120, 50);
            _renderer = new TileRenderer();

            // Attach event handlers
            _form.TerminalControl.KeyDown += MainForm_KeyDown;
            _form.TerminalControl.KeyUp += MainForm_KeyUp;
            _form.Load += MainForm_Load;
        }

        public void MainForm_Load(object? sender, EventArgs e) {
        }

        public void Run() {
            _renderer.Paint(_form.Terminal, _game.Context);
            Application.Run(_form);
        }

        public void Quit() {
            _form.Close();
        }

        public void Update() {
            _form.Update();
        }

        public void Delay() {
            Thread.Sleep(100);
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e) {
            _game.HandleKeyPress(e);
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e) {
            _game.HandleKeyPress(e);
        }

    }
}
