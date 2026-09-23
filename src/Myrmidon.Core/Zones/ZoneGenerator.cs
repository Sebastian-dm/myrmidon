using Bramble.Core;
using GoRogue;
using GoRogue.GameFramework;
using Myrmidon.Core.Parts;
using Myrmidon.Core.ECS;
using Myrmidon.Core.Utilities.Graphics;
using Myrmidon.Core.Utilities.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Myrmidon.Core.Zones;

public interface IZoneGenerator {
    void Generate(Zone zone);
    public Vec GetCenterOfRandomRoom(TileMap map);
    public Vec GetRandomWalkablePosition(TileMap map);
}


public class ZoneGenerator : IZoneGenerator {


    private RandomNumberGenerator rng = new();

    private IMapGenerator _mapGenerator;

    private EntityFactory _entityFactory;


    public ZoneGenerator(Ecs ecsWorld, IMapGenerator mapGenerator) {
        _mapGenerator = mapGenerator;
        _entityFactory = new EntityFactory(ecsWorld);
    }



    public void Generate(Zone zone) {
        Generate(zone, _mapGenerator);
    }


    public void Generate(Zone zone, IMapGenerator mapGen) {
        if (zone.GenerationState == ZoneGenState.NotStarted) {
            zone.GenerationState = ZoneGenState.Terraforming;
            mapGen.Generate(zone.TileMap);
            zone.GenerationState = ZoneGenState.Unpopulated;
        }
        if (zone.GenerationState == ZoneGenState.Unpopulated) {
            zone.GenerationState = ZoneGenState.Populating;
            CreateMonsters(zone);
            CreateTreasure(zone);
            zone.GenerationState = ZoneGenState.Ready;
        }
    }

    public void CreateMonsters(Zone zone) {
        for (int i = 0; i < 30; i++) {
            Vec pos = GetRandomWalkablePosition(zone.TileMap);
            EntityId monsterEntity = _entityFactory.CreateMonster(zone.Id, pos);
            zone.EntityIndex.Add(monsterEntity, pos);
        }
    }


    public void CreateTreasure(Zone zone) {
        for (int i = 0; i < 20; i++) {
            Vec pos = GetRandomWalkablePosition(zone.TileMap);
            EntityId lootEntity = _entityFactory.CreateTreasure(zone.Id, pos);
            zone.EntityIndex.Add(lootEntity, pos);
        }
    }


    // private void PlaceEntityAtRandomWalkable(TileMap map, Entity entity) {
    //     int pos;
    //     bool valid;
    //     do {
    //         pos = rng.Next(0, map.Width * map.Height);
    //         valid = map.Tiles[pos].IsWalkable;
    //     }
    //     while (!valid);
    //
    //     entity.Position = new Vec(pos % map.Width, pos / map.Width);
    //     map.Entities.Add(entity, new Coord(entity.Position.X, entity.Position.Y));
    // }



    public Vec GetRandomWalkablePosition(TileMap map) {
        int pos;
        bool valid;
        do {
            pos = rng.Next(0, map.Tiles.Length);
            valid = map.Ecs.Get<Physics>(map.Tiles[pos]).BlocksMovement == false;
        }
        while (!valid);

        var posVec = new Vec(pos % map.Width, pos / map.Width);
        return posVec;
    }

    public Vec GetCenterOfRandomRoom(TileMap map) {

        if (map.Rooms.Count > 0) {
            int index = rng.Next(map.Rooms.Count);
            return map.Rooms[index].Center;
        }
        return Vec.Zero;
    }

}


public enum ZoneGenState {
    NotStarted,
    Terraforming,
    Unpopulated,
    Populating,
    Ready
}
