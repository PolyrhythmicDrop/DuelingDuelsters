using System.Reflection;
using System.Text;

namespace DuelingDuelsters.Classes
{
    /// <summary>
    /// Manages menus, user input, and almost all of the strings in the game.
    /// <para>
    /// The <c>Narrator</c> class defines methods for running each menu in the game, selecting from binary options, retrieving narration strings based on a round's <see cref="Match.Outcome">Outcome</see>, and processing user input.
    /// </para>
    /// <para>
    ///  The <c>Narrator</c> class also defines the <see cref="Choices">Choices</see> enumeration, which is used to pass menu selections to outside scopes. For example, many methods make use of the <see cref="Choices.Back">Back</see> <c>Choice</c> to return to a previous menu or cancel a decision.
    /// </para>
    /// </summary>
    /// <remarks><![CDATA[
    /// > [!NOTE]
    /// > The Narrator class defines most of the string constants used in Dueling Duelsters. To view these constants, refer to the [Narrator source code on GitHub](https://github.com/PolyrhythmicDrop/DuelingDuelsters/blob/main/src/Classes/Narrator.cs/).
    /// ]]>
    /// </remarks>
    public class Narrator
    {

        /// <exclude />
        ConsoleKeyInfo _keyInfo = new ConsoleKeyInfo();

        /// <summary>
        /// Key info for processing input in certain menus.
        /// </summary>
        public ConsoleKeyInfo KeyInfo
        {
            get => _keyInfo;
            set => _keyInfo = value;
        }

        /// <summary>
        /// The keyboard key entered by the user when prompted.
        /// </summary>
        public ConsoleKey Key
        {
            get => _keyInfo.Key;
        }

        /// <summary>
        /// Available selections the player can make in certain menus. <c>Choices</c> are typically used to pass menu selections to scopes outside a <see cref="Narrator">Narrator</see> object or menu.
        /// </summary>
        /// <remarks>
        /// > [!NOTE]
        /// > Two pairs of Choices (<see cref="Yes">Yes</see> and <see cref="Left">Left</see>; <see cref="No">No</see> and <see cref="Right">Right</see>) have the same underlying <see langword="int">int</see> value. This allows the <see cref="SelectBinary(string)"></see> method to be used for both affirmative menus (<c>"Yes/No"</c>) and action direction menus (<c>"Left/Right"</c>) during a game round while preserving code readability.
        /// </remarks>
        public enum Choices
        {
            /// <summary>
            /// The default <c>Choice</c>, typically used for error checking, validation, and initialization.
            /// </summary>
            Reset,
            /// <summary>
            /// The player has selected the <c>New Game</c> option in the title menu.
            /// </summary>
            NewGame,
            /// <summary>
            /// The player has selected the <c>Help</c> option. This typically runs a help menu for the given <see cref="GameLoop.GameState">GameState</see>.
            /// </summary>
            Help,
            /// <summary>
            /// The player has chosen to exit the game.
            /// </summary>
            Exit,
            /// <summary>
            /// The player has chosen to return to the title screen.
            /// </summary>
            ReturnToTitle,
            /// <summary>
            /// The player has selected the Back option, pressed the Escape key, or otherwise attempted to cancel an input or go back to a different menu. 
            /// </summary>
            Back,
            /// <summary>
            /// The player has selected a positive or first option, either a <c>Yes</c> or a <c>1</c>. 
            /// </summary>
            Yes = 6,
            /// <summary>
            /// The player has selected an action and set the direction for the action to <c>Left</c>.
            /// </summary>
            Left = 6,
            /// <summary>
            /// The player has selected a negative option or alternate option, either a <c>No</c> or a <c>2</c>.
            /// </summary>
            No = 7,
            /// <summary>
            /// The player has selected an action and set the direction for the action to <c>Right</c>.
            /// </summary>
            Right = 7
        }

        /// <summary>
        /// The player's selection for any given menu.
        /// </summary>
        public Choices Choice;

        // ** String Constants ** //

        // ~ File Paths ~ //

        /// <exclude />
        private const string titleHelp = "DuelingDuelsters.res.title-help.txt";
        /// <exclude />
        private const string arenaPic = "DuelingDuelsters.res.arena-entrance.txt";
        /// <exclude />
        private const string actionHelp = "DuelingDuelsters.res.action-help.txt";

        // ~ Title Screen ~ //

        /// <exclude />
        private const string newGame = "1. New Game\n";
        /// <exclude />
        private const string exitGame = "2. Exit\n";
        /// <exclude />
        private const string help = "3. Help\n";

        /// <exclude />
        private const string pressAnyKey = "Press any key to continue...";

        // ~ Player Count Select Screen ~ //

        /// <exclude />
        private const string singlePlayer = "1. One Player\n";
        /// <exclude />
        private const string twoPlayer = "2. Two Players\n";
        /// <exclude />
        private const string returnTitle = "3. Back to Title\n";

        // ~ Character Creation Screen ~ //

