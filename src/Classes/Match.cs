using System.Text;
using static DuelingDuelsters.Classes.Narrator;

namespace DuelingDuelsters.Classes
{

    /// <summary>
    /// Manages data about the current match, including the two players, their stats, and the number of rounds since the match started. A <c>Match</c> object contains methods for processing player actions and generating the results of each round.
    /// </summary>
    public class Match
    {

        /// <summary>
        /// Instantiates a new Match object with two <see cref="Player"/> characters and a <see cref="Narrator"/>.<br/>Sets the <see cref="RoundCounter"/> to <c>1</c> and initializes the match's <see cref="rng"/> member.
        /// </summary>
        /// <param name="playerOne">Player One, the first player.</param>
        /// <param name="playerTwo">Player Two, the second player.</param>
        /// <param name="narrator"><see cref="Narrator"/> object to manage all the action menus and outcome strings.</param>
        public Match(Player playerOne, Player playerTwo, Narrator narrator)
        {
            _playerOne = playerOne;
            _playerTwo = playerTwo;
            _narrator = narrator;
            RoundCounter = 1;
            rng = new Random();
        }

        /// <exclude />
        private Player _playerOne;

        /// <summary>
        /// The first player. This player is always human-controlled.
        /// </summary>
        public Player PlayerOne
        {
            get { return _playerOne; }
            set { _playerOne = value; }
        }

        /// <exclude />
        private Player _playerTwo;

        /// <summary>
        /// The second player. This player can be controlled by a human or by the game's AI, determined by the player's <see cref="Player.Brain"/>.
        /// </summary>
        public Player PlayerTwo
        {
            get { return _playerTwo; }
            set { _playerTwo = value; }
        }

        /// <summary>
        /// The current round number.
        /// <para>
        /// The <c>RoundCounter</c> ticks up after each player's action is processed and the round results are returned. The <c>RoundCounter</c> is reset to <c>1</c> at the start of a match.
        /// </para>
        /// </summary>
        public int RoundCounter
        { get; set; }

        /// <summary>
        /// The match's random number generator instance. Used to figuratively roll the dice to decide hits, damage, and more.
        /// <para>
        /// The RNG is seeded with the current date and time every time a new match is begun.
        /// </para>
        /// </summary>
        private readonly Random rng;

        /// <exclude />
        private Narrator _narrator;

