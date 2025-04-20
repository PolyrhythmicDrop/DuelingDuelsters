using System.Reflection;
using System.Text;

namespace DuelingDuelsters.Classes
{
    /// <summary>
    /// Manages menus, user input, and almost all of the strings in the game.
    /// <para>
    /// The <c>Narrator</c> class defines methods for running each menu in the game, selecting from binary options, retrieving narration strings based on a round's <see cref="Match.Outcome">Outcome</see>, and reading user input.
    /// </para>
    /// </summary>
    public class Narrator
    {

        ConsoleKeyInfo _keyInfo = new ConsoleKeyInfo();
        public ConsoleKeyInfo KeyInfo
        {
            get => _keyInfo;
            set => _keyInfo = value;
        }

        public ConsoleKey Key
        {
            get => _keyInfo.Key;
        }

        public enum Choices
        {
            Reset,
            NewGame,
            Help,
            Exit,
            ReturnToTitle,
            Back,
            Yes = 6,
            Left = 6,
            No = 7,
            Right = 7
        }

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

        private const string newGame = "1. New Game\n";
        private const string exitGame = "2. Exit\n";
        private const string help = "3. Help\n";

        private const string pressAnyKey = "Press any key to continue...";

        // ~ Player Count Select Screen ~ //

        private const string singlePlayer = "1. One Player\n";
        private const string twoPlayer = "2. Two Players\n";
        private const string returnTitle = "3. Back to Title\n";

        // ~ Character Creation Screen ~ //

        private const string createCharacter = "*** PLAYER {0}, CREATE YOUR CHARACTER *** \n";
        private const string selectName = "\nEnter your character's name:\n";
        private const string selectClass = "Welcome, {0}.\n\nSelect your class:\n\n1. {1}\n2. {2}\n3. {3}\n4. {4}\n5. {5}\n6. {6}";
        private const string confirmCharacter = "\nLet's make sure you got everything right. Here's your character:\n\n{0}\n";
        private const string satisfied = "\nAre you satisfied with {0}? Y/n";
        private const string welcomePlayer = "Welcome our newest Duelster:\n\n~~ {0} the {1} ~~\n";
        private const string enterArena = "{0} is entering the arena...\n";

        // ~ Class Summaries ~ //

        private const string normieDescription = "Normie\n------\n ♡ ☒☒☒☐☐ | ⛨  ☒☒☒☐☐ | ⚔  ☒☒☒☐☐ | 👟 ☒☒☒☐☐ \nIf Mario were in this game, he would be a Normie.\n";
        private const string fridgeDescription = "Fridge\n------\n ♡ ☒☒☒☒☒ | ⛨  ☒☒☒☒☒ | ⚔  ☒☐☐☐☐ | 👟 ☒☐☐☐☐ \nCan take whatever you throw at them, but can have trouble dishing it out.\n";
        private const string leeroyDescription = "Leeroy\n------\n ♡ ☒☒☒☐☐ | ⛨  ☒☐☐☐☐ | ⚔  ☒☒☒☒☒ | 👟 ☒☒☒☐☐ \nExpert at bashin', smashin', and crashin', not so much at plannin'.\n";
        private const string gymnastDescription = "Gymnast\n-------\n ♡ ☒☒☒☐☐ | ⛨  ☒☒☐☐☐ | ⚔  ☒☒☐☐☐ | 👟 ☒☒☒☒☒ \nNimble and acrobatic, the Gymnast can dance on the head of a pin and also skewer their opponents with it.\n";
        private const string medicDescription = "Medic\n-----\n ♡ ☒☒☒☒☐ | ⛨  ☒☒☒☐☐ | ⚔  ☒☒☒☐☐ | 👟 ☒☒☐☐☐ | ✜ \nThe only class that can heal, the Medic is durable and doesn't care one whit about the Hippocratic Oath.\n";
        private const string randomDescription = "Random\n ♡ ????? | ⛨  ????? | ⚔  ????? | 👟 ????? \nRoll the dice and let the gods determine your fate.";

        /// <summary>
        /// Confirmation that you are, in fact, ready to duel like your life depends on it (spoiler: it does).
        /// </summary>
        internal const string readyToDuel = "\nAre you ready to duel like you've never duelled before?\n\n1. Yes, let's do this!\n2.No, let's start over.";

        // ~ In-Round Menu Strings ~ //

