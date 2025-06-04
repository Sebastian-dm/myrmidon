using System;
using System.Text;
using System.Collections.Generic;
using Myrmidon.Core.Maps.Tiles;

using Myrmidon.Core.Utilities.Geometry;
using Myrmidon.Core.Rules;
using Myrmidon.Core.UI;
using Myrmidon.Core.Game;
using Myrmidon.Core.Entities;

namespace Myrmidon.Core.Actions {
    // Contains all generic actions performed on entities and tiles
    // including combat, movement, and so on.
    public class ActionController : IActionController {

        private readonly Queue<IAction> _actions;
        private readonly Queue<IAction> _reactions;
        private readonly Queue<IAction> _actionsDone;

        private readonly IGameContext _context;
        private readonly IFovSystem _fov;
        private readonly IUIService _ui;


        public ActionController(IGameContext context, IFovSystem fov, IUIService ui) {
            _context = context;
            _fov = fov;
            _ui = ui;
            _actions = new Queue<IAction>();
            _reactions = new Queue<IAction>();
            _actionsDone = new Queue<IAction>(100);
        }

        public bool Update() {
            //PerformActions();
            //CollectEntityActions();
            return true;

        }

        private IAction? CreateActionFromInput(InputAction command) {
            return command switch {
                InputAction.MovePlayerUp => new WalkAction(_context.World.Player, new Vector(0, -1)),
                InputAction.MovePlayerDown => new WalkAction(_context.World.Player, new Vector(0, 1)),
                InputAction.MovePlayerLeft => new WalkAction(_context.World.Player, new Vector(-1, 0)),
                InputAction.MovePlayerRight => new WalkAction(_context.World.Player, new Vector(1, 0)),
                InputAction.SkipPlayerTurn => new SkipAction(_context.World.Player),
                _ => null
            };
        }


        private void PerformActions() {
            while (_actions.Count > 0) {
                IAction action = _actions.Dequeue();
                ActionResult result = action.Perform(_context);
                
                // Try alternatives
                while (result.Alternative != null) {
                    action = result.Alternative;
                    result = action.Perform(_context);
                }

                if (result.Succeeded) {
                    if (action is WalkAction walk && walk.Performer is Player player) {
                        _fov.Update(_context, walk.Performer.Position);
                        _ui.CenterOnActor(player);
                        _ui.Refresh();
                    }
                }

                _actionsDone.Enqueue(action);
                
            }
        }

        private void CollectEntityActions() {
            foreach (Actor actor in _context.World.Entities.Items) {
                _actions.Enqueue(actor.GetAction());
            }
        }

        public void AddAction(InputAction inputAction) {
            var action = CreateActionFromInput(inputAction);
            if (action != null) {
                AddAction(action);
            }
            else if (inputAction == InputAction.None) {
                // No action to add
            }
            else {
                throw new ArgumentException($"Unknown input action: {inputAction}");
            }
        }

        public void AddAction(IAction action) {
            if (action.IsImmediate) {
                _reactions.Enqueue(action);
            }
            else {
                _actions.Enqueue(action);
            }
        }


    }
}