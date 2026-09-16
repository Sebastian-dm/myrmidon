using Bramble.Core;
using Myrmidon.Core.Actions;
using Myrmidon.Core.Components;
using Myrmidon.Core.Entities;
using Myrmidon.Core.Maps;
using Myrmidon.Core.Maps.Generation;
using Myrmidon.Core.Systems;
using Myrmidon.Core.Zones;
using Myrmidon.Core.Utilities.Graphics;
using Myrmidon.Core.Ecs;

namespace Myrmidon.Core {
    public class WorldManager {

        public IWorldState WorldState { get; private set; }

        public IFovSystem FovSystem { get; private set; }
        public ActionController ActionController;


        private IZoneGenerator _zoneGen;


        public WorldManager(IWorldState worldstate, ActionController actionController, IZoneGenerator zoneGenerator) {
            WorldState = worldstate;
            FovSystem = new FovSystemOctant(worldstate.EcsWorld);
            _zoneGen = zoneGenerator;
            
            ActionController =  actionController;
        }


        public void Update() {
            if (WorldState.Zone.GenerationState != ZoneGenState.Ready) {
                _zoneGen.Generate(WorldState.Zone);
                CreatePlayer(WorldState.Zone);
                return;
            }
            
            
            if (WorldState.EcsWorld.TryGet<Position>(WorldState.PlayerEntity, out var pPos))
                FovSystem.Recompute(WorldState.Zone, pPos.Coords);

        }


        public void CreatePlayer(Zone zone) {
            var entityFactory = new EntityFactory(WorldState.EcsWorld);
            Vec pos = _zoneGen.GetRandomWalkablePosition(zone.Map);
            EntityId playerId = entityFactory.CreatePlayer(zone.Id, pos);
            zone.SpatialIndex.Add(playerId, pos);
            WorldState.PlayerEntity = playerId;
        }
    }
}