        private const string selectDirection = "Which direction?\n1. Left\n2. Right\n";
        private const string selectAction = "\n{0}, select an action:\n";
        private const string confirmAction = "Is this what you want to do? Y/n\n";

        // ~ Outcome Strings ~ //

        private const string swordClash = "The swords of {0} and {1} clash, the sound of ringing steel echoing throughout the arena!\nThe two combatants eye each other over their crossed blades.\n\nIs this the start of an enemies-to-lovers romance? Or another chapter in a long tale of bitter rivalry?\n\n{0} and {1} part with a puff of dust and return to their ready stances.\n";

        private const string bothHit = "{0} and {1} slash each other at the same time, slicing through armor and egos!\n";

        private const string blocked = "{0} handily blocks {1}'s hardy blow!\n{1}'s arm shudders from the impact, staggering them!\n";
        private const string failedBlock = "{0}'s sword slices into {1}'s unguarded side!\n";
        private const string bothBlock = "{0} and {1} hide behind their shields, two turtles scared of the world outside their shells.\nMaybe they will find the courage to face each other in the next round!\n";
        private const string failedDodge = "{0} tries to dodge directly into {1}'s attack!\nAn unfortunate blunder, the price of which {0} will pay in blood and shame!\n";
        private const string dodged = "{0} nimbly sidesteps {1}'s powerful attack!\n{0} uses their cat-like reflexes to mount a bewildering counterattack!\n";
        private const string failedCounter = "{0} whiffs their counter in embarrassing fashion! {1} breathes a sigh of relief.";
        private const string dodgeBlock = "{0} dodges toward {1}'s vulnerable side!\n{0} prepares to take advantage of the situation with extreme prejudice!\n";
        private const string failedDodgeBlock = "{0} rolls their eyes at {1}'s fruitless dancing over the top of their mighty shield.\nThe crowd showers the combatants with boos, thirsting for blood, or at least a little less fancy footwork.\n";
        private const string doubleDodge = "The crowd claps along to an unheard beat as {0} and {1} groove and wiggle across the battlefield, matching each other's steps in a deadly dance of martial prowess.\nNobody takes damage, but everybody has a good time.\n";
        private const string uneventfulHeal = "{0} miraculously heals their wounds with the power of science that could be mistaken for magic!\n{1}'s defensive actions are all for naught!\n";
        private const string failedHealandDefend = "{0} is out of snake oil, thoughts, and prayers! Too bad {1} only took a defensive action.\nThe crowd laughs in derision at the ill-prepared, tactically inept combatants.\n";
        private const string healAndSwing = "{0} recovers health! But {1} has a chance to tear off the Band-Aid...\n";
        private const string failedHealAndSwing = "{0} wastes a turn swatting the flies away from their empty medicine bag!\n{1} takes advantage of {0}'s lack of counting ability!\n";
        private const string healDodge = "{0} uses advanced diagnostic technology to anticipate and dodge {1}'s attack at the last moment!\n{0} is as agile as a CAT scan!\n";
        private const string healCounter = "\n{0} is too woozy from pain meds to dodge {1}'s attack!\n{1} collects on {0}'s medical bills with a vengeance!\n";
        private const string doubleHeal = "\n{0} and {1} engage in rigorous academic debate over the restorative properties of leeches!\nTheir humours are aligned but their ideas are not!\n";
        private const string failedDoubleHeal = "\n{0} sighs luxuriously as a feeling of health and well-being washes over their battle-scarred body!\n{1} takes a long, sad look at their empty medical back and sharpens their sword, ready for revenge!\n";
        private const string doubleFailedHeal = "\n{0} and {1} look at each other sheepishly. They may have doctorates from Duelster College, but they lack the ability to count!\nThe crowd boos and laughs at the inadequacy of {0} and {1}'s fancy book learning when it comes to arena smarts!\n\n";

        // ** Player Action Description Strings **

        private const string swingLeft = "{0} swings their sword to their left!\n";
        private const string swingRight = "{0} swings their sword to their right!\n";
        private const string guardLeft = "{0} raises their shield and guards their left side!\n";
        private const string guardRight = "{0} raises their shield and guards their right side!\n";
        private const string dodgeLeft = "{0} quickly dodges to their left!\n";
        private const string dodgeRight = "{0} quickly dodges to their right!\n";
        private const string heal = "{0} breaks out their emergency trauma kit!\nThey stitch up their gaping wounds and pour isopropyl alcohol over their head!\n";
        private const string failedHeal = "{0} fumbles around in their medical bag for supplies, but they are fresh out!\n{0} can't heal for the rest of the match!\n";
        private const string noAction = "{0} does nothing! How could they be so irresponsible?\n";

