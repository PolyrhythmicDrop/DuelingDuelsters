using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static DuelingDuelsters.Classes.Narrator;

namespace DuelingDuelsters.Classes
{

    /// <summary>
    /// Contains data about the current match, including player information, and the number of rounds since the match started. A Match object contains methods for processing player actions and generating the results of each round.
    /// </summary>
    public class Match
    {

        /// <summary>
        /// Instantiates a new Match object.
        /// </summary>
        /// <param name="playerOne">Player One, the first player.</param>
        /// <param name="playerTwo">Player Two, the second player.</param>
        /// <param name="narrator">Narrator, containing all the string constants and menus.</param>
        public Match(Player playerOne, Player playerTwo, Narrator narrator)
        {
            _playerOne = playerOne;
            _playerTwo = playerTwo;
            _narrator = narrator;
            RoundCounter = 1;
            rng = new Random();
        }

        private Player _playerOne;
        /// <summary>
        /// The first player.
        /// </summary>
        public Player PlayerOne
        {
            get { return _playerOne; }
            set { _playerOne = value; }
        }

        private Player _playerTwo;
        /// <summary>
        /// The second player.
        /// </summary>
        public Player PlayerTwo
        {
            get { return _playerTwo; }
            set { _playerTwo = value; }
        }

        /// <summary>
        /// The current round number. The round counter ticks up after each player's action is processed and the round results are returned.
        /// </summary>
        public int RoundCounter
        { get; set;}

        /// <summary>
        /// Random number generator instance. Used to figuratively roll dice to decide hits, damage, and more.
        /// </summary>
        private readonly Random rng;

        private Narrator _narrator;

        public enum Outcome
        {
            None,
            SwordClash,
            BothHit,
            P1Blocked,
            P2Blocked,
            P1FailedBlock,
            P2FailedBlock,
            BothBlock,
            P1FailedDodge,
            P2FailedDodge,
            P1Dodge,
            P2Dodge,
            P1FailedCounter,
            P2FailedCounter,
            P1DodgeBlock,
            P2DodgeBlock,
            P1BlockP2Dodge,
            P2BlockP1Dodge,
            DoubleDodge,
            P1HealP2Defend,
            P2HealP1Defend,
            P1FailedHealP2Defend,
            P2FailedHealP1Defend,
            P1HealP2Swing,
            P2HealP1Swing,
            P1FailedHealP2Swing,
            P2FailedHealP1Swing,
            HealDodge
        }

        /// <summary>
        /// Plays out the round based on each player's actions.
        /// </summary>
        public void PlayRound()
        {
            do
            {
                Console.WriteLine(DrawRoundHeader());
            }
            while (!_narrator.RunPlayerActionSelect(_playerOne));

            Console.Clear();

            do
            {
                Console.WriteLine(DrawRoundHeader());
            }
            while (!_narrator.RunPlayerActionSelect(_playerTwo));

            _narrator.PressAnyKey();

            GameLoop.GameState = State.OutcomeDisplay;

            do
            {
                // Describe player individual actions
                Console.WriteLine(_narrator.GetPlayerActionNarration(_playerOne));
                Thread.Sleep(500);
                Console.WriteLine(_narrator.GetPlayerActionNarration(_playerTwo));
                Thread.Sleep(500);

                // Get the outcome.
                Outcome actionKey = GetOutcome(_playerOne.ChosenAction, _playerTwo.ChosenAction);
                // Get and print the narration for the given outcome.
                Console.WriteLine(_narrator.GetOutcomeNarration(actionKey, _playerOne.Name, _playerTwo.Name));
                // Process the outcome.
                ProcessOutcome(actionKey);
                // Increment the round counter.
                RoundCounter++;

                // Reset player actions for the next round.
                _playerOne.ActionTaken = false;
                _playerTwo.ActionTaken = false;
            }
            while (_playerOne.ActionTaken == true && _playerTwo.ActionTaken == true);
        }

