using Bramble.Core;
using Myrmidon.Core.Parts;
using Myrmidon.Core.Zones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Myrmidon.Core.ECS;


public  class EntityTileFactory {

    public EntityTileFactory(Ecs ecs) {
        _ecs = ecs;
    }

    private readonly Ecs _ecs;

    public EntityId CreateWall(TileMap map) {
        EntityId entity = map.Ecs.CreateEntity();
        _ecs.Add(entity, new Identity { Name = "Wall" });
        _ecs.Add(entity, new Renderable("tile/wall", (byte)0, "O"));
        _ecs.Add(entity, new Perceptible());
        return entity;
    }

    public EntityId CreateFloor(TileMap map) {
        EntityId entity = map.Ecs.CreateEntity();
        _ecs.Add(entity, new Identity { Name = "Floor" });
        _ecs.Add(entity, new Renderable("text/default", (byte)253, "m"));
        _ecs.Add(entity, new Perceptible());
        return entity;
    }

    public EntityId CreateDoor(TileMap map, bool locked = false, bool closed = true) {
        EntityId entity = map.Ecs.CreateEntity();
        _ecs.Add(entity, new Identity { Name = "Door" });
        _ecs.Add(entity, new Door { IsLocked = locked, IsClosed = closed });
        _ecs.Add(entity, new Renderable("text/default", (byte)'+', "Y"));
        _ecs.Add(entity, new Perceptible());
        return entity;
    }

}