        /// <summary>
        /// Available outcomes for a round. 
        /// <para>
        /// An <c>Outcome</c> is decided by a combination of the two player's chosen <see cref="Player.Action"> Action</see>, the directions of those actions, and dice rolls performed by the <see cref="Match">Match</see>'s <see cref="rng"/> member.
        /// </para> 
        /// <para>
        /// After the round <c>Outcome</c> is determined, it is used to get narration strings from the <see cref="Narrator"/>, calculate damage, counters, and heals, and set the stage for the next round.
        /// </para> 
        /// </summary>
        public enum Outcome
        {
            /// <summary>
            /// The default action. The <c>Outcome</c> is reset to <c>None</c> at the start of the round.
            /// </summary>
            None,
            /// <summary>
            /// <see cref="PlayerOne">Player One</see> and <see cref="PlayerTwo">Player Two</see> swing in the same direction. Neither player takes damage.
            /// </summary>
            SwordClash,
            /// <summary>
            /// <see cref="PlayerOne">Player One</see> and <see cref="PlayerTwo">Player Two</see> swing in opposite directions. Both players take full damage and have a chance to crit. 
            /// </summary>
            BothHit,
            /// <summary>
            /// <see cref="PlayerOne">Player One</see> swings and <see cref="PlayerTwo">Player Two</see> blocks in the same direction. <see cref="PlayerOne">Player One</see> is staggered.
            /// </summary>
            P1Blocked,
            /// <summary>
            /// <see cref="PlayerTwo">Player Two</see> swings and <see cref="PlayerOne">Player One</see> blocks in the same direction. <see cref="PlayerTwo">Player Two</see> is staggered.
            /// </summary>
            P2Blocked,
            /// <summary>
            /// <see cref="PlayerTwo">Player Two</see> swings and <see cref="PlayerOne">Player One</see> blocks in the opposite direction. <see cref="PlayerOne">Player One</see> takes damage.
            /// </summary>
            P1FailedBlock,
            /// <summary>
            /// <see cref="PlayerOne">Player One</see> swings and <see cref="PlayerTwo">Player Two</see> blocks in the opposite direction. <see cref="PlayerTwo">Player Two</see> takes damage.
            /// </summary>
            P2FailedBlock,
            /// <summary>
            /// Both players block in any direction. Neither player takes damage or is staggered.
            /// </summary>
            BothBlock,
            /// <summary>
            /// <see cref="PlayerTwo">Player Two</see> swings and <see cref="PlayerOne">Player One</see> dodges in the opposite direction. <see cref="PlayerTwo">Player Two</see> gets an automatic critical hit.
            /// </summary>
            P1FailedDodge,
            /// <summary>
            /// <see cref="PlayerOne">Player One</see> swings and <see cref="PlayerTwo">Player Two</see> dodges in the opposite direction. <see cref="PlayerOne">Player One</see> gets an automatic crit.
            /// </summary>
            P2FailedDodge,
            /// <summary>
            /// <see cref="PlayerTwo">Player Two</see> swings and <see cref="PlayerOne">Player One</see> dodges in the same direction. <see cref="PlayerOne">Player One</see> gets the chance to counter, and a higher chance to land a critical hit on that counter.
            /// </summary>
            P1Dodge,
            /// <summary>
            /// <see cref="PlayerOne">Player One</see> swings and <see cref="PlayerTwo">Player Two</see> dodges in the same direction. <see cref="PlayerTwo">Player Two</see> gets the chance to counter, and a higher chance to land a critical hit on that counter.
            /// </summary>
            P2Dodge,
            /// <summary>
            /// <see cref="PlayerOne">Player One</see> fails a counter. This Outcome is only used to get narration. 
            /// </summary>
            P1FailedCounter,
            /// <summary>
            /// <see cref="PlayerTwo">Player Two</see> fails a counter. This Outcome is only used to get narration. 
            /// </summary>
            P2FailedCounter,
            /// <summary>
            /// <see cref="PlayerTwo">Player Two</see> blocks and <see cref="PlayerOne">Player One</see> dodges in the same direction. <see cref="PlayerOne">Player One</see> gets chance to counter, though they cannot land a critical hit and the damage is reduced by half.
            /// </summary>
            P1DodgeBlock,
            /// <summary>
            /// <see cref="PlayerOne">Player One</see> blocks and <see cref="PlayerTwo">Player Two</see> dodges in the same direction. <see cref="PlayerTwo">Player Two</see> gets chance to counter, though they cannot land a critical hit and the damage is reduced by half.
            /// </summary>
            P2DodgeBlock,
            /// <summary>
            /// <see cref="PlayerOne">Player One</see> blocks and <see cref="PlayerTwo">Player Two</see> dodges in the opposite direction. Neither player takes damage.
            /// </summary>
            P1BlockP2Dodge,
            /// <summary>
            /// <see cref="PlayerTwo">Player Two</see> blocks and <see cref="PlayerOne">Player One</see> dodges in the opposite direction. Neither player takes damage.
            /// </summary>
            P2BlockP1Dodge,
            /// <summary>
            /// Both players dodge. Nobody takes damage.
            /// </summary>
            DoubleDodge,
            /// <summary>
            /// <see cref="PlayerOne">Player One</see> heals and <see cref="PlayerTwo">Player Two</see> defends or dodges. <see cref="PlayerOne">Player One</see> restores health and nothing happens to <see cref="PlayerTwo">Player Two</see>.
            /// </summary>
            P1HealP2Defend,
            /// <summary>
            /// <see cref="PlayerTwo">Player Two</see> heals and <see cref="PlayerOne">Player One</see> blocks or dodges. <see cref="PlayerTwo">Player Two</see> restores health and nothing happens to <see cref="PlayerOne">Player One</see>.
            /// </summary>
            P2HealP1Defend,
            /// <summary>
            /// <see cref="PlayerOne">Player One</see> fails to heal and <see cref="PlayerTwo">Player Two</see> blocks or dodges. A wasted turn for both players.
            /// </summary>
            P1FailedHealP2Defend,
            /// <summary>
            /// <see cref="PlayerTwo">Player Two</see> fails to heal and <see cref="PlayerOne">Player One</see> blocks or dodges. A wasted turn for both players.
            /// </summary>
            P2FailedHealP1Defend,
            /// <summary>
            /// <see cref="PlayerOne">Player One</see> heals and <see cref="PlayerTwo">Player Two</see> swings. <see cref="PlayerOne">Player One</see> restores health and has a chance to dodge <see cref="PlayerTwo">Player Two</see>'s attack.
            /// </summary>
            P1HealP2Swing,
            /// <summary>
            /// <see cref="PlayerTwo">Player Two</see> heals and <see cref="PlayerOne">Player One</see> swings. <see cref="PlayerTwo">Player Two</see> restores health and has a chance to dodge <see cref="PlayerOne">Player One</see>'s attack.
            /// </summary>
            P2HealP1Swing,
            /// <summary>
            /// <see cref="PlayerOne">Player One</see> fails to heal and <see cref="PlayerTwo">Player Two</see> swings. Player Two has a chance for a critical hit.
            /// </summary>
            P1FailedHealP2Swing,
            /// <summary>
            /// <see cref="PlayerTwo">Player Two</see> fails to heal and <see cref="PlayerOne">Player One</see> swings. Player One has a chance for a critical hit.
            /// </summary>
            P2FailedHealP1Swing,
            /// <summary>
            /// <see cref="PlayerOne">Player One</see> fails to heal and <see cref="PlayerTwo">Player Two</see> heals.
            /// </summary>
            P1FailedHealP2Heal,
            /// <summary>
            /// <see cref="PlayerTwo">Player Two</see> fails to heal and <see cref="PlayerOne">Player One</see> heals.
            /// </summary>
            P2FailedHealP1Heal,
            /// <summary>
            /// Both players fail to heal. A wasted turn for both players.
            /// </summary>
            DoubleFailedHeal,
            /// <summary>
            /// The healing player dodges the attacking player's counterattack.
            /// </summary>
            HealDodge,
            /// <summary>
            /// The healing player gets hit by the attacking player's counterattack.
            /// </summary>
            HealCounter,
            /// <summary>
            /// Both players successfully heal on the same turn.
            /// </summary>
            DoubleHeal
        }

