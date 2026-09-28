using Bramble.Core;
using Myrmidon.Core.ECS;
using Myrmidon.Core.Parts;
using Myrmidon.Core.Signals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Myrmidon.Core.Actions;


internal class AttackAction : IAction {

    public bool IsImmediate { get; } = false;
    
    public readonly EntityId Attacker;
    public readonly EntityId Defender;

    private Position _atkPos;
    private Position _defPos;
    private Identity _atkId;
    private Identity _defId;
    private CombatStats _atkCS;
    private CombatStats _defCS;

    private IWorldState _context;


    public AttackAction(EntityId performer, EntityId subject)
    {
        Attacker = performer;
        Defender = subject;
    }


    private bool IsValidEntity(IWorldState context, EntityId entity)
    {
        var ecs = context.EcsWorld;
        return ecs.Has<Identity>(entity) &&
               ecs.Has<CombatStats>(entity) &&
               ecs.Has<Position>(entity) &&
               ecs.Has<Health>(entity);
    }


    public ActionResult Perform(IWorldState context)
    {
        _context = context;
        
        if (!IsValidEntity(context, Attacker)) return new ActionResult(succeeded: false, alternative: new SkipAction(Attacker));
        if (!IsValidEntity(context, Defender)) return new ActionResult(succeeded: false, alternative: new SkipAction(Attacker));

        _atkId = context.EcsWorld.Get<Identity>(Attacker);
        _defId = context.EcsWorld.Get<Identity>(Defender);
        _atkPos = context.EcsWorld.Get<Position>(Attacker);
        _defPos = context.EcsWorld.Get<Position>(Defender);
        _atkCS = context.EcsWorld.Get<CombatStats>(Attacker);
        _defCS = context.EcsWorld.Get<CombatStats>(Defender);
        
        // Todo: Implement "in range"
        if (!_atkPos.Coords.IsAdjacentTo(_defPos.Coords))
            return new ActionResult(succeeded: false, alternative: new SkipAction(Attacker));
        
        ResolveAttack(Attacker, Defender);
        
        return new ActionResult(succeeded: true);
    }

    private void ResolveAttack(EntityId attacker, EntityId defender)
    {
        var message = new StringBuilder();
        message.Append($"{_atkId.Name} attack {_defId.Name} ");
        if (_atkCS.NoAttacks > 1) message.Append($"(x{_atkCS.NoAttacks})");

        int damage = 0;
        for (int attack = 0; attack < _atkCS.NoAttacks; attack++) {
            
            int roll = GoRogue.DiceNotation.Dice.Roll("1d20");
            int THAC0 = 19 - _atkCS.PlusToHit;

            if (roll >= (THAC0 - _defCS.AC) || roll == 20) {
                damage += GoRogue.DiceNotation.Dice.Roll(_atkCS.Damage);
            }
        }

        if (damage > 0) {
            message.Append($"for {damage} damage.");
            _context.SignalQueue.Enqueue(new SoundSignal("hit"));
        }
        else {
            message.Append($"but miss.");
            _context.SignalQueue.Enqueue(new SoundSignal("miss"));
        }
        _context.SignalQueue.Enqueue(new LogSignal(message.ToString()));
        
        ResolveDamage(Defender, damage);
        
    }


    private void ResolveDamage(EntityId defender, int damage)
    {
        var defHlth = _context.EcsWorld.Get<Health>(defender);
        defHlth.Current -= damage;

        if (defHlth.Current <= 0)
            ResolveDeath(defender);
    }


    private void ResolveDeath(EntityId defender)
    {
        var defId = _context.EcsWorld.Get<Identity>(defender);

        StringBuilder deathMessage = new StringBuilder($"The {defId.Name} died.");

        // Dump inventory
        if (_context.EcsWorld.Has<Inventory>(defender)) {
            var defInv = _context.EcsWorld.Get<Inventory>(defender);
            if (defInv.Items.Count > 0) {

                for (int i = 0; i < defInv.Items.Count; i++) {
                    var item = defInv.Items[i];

                    if (_context.EcsWorld.Has<Position>(item)) {
                        var itemPos = _context.EcsWorld.Get<Position>(item);
                        itemPos.Coords = new Vec(_defPos.Coords.X, _defPos.Coords.Y);
                        itemPos.ZoneId = _defPos.ZoneId;
                        itemPos.Container = null;
                        _context.Zone.EntityIndex.Add(item, itemPos.Coords);
                    }

                    var itemId = _context.EcsWorld.Get<Identity>(item);

                }

                defInv.Items.Clear();
            }
        }
        _context.Zone.EntityIndex.Remove(defender, _defPos.Coords);
        _context.EcsWorld.DestroyEntity(defender);

        _context.SignalQueue.Enqueue(new LogSignal(deathMessage.ToString()));
    }

}
