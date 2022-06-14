using System;
using clodd.Entities;
using Microsoft.Xna.Framework;

namespace clodd.Commands {
    // Contains all generic actions performed on entities and tiles
    // including combat, movement, and so on.
    public class CommandManager {

        //stores the actor's last move action
        private Point _lastMoveActorPoint;
        private Actor _lastMoveActor;

        public CommandManager() {

        }

        // Move the actor BY +/- X&Y coordinates
        // returns true if the move was successful
        // and false if unable to move there
        public bool MoveActorBy(Actor actor, Point position) {
            // store the actor's last move state
            _lastMoveActor = actor;
            _lastMoveActorPoint = position;

            return actor.MoveBy(position);
        }

        // Redo last actor move
        public bool RedoMoveActorBy() {
            // Make sure there is an actor available to redo first!
            if (_lastMoveActor != null) {
                return _lastMoveActor.MoveBy(_lastMoveActorPoint);
            }
            else
                return false;
        }

        // Undo last actor move
        // then clear the undo so it cannot be repeated
        public bool UndoMoveActorBy() {
            // Make sure there is an actor available to undo first!
            if (_lastMoveActor != null) {
                // reverse the directions of the last move
                _lastMoveActorPoint = new Point(-_lastMoveActorPoint.X, -_lastMoveActorPoint.Y);

                if (_lastMoveActor.MoveBy(_lastMoveActorPoint)) {
                    _lastMoveActorPoint = new Point(0, 0);
                    return true;
                }
                else {
                    _lastMoveActorPoint = new Point(0, 0);
                    return false;
                }
            }
            return false;
        }
    }
}