        /// <summary>
        /// The main gameplay loop for a round of a match. Each player selects their actions using the <see cref="Narrator.RunPlayerActionSelect(Player)"></see> method. The selected actions are then narrated and processed into <see cref="Outcome"/> data. The <c>Outcome</c> data is then processed and narrated to the players.
        /// </summary>
        public void PlayRound()
        {
        P1ChooseAction:
            // Change the state
            GameLoop.GameState = State.ActionSelect;
            do
            {
                Console.Clear();
                Console.WriteLine(DrawRoundHeader());
            }
            while (!_narrator.RunPlayerActionSelect(_playerOne));
            if (_narrator.Choice == Choices.Back)
            {
                _narrator.Choice = Choices.Reset;
                Console.Clear();
                goto P1ChooseAction;
            }

            Console.Clear();
        P2ChooseAction:
            do
            {
                Console.Clear();
                Console.WriteLine(DrawRoundHeader());
            }
            while (!_narrator.RunPlayerActionSelect(_playerTwo));
            if (_narrator.Choice == Choices.Back)
            {
                _narrator.Choice = Choices.Reset;
                Console.Clear();
                goto P2ChooseAction;
            }

            GameLoop.GameState = State.OutcomeDisplay;
            Console.Clear();
            Console.WriteLine(DrawRoundHeader());

            ProcessRound();

            // Increment the round counter.
            RoundCounter++;

            // Reset player actions for the next round.
            _playerOne.ActionTaken = false;
            _playerTwo.ActionTaken = false;
        }

