using Bramble.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;

using Myrmidon.App.Input;
using Myrmidon.Core.Actions;
using Myrmidon.Core.Game;
using Myrmidon.Terminal;


namespace Myrmidon.App.UI {

    public interface IUiController {

        public void Run();
        public void Render(IGameState context);
        public void Quit();
    }


    public class UiController : IUiController {

        public Vec Size { get { return new Vec(800, 600); } }

        private IGameState _gameState;
        private readonly MainLoop _mainLoop;
        private readonly Timer _gameTimer;
        private readonly Stopwatch _stopwatch;
        private TimeSpan _accumulator = TimeSpan.Zero;
        private readonly TimeSpan _frameStep = TimeSpan.FromMilliseconds(50);
        private IInputController _inputController;
        private ITerminal _terminal;
        private TileRenderer _renderer;

        


        public UiController(IGameState gameState, MainLoop mainLoop, IActionController actionController) {

            _gameState = gameState;
            _mainLoop = mainLoop;
            _inputController = new InputController(this, actionController); // Handles user input;

            _terminal = new SdlTerminal();
            _terminal.Initialize(800, 600, "Myrmidon");

            _renderer = new TileRenderer();
            _stopwatch = Stopwatch.StartNew();

            // Set up game timer
            //_gameTimer = new Timer(GameTick(this,e),10,10);

            // Attach event handlers
            //_form.TerminalControl.KeyDown += MainForm_KeyDown;
        }


        public void Run() {
            _terminal.Run();
        }

        public void Quit() {
            _terminal.Close();
        }

        public void Render(IGameState context) {
            //if (!context.World.IsMapGenInProgress)
            //    _renderer.Paint(_terminal, context);
            //_form.TerminalControl.Invalidate(); // Refresh the terminal control to show changes
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
