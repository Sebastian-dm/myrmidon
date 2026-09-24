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
        _world.Add(entity, new Identity { Name = "Player" });
        _world.Add(entity, new Position { Coords = position, ZoneId = zoneId });
        _world.Add(entity, new Health {
            Current = 20,
            Maximum = 20
        });
        _world.Add(entity, new CombatStats {
            AttackChance = rng.Next(0, 100),
            NoAttacks = rng.Next(0, 10),
            BlockChance = rng.Next(0, 50),
            NoBlocks = rng.Next(0, 10)
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
            Current = 3,
            Maximum = 3
        });
        _world.Add(entity, new CombatStats {
            AttackChance = rng.Next(0, 50),
            NoAttacks = rng.Next(0, 1),
            BlockChance = rng.Next(0, 10),
            NoBlocks = rng.Next(0, 1)
        });

        var inv = new Inventory();
        _world.Add(entity, inv);

        EntityId remains = _world.CreateEntity();
        _world.Add(remains, new Identity { Name = "remains", Groups = new List<string> { "Item" } });
        _world.Add(remains, new Position { Container = entity });
        _world.Add(remains, new Renderable("etc/robert", (byte)24, "r", colorAccent: "W"));
        _world.Add(remains, new Perceptible());
        inv.Items.Add(remains);



        var tal = rng.Next(0, 100);
        if (tal > 25)
            _world.Add(entity, new Renderable("etc/robert", (byte)17, "white", colorAccent: "R"));
        else
            _world.Add(entity, new Renderable("etc/robert", (byte)23, "w", colorAccent: "white"));
        _world.Add(entity, new Perceptible());


        return entity;
    }


    public EntityId CreateTreasure(int zoneId, Vec position) {
        EntityId entity = _world.CreateEntity();
        _world.Add(entity, new Identity { Name = "a pile of gold", Groups = new List<string> { "Treasure", "Item"} });
        _world.Add(entity, new Position { Coords = position, ZoneId = zoneId });
        _world.Add(entity, new Renderable("etc/robert", (byte)21, "y", colorAccent: "W"));
        _world.Add(entity, new Perceptible());
        return entity;
    }


}
