using Bramble.Core;
using Malison.Core;
using Malison.WinForms;
using Myrmidon.App.Input;
using Myrmidon.Core.Actions;
using Myrmidon.Core.Game;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;


namespace Myrmidon.App.UI {

    public interface IUiController {

        public void Run();
        public void Render(IGameState context);
        public void Quit();
    }


    public class UiController : IUiController {

        public string Title { get { return _form.Text; } }
        public Vec Size { get { return _form.Terminal.Size; } }

        private IGameState _gameState;
        private readonly MainLoop _mainLoop;
        private readonly Timer _gameTimer;
        private readonly Stopwatch _stopwatch;
        private TimeSpan _accumulator = TimeSpan.Zero;
        private readonly TimeSpan _frameStep = TimeSpan.FromMilliseconds(50);
        private IInputController _inputController;
        private TerminalForm _form;
        private TileRenderer _renderer;

        


        public UiController(IGameState gameState, MainLoop mainLoop, IActionController actionController) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            _gameState = gameState;
            _mainLoop = mainLoop;
            _inputController = new InputController(this, actionController); // Handles user input;

            _form = new TerminalForm("Myrmidon", 80, 30);
            _renderer = new TileRenderer();
            _stopwatch = Stopwatch.StartNew();

            // Set up game timer
            _gameTimer = new Timer { Interval = 10 };
            _gameTimer.Tick += GameTick;
            _gameTimer.Start();

            // Attach event handlers
            //_form.TerminalControl.KeyDown += MainForm_KeyDown;
            _form.Load += MainForm_Load;
        }




        public void MainForm_Load(object? sender, EventArgs e) {
        }

        public void Run() {
            Application.Run(_form);
        }

        public void Quit() {
            _form.Close();
        }

        public void Render(IGameState context) {
            if (!context.World.IsMapGenInProgress)
                _renderer.Paint(_form.Terminal, context);
            _form.TerminalControl.Invalidate(); // Refresh the terminal control to show changes
        }




        private void GameTick(object? sender, EventArgs e) {

            _inputController.HandleInput();

            // Add time since last tick to accumulator
            _accumulator += _stopwatch.Elapsed;
            _stopwatch.Restart();

            // Update simulation at a fixed timestep
            if (_accumulator >= _frameStep) {
                _mainLoop.Tick();
                _accumulator -= _frameStep;

                //UpdateAnimations(_gameState);
                Render(_gameState);
            }
            
        }

    }
}
