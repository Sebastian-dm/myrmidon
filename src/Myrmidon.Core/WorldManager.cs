using Bramble.Core;
using Myrmidon.Core.Actions;
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
            FovSystem = new FovSystemOctant();
            _zoneGen = zoneGenerator;
            
            ActionController =  actionController;
        }


        public void Update() {
            if (WorldState.Zone.GenerationState != ZoneGenState.Ready) {
                _zoneGen.Generate(WorldState.Zone);
                WorldState.Player = CreatePlayer(WorldState.Zone);
                return;
            }
            
            FovSystem.Recompute(WorldState.Zone.Map, WorldState.Player.Position);

        }


        public Player CreatePlayer(Zone zone) {
            
            // Old method
            var player = new Player(new Color(20, 255, 255), Color.Transparent);
            player.Position = _zoneGen.GetCenterOfRandomRoom(zone.Map);
            zone.Map.AddEntity(player);


            // New method
            var entityFactory = new EntityFactory(WorldState.EcsWorld);
            Vec pos = _zoneGen.GetRandomWalkablePosition(zone.Map);
            EntityId playerId = entityFactory.CreatePlayer(zone.Id, pos);
            zone.SpatialIndex.Add(playerId, pos);

            return player;
        }
    }
}
