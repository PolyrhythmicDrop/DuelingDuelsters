using System.Text;
using static DuelingDuelsters.Classes.Narrator;

namespace DuelingDuelsters.Classes
{

    /// <summary>
    /// Manages data about the current match, including the two players and the number of rounds since the match started.
    /// <para>
    ///  A <c>Match</c> object defines methods for processing player actions, generating the round header at the start of each round, applying damage and heals, deciding on a match victor, and generating and processing the <see cref="Outcome">Outcome</see> of each round.
    /// </para>
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
        /// <description>The Outcome is processed using the <see cref="ProcessOutcome(Outcome)"></see> method.</description>
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

        /// <summary>
        /// Generates an <see cref="Outcome"/> based on the chosen actions of the two players.
        /// </summary>
        /// <param name="actionOne">The action chosen by <see cref="PlayerOne"/>.</param>
        /// <param name="actionTwo">The action chosen by <see cref="PlayerTwo"/>.</param>
        /// <returns>An <see cref="Outcome"/> object. This Outcome is used to generate narration and round results.</returns>
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

        /// <summary>
        /// Calls different methods based on the <see cref="Outcome"/> passed in the <paramref name="actionKey"/> parameter. These methods apply attack damage, roll for counters and dodges, heal players, and essentially convert actions taken during the round into meaningful results.
        /// </summary>
        /// <param name="actionKey">The <c>Outcome</c> used to determine which set of methods to call. This <c>Outcome</c> is generated by the <see cref="GetOutcome(Player.Action, Player.Action)"/> method based on each player's actions.</param>
        /// <exception cref="ArgumentNullException">The <c>Outcome</c> passed as an <paramref name="actionKey"/> argument was not a valid Outcome.</exception>
        private void ProcessOutcome(Outcome actionKey)
        {
            try
            {
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
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Executes a counterattack by the attacking player against the defending player.
        /// <para>
        /// <list type="number">
        ///  <item>
        ///    <description>Calls the <see cref="Player.IsCounter()"/> method to determine whether the attacking player successfully countered.</description>
        ///  </item>
        ///  <item>
        ///    <description>Based on the result of <see cref="Player.IsCounter()"/>, either narrates the Outcome for a failed counter or applies attack damage and resets the <see cref="Player.IsCountering"/> variable.</description>
        ///  </item>
        /// </list>
        /// </para>
        /// </summary>
        /// <param name="atkPlayer">The attacking player. This player is attempting a counterattack.</param>
        /// <param name="defPlayer">The defending player. This player is defending from the potential counterattack.</param>
        private void CounterAttack(Player atkPlayer, Player defPlayer)
        {
            atkPlayer.IsCounter();
            if (atkPlayer.IsCountering == false)
            {
                Outcome outcome = atkPlayer == PlayerOne ? Outcome.P1FailedCounter : Outcome.P2FailedCounter;
                Console.WriteLine(_narrator.GetOutcomeNarration(outcome, atkPlayer.Name, defPlayer.Name));
            }
            else
            {
                ApplyAttackResult(atkPlayer, defPlayer);
                atkPlayer.IsCountering = false;
            }
        }

        /// <summary>
        /// Determines whether or not a healing player is successfully attacked by the other player, then processes the result.
        /// <para>
        /// When one player heals and the other swings their sword, the healing player has a chance to dodge the attacking player's attack. This method: 
        /// <list type="number">
        ///  <item>
        ///    <description>Calls the match's <see cref="rng"/> to determine whether or not the healer dodges the attack.</description>
        ///  </item>
        ///  <item>
        ///    <description>Narrates the outcome based on results of the dodge roll.</description>
        ///  </item>
        ///  <item>
        ///    <description>Resets the healing player's <see cref="Player.IsHealing"/> flag to <see langword="false"/> regardless of the dodge roll outcome.</description>
        ///  </item>
        ///  <item>
        ///    <description>Applies attack results if the dodge was unsuccessful.</description>
        ///  </item>
        /// </list>
        /// </para> 
        /// </summary>
        /// <remarks>If the healing player attempts to heal when they cannot heal (for example, if their medical bag is empty), the healing player does not get a chance to dodge, and the attacking player gets a free attack.</remarks>
        /// <param name="healer">The healing player.</param>
        /// <param name="attacker">The attacking player.</param>
        
        private void AttemptHealCounter(Player healer, Player attacker)
        {
            // P1 has a chance to dodge P2's attack if they successfully heal and roll a 6 or greater.
            switch (healer.IsHealing)
            {
                case true:
                    int healDodgeRoll = rng.Next(0, 10);
                    if (healDodgeRoll >= 6)
                    {
                        Console.WriteLine(_narrator.GetOutcomeNarration(Outcome.HealDodge, healer.Name, attacker.Name));
                        healer.IsHealing = false;
                        break;
                    }
                    else
                    {
                        Console.WriteLine(_narrator.GetOutcomeNarration(Outcome.HealCounter, healer.Name, attacker.Name));
                        healer.IsHealing = false;
                        ApplyAttackResult(attacker, healer);
                        break;
                    }
                case false:
                    ApplyAttackResult(attacker, healer);
                    break;
            }
        }

        /// <summary>
        /// Applies damage to a defending player after a successful attack by an attacking player. The damage applied is based on:
        /// <list type="bullet">
        ///  <item>
        ///    <description>The attacking and defending players' stats.</description>
        ///  </item>
        ///  <item>
        ///    <description>Whether or not the defending player is blocking while the attacking player is executing a counterattack.</description>
        ///  </item>
        ///  <item>
        ///    <description>Whether or not the attacking player has scored a critical hit.</description>
        ///  </item>
        /// </list>
        /// </summary>
        /// <remarks>This method calls <see cref="Player.IsCrit(Player)"/> to determine if the attacking player has scored a critical hit. If the critical hit is successful, this method calls <see cref="Player.CalculateCritDamage()"/> to determine how much damage the critical hit adds to the attack damage.</remarks>
        /// <param name="atkPlayer">The attacking player.</param>
        /// <param name="defPlayer">The defending player.</param>
        private void ApplyAttackResult(Player atkPlayer, Player defPlayer)
        {
            int atkBaseDamage = atkPlayer.CalculateBaseDamage(defPlayer);

            // Divide base damage by two if the defending player is blocking during a counter.
            if (atkPlayer.IsCountering == true && (defPlayer.ChosenAction == Player.Action.blockR || defPlayer.ChosenAction == Player.Action.blockL))
            {
                atkBaseDamage = atkBaseDamage / 2;
            }

            int critDamage = 0;
            if (atkPlayer.IsCrit(defPlayer))
            {
                Console.WriteLine($"{atkPlayer.Name} scores a **CRITICAL HIT!**");
                Thread.Sleep(500);
                critDamage = atkPlayer.CalculateCritDamage();
                // If staggered and then hit, defending player is no longer staggered.
                defPlayer.IsStaggered = false;
            }
            
            int atkTotalDamageGiven = atkBaseDamage + critDamage;

            Console.WriteLine($"{defPlayer.Name} is hit for {atkTotalDamageGiven} damage!\n");
            defPlayer.Health -= atkTotalDamageGiven;
        }

        /// <summary>
        /// Draws the round header during a match using data from the players. The round header is updated after every round.
        /// </summary>
        /// <returns>A built string containing the player's names, health, heals remaining (if a player is a Medic), and the round count.</returns>
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
            string blankChars = new (' ', headerLength - 2);
            // Create a variable for the VS. that checks to see whether the sum of p1 & p2's character name length is even. If even, the variable is "VS.". If odd, the variable is "VS. ". This should account for the divide by two error.
            string versus = headerLength % 2 == 0 ? " VS." : "VS. ";
            // Get length of versus.
            int versusLength = versus.Length;
            // Create a string for the spaces between character names, include room for the VS.
            string nameBlanks = new (' ', ((headerLength - (p1CharNameLength + p2CharNameLength)) / 2) - 4);
            // Create a string for the space between dashes under character names
            string underNameBlanks = new (' ', headerLength - (p1CharNameLength + p2CharNameLength) - 4);
            // Get lengths for P1 & P2 health statuses
            int p1HealthStatusLength = PlayerOne.HealthReadout.Length;
            int p2HealthStatusLength = PlayerTwo.HealthReadout.Length;
            // Create a string for the spaces between health statuses
            string healthBlanks = new (' ', headerLength - (p1HealthStatusLength + p2HealthStatusLength) - 4);
            // Create dashes corresponding to player name length
            string p1Dashes = new ('-', p1CharNameLength);
            string p2Dashes = new ('-', p2CharNameLength);
            // Create string for round number readout
            string roundReadout = $"ROUND #{RoundCounter}";
            // Get length of the roundReadout
            int roundReadoutLength = roundReadout.Length;
            // Create an extra space for the round readout if headerLength is odd.
            int roundOddSpace = headerLength % 2 == 0 ? 0 : 1;
            string roundOddMod = new (' ', roundOddSpace);
            // Create string for spaces before and after the round readout
            string roundBlanks = new (' ', (headerLength / 2) - (roundReadoutLength / 2) - (versusLength / 2));
            // Create string for blank spaces between character classes
            string classBlanks = new (' ', headerLength - (p1ClassNameLength + p2ClassNameLength) - 4);


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

        /// <summary>
        /// Declares a victor at the end of a match.
        /// </summary>
        /// <remarks>If the round ended in a draw, the victor is "nobody". This is usually the result of both players taking mortal damage at the same time.
        /// </remarks>
        public void DeclareVictor()
        {
            GameLoop.GameState = State.VictoryScreen;
            string victor;
            if (_playerOne.Health == 0 && _playerTwo.Health > 0)
            {
                victor = _playerOne.Name;
            }
            else if (_playerTwo.Health == 0 && _playerOne.Health > 0)
            {
                victor = _playerOne.Name;
            }
            // A draw: both players were defeated in the same round.
            else
            {
                victor = "nobody";
            }

            // Capitalize the victor's name for maximum victoriousness.
            string upperVictor = victor.ToUpper();

            // Declare the victor for all to see!
            Console.Clear();
            Console.WriteLine(string.Format(declareVictor, victor, upperVictor));
            Console.WriteLine("\n\n\n");
        }
    }
}
