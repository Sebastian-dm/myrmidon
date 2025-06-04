using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Myrmidon.Core.Entities;
using Myrmidon.Core.Maps.Tiles;
using Myrmidon.Core.Game;

namespace Myrmidon.Core.Actions {
    internal class OpenDoorAction : IAction {

        public bool IsImmediate { get; } = false;
        public readonly Actor Performer;
        public readonly TileDoor Door;

        public OpenDoorAction(Actor performer, TileDoor door) {
            Performer = performer;
            Door = door;
        }

        public ActionResult Perform(IGameContext context) {
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
                //Program.UIManager.MessageLog.Add($"{Performer.Name} opened {door.Name}");
            }
        }
    }
}
