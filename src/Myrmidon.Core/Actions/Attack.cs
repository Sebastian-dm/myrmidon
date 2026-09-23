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
    public readonly EntityId Performer;
    public readonly EntityId Subject;

    private IWorldState _context;


    public AttackAction(EntityId performer, EntityId subject)
    {
        Performer = performer;
        Subject = subject;
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
        if (!IsValidEntity(context, Performer)) return new ActionResult(succeeded: false, alternative: new SkipAction(Performer));
        if (!IsValidEntity(context, Subject)) return new ActionResult(succeeded: false, alternative: new SkipAction(Performer));

        var perfPos = context.EcsWorld.Get<Position>(Performer);
        var subjPos = context.EcsWorld.Get<Position>(Subject);

        if (perfPos.Coords.IsAdjacentTo(subjPos.Coords)) {
            _context = context;
            Attack(Performer, Subject);
            return new ActionResult(succeeded: true);
        }
        else {
            return new ActionResult(succeeded: false,
            alternative: new SkipAction(Performer)
            );
        }
    }


    public void Attack(EntityId attacker, EntityId defender)
    {
        StringBuilder attackMessage = new StringBuilder();
        StringBuilder defenseMessage = new StringBuilder();

        int hits = ResolveAttack(attacker, defender, attackMessage);
        int blocks = ResolveDefense(defender, hits, attackMessage, defenseMessage);

        _context.SignalQueue.Enqueue(new LogSignal(attackMessage.ToString()));
        if (!string.IsNullOrWhiteSpace(defenseMessage.ToString())) {
            _context.SignalQueue.Enqueue(new LogSignal(defenseMessage.ToString()));
        }

        ResolveDamage(defender, hits - blocks);
    }


    private int ResolveAttack(EntityId attacker, EntityId defender, StringBuilder attackMessage)
    {
        var atkId = _context.EcsWorld.Get<Identity>(attacker);
        var defId = _context.EcsWorld.Get<Identity>(defender);
        var atkCS = _context.EcsWorld.Get<CombatStats>(attacker);

        attackMessage.Append($"{atkId.Name} attacks {defId.Name}, ");

        int hits = 0;
        for (int dice = 0; dice < atkCS.NoAttacks; dice++) {

            int diceOutcome = GoRogue.DiceNotation.Dice.Roll("1d100");

            if (diceOutcome >= 100 - atkCS.AttackChance)
                hits++;
        }

        return hits;
    }


    private int ResolveDefense(EntityId defender, int hits, StringBuilder attackMessage, StringBuilder defenseMessage)
    {
        var defId = _context.EcsWorld.Get<Identity>(defender);
        var defCS = _context.EcsWorld.Get<CombatStats>(defender);

        int blocks = 0;
        if (hits > 0) {
            attackMessage.Append($"scoring {hits} hits.");
            defenseMessage.Append($" {defId.Name} defends and rolls: ");

            for (int dice = 0; dice < defCS.NoBlocks; dice++) {
                int diceOutcome = GoRogue.DiceNotation.Dice.Roll("1d100");
                if (diceOutcome >= 100 - defCS.BlockChance)
                    blocks++;
            }
            defenseMessage.Append($"resulting in {blocks} blocks.");
        }
        else {
            attackMessage.Append("and misses completely.");
        }
        return blocks;
    }


    private void ResolveDamage(EntityId defender, int damage)
    {
        var defId = _context.EcsWorld.Get<Identity>(defender);
        var defHlth = _context.EcsWorld.Get<Health>(defender);

        if (damage > 0) {
            defHlth.Current -= damage;
            _context.SignalQueue.Enqueue(new LogSignal($" {defId.Name} was hit for {damage} damage."));

            if (defHlth.Current <= 0) {
                ResolveDeath(defender);
            }
        }
        else {
            _context.SignalQueue.Enqueue(new LogSignal($"{defId.Name} blocked all damage."));
        }
    }


    private void ResolveDeath(EntityId defender)
    {
        var defId = _context.EcsWorld.Get<Identity>(defender);
        var defPos = _context.EcsWorld.Get<Position>(defender);

        StringBuilder deathMessage = new StringBuilder($"{defId.Name} died");

        // Dump inventory
        if (_context.EcsWorld.Has<Inventory>(defender)) {
            var defInv = _context.EcsWorld.Get<Inventory>(defender);
            if (defInv.Items.Count > 0) {

                deathMessage.Append(" and dropped");

                for (int i = 0; i < defInv.Items.Count; i++) {
                    var item = defInv.Items[i];

                    if (_context.EcsWorld.Has<Position>(item)) {
                        var itemPos = _context.EcsWorld.Get<Position>(item);
                        itemPos.Coords = defPos.Coords;
                        itemPos.Container = null;
                        _context.Zone.EntityIndex.Add(item, itemPos.Coords);
                    }

                    var itemId = _context.EcsWorld.Get<Identity>(item);
                    deathMessage.Append(", " + itemId.Name);

                }

                defInv.Items.Clear();
            }
        }
        else {
            deathMessage.Append('.');
        }
        _context.Zone.EntityIndex.Remove(defender, defPos.Coords);
        _context.EcsWorld.DestroyEntity(defender);

        _context.SignalQueue.Enqueue(new LogSignal(deathMessage.ToString()));
    }

}
