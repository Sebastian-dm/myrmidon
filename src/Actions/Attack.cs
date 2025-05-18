using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

using myrmidon.Entities;
using myrmidon.Geometry;

namespace myrmidon.Actions {
    internal class AttackAction : Action {

        public readonly Actor Subject;

        public AttackAction(Actor performer, Actor subject) : base(performer) {
            Subject = subject;
        }

        public override ActionResult Perform() {
            
            double Distance = (Performer.Position - Subject.Position).ToVector2().LengthSquared();
            if (Distance < 2) {
                Attack(Performer, Subject);
                return new ActionResult(succeeded: true);
            }
            else {
                return new ActionResult( succeeded: false,
                alternative: new SkipAction(Performer)
                );
            }
        }

        // Executes an attack from an attacking actor
        // on a defending actor, and then describes
        // the outcome of the attack in the Message Log
        public void Attack(Actor attacker, Actor defender) {
            // Create two messages that describe the outcome
            // of the attack and defense
            StringBuilder attackMessage = new StringBuilder();
            StringBuilder defenseMessage = new StringBuilder();

            // Count up the amount of attacking damage done
            // and the number of successful blocks
            int hits = ResolveAttack(attacker, defender, attackMessage);
            int blocks = ResolveDefense(defender, hits, attackMessage, defenseMessage);

            // Display the outcome of the attack & defense
            Program.UIManager.MessageLog.Add(attackMessage.ToString());
            if (!string.IsNullOrWhiteSpace(defenseMessage.ToString())) {
                Program.UIManager.MessageLog.Add(defenseMessage.ToString());
            }

            int damage = hits - blocks;

            // The defender now takes damage
            ResolveDamage(defender, damage);
        }


        // Calculates the outcome of an attacker's attempt
        // at scoring a hit on a defender, using the attacker's
        // AttackChance and a random d100 roll as the basis.
        // Modifies a StringBuilder message that will be displayed
        // in the MessageLog
        private int ResolveAttack(Actor attacker, Actor defender, StringBuilder attackMessage) {
            // Create a string that expresses the attacker and defender's names
            int hits = 0;
            attackMessage.AppendFormat("{0} attacks {1}, ", attacker.Name, defender.Name);

            // The attacker's Attack value determines the number of D100 dice rolled
            for (int dice = 0; dice < attacker.AttackStrength; dice++) {
                //Roll a single D100 and add its results to the attack Message
                int diceOutcome = GoRogue.DiceNotation.Dice.Roll("1d100");

                //Resolve the dicing outcome and register a hit, governed by the
                //attacker's AttackChance value.
                if (diceOutcome >= 100 - attacker.AttackChance)
                    hits++;
            }

            return hits;
        }


        // Calculates the outcome of a defender's attempt
        // at blocking incoming hits.
        // Modifies a StringBuilder messages that will be displayed
        // in the MessageLog, expressing the number of hits blocked.
        private int ResolveDefense(Actor defender, int hits, StringBuilder attackMessage, StringBuilder defenseMessage) {
            int blocks = 0;
            if (hits > 0) {
                // Create a string that displays the defender's name and outcomes
                attackMessage.AppendFormat("scoring {0} hits.", hits);
                defenseMessage.AppendFormat(" {0} defends and rolls: ", defender.Name);

                //The defender's Defense value determines the number of D100 dice rolled
                for (int dice = 0; dice < defender.DefenseStrength; dice++) {
                    //Roll a single D100 and add its results to the defense Message
                    int diceOutcome = GoRogue.DiceNotation.Dice.Roll("1d100");

                    //Resolve the dicing outcome and register a block, governed by the
                    //attacker's DefenceChance value.
                    if (diceOutcome >= 100 - defender.DefenseChance)
                        blocks++;
                }
                defenseMessage.AppendFormat("resulting in {0} blocks.", blocks);
            }
            else {
                attackMessage.Append("and misses completely!");
            }
            return blocks;
        }


        // Calculates the damage a defender takes after a successful hit
        // and subtracts it from its Health
        // Then displays the outcome in the MessageLog.
        private void ResolveDamage(Actor defender, int damage) {
            if (damage > 0) {
                defender.Health = defender.Health - damage;
                Program.UIManager.MessageLog.Add($" {defender.Name} was hit for {damage} damage");
                if (defender.Health <= 0) {
                    ResolveDeath(defender);
                }
            }
            else {
                Program.UIManager.MessageLog.Add($"{defender.Name} blocked all damage!");
            }
        }


        // Removes an Actor that has died
        // and displays a message showing
        // the actor that has died, and they loot they dropped
        private void ResolveDeath(Actor defender) {
            // Set up a customized death message
            StringBuilder deathMessage = new StringBuilder($"{defender.Name} died");

            // dump the dead actor's inventory (if any)
            // at the map position where it died
            if (defender.Inventory.Count > 0) {
                deathMessage.Append(" and dropped");

                foreach (Item item in defender.Inventory) {
                    // move the Item to the place where the actor died
                    item.Position = defender.Position;

                    // Now let the MultiSpatialMap know that the Item is visible
                    Program.World.CurrentMap.Add(item);

                    // Append the item to the deathMessage
                    deathMessage.Append(", " + item.Name);
                }

                // Clear the actor's inventory. Not strictly
                // necessary, but makes for good coding habits!
                defender.Inventory.Clear();
            }
            else {
                // The monster carries no loot, so don't show any loot dropped
                deathMessage.Append(".");
            }

            // actor goes bye-bye
            Program.World.CurrentMap.Remove(defender);

            // Now show the deathMessage in the messagelog
            Program.UIManager.MessageLog.Add(deathMessage.ToString());
        }





    }
}
