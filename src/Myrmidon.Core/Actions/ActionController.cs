using Bramble.Core;
using Myrmidon.Core.Entities;
using Myrmidon.Core.Game;
using Myrmidon.Core.Maps.Tiles;
using Myrmidon.Core.Rules;

using System;
using System.Collections.Generic;
using System.Text;
using static System.Collections.Specialized.BitVector32;

namespace Myrmidon.Core.Actions {

    public interface IActionController {

        public bool IsPlayersTurn { get; }
        public bool CanAcceptInput { get; }
        public bool HasPendingActions { get; }
        public void AddFromPlayerInput(InputAction command);
        public void Add(IAction action);
        public void ResolveAllActions();
        public void ResolveNextAction();
        public void CollectEntityActions();

    }


    public class ActionController : IActionController {

        public bool IsPlayersTurn {get; private set;} = true;

        public bool CanAcceptInput => (IsPlayersTurn && _reactionQueue.Count == 0 && _actionQueue.Count == 0);
        public bool HasPendingActions => (_reactionQueue.Count > 0 || _actionQueue.Count > 0);


        private readonly Queue<IAction> _actionQueue = new Queue<IAction>();
        private readonly Queue<IAction> _reactionQueue = new Queue<IAction>();
        private readonly Queue<IAction> _actionsHistory = new Queue<IAction>(100);

        private readonly IGameState _gameState;
        private readonly IFovSystem _fov;




        public ActionController(IGameState gameState, IFovSystem fov) {
            _gameState = gameState;
            _fov = fov;
        }




        public void AddFromPlayerInput(InputAction inputAction) {
            var action = CreateActionFromInput(inputAction);
            if (action != null) {
                Add(action);
            }
            else if (inputAction == InputAction.None) {
                // No action to add
                return;
            }
            else {
                throw new ArgumentException($"Unknown input action: {inputAction}");
            }
        }

        public void Add(IAction action) {
            if (CanAcceptInput) {
                if (action.IsImmediate)
                    _reactionQueue.Enqueue(action);
                else
                    _actionQueue.Enqueue(action);
            }
        }

        public void ResolveAllActions() {
            while (HasPendingActions) {
                ResolveNextAction();
            }

            // After resolving all actions, switch turns
            // and update the FOV if needed
            IsPlayersTurn = !IsPlayersTurn;
            if (!IsPlayersTurn) {
                _fov.Recompute(_gameState, _gameState.World.Player.Position);
            }
        }

        public void ResolveNextAction() {

            if (!HasPendingActions) return;

            IAction? action;
            if (_reactionQueue.Count > 0) {
                action = _reactionQueue.Dequeue();
            }
            else {
                action = _actionQueue.Dequeue();
            }

            var result = action.Perform(_gameState);
            if (result.Alternative != null)
                Add(result.Alternative);
            else if (result.Succeeded)
                _actionsHistory.Enqueue(action);
        }
            



        private IAction? CreateActionFromInput(InputAction command) {
            return command switch {
                InputAction.MovePlayerUp => new WalkAction(_gameState.World.Player, new Vec(0, -1)),
                InputAction.MovePlayerDown => new WalkAction(_gameState.World.Player, new Vec(0, 1)),
                InputAction.MovePlayerLeft => new WalkAction(_gameState.World.Player, new Vec(-1, 0)),
                InputAction.MovePlayerRight => new WalkAction(_gameState.World.Player, new Vec(1, 0)),
                InputAction.SkipPlayerTurn => new SkipAction(_gameState.World.Player),
                _ => null
            };
        }


        public void CollectEntityActions() {
            foreach (Actor actor in _gameState.World.Entities.Items) {
                _actionQueue.Enqueue(actor.GetAction());
            }
        }


    }
}