using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DuelingDuelsters.Classes
{
    // Class for player characters. Handles stats and actions for player characters.
    public class Player
    {
        // *** Properties ***

        // Player's name
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        // Player's class
        private string playerClass;
        public string PlayerClass
        {
            get { return playerClass; }
            // only sets the player class if it is one of the specified classes.
            set
            {
                if (value == "Normie" || value == "Fridge" || value == "Leeroy" || value == "Gymnast" || value == "Medic")
                {
                    this.playerClass = value;
                }
            }
        }
        // Player's current health
        private int _health;
        public int Health
        {
            get { return _health; }
            // Set health to 0 if value set would be less than or equal to 0. Health cannot be negative.
            // Also make sure health cannot surpass maximum health, for healing purposes.
            set
            {
                if (value <= 0)
                { _health = 0; }
                else if (value >= this.MaxHealth)
                {
                    _health = this.MaxHealth;
                }
                else { _health = value; }
            }
        }
        // Player's maximum health
        public int MaxHealth
        { get; set; }
        // Player's health readout: Health / MaxHealth
        private string _healthReadout;
        public string HealthReadout
        {
            get
            {
                string healthString = Health.ToString();
                string maxHealthString = MaxHealth.ToString();
                _healthReadout = $"{healthString} / {maxHealthString}";
                return _healthReadout;
            }
        }
        // Player's attack
        public int Attack
        { get; set; }
        // Player's defense
        public int Defense
        { get; set; }
        // Player's speed
        public int Speed
        { get; set; }

        private string charSheet;
        public string CharSheet
        {
            get
            {
                return this.BuildCharacterSheet();
            }
            set { this.charSheet = value; }
        }

        // ** Action properties **
        // Action taken flag: True if the player has taken an action, false if they have not
        public bool ActionTaken
        { get; set; }
        // Enums for possible player actions
        public enum Action
        {
            none,
            swingR,
            swingL,
            blockR,
            blockL,
            dodgeR,
            dodgeL,
            heal
        }

        // The action the player has chosen
        public Action ChosenAction
        { get; set; }

        // ** Damage Properties **
        private int _baseDamage;
        public int BaseDamage
        {
            get { return _baseDamage; }
            // when CalculateBaseDamage returns less than 0, set _baseDamage to 0 instead.
            set
            {
                if (value <= 0)
                { _baseDamage = 0; }
                else { _baseDamage = value; }
            }
        }

        // ** Status Flags **

        // Flag for whether or not the player is staggered. 
        public bool IsStaggered
        { get; set; }

        // Flag for whether or not the player is countering.
        public bool IsCountering
        { get; set; }

        // Flag for whether or not the player is healing.
        public bool IsHealing
        { get; set; }

        // Heal count. Medics can only heal 3 times per match.
        private int _healCount;
        public int HealCount
        {
            get { return _healCount; }
            set
            {
                if (_healCount < 3)
                {
                    _healCount = value;
                }
                else
                {
                    _healCount = 3;
                }
            }
        }

        // *** Constructors ***

        public Player()
        {
            this.Name = name;
            this.PlayerClass = playerClass;
            this.ActionTaken = false;
            this.HealCount = 0;
        }

        // *** Constants ***

        // Description for Normie character class
        private const string DescNormie = "Normie\n------\nAbsolutely average at absolutely everything.\nIf Mario were in this game, he would be a Normie.\n";
        // Description for Fridge character class
        private const string DescFridge = "Fridge\n------\nHigh defense, low attack, average speed.\nCan take whatever you throw at them, but can have trouble dishing it out.\n";
        // Description for Leeroy character class
        private const string DescLeeroy = "Leeroy\n------\nHigh attack, low defense, average speed.\nExpert at bashin', smashin', and crashin', not so much at plannin'.\n";
        // Description for Gymnast character class
        private const string DescGymnast = "Gymnast\n-------\nHigh speed, low defense, average attack.\nNimble and acrobatic, the Gymnast can dance on the head of a pin, and also skewer their opponents with it.\n";
        // Description for Medic character class
        private const string DescMedic = "Medic\n-----\nHigh health, slightly lower attack, good speed, and average defense.\nThe only class that can heal, the Medic is durable and doesn't care one whit about the Hippocratic Oath.\n";
        // Instructions/Help screen
        private const string helpInstructions = "Here is the help that I'm going to write eventually!\n";
        // Key info
        private ConsoleKeyInfo key;
        // Random number generator
        private readonly Random rng = new Random();

        // *** Methods ***

        /// <summary>
        /// Creates a character with a name, class, and stats.
        /// </summary>
        public void CreateCharacter()
        {
            // set nullify charName and charClass to start while loop
            string? charName = null;
            string? charClass = null;
            // while loop for character creation
            while (string.IsNullOrEmpty(charName) || string.IsNullOrEmpty(charClass))
            {
                do
                {
                    // User is prompted to enter their character's name:
                    Console.WriteLine("\nEnter your character's name:\n");
                    // Character's name entered and then assigned to the charName variable:
                    charName = Console.ReadLine();
                }
                while (string.IsNullOrEmpty(charName) == true);

                // User is prompted to enter their character's class:
                while (string.IsNullOrEmpty(charClass) == true)
                {
                    Console.Clear();
                    string characterPrompt = $"Welcome, {charName}.\n\nPlease enter your character's class:\n\n1. {DescNormie}\n2. {DescFridge}\n3. {DescLeeroy}\n4. {DescGymnast}\n5. {DescMedic}";
                    Console.WriteLine(characterPrompt);
                    charClass = Console.ReadLine();
                    // Parse character class
                    if (charClass == "1" || charClass == "Normie" || charClass == "normie")
                    {
                        this.Name = charName;
                        this.PlayerClass = "Normie";
                        this.MaxHealth = 15;
                        this.Health = this.MaxHealth;
                        this.Attack = 10;
                        this.Defense = 10;
                        this.Speed = 5;
                    }
                    else if (charClass == "2" || charClass == "Fridge" || charClass == "fridge")
                    {
                        this.Name = charName;
                        this.PlayerClass = "Fridge";
                        this.MaxHealth = 25;
                        this.Health = this.MaxHealth;
                        this.Attack = 7;
                        this.Defense = 15;
                        this.Speed = 5;
                    }
                    else if (charClass == "3" || charClass == "Leeroy" || charClass == "leeroy")
                    {
                        this.Name = charName;
                        PlayerClass = "Leeroy";
                        this.MaxHealth = 18;
                        this.Health = this.MaxHealth;
                        Attack = 15;
                        Defense = 5;
                        Speed = 6;
                    }
                    else if (charClass == "4" || charClass == "Gymnast" || charClass == "gymnast")
                    {
                        this.Name = charName;
                        PlayerClass = "Gymnast";
                        this.MaxHealth = 15;
                        this.Health = this.MaxHealth;
                        Attack = 8;
                        Defense = 6;
                        Speed = 10;
                    }
                    else if (charClass == "5" || charClass == "Medic" || charClass == "medic")
                    {
                        this.Name = charName;
                        PlayerClass = "Medic";
                        this.MaxHealth = 20;
                        this.Health = this.MaxHealth;
                        Attack = 9;
                        Defense = 8;
                        Speed = 6;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine($"{charClass} is not a valid class. Please try again.");
                        charClass = null;
                        continue;
                    }
                }
                // Print player's name, chosen class, and stats, then ask player if they want to use this character, or start over.
                do
                {
                    Console.Clear();
                    Console.WriteLine("\nLet's make sure you got everything right. Here's your character:\n\n" + $"{this.CharSheet}\n");
                    Console.WriteLine($"Are you satisfied with {this.Name}? Y/n");
                    key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Y)
                    {
                        Console.Clear();
                        break;
                    }
                    else if (key.Key == ConsoleKey.N)
                    {
                        Console.Clear();
                        CreateCharacter();
                    }
                    else
                    {
                        continue;
                    }
                }
                while (key.Key != ConsoleKey.Y || key.Key != ConsoleKey.N);
            }
        }

        /// <summary>
        /// Builds a character sheet for the character.
        /// </summary>
        /// <returns>A string containing the character sheet and a fancy asterisk border.</returns>
        public string BuildCharacterSheet()
        {
            // String stats and length of said strings
            int charNameLength = this.Name.Length;
            string charHealth = $"Health: {this.Health} / {this.MaxHealth}";
            int charHealthLength = charHealth.Length;
            string charClass = $"Class: {this.PlayerClass}";
            int charClassLength = charClass.Length;
            string charAttack = $"Attack: {this.Attack}";
            int charAttackLength = charAttack.Length;
            string charDefense = $"Defense: {this.Defense}";
            int charDefenseLength = charDefense.Length;
            string charSpeed = $"Speed: {this.Speed}";
            int charSpeedLength = charSpeed.Length;

            // array of all lengths
            int[] statLengths = { charNameLength, charHealthLength, charAttackLength, charDefenseLength, charSpeedLength, charClassLength };
            // get highest length from array
            int highestLength = statLengths.Max();
            // Set the width of the character sheet
            int charSheetLength = 6 + highestLength;

            // Set the spacers after each stat and the asterisks
            int asteriskCompensator = 3;
            int nameSpacerLength = charSheetLength - charNameLength - asteriskCompensator;
            int classSpacerLength = charSheetLength - charClassLength - asteriskCompensator;
            int healthSpacerLength = charSheetLength - charHealthLength - asteriskCompensator;
            int attackSpacerLength = charSheetLength - charAttackLength - asteriskCompensator;
            int defenseSpacerLength = charSheetLength - charDefenseLength - asteriskCompensator;
            int speedSpacerLength = charSheetLength - charSpeedLength - asteriskCompensator;
            int blankSpacerLength = charSheetLength - asteriskCompensator + 1;

            // Build the spacers
            System.Text.StringBuilder blankBuilder = new System.Text.StringBuilder();
            blankBuilder.Append(' ', blankSpacerLength);
            string emptySpacer = blankBuilder.ToString();
            blankBuilder.Clear();
            blankBuilder.Append(' ', nameSpacerLength);
            string nameSpacer = blankBuilder.ToString();
            blankBuilder.Clear();
            blankBuilder.Append(' ', classSpacerLength);
            string classSpacer = blankBuilder.ToString();
            blankBuilder.Clear();
            blankBuilder.Append(' ', healthSpacerLength);
            string healthSpacer = blankBuilder.ToString();
            blankBuilder.Clear();
            blankBuilder.Append(' ', attackSpacerLength);
            string attackSpacer = blankBuilder.ToString();
            blankBuilder.Clear();
            blankBuilder.Append(' ', defenseSpacerLength);
            string defenseSpacer = blankBuilder.ToString();
            blankBuilder.Clear();
            blankBuilder.Append(' ', speedSpacerLength);
            string speedSpacer = blankBuilder.ToString();


            // Build the divider that goes above and below the name.
            System.Text.StringBuilder charSheetDivider = new System.Text.StringBuilder();
            charSheetDivider.Append('-', charNameLength);
            string charSheetDiv = charSheetDivider.ToString();

            // Build character sheet
            System.Text.StringBuilder charSheetBuilder = new System.Text.StringBuilder();
            charSheetBuilder.Append('*', charSheetLength);
            charSheetBuilder.Append("\n");
            charSheetBuilder.AppendLine($"*{emptySpacer}*");
            charSheetBuilder.AppendLine($"* {this.Name}{nameSpacer}*");
            charSheetBuilder.AppendLine($"* {charSheetDiv}{nameSpacer}*");
            charSheetBuilder.AppendLine($"* {charClass}{classSpacer}*");
            charSheetBuilder.AppendLine($"* {charHealth}{healthSpacer}*");
            charSheetBuilder.AppendLine($"* {charAttack}{attackSpacer}*");
            charSheetBuilder.AppendLine($"* {charDefense}{defenseSpacer}*");
            charSheetBuilder.AppendLine($"* {charSpeed}{speedSpacer}*");
            charSheetBuilder.AppendLine($"*{emptySpacer}*");
            charSheetBuilder.Append('*', charSheetLength);


            string characterSheet = charSheetBuilder.ToString();
            return characterSheet;
        }

        /// <summary>
        /// Resets the character to their default health and stats for a rematch.
        /// </summary>
        public void ResetCharacterHealth()
        {
            this.Health = this.MaxHealth;
        }

        // ** Action Methods **

        /// <summary>
        /// Selects an action for the player to undertake.
        /// </summary>
        public void SelectAction()
        {
            // Set string variables
            string swingOption = "1. Swing your sword.";
            string blockOption = "2. Block with your shield.";
            string dodgeOption = "3. Dodge your opponent's attack.";
            string helpOption = "4. Help";
            string healOption = "4. Heal yourself.";
            string chooseDirection = "Which direction?\n1. Left\n2. Right\n";
            // Create a new instance of StringBuilder to build the action list.
            System.Text.StringBuilder actionBuilder = new StringBuilder();
            // Create the initial action list for Medic and class that is not the medic.
            if (this.PlayerClass != "Medic")
            {
                actionBuilder.AppendLine(swingOption);
                actionBuilder.AppendLine(blockOption);
                actionBuilder.AppendLine(dodgeOption);
                actionBuilder.AppendLine(helpOption);
            }
            else
            {
                helpOption = "5. Help";
                actionBuilder.AppendLine(swingOption);
                actionBuilder.AppendLine(blockOption);
                actionBuilder.AppendLine(dodgeOption);
                actionBuilder.AppendLine(healOption);
                actionBuilder.AppendLine(helpOption);
            }
            string actionList = actionBuilder.ToString();
            do
                {
                // Player is prompted to choose an action
                Console.WriteLine($"{this.Name}, select an action:\n");
                Console.WriteLine(actionList);
                key = Console.ReadKey(true);
                // Player selects 1. Swing your sword
                if (key.Key == ConsoleKey.D1)
                {
                    // Player is prompted to choose their direction
                    Console.WriteLine(chooseDirection);
                    key = Console.ReadKey(true);
                    // Player selects 1. Left
                    if (key.Key == ConsoleKey.D1)
                    {
                        // Player receives summary of their action, swinging the sword left.
                        Console.WriteLine("Is this what you want to do? Y/n\n");
                        key = Console.ReadKey(true);
                        // Player selects Y to confirm. The action taken flag is set to True and the player's action is set to swingL
                        if (key.Key == ConsoleKey.Y)
                        {
                            this.ActionTaken = true;
                            this.ChosenAction = Action.swingL;
                        }
                        // Player selects N to start over
                        else if (key.Key == ConsoleKey.N)
                        {
                        }
                    }
                    // Player selects 2. Right
                    else if (key.Key == ConsoleKey.D2)
                    {
                        // Player receives summary of their action, swinging the sword right.
                        Console.WriteLine("Is this what you want to do? Y/n\n");
                        key = Console.ReadKey(true);
                        // Player selects Y to confirm. The action taken flag is set to True and the player's action is set to swingR
                        if (key.Key == ConsoleKey.Y)
                        {
                            this.ActionTaken = true;
                            this.ChosenAction = Action.swingR;
                        }
                        // Player selects N to start over
                        else if (key.Key == ConsoleKey.N)
                        {
                        }
                    }
                }
                // Player selects 2. Block with your shield.
                else if (key.Key == ConsoleKey.D2)
                {
                    // Player is prompted to choose their direction
                    Console.WriteLine(chooseDirection);
                    key = Console.ReadKey(true);
                    // Player selects 1. Left
                    if (key.Key == ConsoleKey.D1)
                    {
                        // Player receives summary of their action, blocking to the left.
                        Console.WriteLine("Is this what you want to do? Y/n\n");
                        key = Console.ReadKey(true);
                        // Player selects Y to confirm. The action taken flag is set to True and the player's action is set to swingL
                        if (key.Key == ConsoleKey.Y)
                        {
                            this.ActionTaken = true;
                            this.ChosenAction = Action.blockL;
                        }
                        // Player selects N to start over
                        else if (key.Key == ConsoleKey.N)
                        {
                        }
                    }
                    // Player selects 2. Right
                    else if (key.Key == ConsoleKey.D2)
                    {
                        // Player receives summary of their action, blocking to the right.
                        Console.WriteLine("Is this what you want to do? Y/n\n");
                        key = Console.ReadKey(true);
                        // Player selects Y to confirm. The action taken flag is set to True and the player's action is set to swingR
                        if (key.Key == ConsoleKey.Y)
                        {
                            this.ActionTaken = true;
                            this.ChosenAction = Action.blockR;
                        }
                        // Player selects N to start over
                        else if (key.Key == ConsoleKey.N)
                        {
                        }
                    }
                }
                // Player select 3. Dodge your opponent's attack.
                else if (key.Key == ConsoleKey.D3)
                {
                    // Player is prompted to choose their direction
                    Console.WriteLine(chooseDirection);
                    key = Console.ReadKey(true);
                    // Player selects 1. Left
                    if (key.Key == ConsoleKey.D1)
                    {
                        // Player receives summary of their action, dodging to the left.
                        Console.WriteLine("Is this what you want to do? Y/n\n");
                        key = Console.ReadKey(true);
                        // Player selects Y to confirm. The action taken flag is set to True and the player's action is set to swingL
                        if (key.Key == ConsoleKey.Y)
                        {
                            this.ActionTaken = true;
                            this.ChosenAction = Action.dodgeL;
                        }
                        // Player selects N to start over
                        else if (key.Key == ConsoleKey.N)
                        {

                        }
                    }
                    // Player selects 2. Right
                    else if (key.Key == ConsoleKey.D2)
                    {
                        // Player receives summary of their action, dodging to the right.
                        Console.WriteLine("Is this what you want to do? Y/n\n");
                        key = Console.ReadKey(true);
                        // Player selects Y to confirm. The action taken flag is set to True and the player's action is set to swingR
                        if (key.Key == ConsoleKey.Y)
                        {
                            this.ActionTaken = true;
                            this.ChosenAction = Action.dodgeR;
                        }
                        // Player selects N to start over
                        else if (key.Key == ConsoleKey.N)
                        {
                        }
                    }
                }
                // Player is not a medic and selects 4. Help or player is a medic and selects 5. Help
                else if ((key.Key == ConsoleKey.D4 && this.PlayerClass != "Medic") || (key.Key == ConsoleKey.D5 && this.PlayerClass == "Medic"))
                {
                    Console.Clear();
                    Console.WriteLine(helpInstructions);
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey(true);
                    break;
                }
                // Player is a medic and selects 4. Heal yourself.
                else if (key.Key == ConsoleKey.D4 && this.PlayerClass == "Medic")
                {
                    // Player is prompted to choose their direction (it's not a real option because the direction of the healing doesn't matter, but it effectively hides their action from the other player.
                    Console.WriteLine(chooseDirection);
                    key = Console.ReadKey(true);
                        // Player receives summary of their action
                        Console.WriteLine("Is this what you want to do? Y/n\n");
                        key = Console.ReadKey(true);
                        // Player selects Y to confirm. The action taken flag is set to True and the player's action is set to swingL
                        if (key.Key == ConsoleKey.Y)
                        {
                            this.ActionTaken = true;
                            this.ChosenAction = Action.heal;
                        }
                        // Player selects N to start over
                        else if (key.Key == ConsoleKey.N)
                        {
                        }
                }
                else
                {
                    break;
                }
            }
            while (this.ActionTaken == false);
        }

        /// <summary>
        /// Build strings describing the player's action.
        /// </summary>
        /// <returns></returns>
        public string? DescribePlayerAction()
        {
            string? playerActionDescription;
            if (this.ChosenAction == Action.swingL)
            {
                playerActionDescription = $"{this.Name} swings their sword to their left!";
                return playerActionDescription;
            }
            else if (this.ChosenAction == Action.swingR)
            {
                playerActionDescription = $"{this.Name} swings their sword to their right!";
                return playerActionDescription;
            }
            else if (this.ChosenAction == Action.blockL)
            {
                playerActionDescription = $"{this.Name} raises their shield and guards their left side!";
                return playerActionDescription;
            }
            else if (this.ChosenAction == Action.blockR)
            {
                playerActionDescription = $"{this.Name} raises their shield and guards their right side!";
                return playerActionDescription;
            }
            else if (this.ChosenAction == Action.dodgeL)
            {
                playerActionDescription = $"{this.Name} quickly dodges to their left!";
                return playerActionDescription;
            }
            else if (this.ChosenAction == Action.dodgeR)
            {
                playerActionDescription = $"{this.Name} quickly dodges to their right!";
                return playerActionDescription;
            }
            // Healing action if player has healed less than 3 times.
            else if (this.ChosenAction == Action.heal && this.HealCount < 3)
            {
                playerActionDescription = $"{this.Name} breaks out their emergency trauma kit!\nThey stich up their gaping wounds and pour isopropyl alcohol over their head!";
                return playerActionDescription;
            }
            // Healing action description if player has healed 3 times and cannot heal any more
            else if (this.ChosenAction == Action.heal && this.HealCount >= 3)
            {
                playerActionDescription = $"{this.Name} fumbles around in their medical bag for supplies, but they are fresh out!\n{this.Name} can't heal for the rest of the match!";
                return playerActionDescription;
            }
            else if (this.ChosenAction == Action.none)
            {
                playerActionDescription = $"{this.Name} does nothing! How could they be so irresponsible?";
                return playerActionDescription;
            }
            else
            {
                playerActionDescription = null;
                return playerActionDescription;
            }
        }

        // ** Damage Methods **

        /// <summary>
        /// Calculate base damage for the attacking player.
        /// </summary>
        /// <param name="defPlayer">The defending player.</param>
        /// <returns></returns>
        public int CalculateBaseDamage(Player defPlayer)
        {
            int baseDamage = this.Attack - (defPlayer.Defense / 2);
            this.BaseDamage = baseDamage;
            return this.BaseDamage;
        }

        /// <summary>
        /// Calculate whether or not a hit is a critical hit.
        /// </summary>
        /// <param name="defPlayer">The defending player. Used to determine stagger condition.</param>
        /// <returns>true = Critical hit.</returns>
        public bool IsCrit(Player defPlayer)
        {
            int critRoll = rng.Next(0, 20) + (this.Speed / 2);
            // increase chance to crit if attacking player is countering and the defending player is not blocking.
            if (this.IsCountering == true && (defPlayer.ChosenAction != Action.blockL && defPlayer.ChosenAction != Action.blockR))
            { critRoll = critRoll + 5; }
            // Crit is successful if the crit roll is >= 12, if the defending player is staggered, or if the defending player is dodging.
            if (critRoll >= 12 || defPlayer.IsStaggered == true || (defPlayer.ChosenAction == Action.dodgeL || defPlayer.ChosenAction == Action.dodgeR))
            { return true; }
            else if (this.IsCountering == true && (defPlayer.ChosenAction == Action.blockL || defPlayer.ChosenAction == Action.blockR))
            { return false; }
            else { return false; }
        }

        /// <summary>
        /// Calculate damage for a critical hit. Critical hits ignore opposing player's defense, and the damage is applied after base damage is calculated. Critical damage is based on RNG + player's attack / 3.
        /// </summary>
        /// <returns></returns>
        public int CalculateCritDamage()
        {
            int critDamage = rng.Next(1, 5) + (this.Attack / 2);
            return critDamage;
        }

        /// <summary>
        /// Roll to determine whether or not a round results in a counter.
        /// </summary>
        /// <returns>True = The player counters.</returns>
        public bool IsCounter()
        {
            int counterRoll = rng.Next(0, 20) + this.Speed;
            if (counterRoll >= 14)
            { return this.IsCountering = true; }
            else 
            { return this.IsCountering = false; }
        }

        /// <summary>
        /// Calculate heal amount, set heal flag for actions.
        /// </summary>
        /// <returns></returns>
        public bool HealSelf()
        {
            int healAmount = rng.Next(1, 10);
            string healAmountString;
            if (healAmount + this.Health <= this.MaxHealth)
            {
                healAmountString = healAmount.ToString();
            }
            else
            {
                 healAmount = this.MaxHealth - this.Health;
                 healAmountString = healAmount.ToString();
            }
            this.Health = this.Health + healAmount;
            HealCount++;
            Console.WriteLine($"{this.Name} heals for {healAmountString} health!");
            return this.IsHealing = true;
        }
    }
}
