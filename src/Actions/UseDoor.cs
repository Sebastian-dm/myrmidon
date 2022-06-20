using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using clodd.Entities;
using clodd.Tiles;

namespace clodd.Actions {
    internal class OpenDoorAction : Action {

        public readonly TileDoor Door;

        public OpenDoorAction(Actor performer, TileDoor door) : base(performer) {
            Door = door;
        }

        public override ActionResult Perform() {
            try {
                OpenDoor(Door);
                return new ActionResult(succeeded: true);
            }
            catch (Exception) {
                return new ActionResult(succeeded: false,
                alternative: new SkipAction(Performer)
                );
                throw;
            }
        }

        public void OpenDoor(TileDoor door) {
            // Handle a locked door
            if (door.Locked) {
                // We have no way of opening a locked door for the time being.
            }
            // Handled an unlocked door that is closed
            else if (!door.Locked && !door.IsOpen) {
                door.Open();
                GameLoop.UIManager.MessageLog.Add($"{Performer.Name} opened {door.Name}");
            }
        }
    }
}