        /// <exclude />
        private const string createCharacter = "*** PLAYER {0}, CREATE YOUR CHARACTER *** \n";
        /// <exclude />
        private const string selectName = "\nEnter your character's name:\n";
        /// <exclude />
        private const string selectClass = "Welcome, {0}.\n\nSelect your class:\n\n1. {1}\n2. {2}\n3. {3}\n4. {4}\n5. {5}\n6. {6}";
        /// <exclude />
        private const string confirmCharacter = "\nLet's make sure you got everything right. Here's your character:\n\n{0}\n";
        /// <exclude />
        private const string satisfied = "\nAre you satisfied with {0}? Y/n";
        /// <exclude />
        private const string welcomePlayer = "Welcome our newest Duelster:\n\n~~ {0} the {1} ~~\n";
        /// <exclude />
        private const string enterArena = "{0} is entering the arena...\n";

        // ~ Class Summaries ~ //

        /// <exclude />
        private const string normieDescription = "Normie\n------\n ♡ ☒☒☒☐☐ | ⛨  ☒☒☒☐☐ | ⚔  ☒☒☒☐☐ | 👟 ☒☒☒☐☐ \nAbsolutely average at absolutely everything. If Mario was in this game, he would be a Normie.\n";
        /// <exclude />
        private const string fridgeDescription = "Fridge\n------\n ♡ ☒☒☒☒☒ | ⛨  ☒☒☒☒☒ | ⚔  ☒☐☐☐☐ | 👟 ☒☐☐☐☐ \nCan take whatever you throw at them, but can have trouble dishing it out.\n";
        /// <exclude />
        private const string leeroyDescription = "Leeroy\n------\n ♡ ☒☒☒☐☐ | ⛨  ☒☐☐☐☐ | ⚔  ☒☒☒☒☒ | 👟 ☒☒☒☐☐ \nExpert at bashin', smashin', and crashin', not so much at plannin'.\n";
        /// <exclude />
        private const string gymnastDescription = "Gymnast\n-------\n ♡ ☒☒☒☐☐ | ⛨  ☒☒☐☐☐ | ⚔  ☒☒☐☐☐ | 👟 ☒☒☒☒☒ \nNimble and acrobatic, the Gymnast can dance on the head of a pin and also skewer their opponents with it.\n";
        /// <exclude />
        private const string medicDescription = "Medic\n------\n ♡ ☒☒☒☒☐ | ⛨  ☒☒☒☐☐ | ⚔  ☒☒☒☐☐ | 👟 ☒☒☐☐☐ | ✜ \nThe only class that can heal, the Medic is durable and doesn't care one whit about the Hippocratic Oath.\n";
        /// <exclude />
        private const string randomDescription = "Random\n------\n ♡ ????? | ⛨  ????? | ⚔  ????? | 👟 ????? \nRoll the dice and let the gods determine your fate.";

        /// <summary>
        /// Confirmation that you are, in fact, ready to duel like your life depends on it (spoiler: it does).
        /// </summary>
        internal const string readyToDuel = "\nAre you ready to duel like you've never duelled before?\n\n1. Yes, let's do this!\n2. No, let's start over.";

        // ~ In-Round Menu Strings ~ //

        /// <exclude />
        private const string selectDirection = "Which direction?\n1. Left\n2. Right\n";
        /// <exclude />
        private const string selectAction = "\n{0}, select an action:\n";
        /// <exclude />
        private const string confirmAction = "Is this what you want to do? Y/n\n";

        // ~ Outcome Strings ~ //

        /// <exclude />
        private const string swordClash = "The swords of {0} and {1} clash, the sound of ringing steel echoing throughout the arena!\nThe two combatants eye each other over their crossed blades.\n\nIs this the start of an enemies-to-lovers romance? Or another chapter in a long tale of bitter rivalry?\n\n{0} and {1} part with a puff of dust and return to their ready stances.\n";

        /// <exclude />
        private const string bothHit = "{0} and {1} slash each other at the same time, slicing through armor and egos!\n";

