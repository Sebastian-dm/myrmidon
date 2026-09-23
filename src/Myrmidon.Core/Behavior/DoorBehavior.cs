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


        public void Open(EntityId doorEntity) {
            if (!_ecs.Has<Door>(doorEntity) || !_ecs.Has<Physics>(doorEntity))
                return;
            var door = _ecs.Get<Door>(doorEntity);
            
            if(!door.IsLocked) {
                door.IsClosed = false;

                var physics = _ecs.Get<Physics>(doorEntity);
                physics.BlocksMovement = false;
                physics.BlocksLineOfSight = false;

                SetVariantByte(doorEntity, [door.IsClosed]);

            }
        }


        public void Close(EntityId doorEntity) {
            if (!_ecs.Has<Door>(doorEntity) || !_ecs.Has<Physics>(doorEntity))
                return;
            
            var door = _ecs.Get<Door>(doorEntity);
            door.IsClosed = true;

            var physics = _ecs.Get<Physics>(doorEntity);
            physics.BlocksMovement = true;
            physics.BlocksLineOfSight = true;

            SetVariantByte(doorEntity, [door.IsClosed]);
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

        private void SetVariantByte(EntityId entity, bool[] flags){
            byte n = 0;
            foreach( var flag in flags)
                n |= (byte)(flag ? 1 : 0);
            
            _ecs.Get<Renderable>(entity).SetVariant(n);
        }

    }
}