        /// <summary>
        /// Processes the round's actions and outcomes.
        /// <list type="bullet">
        /// <item>
        /// <description>Actions are narrated using the <see cref="Narrator.GetPlayerActionNarration(Player)"></see> method.</description>
        /// </item>
        /// <item>
        /// <description>The round's <see cref="Outcome"></see> is generated using the <see cref="GetOutcome(Player.Action, Player.Action)"></see> method.</description>
        /// </item>
        /// <item>
        /// <description>The Outcome is narrated using the <see cref="Narrator.GetOutcomeNarration(Outcome, string?, string?)"></see> method.</description>
        /// </item>
        /// <item>
        /// <description>The Outcome is processed using <see cref="Match.ProcessOutcome(Outcome)"></see> method.</description>
        /// </item>
        /// </list>
        /// </summary>
        private void ProcessRound()
        {
            // Describe player individual actions
            Console.WriteLine("\n" + _narrator.GetPlayerActionNarration(_playerOne));
            Thread.Sleep(700);
            Console.WriteLine(_narrator.GetPlayerActionNarration(_playerTwo));
            Thread.Sleep(700);

            // Get the outcome.
            Outcome actionKey = GetOutcome(_playerOne.ChosenAction, _playerTwo.ChosenAction);
            // Get and print the narration for the given outcome.
            Console.WriteLine(_narrator.GetOutcomeNarration(actionKey, _playerOne.Name, _playerTwo.Name));
            Thread.Sleep(700);
            // Process the outcome.
            ProcessOutcome(actionKey);
            Thread.Sleep(700);

            _narrator.PressAnyKey();

        }