        /// <exclude />
        private const string blocked = "{0} handily blocks {1}'s hardy blow!\n{1}'s arm shudders from the impact, staggering them!\n";
        /// <exclude />
        private const string failedBlock = "{0}'s sword slices into {1}'s unguarded side!\n";
        /// <exclude />
        private const string bothBlock = "{0} and {1} hide behind their shields, two turtles scared of the world outside their shells.\nMaybe they will find the courage to face each other in the next round!\n";
        /// <exclude />
        private const string failedDodge = "{0} tries to dodge directly into {1}'s attack!\nAn unfortunate blunder, the price of which {0} will pay in blood and shame!\n";
        /// <exclude />
        private const string dodged = "{0} nimbly sidesteps {1}'s powerful attack!\n{0} uses their cat-like reflexes to mount a bewildering counterattack!\n";
        /// <exclude />
        private const string failedCounter = "{0} whiffs their counter in embarrassing fashion! {1} breathes a sigh of relief.";
        /// <exclude />
        private const string dodgeBlock = "{0} dodges toward {1}'s vulnerable side!\n{0} prepares to take advantage of the situation with extreme prejudice!\n";
        /// <exclude />
        private const string failedDodgeBlock = "{0} rolls their eyes at {1}'s fruitless dancing over the top of their mighty shield.\nThe crowd showers the combatants with boos, thirsting for blood, or at least a little less fancy footwork.\n";
        /// <exclude />
        private const string doubleDodge = "The crowd claps along to an unheard beat as {0} and {1} groove and wiggle across the battlefield, matching each other's steps in a deadly dance of martial prowess.\nNobody takes damage, but everybody has a good time.\n";
        /// <exclude />
        private const string uneventfulHeal = "{0} miraculously heals their wounds with the power of science that could be mistaken for magic!\n{1}'s defensive actions are all for naught!\n";
        /// <exclude />
        private const string failedHealandDefend = "{0} is out of snake oil, thoughts, and prayers! Too bad {1} only took a defensive action.\nThe crowd laughs in derision at the ill-prepared, tactically inept combatants.\n";
        /// <exclude />
        private const string healAndSwing = "{0} recovers health! But {1} has a chance to tear off the Band-Aid...\n";
        /// <exclude />
        private const string failedHealAndSwing = "{0} wastes a turn swatting the flies away from their empty medicine bag!\n{1} takes advantage of {0}'s lack of counting ability!\n";
        /// <exclude />
        private const string healDodge = "{0} uses advanced diagnostic technology to anticipate and dodge {1}'s attack at the last moment!\n{0} is as agile as a CAT scan!\n";
        /// <exclude />
        private const string healCounter = "\n{0} is too woozy from pain meds to dodge {1}'s attack!\n{1} collects on {0}'s medical bills with a vengeance!\n";
        /// <exclude />
        private const string doubleHeal = "\n{0} and {1} engage in rigorous academic debate over the restorative properties of leeches!\nTheir humours are aligned but their ideas are not!\n";
        /// <exclude />
        private const string failedDoubleHeal = "\n{0} sighs luxuriously as a feeling of health and well-being washes over their battle-scarred body!\n{1} takes a long, sad look at their empty medical back and sharpens their sword, ready for revenge!\n";
        /// <exclude />
        private const string doubleFailedHeal = "\n{0} and {1} look at each other sheepishly. They may have doctorates from Duelster College, but they lack the ability to count!\nThe crowd boos and laughs at the inadequacy of {0} and {1}'s fancy book learning when it comes to arena smarts!\n\n";

        // ** Player Action Description Strings **

        /// <exclude />
        private const string swingLeft = "{0} swings their sword to their left!\n";
        /// <exclude />
        private const string swingRight = "{0} swings their sword to their right!\n";
        /// <exclude />
        private const string guardLeft = "{0} raises their shield and guards their left side!\n";
        /// <exclude />
        private const string guardRight = "{0} raises their shield and guards their right side!\n";
        /// <exclude />
        private const string dodgeLeft = "{0} quickly dodges to their left!\n";
        /// <exclude />
        private const string dodgeRight = "{0} quickly dodges to their right!\n";
        /// <exclude />
        private const string heal = "{0} breaks out their emergency trauma kit!\nThey stitch up their gaping wounds and pour isopropyl alcohol over their head!\n";
        /// <exclude />
        private const string failedHeal = "{0} fumbles around in their medical bag for supplies, but they are fresh out!\n{0} can't heal for the rest of the match!\n";
        /// <exclude />
        private const string noAction = "{0} does nothing! How could they be so irresponsible?\n";

        // ** Victory String **

        /// <summary>
        /// String constant that declares the victor at the end of a match.
        /// </summary>
        public const string declareVictor = "\n*** {0} is victorious! ***\n\nAll hail the most dueling Duelster of them all:\n\n*** {1} ***\n";
        /// <summary>
        /// Dialog that displays at the end of each match. Provides follow-up options for the player.
        /// </summary>
        public const string postMatchOptions = "What do you want to do next?\n\n1. Return to title\n2. Rematch\n";

        /// <summary>
        /// Gets the appropriate narration string for the selected player action.
        /// </summary>
        /// <param name="player">The player to narrate. The narration for the player's <see cref="Player.ChosenAction">ChosenAction</see> is returned.</param>
        /// <returns>A description of the player's action.</returns>
        /// <exception cref="ArgumentNullException">The player has not chosen an action. This probably means this method was called before action selection. Did you mean to call <see cref="Narrator.GetOutcomeNarration(Match.Outcome, string?, string?)"/> instead?</exception>
        /// <exception cref="NullReferenceException">The player's chosen action does not have a matching narration string.</exception>
        public string GetPlayerActionNarration(Player player)
        {
            string? playerActionDescription;
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
                    case Player.Action.heal when player.CanHeal:
                        playerActionDescription = string.Format(heal, player.Name);
                        break;
                    case Player.Action.heal when !player.CanHeal:
                        playerActionDescription = string.Format(failedHeal, player.Name);
                        break;
                    case Player.Action.none:
                        playerActionDescription = string.Format(noAction, player.Name);
                        break;
                }
                if (playerActionDescription == null)
                {
                    throw new NullReferenceException("playerActionDescription is null!");
                }
            }
            catch (SystemException e)
            {
                return e.Message;
            }

