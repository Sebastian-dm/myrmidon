using Bramble.Core;
using Malison.Core;
using Malison.WinForms;
using Myrmidon.App.Input;
using Myrmidon.Core.Game;
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

    public interface IUiController {

        public void Quit();
    }


    public class UiController : IUiController {

        public string Title { get { return _form.Text; } }

        public Vec Size { get { return _form.Terminal.Size; } }

        
        private IGameState _context;
        private InputController _inputController;

        private TerminalForm _form;
        private TileRenderer _renderer;

        public UiController(IGameState context) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            _context = context;
            _inputController = new InputController(this, context.);

            _form = new TerminalForm("Myrmidon", 80, 30);
            _renderer = new TileRenderer();

            // Attach event handlers
            _form.TerminalControl.KeyDown += MainForm_KeyDown;
            _form.TerminalControl.KeyUp += MainForm_KeyUp;
            _form.Load += MainForm_Load;
        }

        public void MainForm_Load(object? sender, EventArgs e) {
        }

        public void Run() {
            _renderer.Paint(_form.Terminal, _context);
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
            _inputController.HandleInput(e);
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e) {
            _inputController.HandleInput(e);
        }

    }
}