        private Outcome GetOutcome(Player.Action actionOne, Player.Action actionTwo)
        {
            Outcome outcome = Outcome.None;

            // ** Double SWING Outcomes **

            if ((actionOne == Player.Action.swingL && actionTwo == Player.Action.swingL) ||
                (actionOne == Player.Action.swingR && actionTwo == Player.Action.swingR))
            {
                outcome = Outcome.SwordClash;
            }
            else if ((actionOne == Player.Action.swingL && actionTwo == Player.Action.swingR) ||
                (actionOne == Player.Action.swingR && actionTwo == Player.Action.swingL))
            {
                outcome = Outcome.BothHit;
            }

            // ** SWING + BLOCK Outcomes **

            else if ((actionOne == Player.Action.swingL && actionTwo == Player.Action.blockL) ||
                (actionOne == Player.Action.swingR && actionTwo == Player.Action.blockR))
            {
                outcome = Outcome.P1Blocked;
            }
            else if ((actionTwo == Player.Action.swingL && actionOne == Player.Action.blockL) ||
                (actionTwo == Player.Action.swingR && actionOne == Player.Action.blockR))
            {
                outcome = Outcome.P2Blocked;
            }
            else if ((actionTwo == Player.Action.swingL && actionOne == Player.Action.blockR) ||
                (actionTwo == Player.Action.swingR && actionOne == Player.Action.blockL))
            {
                outcome = Outcome.P1FailedBlock;
            }
            else if ((actionOne == Player.Action.swingL && actionTwo == Player.Action.blockR) ||
                (actionOne == Player.Action.swingR && actionTwo == Player.Action.blockL))
            {
                outcome = Outcome.P2FailedBlock;
            }

            // ** Double BLOCK Outcome **

            else if ((actionOne == Player.Action.blockL || actionOne == Player.Action.blockR) &&
                (actionTwo == Player.Action.blockL || actionTwo == Player.Action.blockR))
            {
                outcome = Outcome.BothBlock;
            }

            // ** SWING + DODGE Outcomes **

            else if ((actionTwo == Player.Action.swingL && actionOne == Player.Action.dodgeR) ||
                (actionTwo == Player.Action.swingR && actionOne == Player.Action.dodgeL))
            {
                outcome = Outcome.P1FailedDodge;
            }
            else if ((actionOne == Player.Action.swingL && actionTwo == Player.Action.dodgeR) ||
                (actionOne == Player.Action.swingR && actionTwo == Player.Action.dodgeL))
            {
                outcome = Outcome.P2FailedDodge;
            }
            else if ((actionTwo == Player.Action.swingL && actionOne == Player.Action.dodgeL) ||
                (actionTwo == Player.Action.swingR && actionOne == Player.Action.dodgeR))
            {
                outcome = Outcome.P1Dodge;
            }
            else if ((actionOne == Player.Action.swingL && actionTwo == Player.Action.dodgeL) ||
                (actionOne == Player.Action.swingR && actionTwo == Player.Action.dodgeR))
            {
                outcome = Outcome.P2Dodge;
            }

            // ** BLOCK + DODGE Outcomes **

            else if ((actionTwo == Player.Action.blockL && actionOne == Player.Action.dodgeL) ||
                (actionTwo == Player.Action.blockR && actionOne == Player.Action.dodgeR))
            {
                outcome = Outcome.P1DodgeBlock;
            }
            else if ((actionOne == Player.Action.blockL && actionTwo == Player.Action.dodgeL) ||
                (actionOne == Player.Action.blockR && actionTwo == Player.Action.dodgeR))
            {
                outcome = Outcome.P2DodgeBlock;
            }
            else if ((actionOne == Player.Action.blockL && actionTwo == Player.Action.dodgeR) ||
                (actionOne == Player.Action.blockR && actionTwo == Player.Action.dodgeL))
            {
                outcome = Outcome.P1BlockP2Dodge;
            }
            else if ((actionTwo == Player.Action.blockL && actionOne == Player.Action.dodgeR) ||
                (actionTwo == Player.Action.blockR && actionOne == Player.Action.dodgeL))
            {
                outcome = Outcome.P2BlockP1Dodge;
            }

            // ** Double DODGE Outcomes ** 

            else if ((actionOne == Player.Action.dodgeL || actionOne == Player.Action.dodgeR) &&
                (actionTwo == Player.Action.dodgeL || actionTwo == Player.Action.dodgeR))
            {
                outcome = Outcome.DoubleDodge;
            }

            // ** HEAL Outcomes **

            else if (actionOne == Player.Action.heal && (actionTwo != Player.Action.swingL && actionTwo != Player.Action.swingR && actionTwo != Player.Action.heal))
            {
                if (_playerOne.CanHeal)
                { outcome = Outcome.P1HealP2Defend; }
                else
                { outcome = Outcome.P1FailedHealP2Defend; }
            }
            else if (actionTwo == Player.Action.heal && (actionOne != Player.Action.swingL && actionOne != Player.Action.swingR && actionOne != Player.Action.heal))
            {
                if (_playerTwo.CanHeal)
                { outcome = Outcome.P2HealP1Defend; }
                else
                { outcome = Outcome.P2FailedHealP1Defend; }
            }
            else if (actionOne == Player.Action.heal && (actionTwo == Player.Action.swingL || actionTwo == Player.Action.swingR))
            {
                if (_playerOne.CanHeal)
                {
                    outcome = Outcome.P1HealP2Swing;

                }
                else
                {
                    outcome = Outcome.P1FailedHealP2Swing;
                }
            }
            else if (actionTwo == Player.Action.heal && (actionOne == Player.Action.swingL || actionOne == Player.Action.swingR))
            {
                if (_playerTwo.CanHeal)
                {
                    outcome = Outcome.P2HealP1Swing;

                }
                else
                {
                    outcome = Outcome.P2FailedHealP1Swing;
                }
            }
            else if (actionOne == Player.Action.heal && actionTwo == Player.Action.heal)
            {
                if (_playerOne.CanHeal && _playerTwo.CanHeal)
                {
                    outcome = Outcome.DoubleHeal;
                }
                else if (_playerOne.CanHeal && !_playerTwo.CanHeal)
                {
                    outcome = Outcome.P2FailedHealP1Heal;
                }
                else if (!_playerOne.CanHeal && _playerTwo.CanHeal)
                {
                    outcome = Outcome.P1FailedHealP2Heal;
                }
                else if (!_playerOne.CanHeal && !_playerTwo.CanHeal)
                {
                    outcome = Outcome.DoubleFailedHeal;
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
                    case Outcome.P1FailedHealP2Heal:
                    case Outcome.P2FailedHealP1Heal:
                    case Outcome.DoubleFailedHeal:
                    case Outcome.DoubleHeal:
                        _playerOne.HealSelf();
                        _playerTwo.HealSelf();
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

        /// <summary>
        /// Hello, <see cref="Player"/> 
        /// </summary>
        /// <param name="healer"></param>
        /// <param name="counter"></param>
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
                        Console.WriteLine(_narrator.GetOutcomeNarration(Outcome.HealCounter, healer.Name, counter.Name));
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
            else { critDamage = 0; }
            ;
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
            int p1ClassNameLength = PlayerOne.Class.ToString().Length;
            int p2ClassNameLength = PlayerTwo.Class.ToString().Length;
            // Get the length of both names + 40
            int headerLength = p1CharNameLength + p2CharNameLength + 40;
            // Create a string for the spaces between character names and left/right border
            System.String blankChars = new string(' ', (headerLength - 2));
            // Create a variable for the VS. that checks to see whether the sum of p1 & p2's character name length is even. If even, the variable is "VS.". If odd, the variable is "VS. ". This should account for the divide by two error.
            string versus = headerLength % 2 == 0 ? " VS." : "VS. ";
            // Get length of versus.
            int versusLength = versus.Length;
            // Create a string for the spaces between character names, include room for the VS.
            System.String nameBlanks = new string(' ', ((headerLength - (p1CharNameLength + p2CharNameLength)) / 2) - 4);
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
            string roundBlanks = new string(' ', (headerLength / 2) - (roundReadoutLength / 2) - (versusLength / 2));
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
            stringBuilder.AppendLine($"* {PlayerOne.Class.ToString()}{classBlanks}{PlayerTwo.Class.ToString()} *");
            // Health status line
            stringBuilder.AppendLine($"* {PlayerOne.HealthReadout}{healthBlanks}{PlayerTwo.HealthReadout} *");
            // Append heals remaining if a character is a medic.
            if (_playerOne.Class == Player.PlayerClass.Medic && _playerTwo.Class != Player.PlayerClass.Medic)
            {
                int p1HealsLeft = 3 - _playerOne.HealCount;
                string healsLeftString = $"Heals Left: {p1HealsLeft}";
                int healsLeftLength = healsLeftString.Length;
                // Create a string for the space between player heals left sections.
                string healsLeftBlanks = new string(' ', headerLength - healsLeftLength - 4);
                stringBuilder.AppendLine($"* {healsLeftString}{healsLeftBlanks} *");
            }
            else if (_playerTwo.Class == Player.PlayerClass.Medic && _playerOne.Class != Player.PlayerClass.Medic)
            {
                int p2HealsLeft = 3 - _playerTwo.HealCount;
                string healsLeftString = $"Heals Left: {p2HealsLeft}";
                int healsLeftLength = healsLeftString.Length;
                // Create a string for the space between player heals left sections.
                string healsLeftBlanks = new string(' ', headerLength - healsLeftLength - 4);
                stringBuilder.AppendLine($"* {healsLeftBlanks}{healsLeftString} *");
            }
            else if (_playerOne.Class == Player.PlayerClass.Medic && _playerTwo.Class == Player.PlayerClass.Medic)
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

        public void DeclareVictor()
        {
            GameLoop.GameState = State.VictoryScreen;
            // Set the victor
            string victor;
            if (_playerOne.Health == 0 && _playerTwo.Health > 0)
            {
                victor = _playerOne.Name;
            }
            else if (_playerTwo.Health == 0 && _playerOne.Health > 0)
            {
                victor = _playerOne.Name;
            }
            else
            {
                victor = "nobody";
            }

            // Capitalize victor for maximum victoriousness
            string upperVictor = victor.ToUpper();

            // Declare the victor for all to see
            Console.Clear();
            Console.WriteLine(string.Format(Narrator.declareVictor, victor, upperVictor));
            Console.WriteLine("\n\n\n");
        }


    }
}