        private Outcome GetOutcome(Player.Action actionOne, Player.Action actionTwo)
        {
            Outcome outcome = Outcome.None;
           
            // SWING actions
            // Player One and Player Two swing in the same direction. Neither player takes damage.
            if ((actionOne == Player.Action.swingL && actionTwo == Player.Action.swingL) || 
                (actionOne == Player.Action.swingR && actionTwo == Player.Action.swingR))
            {
                outcome = Outcome.SwordClash;
            }
            // Player One and Player Two swing in opposite directions. Both players take full damage and have a chance to crit. 
            else if ((actionOne == Player.Action.swingL && actionTwo == Player.Action.swingR) || 
                (actionOne == Player.Action.swingR && actionTwo == Player.Action.swingL))
            {
                outcome = Outcome.BothHit;
            }

            // SWING + BLOCK actions
            // P1 swings and P2 blocks in the same direction.  P1 is staggered.
            else if ((actionOne == Player.Action.swingL && actionTwo == Player.Action.blockL) || 
                (actionOne == Player.Action.swingR && actionTwo == Player.Action.blockR))
            {
                outcome = Outcome.P1Blocked;                
            }
            // P2 swings and P1 blocks in the same direction. P2 is staggered.
            else if ((actionTwo == Player.Action.swingL && actionOne == Player.Action.blockL) || 
                (actionTwo == Player.Action.swingR && actionOne == Player.Action.blockR))
            {
                outcome = Outcome.P2Blocked;
            }
            // P2 swings and P1 blocks in the opposite direction. P1 takes damage.
            else if ((actionTwo == Player.Action.swingL && actionOne == Player.Action.blockR) || 
                (actionTwo == Player.Action.swingR && actionOne == Player.Action.blockL))
            {
                outcome = Outcome.P1FailedBlock;
            }
            // P1 swings and P2 blocks in the opposite direction. P2 takes damage.
            else if ((actionOne == Player.Action.swingL && actionTwo == Player.Action.blockR) || 
                (actionOne == Player.Action.swingR && actionTwo == Player.Action.blockL))
            {
                outcome = Outcome.P2FailedBlock;
            }

            // Double BLOCK action
            // Both players block in any direction. Neither player takes damage or is staggered.
            else if ((actionOne == Player.Action.blockL || actionOne == Player.Action.blockR) && 
                (actionTwo == Player.Action.blockL || actionTwo == Player.Action.blockR))
            {
                outcome = Outcome.BothBlock;
            }

            // SWING + DODGE actions
            // P2 swings and P1 dodges in the opposite direction. P2 gets an automatic crit.
            else if ((actionTwo == Player.Action.swingL && actionOne == Player.Action.dodgeR) || 
                (actionTwo == Player.Action.swingR && actionOne == Player.Action.dodgeL))
            {
                outcome = Outcome.P1FailedDodge;
            }
            // P1 swings and P2 dodges in the opposite direction. P1 gets an automatic crit.
            else if ((actionOne == Player.Action.swingL && actionTwo == Player.Action.dodgeR) || 
                (actionOne == Player.Action.swingR && actionTwo == Player.Action.dodgeL))
            {
                outcome = Outcome.P2FailedDodge;
            }
            // P2 swings and P1 dodges in the same direction.
            // P1 gets the chance to counter, and a higher chance to crit on that counter.
            else if ((actionTwo == Player.Action.swingL && actionOne == Player.Action.dodgeL) ||
                (actionTwo == Player.Action.swingR && actionOne == Player.Action.dodgeR))
            {
                outcome = Outcome.P1Dodge;
            }
            // P1 swings and P2 dodges in the same direction.
            // P2 gets the chance to counter, and a higher chance to crit on that counter.
            else if ((actionOne == Player.Action.swingL && actionTwo == Player.Action.dodgeL) || 
                (actionOne == Player.Action.swingR && actionTwo == Player.Action.dodgeR))
            {
                outcome = Outcome.P2Dodge;
            }

            // BLOCK + DODGE actions
            // P2 blocks and P1 dodges in the same direction. P1 gets chance to counter, though they cannot crit and the damage is reduced by half.
            else if ((actionTwo == Player.Action.blockL && actionOne == Player.Action.dodgeL) || 
                (actionTwo == Player.Action.blockR && actionOne == Player.Action.dodgeR))
            {
                outcome = Outcome.P1DodgeBlock;
            }
            // P1 blocks and P2 dodges in the same direction. P2 gets chance to counter, though they cannot crit and the damage is reduced by half.
            else if ((actionOne == Player.Action.blockL && actionTwo == Player.Action.dodgeL) || 
                (actionOne == Player.Action.blockR && actionTwo == Player.Action.dodgeR))
            {
                outcome = Outcome.P2DodgeBlock;
            }
            // P1 blocks and P2 dodges in the opposite direction. Neither player takes damage.
            else if ((actionOne == Player.Action.blockL && actionTwo == Player.Action.dodgeR) ||
                (actionOne == Player.Action.blockR && actionTwo == Player.Action.dodgeL))
            {
                outcome = Outcome.P1BlockP2Dodge;
            }
            // P2 blocks and P1 dodges in the opposite direction. Neither player takes damage.
            else if ((actionTwo == Player.Action.blockL && actionOne == Player.Action.dodgeR) || 
                (actionTwo == Player.Action.blockR && actionOne == Player.Action.dodgeL))
            {
                outcome = Outcome.P2BlockP1Dodge;
            }

            // Double DODGE action
            // Both players dodge. Nobody takes damage.
            else if ((actionOne == Player.Action.dodgeL || actionOne == Player.Action.dodgeR) &&
                (actionTwo == Player.Action.dodgeL || actionTwo == Player.Action.dodgeR))
            {
                outcome = Outcome.DoubleDodge;
            }

            // HEAL actions
            // P1 heals and P2 does anything other than swing. P1 restores health and nothing happens to P2.
            else if (actionOne == Player.Action.heal && (actionTwo != Player.Action.swingL && actionTwo != Player.Action.swingR))
            {
                if (_playerOne.CanHeal)
                { outcome = Outcome.P1HealP2Defend; }
                else 
                { outcome = Outcome.P1FailedHealP2Defend; }
            }
            // P2 heals and P1 does anything other than swing. P2 restores health and nothing happens to P1.
            else if (actionTwo == Player.Action.heal && (actionOne != Player.Action.swingL && actionOne != Player.Action.swingR))
            {
                if (_playerTwo.CanHeal)
                { outcome = Outcome.P2HealP1Defend; }
                else 
                { outcome = Outcome.P2FailedHealP1Defend; }
            }

            // P1 heals and P2 swings. P1 restores health and has a chance to dodge P2's attack.
            else if (actionOne == Player.Action.heal && (actionTwo == Player.Action.swingL || actionTwo == Player.Action.swingR))
            {
                // P1 heals because they have healed less than 3 times. P2 has a chance to land an attack.
                if (_playerOne.CanHeal)
                {
                    outcome = Outcome.P1HealP2Swing;
                    
                }
                // P1 cannot heal because they have healed 3 times. P2 counters.
                else
                {
                    outcome = Outcome.P1FailedHealP2Swing;
                }
            }
            // P2 heals and P1 swings. P2 restores health and has a chance to dodge P1's attack.
            else if (actionTwo == Player.Action.heal && (actionOne == Player.Action.swingL || actionOne == Player.Action.swingR))
            {
                // P2 heals because they have healed less than 3 times. P1 has a chance to land an attack.
                if (_playerTwo.CanHeal)
                {
                    outcome = Outcome.P2HealP1Swing;

                }
                // P2 cannot heal because they have healed 3 times. P1 counters.
                else
                {
                    outcome = Outcome.P2FailedHealP1Swing;
                }
            }

            return outcome;
        }

