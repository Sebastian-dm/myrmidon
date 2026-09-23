using Myrmidon.Core.ECS;
using Myrmidon.Core.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myrmidon.Core.Behavior {
    internal class DoorBehavior {

        private Ecs _ecs;


        public DoorBehavior(Ecs ecs) {
            _ecs = ecs;
        }


        public void Open(EntityId entity) {
            if (!_ecs.Has<Door>(entity) || !_ecs.Has<Physics>(entity))
                return;
            var door = _ecs.Get<Door>(entity);
            
            if(!door.IsLocked) {
                door.IsClosed = false;

                var physics = _ecs.Get<Physics>(entity);
                physics.BlocksMovement = false;
                physics.BlocksLineOfSight = false;
            }
        }


        public void Close(EntityId entity) {
            if (!_ecs.Has<Door>(entity) || !_ecs.Has<Physics>(entity))
                return;
            
            var door = _ecs.Get<Door>(entity);
            door.IsClosed = true;

            var physics = _ecs.Get<Physics>(entity);
            physics.BlocksMovement = true;
            physics.BlocksLineOfSight = true;
        }


        public void Lock(EntityId entity) {
            if (!_ecs.Has<Door>(entity) || !_ecs.Has<Physics>(entity))
                return;

            var door = _ecs.Get<Door>(entity);
            if (door.IsClosed)
                door.IsLocked = true;
        }


        public void Unlock(EntityId entity) {
            if (!_ecs.Has<Door>(entity) || !_ecs.Has<Physics>(entity))
                return;
            var door = _ecs.Get<Door>(entity);
            door.IsLocked = false;
        }

    }
}
