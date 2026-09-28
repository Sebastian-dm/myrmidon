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


namespace Myrmidon.Core.ECS;


public class EntityFactory {


    private readonly Ecs _world;
    private RandomNumberGenerator rng = new RandomNumberGenerator();


    public EntityFactory(Ecs world) {
        _world = world;
    }


    public EntityId CreatePlayer(int zoneId, Vec position) {
        EntityId entity = _world.CreateEntity();
        _world.Add(entity, new Identity { Name = "You" });
        _world.Add(entity, new Position { Coords = position, ZoneId = zoneId });
        _world.Add(entity, new Health {
            Current = 8,
            Maximum = 8
        });
        _world.Add(entity, new CombatStats {
            PlusToHit = 2,
            NoAttacks = 1,
            AC = 7,
            Damage = "1d8"
        });
        _world.Add(entity, new Inventory());
        _world.Add(entity, new Renderable("etc/robert", (byte)22, "white", "B"));
        _world.Add(entity, new Perceptible() {Explored = true, LightLevel = 1.0f, Visible = true});
        return entity;
    }


    public EntityId CreateMonster(int zoneId, Vec position) {
        EntityId entity = _world.CreateEntity();

        _world.Add(entity, new Identity { Name = "ghost", Groups = new List<string> { "Creature", "Chaotic" } });
        _world.Add(entity, new Brain());
        _world.Add(entity, new Position { Coords = position, ZoneId = zoneId });
        _world.Add(entity, new Health {
            Maximum = 6,
            Current = 6,
        });
        _world.Add(entity, new CombatStats {
            PlusToHit = 0,
            NoAttacks = 1,
            AC = 9,
            Damage = "1d6"
        });

        var inv = new Inventory();
        _world.Add(entity, inv);
        var treasure = CreateTreasure(entity);
        inv.Items.Add(treasure);

        var roll = rng.Next(0, 100);
        if (roll > 33)
            _world.Add(entity, new Renderable("etc/robert", (byte)17, "white", colorAccent: "R"));
        else
            _world.Add(entity, new Renderable("etc/robert", (byte)23, "w", colorAccent: "white"));
        _world.Add(entity, new Perceptible());


        return entity;
    }

    public EntityId CreateFood(int zoneId, Vec position)
    {
        EntityId entity = _world.CreateEntity();
        _world.Add(entity, new Identity { Name = "fungus", Groups = new List<string> { "Food", "Item"} });
        _world.Add(entity, new Position { Coords = position, ZoneId = zoneId });
        _world.Add(entity, new Renderable("etc/robert", (byte)24, "g", colorAccent: "G"));
        _world.Add(entity, new Perceptible());
        return entity;
    }


    public EntityId CreateTreasure(EntityId owner) {
        EntityId entity = _world.CreateEntity();
        _world.Add(entity, new Identity { Name = "treasure", Groups = new List<string> { "Treasure", "Item" } });
        _world.Add(entity, new Position { Container = owner });
        _world.Add(entity, new Renderable("etc/robert", (byte)21, "y", colorAccent: "W"));
        _world.Add(entity, new Perceptible());
        return entity;
    }


}