        private void ProcessOutcome(Outcome actionKey)
        {
            try
            {
                // Apply the attack results based on the outcome.
                switch (actionKey)
                {
                    default:
                        throw new ArgumentNullException(actionKey.ToString(), "No matching actionKey passed!");
                    case Outcome.SwordClash:
                    case Outcome.P1BlockP2Dodge:
                    case Outcome.P2BlockP1Dodge:
                    case Outcome.DoubleDodge:
                    case Outcome.P1FailedHealP2Defend:
                    case Outcome.P2FailedHealP1Defend:
                    case Outcome.BothBlock:
                        break;

                    case Outcome.BothHit:
                        ApplyAttackResult(_playerOne, _playerTwo);
                        Thread.Sleep(500);
                        ApplyAttackResult(_playerTwo, _playerOne);
                        break;

                    case Outcome.P1Blocked:
                        _playerOne.IsStaggered = true;
                        break;

                    case Outcome.P2Blocked:
                        _playerTwo.IsStaggered = true;
                        break;

                    case Outcome.P1FailedBlock:
                    case Outcome.P1FailedDodge:
                        ApplyAttackResult(_playerTwo, _playerOne);
                        break;

                    case Outcome.P2FailedBlock:
                    case Outcome.P2FailedDodge:
                        ApplyAttackResult(_playerOne, _playerTwo);
                        break;

                    case Outcome.P1Dodge:
                    case Outcome.P1DodgeBlock:
                        CounterAttack(_playerOne, _playerTwo);
                        break;

                    case Outcome.P2Dodge:
                    case Outcome.P2DodgeBlock:
                        CounterAttack(_playerTwo, _playerOne);
                        break;

                    case Outcome.P1HealP2Defend:
                        _playerOne.HealSelf();
                        break;

                    case Outcome.P2HealP1Defend:
                        _playerTwo.HealSelf();
                        break;

                    case Outcome.P1HealP2Swing:
                    case Outcome.P1FailedHealP2Swing:
                        _playerOne.HealSelf();
                        AttemptHealCounter(_playerOne, _playerTwo);
                        break;

                    case Outcome.P2HealP1Swing:
                    case Outcome.P2FailedHealP1Swing:
                        _playerTwo.HealSelf();
                        AttemptHealCounter(_playerTwo, _playerOne);
                        break;
                } 
            }
            catch (SystemException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void CounterAttack(Player atkPlayer, Player defPlayer)
        {
            atkPlayer.IsCounter();
            if (atkPlayer.IsCountering == false)
            {
                Console.WriteLine(_narrator.GetOutcomeNarration(Outcome.P1FailedCounter, atkPlayer.Name, defPlayer.Name));
            }
            else
            {
                ApplyAttackResult(atkPlayer, defPlayer);
                atkPlayer.IsCountering = false;
            }
        }

        private void AttemptHealCounter(Player healer, Player counter)
        {
            // P1 has a chance to dodge P2's attack if they successfully heal and roll a 6 or greater.
            switch (healer.IsHealing)
            {
                case (true):
                    int healDodgeRoll = rng.Next(0, 10);
                    if (healDodgeRoll >= 6)
                    {
                        Console.WriteLine(_narrator.GetOutcomeNarration(Outcome.HealDodge, healer.Name, counter.Name));
                        healer.IsHealing = false;
                        break;
                    }
                    else
                    {
                        ApplyAttackResult(counter, healer);
                        break;
                    }
                case (false):
                    ApplyAttackResult(counter, healer);
                    break;
            }
        }
        
        /// <summary>
        /// Calculates damage to a defending player after a successful attack by an attacking player, calculates whether the hit crits and for how much damage, and then applies the damage to the defending player.
        /// </summary>
        /// <param name="atkPlayer">The attacking player.</param>
        /// <param name="defPlayer">The defending player.</param>
        private void ApplyAttackResult(Player atkPlayer, Player defPlayer)
        { 
            // Calculate attacking player one's base damage
            int atkBaseDamage = atkPlayer.CalculateBaseDamage(defPlayer);
            // Divide base damage by two if the defending player is blocking during a counter.
            if (atkPlayer.IsCountering == true && (defPlayer.ChosenAction == Player.Action.blockR || defPlayer.ChosenAction == Player.Action.blockL)) 
            {
                atkBaseDamage = atkBaseDamage / 2;
            }
            // Initialize attacking player crit damage
            int critDamage;
            // Calculate whether attacking has succeeded in a critical hit
            bool isCrit = atkPlayer.IsCrit(defPlayer);
            // If the critical hit is successful, calculate the crit damage and print critical hit message
            if (isCrit) 
            {
                Console.WriteLine($"{atkPlayer.Name} scores a **CRITICAL HIT!**");
                Thread.Sleep(500);
                critDamage = atkPlayer.CalculateCritDamage();
                // If staggered and then hit, defending player is no longer staggered.
                defPlayer.IsStaggered = false;
            }
            else { critDamage = 0; };
            // Get the total damage amount
            int atkTotalDamageGiven = atkBaseDamage + critDamage;
            // Print damage taken by defending player
            Console.WriteLine($"{defPlayer.Name} is hit for {atkTotalDamageGiven} damage!\n");
            // Apply damage to defending player's health
            defPlayer.Health = defPlayer.Health - atkTotalDamageGiven;
        }

        /// <summary>
        /// Draws the round header during a match using data from the players.
        /// </summary>
        /// <returns></returns>
        public string DrawRoundHeader()
        {
            // Get the length of the current player names, health, and classes
            int p1CharNameLength = PlayerOne.Name.Length;
            int p2CharNameLength = PlayerTwo.Name.Length;
            int p1ClassNameLength = PlayerOne.PlayerClass.Length;
            int p2ClassNameLength = PlayerTwo.PlayerClass.Length;
            // Get the length of both names + 40
            int headerLength = p1CharNameLength + p2CharNameLength + 40;
            // Create a string for the spaces between character names and left/right border
            System.String blankChars = new string(' ', (headerLength - 2));
            // Create a variable for the VS. that checks to see whether the sum of p1 & p2's character name length is even. If even, the variable is "VS.". If odd, the variable is "VS. ". This should account for the divide by two error.
            string versus = headerLength % 2 == 0 ? " VS." : "VS. ";
            // Get length of versus.
            int versusLength = versus.Length;
            // Create a string for the spaces between character names, include room for the VS.
            System.String nameBlanks = new string(' ', ((headerLength - (p1CharNameLength + p2CharNameLength)) / 2 ) - 4);
            // Create a string for the space between dashes under character names
            System.String underNameBlanks = new string(' ', headerLength - (p1CharNameLength + p2CharNameLength) - 4);
            // Get lengths for P1 & P2 health statuses
            int p1HealthStatusLength = PlayerOne.HealthReadout.Length;
            int p2HealthStatusLength = PlayerTwo.HealthReadout.Length;
            // Create a string for the spaces between health statuses
            System.String healthBlanks = new string(' ', (headerLength - (p1HealthStatusLength + p2HealthStatusLength) - 4));
            // Create dashes corresponding to player name length
            System.String p1Dashes = new string('-', p1CharNameLength);
            System.String p2Dashes = new string('-', p2CharNameLength);
            // Create string for round number readout
            string roundReadout = $"ROUND #{RoundCounter}";
            // Get length of the roundReadout
            int roundReadoutLength = roundReadout.Length;
            // Create an extra space for the round readout if headerLength is odd.
            int roundOddSpace = headerLength % 2 == 0 ? 0 : 1;
            string roundOddMod = new string(' ', roundOddSpace);
            // Create string for spaces before and after the round readout
            string roundBlanks = new string(' ', (headerLength / 2 ) - (roundReadoutLength / 2) - (versusLength / 2 ));
            // Create string for blank spaces between character classes
            string classBlanks = new string(' ', (headerLength - (p1ClassNameLength + p2ClassNameLength) - 4));


            // String builder actions
            // Create instance of string builder to build the round header
            StringBuilder stringBuilder = new StringBuilder();
            // Build a line of stars that is the two character names + 20, for the top of the box
            stringBuilder.Append('*', headerLength);
            // New line after header
            stringBuilder.Append("\n");
            // Blanks before round number readout
            stringBuilder.AppendLine($"*{blankChars}*");
            // Round number readout
            stringBuilder.AppendLine($"* {roundBlanks}{roundReadout}{roundBlanks}{roundOddMod} *");
            // Blanks after round number readout
            stringBuilder.AppendLine($"*{blankChars}*");
            // Stars after round number readout
            stringBuilder.Append('*', headerLength);
            stringBuilder.Append("\n");
            // Second line of header
            stringBuilder.AppendLine($"*{blankChars}*");
            // Player one name vs. Player two name
            stringBuilder.AppendLine($"* {PlayerOne.Name}{nameBlanks}{versus}{nameBlanks}{PlayerTwo.Name} *");
            // Dashes underneath player names
            stringBuilder.AppendLine($"* {p1Dashes}{underNameBlanks}{p2Dashes} *");
            // Player class line
            stringBuilder.AppendLine($"* {PlayerOne.PlayerClass}{classBlanks}{PlayerTwo.PlayerClass} *");
            // Health status line
            stringBuilder.AppendLine($"* {PlayerOne.HealthReadout}{healthBlanks}{PlayerTwo.HealthReadout} *");
            // Append heals remaining if a character is a medic.
            if (_playerOne.PlayerClass == "Medic" && _playerTwo.PlayerClass != "Medic")
            {
                int p1HealsLeft = 3 - _playerOne.HealCount;
                string healsLeftString = $"Heals Left: {p1HealsLeft}";
                int healsLeftLength = healsLeftString.Length;
                // Create a string for the space between player heals left sections.
                string healsLeftBlanks = new string(' ', headerLength - healsLeftLength - 4);
                stringBuilder.AppendLine($"* {healsLeftString}{healsLeftBlanks} *");
            }
            else if (_playerTwo.PlayerClass == "Medic" && _playerOne.PlayerClass != "Medic")
            {
                int p2HealsLeft = 3 - _playerTwo.HealCount;
                string healsLeftString = $"Heals Left: {p2HealsLeft}";
                int healsLeftLength = healsLeftString.Length;
                // Create a string for the space between player heals left sections.
                string healsLeftBlanks = new string(' ', headerLength - healsLeftLength - 4);
                stringBuilder.AppendLine($"* {healsLeftBlanks}{healsLeftString} *");
            }
            else if (_playerOne.PlayerClass == "Medic" && _playerTwo.PlayerClass == "Medic")
            {
                int p1HealsLeft = 3 - _playerOne.HealCount;
                int p2HealsLeft = 3 - _playerTwo.HealCount;
                string p1healsLeftString = $"Heals Left: {p1HealsLeft}";
                string p2healsLeftString = $"Heals Left: {p2HealsLeft}";
                int p1healsLeftLength = p1healsLeftString.Length;
                int p2healsLeftLength = p2healsLeftString.Length;
                // Create a string for the space between player heals left sections.
                string healsLeftBlanks = new string(' ', headerLength - (p1healsLeftLength + p2healsLeftLength) - 4);
                stringBuilder.AppendLine($"* {p1healsLeftString}{healsLeftBlanks}{p2healsLeftString} *");
            }

            // Blank line with border
            stringBuilder.AppendLine($"*{blankChars}*");
            // Footer of stars
            stringBuilder.Append('*', headerLength);
            // Convert stringBuilder to string
            string roundHeader = stringBuilder.ToString();

            // Return the built header
            return roundHeader;
        }


    }
}
