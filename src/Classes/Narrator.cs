using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuelingDuelsters.Classes
{
    internal class Narrator
    {

        // ** Outcome Strings **

        private const string swordClash = "\nThe swords of {0} and {1} clash, the sound of ringing steel echoing throughout the arena!\nThe two combatants eye each other over their crossed blades.\nIs this the start of an enemies-to-lovers romance? Or another chapter in a long tale of bitter rivalry?\n{0} and {1} part with a puff of dust and return to their ready stances.\n";

        private const string bothHit = "\n{0} and {1} slash each other at the same time, slicing through armor and egos!\n";

        private const string blocked = "\n{0} handily blocks {1}'s hardy blow!\n{1}'s arm shudders from the impact, staggering them!\n";
        private const string failedBlock = "\n{0}'s sword slices into {1}'s unguarded side!\n";
        private const string bothBlock = "\n{0} and {1} hide behind their shields, two turtles scared of the world outside their shells.\nMaybe they will find the courage to face each other in the next round!\n";
        private const string failedDodge = "\n{0} tries to dodge directly into {1}'s attack!\nAn unfortunate blunder, the price of which {0} will pay in blood and shame!\n";
        private const string dodged = "\n{0} nimbly sidesteps {1}'s powerful attack!\n{0} uses their cat-like reflexes to mount a bewildering counterattack!\n";
        private const string failedCounter = "{0} whiffs their counter in embarrassing fashion! {1} breathes a sigh of relief.";
        private const string dodgeBlock = "\n{0} dodges toward {1}'s vulnerable side!\n{0} prepares to take advantage of the situation with extreme prejudice!\n";
        private const string failedDodgeBlock = "\n{0} rolls their eyes at {1}'s fruitless dancing over the top of their mighty shield.\nThe crowd showers the combatants with boos, thirsting for blood, or at least a little less fancy footwork.\n";
        private const string doubleDodge = "\nThe crowd claps along to an unheard beat as {0} and {1} groove and wiggle across the battlefield, matching each other's steps in a deadly dance of martial prowess.\nNobody takes damage, but everybody has a good time.\n";
        private const string uneventfulHeal = "\n{0} miraculously heals their wounds with the power of science that could be mistaken for magic!\n{1}'s defensive actions are all for naught!\n";
        private const string failedHealandDefend = "\n{0} is out of snake oil, thoughts, and prayers! Too bad {1} only took a defensive action.\nThe crowd laughs in derision at the ill-prepared, tactically inept combatants.\n";
        private const string healAndSwing = "\n{0} recovers health! But {1} has a chance to tear off the Band-Aid...\n";
        private const string failedHealAndSwing = "\n{0} wastes a turn swatting the flies away from their empty medicine bag!\n{1} takes advantage of {0}'s lack of counting ability!\n";
        private const string healDodge = "\n{0} uses advanced diagnostic technology to anticipate and dodge {1}'s attack at the last moment!\n{0} is as agile as a CAT scan!\n\n";

        // ** Player Action Strings **

        private const string swingLeft = "{0} swings their sword to their left!\n";
        private const string swingRight = "{0} swings their sword to their right!\n";
        private const string guardLeft = "{0} raises their shield and guards their left side!\n";
        private const string guardRight = "{0} raises their shield and guards their right side!\n";
        private const string dodgeLeft = "{0} quickly dodges to their left!\n";
        private const string dodgeRight = "{0} quickly dodges to their right!\n";
        private const string heal = "{0} breaks out their emergency trauma kit!\nThey stitch up their gaping wounds and pour isopropyl alcohol over their head!\n";
        private const string failedHeal = "{0} fumbles around in their medical bag for supplies, but they are fresh out!\n{0} can't heal for the rest of the match!\n";
        private const string noAction = "{0} does nothing! How could they be so irresponsible?\n";

        public string GetPlayerActionNarration(Player player)
        {
            string? playerActionDescription = null;

            try 
            {
                switch (player.ChosenAction)
                {
                    default:
                        throw new ArgumentNullException("Player has not chosen an action!");
                    case Player.Action.swingL:
                        playerActionDescription = string.Format(swingLeft, player.Name);
                        break;
                    case Player.Action.swingR:
                        playerActionDescription = string.Format(swingRight, player.Name);
                        break;
                    case Player.Action.blockL:
                        playerActionDescription = string.Format(guardLeft, player.Name);
                        break;
                    case Player.Action.blockR:
                        playerActionDescription = string.Format(guardRight, player.Name);
                        break;
                    case Player.Action.dodgeL:
                        playerActionDescription = string.Format(dodgeLeft, player.Name);
                        break;
                    case Player.Action.dodgeR:
                        playerActionDescription = string.Format(dodgeRight, player.Name);
                        break;
                    // Healing player.ChosenAction if player has healed less than 3 times.
                    case Player.Action.heal when player.CanHeal:
                        playerActionDescription = string.Format(heal, player.Name);
                        break;
                    // Healing player.ChosenAction description if player has healed 3 times and cannot heal any more
                    case Player.Action.heal when !player.CanHeal:
                        playerActionDescription = string.Format(failedHeal, player.Name);
                        break;
                    case Player.Action.none:
                        playerActionDescription = string.Format(noAction, player.Name);
                        break;
                }
                if (playerActionDescription == null)
                {
                    throw new NullReferenceException("PlayerActionDescription is null!");
                }
            }
            catch (SystemException e)
            {
                return e.Message;
            }

            return playerActionDescription;
        }

        public string GetOutcomeNarration(Match.Outcome outcome, string? pOne, string? pTwo)
        {
            string? narration = null;

            try 
            {
                switch (outcome)
                {
                    default:
                        throw (new NullReferenceException("You did not pass an outcome!"));
                    case Match.Outcome.None:
                        narration = "You didn't supply any story beats to narrate!";
                        throw (new ArgumentException(narration, "Match.Outcome"));
                    case Match.Outcome.SwordClash:
                        narration = string.Format(swordClash, pOne, pTwo);
                        break;
                    case Match.Outcome.BothHit:
                        narration = string.Format(bothHit, pOne, pTwo);
                        break;
                    case Match.Outcome.P1Blocked:
                        narration = string.Format(blocked, pOne, pTwo);
                        break;
                    case Match.Outcome.P2Blocked:
                        narration = string.Format(blocked, pTwo, pOne);
                        break;
                    case Match.Outcome.P1FailedBlock:
                        narration = string.Format(failedBlock, pTwo, pOne);
                        break;
                    case Match.Outcome.P2FailedBlock:
                        narration = string.Format(failedBlock, pOne, pTwo);
                        break;
                    case Match.Outcome.BothBlock:
                        narration = string.Format(bothBlock, pOne, pTwo);
                        break;
                    case Match.Outcome.P1FailedDodge:
                        narration = string.Format(failedDodge, pOne, pTwo);
                        break;
                    case Match.Outcome.P2FailedDodge:
                        narration = string.Format(failedDodge, pTwo, pOne);
                        break;
                    case Match.Outcome.P1Dodge:
                        narration = string.Format(dodged, pOne, pTwo);
                        break;
                    case Match.Outcome.P2Dodge:
                        narration = string.Format(dodged, pTwo, pOne);
                        break;
                    case Match.Outcome.P1FailedCounter:
                        narration = string.Format(failedCounter, pOne, pTwo);
                        break;
                    case Match.Outcome.P2FailedCounter:
                        narration = string.Format(failedCounter, pTwo, pOne);
                        break;
                    case Match.Outcome.P1DodgeBlock:
                        narration = string.Format(dodgeBlock, pOne, pTwo);
                        break;
                    case Match.Outcome.P2DodgeBlock:
                        narration = string.Format(dodgeBlock, pTwo, pOne);
                        break;
                    case Match.Outcome.P1BlockP2Dodge:
                        narration = string.Format(failedDodgeBlock, pOne, pTwo);
                        break;
                    case Match.Outcome.P2BlockP1Dodge:
                        narration = string.Format(failedDodgeBlock, pTwo, pOne);
                        break;
                    case Match.Outcome.DoubleDodge:
                        narration = string.Format(doubleDodge, pOne, pTwo);
                        break;
                    case Match.Outcome.P1HealP2Defend:
                        narration = string.Format(uneventfulHeal, pOne, pTwo);
                        break;
                    case Match.Outcome.P2HealP1Defend:
                        narration = string.Format(uneventfulHeal, pTwo, pOne);
                        break;
                    case Match.Outcome.P1FailedHealP2Defend:
                        narration = string.Format(failedHealandDefend, pOne, pTwo);
                        break;
                    case Match.Outcome.P2FailedHealP1Defend:
                        narration = string.Format(failedHealandDefend, pTwo, pOne);
                        break;
                    case Match.Outcome.P1HealP2Swing:
                        narration = string.Format(healAndSwing, pOne, pTwo);
                        break;
                    case Match.Outcome.P2HealP1Swing:
                        narration = string.Format(healAndSwing, pTwo, pOne);
                        break;
                    case Match.Outcome.P1FailedHealP2Swing:
                        narration = string.Format(failedHealAndSwing, pOne, pTwo);
                        break;
                    case Match.Outcome.P2FailedHealP1Swing:
                        narration = string.Format(failedHealAndSwing, pTwo, pOne);
                        break;
                    case Match.Outcome.HealDodge:
                        narration = string.Format(healDodge, pOne, pTwo);
                        break;
                    }
                if (narration == null)
                {
                    throw (new NullReferenceException("Narration is null! Did you pass the right value?"));
                }
                }
            catch (SystemException e)
            {
                return e.Message;
            }

            return narration;
        }
    }
}
