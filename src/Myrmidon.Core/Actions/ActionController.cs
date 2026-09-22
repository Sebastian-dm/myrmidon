using Bramble.Core;
using Myrmidon.Core.Systems;

using System;
using System.Collections.Generic;
using System.Text;
using Myrmidon.Core.Parts;
using Myrmidon.Core.ECS;
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

        private readonly IWorldState _gameState;




        public ActionController(IWorldState gameState) {
            _gameState = gameState;
        }




        public void AddFromPlayerInput(InputAction inputAction) {
            var action = CreateActionFromInput(inputAction);
            if (action != null && CanAcceptInput) {
                Add(action);
                IsPlayersTurn = false;
            }
            else if (inputAction == InputAction.None) {
                // No action to add
                return;
            }
            else {
                //throw new ArgumentException($"Unknown input action: {inputAction}");
                return;
            }
        }

        public void Add(IAction action) {
            if (action.IsImmediate)
                _reactionQueue.Enqueue(action);
            else
                _actionQueue.Enqueue(action);
        }

        public void ResolveAllActions() {
            while (HasPendingActions) {
                ResolveNextAction();
            }

            // After resolving all actions, switch turns
            IsPlayersTurn = true;
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
                InputAction.MovePlayerN => new WalkAction(_gameState.PlayerEntity, new Vec(0, -1)),
                InputAction.MovePlayerNe => new WalkAction(_gameState.PlayerEntity, new Vec(1, -1)),
                InputAction.MovePlayerS => new WalkAction(_gameState.PlayerEntity, new Vec(0, 1)),
                InputAction.MovePlayerSe => new WalkAction(_gameState.PlayerEntity, new Vec(1, 1)),
                InputAction.MovePlayerW => new WalkAction(_gameState.PlayerEntity, new Vec(-1, 0)),
                InputAction.MovePlayerSw => new WalkAction(_gameState.PlayerEntity, new Vec(-1, 1)),
                InputAction.MovePlayerE => new WalkAction(_gameState.PlayerEntity, new Vec(1, 0)),
                InputAction.MovePlayerNw => new WalkAction(_gameState.PlayerEntity, new Vec(-1, -1)),
                InputAction.SkipPlayerTurn => new SkipAction(_gameState.PlayerEntity),
                _ => null
            };
        }


        public void CollectEntityActions()
        {
            foreach (EntityId actor in _gameState.Zone.EntityIndex.All()) {
                
                if (_gameState.EcsWorld.TryGet<Brain>(actor, out var brain)) {
                    var action = brain.GetAction(actor);
                    _actionQueue.Enqueue(action);
                }
            }
        }


    }
}