        // ** Victory String **

        public const string declareVictor = "\n*** {0} is victorious! ***\n\nAll hail the most dueling duelster of them all:\n\n*** {1} ***\n";
        public const string postMatchOptions = "What do you want to do next?\n\n1. Return to title\n2. Rematch\n";

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
                    throw (new NullReferenceException("Narration is null! Did you pass the right value?"));
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

        public bool RunCharacterCreation(Player player, int playerNumber)
        {
            bool success = false;

            // Create a character
            while (player.Name == null && player.Class == Player.PlayerClass.None)
            {

                // Prompt to set the player's name.
                do
                {
                    Console.WriteLine(string.Format(createCharacter, playerNumber));
                    Console.WriteLine(selectName);
                }
                while (!SetPlayerName(player));

                if (Choice == Choices.Back)
                {
                    success = true;
                    //Console.Clear();
                    return success;
                }

                Console.Clear();

                // User is prompted to enter their character's class.
                do
                {
                    Console.WriteLine(string.Format(selectClass, player.Name, normieDescription, fridgeDescription, leeroyDescription, gymnastDescription, medicDescription, randomDescription));
                }
                while (!SetPlayerClass(player));
                // If player enters Escape during class selection, return to name entry.
                if (Choice == Choices.Back)
                {
                    Console.Clear();
                    continue;
                }

                // Confirm player name, class, and stats.
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
                    // If no, return to the start.
                    case Choices.No:
                    case Choices.Back:
                        Console.Clear();
                        success = false;
                        player.Class = Player.PlayerClass.None;
                        player.Name = null;
                        break;
                }
            }

            // Final error checking and action list creation.
            try
            {
                if (player.Class == Player.PlayerClass.None)
                {
                    success = false;
                    throw new ArgumentNullException("Class", "Character needs a class!");
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

            Console.WriteLine(string.Format(enterArena, player.Name));
            Console.WriteLine(DrawArena());

            // Wait for input before continuing.
            while (!PressAnyKey());

            return success;

        }

        private bool SetPlayerName(Player player)
        {
            bool success = false;

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
                        player.MaxHealth = 20;
                        player.Health = player.MaxHealth;
                        player.Attack = 15;
                        player.Defense = 5;
                        player.Speed = 6;
                        break;
                    }
                case ConsoleKey.D4:
                case ConsoleKey.NumPad4:
                    {
                        player.Class = Player.PlayerClass.Gymnast;
                        player.MaxHealth = 20;
                        player.Health = player.MaxHealth;
                        player.Attack = 8;
                        player.Defense = 6;
                        player.Speed = 10;
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
                    // Randomize character choice.
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

        private void RandomSetPlayerClass(Player player)
        {
            Random rand = new();
            int choice = rand.Next(1, 5);
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

        public bool RunHelpScreen()
        {
            bool success = false;
            string? filePath = null;

            // Set the file path to use from the game's state.
            try
            {
                switch (GameLoop.GameState)
                {
                    default:
                        throw new ArgumentNullException("state", "State cannot be null!");
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
        /// Prompts the player to choose an action to take.
        /// </summary>
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

        public bool ComputerActionSelect(Player player)
        {
            bool success = false;

            int time = (int)DateTime.Now.Ticks;
            Random rand = new(time);
            int choice = 0;

            if (player.Class != Player.PlayerClass.Medic || (player.Class == Player.PlayerClass.Medic && !player.CanHeal) || (player.Class == Player.PlayerClass.Medic && player.Health == player.MaxHealth))
            {
                choice = rand.Next(0, 5);
            }
            else if (player.Class == Player.PlayerClass.Medic && (player.Health < (player.MaxHealth / 2)))
            {
                choice = rand.Next(0, 8);
            }
            else
            {
                choice = rand.Next(0, 6);
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
        /// Presents the player with a binary choice (yes/no, 1/2, left/right, etc.). Also provides the option of pressing Esc to go back.
        /// </summary>
        /// <param name="dialog">Dialog to display to the player. This also includes the choices they can select.</param>
        /// <returns></returns>
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
                return ("There should be a picture of an arena here...");
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
