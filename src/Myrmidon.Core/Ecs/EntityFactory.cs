using Bramble.Core;
using Myrmidon.Core.Parts;
using Myrmidon.Core.Utilities.Random;
using Myrmidon.Core.Zones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;


namespace Myrmidon.Core.Ecs;


public class EntityFactory {


    private readonly EcsWorld _world;
    private RandomNumberGenerator rng = new RandomNumberGenerator();


    public EntityFactory(EcsWorld world) {
        _world = world;
    }


    public EntityId CreatePlayer(int zoneId, Vec position) {
        EntityId entity = _world.CreateEntity();
        _world.Add(entity, new Identity { Name = "Player" });
        _world.Add(entity, new Position { Coords = position, ZoneId = zoneId });
        _world.Add(entity, new Health {
            Current = 20,
            Maximum = 20
        });
        _world.Add(entity, new CombatStats {
            AttackChance = rng.Next(0, 50),
            AttackStrength = rng.Next(0, 10),
            DefenseChance = rng.Next(0, 50),
            DefenseStrength = rng.Next(0, 10)
        });
        _world.Add(entity, new Inventory());
        _world.Add(entity, new Renderable("text/default", (byte)'@', "B", "M"));
        _world.Add(entity, new Perceptible() {Explored = true, LightLevel = 1.0f, Visible = true});
        return entity;
    }


    public EntityId CreateMonster(int zoneId, Vec position) {
        EntityId entity = _world.CreateEntity();

        _world.Add(entity, new Identity { Name = "a common goblin" });
        _world.Add(entity, new Brain());
        _world.Add(entity, new Position { Coords = position, ZoneId = zoneId });
        _world.Add(entity, new Health {
            Current = 3,
            Maximum = 3
        });
        _world.Add(entity, new CombatStats {
            AttackChance = rng.Next(0, 50),
            AttackStrength = rng.Next(0, 10),
            DefenseChance = rng.Next(0, 50),
            DefenseStrength = rng.Next(0, 10)
        });
        _world.Add(entity, new Inventory());
        _world.Add(entity, new Renderable("text/default", (byte)'M', "R"));
        _world.Add(entity, new Perceptible());


        return entity;
    }

    public EntityId CreateTreasure(int zoneId, Vec position) {
        EntityId entity = _world.CreateEntity();
        _world.Add(entity, new Identity { Name = "a pile of gold" });
        _world.Add(entity, new Position { Coords = position, ZoneId = zoneId });
        _world.Add(entity, new Renderable("text/default", (byte)'$', "Y"));
        _world.Add(entity, new Perceptible());
        return entity;
    }



}