            return playerActionDescription;
        }

       /// <summary>
       /// Gets the appropriate narration string for the round's <see cref="Match.Outcome">Outcome</see>.
       /// </summary>
       /// <param name="outcome">The <c>Outcome</c> for which to retrieve narration.</param>
       /// <param name="pOne">Player One</param>
       /// <param name="pTwo">Player Two</param>
       /// <returns>A description of the round's outcome.</returns>
        public string GetOutcomeNarration(Match.Outcome outcome, string? pOne, string? pTwo)
        {
            string? narration;
            try 
            {
                switch (outcome)
                {
                    default:
                        throw new NullReferenceException("You did not pass an outcome!");
                    case Match.Outcome.None:
                        narration = "You didn't supply any story beats to narrate!";
                        throw new ArgumentException(narration, "Match.Outcome");
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
                    case Match.Outcome.HealCounter:
                        narration = string.Format(healCounter, pOne, pTwo);
                        break;
                    case Match.Outcome.DoubleHeal:
                        narration = string.Format(doubleHeal, pOne, pTwo);
                        break;
                    case Match.Outcome.P1FailedHealP2Heal:
                        narration = string.Format(failedDoubleHeal, pTwo, pOne);
                        break;
                    case Match.Outcome.P2FailedHealP1Heal:
                        narration = string.Format(failedDoubleHeal, pOne, pTwo);
                        break;
                    case Match.Outcome.DoubleFailedHeal:
                        narration = string.Format(doubleFailedHeal, pOne, pTwo);
                        break;
                    }
                if (narration == null)
                {
                    throw new NullReferenceException("Narration is null! Did you pass the right value?");
                }
                }
            catch (SystemException e)
            {
                return e.Message;
            }

            return narration;
        }

        /// <summary>
        /// Draws the title screen menu and processes user input. 
        /// </summary>
        /// <returns><c>true</c> if the player made a valid selection. A <c>true</c> result also breaks the player out of the title menu loop.<br/>
        /// <c>false</c> if the player did not make a valid selection. The title menu loop continues if <cref><c>RunTitleMenu()</c></cref> returns false.</returns>
        public bool RunTitleMenu()
        {
            bool success = false;

            Console.WriteLine($"{newGame}\n{exitGame}\n{help}");
            _keyInfo = Console.ReadKey(true);
            switch (Key)
            {
                default:
                    {
                        success = false;
                        break;
                    }
                // If the user selects "New Game", begin character creation for Player 1.
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    {
                        Choice = Choices.NewGame;
                        success = true;
                        break;
                    }
                // If the user selects "Exit," close the program.
                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    {
                        Choice = Choices.Exit;
                        success = true;
                        break;
                    }
                // If the user selects "Instructions", displays the help screen.
                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    {
                        Choice = Choices.Help;
                        success = true;
                        break;
                    }
                case ConsoleKey.None:
                    {
                        success = false;
                        break;
                    }
            }
            return success;
        }

        /// <summary>
        /// Runs the character creation menu for a single player. The character creation menu has three sub-menus, each run by a separate method:
        /// <list type="number">
        ///  <item>
        ///    <description><strong>Name Selection</strong> - <see cref="SetPlayerName(Player)"/></description>
        ///  </item>
        ///  <item>
        ///    <description><strong>Class Selection</strong> - <see cref="SetPlayerClass(Player)"/></description>
        ///  </item>
        ///  <item>
        ///    <description><strong>Confirmation</strong> - <see cref="SelectBinary(string)"/></description>
        ///  </item>
        /// </list>
        /// </summary>
        /// <param name="player">The character to create. The selected name and class will be applied to this player.</param>
        /// <param name="playerNumber">The number of the player. This value is mostly used for narration.</param>
        /// <returns><c>true</c> if character creation completed successfully.<br/>
        /// <c>false</c> if an error occurred or the player cancelled.</returns>
        public bool RunCharacterCreation(Player player, int playerNumber)
        {
            bool success = false;

            // Create a character
            while (player.Name == null && player.Class == Player.PlayerClass.None)
            {

                // Name Selection
                do
                {
                    Console.WriteLine(string.Format(createCharacter, playerNumber));
                    Console.WriteLine(selectName);
                }
                while (!SetPlayerName(player));

                if (Choice == Choices.Back)
                {
                    success = true;                
                    return success;
                }

                Console.Clear();

                // Class Selection
                do
                {
                    Console.WriteLine(string.Format(selectClass, player.Name, normieDescription, fridgeDescription, leeroyDescription, gymnastDescription, medicDescription, randomDescription));
                }
                while (!SetPlayerClass(player));
                
                if (Choice == Choices.Back)
                {
                    Console.Clear();
                    continue;
                }

                // Confirmation
                Console.Clear();
                string confirm = string.Format(confirmCharacter, player.CharSheet) + string.Format(satisfied, player.Name);
                while (!SelectBinary(confirm));
                switch (Choice)
                {
                    default:
                        Console.Clear();
                        success = false;
                        continue;
                    case Choices.Yes:
                        success = true;
                        break;
                    case Choices.No:
                    case Choices.Back:
                        Console.Clear();
                        success = false;
                        player.Class = Player.PlayerClass.None;
                        player.Name = null;
                        break;
                }
            }

            // Final error checking
            try
            {
                if (player.Class == Player.PlayerClass.None)
                {
                    success = false;
                    throw new ArgumentNullException("player", "Character needs a class!");
                }
                else
                {
                    // Set the player's action list.
                    player.BuildActionList();
                    success = true;
                }
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.Message);
                return success;
            }

            Console.Clear();

            // Welcome the new character.
            Console.WriteLine(string.Format(welcomePlayer, player.Name, player.Class.ToString()));
            Thread.Sleep(1000);

            // Display arena entrance ASCII
            Console.WriteLine(string.Format(enterArena, player.Name));
            Console.WriteLine(DrawArena());

            // Wait for input before continuing.
            while (!PressAnyKey());

            return success;

        }

        /// <summary>
        /// Menu to set a player's name during character creation.
        /// </summary>
        /// <param name="player">The player to name.</param>
        /// <returns><c>true</c> if the player's name was set successfully.<br/>
        /// <c>false</c> if the menu is still running or the player's name was not entered successfully.</returns>
        private bool SetPlayerName(Player player)
        {
            bool success = false;
            Choice = Choices.Reset;

            // Prompt to set the player's name.
            do
            {
                try
                {
                    player.Name = ReadLineWithCancel();
                    if (player.Name == null && Choice != Choices.Back)
                    {
                        throw new NullReferenceException("Player name cannot be null!");
                    }
                    else if (Choice == Choices.Back)
                    {
                        break;
                    }
                    else if (player.Name != null)
                    {
                        success = true;
                    }
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
            }
            while (player.Name == null && Choice != Choices.Back);

            // Return to player count select if player hits Esc while entering player name.
            if (Choice == Choices.Back)
            {
                player.Name = null;
                player.Class = Player.PlayerClass.None;
                success = true;
                return success;
            }

            return success;
        }

        /// <summary>
        /// Menu to set a player's class during character creation. Once a class is selected, this method also sets the player's stats according to the chosen class.
        /// </summary>
        /// <param name="player">The player whose class is set.</param>
        /// <returns><c>true</c> if the player's class was set successfully.<br/>
        /// <c>false</c> if the menu is still running or the player's class was not set successfully.</returns>
        private bool SetPlayerClass(Player player)
        {
            bool success = false;
            Choice = Choices.Reset;

            _keyInfo = Console.ReadKey(true);
            // Set character stats based on chosen class
            switch (Key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    {
                        player.Class = Player.PlayerClass.Normie;
                        player.MaxHealth = 20;
                        player.Health = player.MaxHealth;
                        player.Attack = 10;
                        player.Defense = 8;
                        player.Speed = 5;
                        break;
                    }
                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    {
                        player.Class = Player.PlayerClass.Fridge;
                        player.MaxHealth = 30;
                        player.Health = player.MaxHealth;
                        player.Attack = 7;
                        player.Defense = 15;
                        player.Speed = 3;
                        break;

                    }
                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    {
                        player.Class = Player.PlayerClass.Leeroy;
                        player.MaxHealth = 16;
                        player.Health = player.MaxHealth;
                        player.Attack = 16;
                        player.Defense = 4;
                        player.Speed = 6;
                        break;
                    }
                case ConsoleKey.D4:
                case ConsoleKey.NumPad4:
                    {
                        player.Class = Player.PlayerClass.Gymnast;
                        player.MaxHealth = 18;
                        player.Health = player.MaxHealth;
                        player.Attack = 8;
                        player.Defense = 6;
                        player.Speed = 12;
                        break;
                    }

                case ConsoleKey.D5:
                case ConsoleKey.NumPad5:
                    {
                        player.Class = Player.PlayerClass.Medic;
                        player.MaxHealth = 25;
                        player.Health = player.MaxHealth;
                        player.Attack = 9;
                        player.Defense = 8;
                        player.Speed = 4;
                        break;
                    }
                case ConsoleKey.D6:
                case ConsoleKey.NumPad6:
                    {
                        RandomSetPlayerClass(player);
                        break;
                    }
                // Start over
                case ConsoleKey.Escape:
                    {
                        SelectBinary("Do you really want to reset your character? Y/n");
                        switch (Choice)
                        {
                            case Choices.Yes:
                                player.Class = Player.PlayerClass.None;
                                player.Name = null;
                                Choice = Choices.Back;
                                Console.Clear();
                                break;
                            case Choices.No:
                            case Choices.Back:
                                Choice = Choices.Reset;
                                Console.Clear();
                                break;
                            default:
                                break;
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            if (player.Class != Player.PlayerClass.None || Choice == Choices.Back)
            {
                success = true;
            }

            return success;
        }

        /// <summary>
        /// Randomly selects a player's class. This method is called if the player selects the <c>Random</c> option during the class selection menu (<see cref="SetPlayerClass(Player)"/>)
        /// </summary>
        /// <param name="player">The player whose class is randomized.</param>
        private void RandomSetPlayerClass(Player player)
        {
            int choice = Match.rng.Next(1, 5);
            switch (choice)
            {
                case 1:
                    {
                        player.Class = Player.PlayerClass.Normie;
                        player.MaxHealth = 20;
                        player.Health = player.MaxHealth;
                        player.Attack = 10;
                        player.Defense = 8;
                        player.Speed = 5;
                        break;
                    }
                case 2:
                    {
                        player.Class = Player.PlayerClass.Fridge;
                        player.MaxHealth = 30;
                        player.Health = player.MaxHealth;
                        player.Attack = 7;
                        player.Defense = 15;
                        player.Speed = 3;
                        break;

                    }
                case 3:
                    {
                        player.Class = Player.PlayerClass.Leeroy;
                        player.MaxHealth = 20;
                        player.Health = player.MaxHealth;
                        player.Attack = 15;
                        player.Defense = 5;
                        player.Speed = 6;
                        break;
                    }
                case 4:
                    {
                        player.Class = Player.PlayerClass.Gymnast;
                        player.MaxHealth = 20;
                        player.Health = player.MaxHealth;
                        player.Attack = 8;
                        player.Defense = 6;
                        player.Speed = 10;
                        break;
                    }

                case 5:
                    {
                        player.Class = Player.PlayerClass.Medic;
                        player.MaxHealth = 25;
                        player.Health = player.MaxHealth;
                        player.Attack = 9;
                        player.Defense = 8;
                        player.Speed = 4;
                        break;
                    }
            }
        }

        /// <summary>
        /// Menu for selecting the number of players in a match. If the player selects single player, the passed <paramref name="brain"/> is converted to a <c>Computer</c> brain. If the player selects two player, the passed <paramref name="brain"/> is converted to a <c>Human</c> brain.
        /// </summary>
        /// <param name="brain">The brain to modify, based on the player's selection.</param>
        /// <returns><c>true</c> if the menu ran successfully and a player count was selected or the player chose to return to the title menu.<br/>
        /// <c>false</c> if the player did not make a choice or a valid choice.</returns>
        public bool RunPlayerCountMenu(out Player.PlayerBrain? brain)
        {
            bool success = false;

            Console.WriteLine(singlePlayer);
            Console.WriteLine(twoPlayer);
            Console.WriteLine(returnTitle);

            _keyInfo = Console.ReadKey(true);
            switch (Key)
            {
                default:
                    {
                        brain = null;
                        success = false;
                        break;
                    }
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    {
                        brain = Player.PlayerBrain.Computer;
                        success = true;
                        break;
                    }
                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    {
                        brain = Player.PlayerBrain.Human;
                        success = true;
                        break;
                    }
                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                case ConsoleKey.Escape:
                    {
                        brain = null;
                        Choice = Choices.ReturnToTitle;
                        success = true;
                        break;
                    }
            }

            return success;
        }

        /// <summary>
        /// Draws a help screen. The help screen's content depends on the current <see cref="GameLoop.GameState">GameState</see>.
        /// </summary>
        /// <returns><c>true</c> if the player chose to exit the help screen. The player is returned to the menu they opened the help screen from.<br/>
        /// <c>false</c> while the help screen is running and the player has not entered a valid key.</returns>
        public bool RunHelpScreen()
        {
            bool success = false;
            string? filePath;

            try
            {
                switch (GameLoop.GameState)
                {
                    default:
                        throw new ArgumentNullException("GameLoop.GameState", "State cannot be null!");
                    case State.TitleScreen:
                        filePath = titleHelp;
                        break;
                    case State.ActionSelect:
                        filePath = actionHelp;
                        break;
                }
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.Message);
                return success;
            }

            Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(filePath);

            try
            {
                if (stream == null)
                {
                    throw new ArgumentNullException("Help Screen", "Could not load the help screen!");
                }
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Real duelsters require no assistance in the art of the duel.\nPress some buttons, execute some martial pirouettes, see what happens!");
                return false;
            }

            StreamReader streamReader = new StreamReader(stream);
            StringBuilder helpBuilder = new StringBuilder();

            string? helpLine = streamReader.ReadLine();
            do
            {
                helpBuilder.AppendLine($"{helpLine}");
                helpLine = streamReader.ReadLine();
            }
            while (helpLine != null);

            string helpScreen = helpBuilder.ToString();
            if (helpScreen != null)
            {
                GameLoop.GameState = State.HelpScreen;
                Console.WriteLine(helpScreen);
                success = true;
            }

            while (!PressAnyKey())
            { }

            streamReader.Dispose();

            return success;
        }

        /// <summary>
        /// Menu for selecting an action during a round.
        /// <para>
        /// <list type="bullet">
        ///  <item>
        ///    <description>If the player is a human-controlled player, the <see cref="HumanActionSelect(Player)"/> method is called.</description>
        ///  </item>
        ///  <item>
        ///    <description>If the player is a computer controlled player, the <see cref="ComputerActionSelect(Player)"/> method is called.</description>
        ///  </item>
        /// </list>
        /// </para>
        /// </summary>
        /// <param name="player">The player who is selecting an action.</param>
        /// <returns><c>true</c> if the player selected a valid action or if the player chose to go back.<br/>
        /// <c>false</c> if the action select menu is still running or the player did not select a valid action.</returns>
        public bool RunPlayerActionSelect(Player player)
        {
            Choice = Choices.Reset;
            bool success = false;

            if (player.Brain == Player.PlayerBrain.Human)
            {
                while (!HumanActionSelect(player));
                if (Choice == Choices.Back)
                {
                    Console.Clear();
                    return success;
                }
                else if (player.ActionTaken == true && player.ChosenAction != Player.Action.none)
                {
                    success = true;
                    return success;
                }
            }
            // AI action choice
            else if (player.Brain == Player.PlayerBrain.Computer)
            {
                while (!ComputerActionSelect(player) && player.ActionTaken == false);
                if (player.ChosenAction != Player.Action.none)
                { success = true; }
            }

            return success;
        }

        /// <summary>
        /// Menu for a human player to select an action during a round. The action the player selects is set as the <see cref="Player.ChosenAction"></see> value for the player.
        /// </summary>
        /// <param name="player">The human player who is selecting an action.</param>
        /// <returns><c>true</c> if the player selected a valid action or if the player chose to go back.<br/>
        /// <c>false</c> if the action select menu is still running or the player did not select a valid action.</returns>
        public bool HumanActionSelect(Player player)
        {
            bool success = false;

            // Player is prompted to choose an action
            Console.WriteLine(string.Format(selectAction, player.Name));
            Console.WriteLine(player.ActionList);
            _keyInfo = Console.ReadKey(true);

            switch (Key)
            {
                default:
                    success = false;
                    break;
                // Escape to back out
                case ConsoleKey.Escape:
                    UndoActionSelection(player, out success);
                    return success;
                // Swing sword.
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    // Choose direction.
                    while (!SelectBinary(selectDirection));
                    switch (Choice)
                    {
                        case Choices.Back:
                            UndoActionSelection(player, out success);
                            return success;
                        case Choices.Left:
                            player.ChosenAction = Player.Action.swingL;
                            break;
                        case Choices.Right:
                            player.ChosenAction = Player.Action.swingR;
                            break;
                    }
                    break;
                // Block with shield.
                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    // Choose direction.
                    while (!SelectBinary(selectDirection)) ;
                    switch (Choice)
                    {
                        case Choices.Back:
                            UndoActionSelection(player, out success);
                            return success;
                        case Choices.Left:
                            player.ChosenAction = Player.Action.blockL;
                            break;
                        case Choices.Right:
                            player.ChosenAction = Player.Action.blockR;
                            break;
                    }
                    break;
                // Dodge
                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    // Choose direction.
                    while (!SelectBinary(selectDirection)) ;
                    switch (Choice)
                    {
                        case Choices.Back:
                            UndoActionSelection(player, out success);
                            return success;
                        case Choices.Left:
                            player.ChosenAction = Player.Action.dodgeL;
                            break;
                        case Choices.Right:
                            player.ChosenAction = Player.Action.dodgeR;
                            break;
                    }
                    break;
                // Help (if not medic) or Heal (if medic)
                case ConsoleKey.D4:
                case ConsoleKey.NumPad4:
                    // Open the help screen if not a medic.
                    if (player.Class != Player.PlayerClass.Medic)
                    {
                        Console.Clear();
                        RunHelpScreen();
                        UndoActionSelection(player, out success);
                        return success;
                    }
                    else
                    {
                        // Choose heal "direction". This doesn't actually matter, it's just to disguise the healer's action from the other player.
                        while (!SelectBinary(selectDirection));
                        switch (Choice)
                        {
                            default:
                                break;
                            case Choices.Back:
                                UndoActionSelection(player, out success);
                                return success;
                            case Choices.Left:
                            case Choices.Right:
                                player.ChosenAction = Player.Action.heal;
                                break;
                        }
                        break;
                    }
                case ConsoleKey.D5:
                case ConsoleKey.NumPad5:
                    // Open the help screen if a Medic.
                    if (player.Class == Player.PlayerClass.Medic)
                    {
                        Console.Clear();
                        RunHelpScreen();
                        UndoActionSelection(player, out success);
                        return success;
                    }
                    else
                    {
                        break;
                    }
            }

            // Confirm action.
            if (player.ChosenAction != Player.Action.none)
            {
                while (!SelectBinary(confirmAction));
                switch (Choice)
                {
                    case Choices.Back:
                        UndoActionSelection(player, out success);
                        return success;
                    case Choices.No:
                        UndoActionSelection(player, out success);
                        return success;
                    case Choices.Yes:
                        Choice = Choices.Reset;
                        player.ActionTaken = true;
                        success = true;
                        return success;
                }
            }
            else
            {
                UndoActionSelection(player, out success);
                return success;
            }

            return success;
        }

        /// <summary>
        /// Selects an action for a computer player during a round based on numbers generated by the match's <see cref="Match.rng">RNG</see>.
        /// </summary>
        /// <remarks>If the <paramref name="player"/> is a <see cref="Player.PlayerClass.Medic">Medic</see>, they have access to the Heal action and a higher chance to heal if their health is below 50%.</remarks>
        /// <param name="player">The computer player who is selecting an action.</param>
         /// <returns><c>true</c> if the computer player selected a valid action.<br/>
        /// <c>false</c> if the action select menu is still running or if the computer player did not select a valid action.</returns>
        public bool ComputerActionSelect(Player player)
        {
            bool success;
            int choice;

            if (player.Class != Player.PlayerClass.Medic || 
            (player.Class == Player.PlayerClass.Medic && !player.CanHeal) ||
            (player.Class == Player.PlayerClass.Medic && player.Health == player.MaxHealth))
            {
                choice = Match.rng.Next(0, 5);
            }
            else if (player.Class == Player.PlayerClass.Medic && (player.Health < (player.MaxHealth / 2)))
            {
                choice = Match.rng.Next(0, 8);
            }
            else
            {
                choice = Match.rng.Next(0, 6);
            }

            switch (choice)
            {
                case 0:
                    player.ChosenAction = Player.Action.swingR;
                    break;
                case 1:
                    player.ChosenAction = Player.Action.swingL;
                    break;
                case 2:
                    player.ChosenAction = Player.Action.blockR;
                    break;
                case 3:
                    player.ChosenAction = Player.Action.blockL;
                    break;
                case 4:
                    player.ChosenAction = Player.Action.dodgeR;
                    break;
                case 5:
                    player.ChosenAction = Player.Action.dodgeL;
                    break;
                case >= 6:
                    player.ChosenAction = Player.Action.heal;
                    break;
            }
            player.ActionTaken = true;
            
            success = true;

            return success;
        }

        /// <summary>
        /// Runs a "press any key to continue" dialog and waits for the user to press a key.
        /// </summary>
        /// <returns><c>true</c> if the player pressed a key.<br/>
        /// <c>false</c> if the player did not press a key.</returns>
        public bool PressAnyKey()
        {
            bool success = false;
            Choice = Choices.Reset;

            Console.WriteLine(pressAnyKey);
            _keyInfo = Console.ReadKey(true);

            try
            {
                if (Key == ConsoleKey.None)
                {
                    throw new ArgumentNullException("Key press", "no key pressed");
                }
                else
                {
                    success = true;
                }
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.Message);
            }

            return success;
        }

        /// <summary>
        /// Presents the player with a binary choice (yes/no, 1/2, left/right, etc.). Also provides the option of pressing the Escape key to go back. The selected option is written to the narrator's <see cref="Narrator.Choice">Choice</see> member.
        /// </summary>
        /// <param name="dialog">Dialog to display to the player. The <paramref name="dialog"/> argument should also include the choices the player can select.</param>
        /// <returns><c>true</c> if the player selected a valid choice (Positive, Negative, or Back).<br/>
        /// <c>false</c> if the player did not select a valid choice.</returns>
        public bool SelectBinary(string dialog)
        {
            bool success = false;
            Choice = Choices.Reset;

            Console.WriteLine(dialog);

            _keyInfo = Console.ReadKey(true);
            switch (Key)
            {
                default:
                    {
                        success = false;
                        Console.Clear();
                        break;
                    }
                case ConsoleKey.Y:
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    {
                        Choice = (Choices)6;
                        success = true;
                        break;
                    }
                case ConsoleKey.N:
                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    {
                        Choice = (Choices)7;
                        success = true;
                        break;
                    }
                case ConsoleKey.Escape:
                    {
                        Choice = Choices.Back;
                        success = true;
                        break;
                    }
            }

            return success;
        }

        /// <summary>
        /// Draws an ASCII image of a gladatorial arena entrance from a file.
        /// </summary>
        /// <remarks>This method is run at the end of character creation to welcome the new character to the arena.</remarks>
        /// <returns>The foreboding entrance to the new character's chance at glorious victory or death-inducing defeat.</returns>
        public string DrawArena()
        {
            // Get the file path for the arena.
            Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(arenaPic);
            try
            {
                if (stream == null)
                {
                    throw new ArgumentNullException("Could not load the arena ASCII!");
                }
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.Message);
                return "There should be a picture of an arena here...";
            }

            StreamReader? streamReader = new StreamReader(stream);
            StringBuilder arenaBuilder = new StringBuilder();
            string? arena = streamReader.ReadLine();
            do
            {
                arenaBuilder.AppendLine(arena);
                arena = streamReader.ReadLine();
            }
            while (arena != null);

            streamReader.Dispose();

            return arenaBuilder.ToString();
        }

        /// <summary>
        /// Custom implementation of <c>Console.ReadLine()</c> that allows the player to press the Escape key to back out of text entry.
        /// </summary>
        /// <remarks>This method is called during character creation to allow the player to return to the previous menu while entering a name for their character.</remarks>
        /// <returns>The text the user entered into the console.</returns>
        private string? ReadLineWithCancel()
        {
            Choice = Choices.Reset;

            string? result = null;

            StringBuilder builder = new StringBuilder();

            _keyInfo = Console.ReadKey(true);

            while (Key != ConsoleKey.Enter && Key != ConsoleKey.Escape)
            {
                Console.Write(_keyInfo.KeyChar);
                builder.Append(_keyInfo.KeyChar);
                _keyInfo = Console.ReadKey(true);
            }

            if (Key == ConsoleKey.Enter)
            {
                result = builder.ToString();
            }
            else if (Key == ConsoleKey.Escape)
            {
                Choice = Choices.Back;
            }

            return result;
        }

        /// <summary>
        /// Resets the passed player's action variables and sets the narrator's <see cref="Choice">Choice</see> member to <see cref="Choices.Back">Back</see>.
        /// </summary>
        /// <remarks>This method is called during action selection if the player chooses to undo their current action selection.</remarks>
        /// <param name="player">The player whose action variables are reset.</param>
        /// <param name="success">The <c>success</c> parameter for the action selection menu. This method sets <paramref name="success"/> to <c>true</c> to break out of the action select loop and return to the previous menu.</param>
        private void UndoActionSelection(Player player, out bool success)
        {
            Choice = Choices.Back;
            player.ActionTaken = false;
            player.ChosenAction = Player.Action.none;
            Console.Clear();
            success = true;
        }
    }
}
