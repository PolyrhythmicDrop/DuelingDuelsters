using System.Text;

namespace DuelingDuelsters.Classes
{
    /// <summary>
    /// Class for player characters. Handles stats and actions for player characters.
    /// </summary>
    public class Player
    {
        public Player(PlayerBrain brain)
        {
            Brain = brain;
        }
        
        private string name;
        /// <summary>
        /// Player's name
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public enum PlayerClass
        {
            None,
            Normie,
            Fridge,
            Leeroy,
            Gymnast,
            Medic
        }
        
        private PlayerClass _class;
        /// <summary>
        /// Player's class.
        /// </summary>
        public PlayerClass Class
        {
            get { return _class; }
            set { _class = value; }
        }

        /// <summary>
        /// The possible types of brain controlling the player, human or computer.
        /// </summary>
        public enum PlayerBrain
        {
            Human,
            Computer
        }

        /// <summary>
        /// This player's brain.
        /// </summary>
        public PlayerBrain Brain;
        
        private int _health;
        /// <summary>
        /// Player's current health
        /// </summary>
        public int Health
        {
            get { return _health; }
            // Set health to 0 if value set would be less than or equal to 0. Health cannot be negative.
            // Also make sure health cannot surpass maximum health, for healing purposes.
            set
            {
                if (value <= 0)
                { _health = 0; }
                else if (value >= MaxHealth)
                {
                    _health = MaxHealth;
                }
                else { _health = value; }
            }
        }
        /// <summary>
        /// Player's maximum health
        /// </summary>
        public int MaxHealth
        { get; set; }
        /// <summary>
        /// Player's health readout: Health / MaxHealth
        /// </summary>
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
        /// <summary>
        /// Player's attack
        /// </summary>
        public int Attack
        { get; set; }
        /// <summary>
        /// Player's defense
        /// </summary>
        public int Defense
        { get; set; }
        /// <summary>
        /// Player's speed
        /// </summary>
        public int Speed
        { get; set; }

        private string charSheet;
        /// <summary>
        /// The character's character sheet. The sheet is built every time it's retrieved.
        /// </summary>
        public string CharSheet
        {
            get
            {
                return BuildCharacterSheet();
            }
            set { charSheet = value; }
        }

        public string ActionList;

        // ** Action properties **
        /// <summary>
        /// Action taken flag: True if the player has taken an action, false if they have not
        /// </summary>
        public bool ActionTaken
        { get; set; }
        /// <summary>
        /// All possible player actions as enums.
        /// </summary>
        public enum Action
        {
            /// <summary>
            /// No action has been taken.
            /// </summary>
            none,
            /// <summary>
            /// The player swings right.
            /// </summary>
            swingR,
            /// <summary>
            /// The player swings left.
            /// </summary>
            swingL,
            /// <summary>
            /// The player blocks right.
            /// </summary>
            blockR,
            /// <summary>
            /// The player blocks left.
            /// </summary>
            blockL,
            /// <summary>
            /// The player dodges right.
            /// </summary>
            dodgeR,
            /// <summary>
            /// The player dodges left.
            /// </summary>
            dodgeL,
            /// <summary>
            /// The player heals.
            /// </summary>
            heal
        }

        /// <summary>
        /// The action the player has chosen.
        /// </summary>
        public Action ChosenAction
        { get; set; }

        // ** Damage Properties **
        private int _baseDamage;
        /// <summary>
        /// The base damage for this player.
        /// </summary>
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

        /// <summary>
        /// Flag for whether or not the player is staggered. 
        /// </summary>
        public bool IsStaggered
        { get; set; }

        /// <summary>
        /// Flag for whether or not the player is countering.
        /// </summary>
        public bool IsCountering
        { get; set; }

        /// <summary>
        /// Flag for whether or not the player is healing.
        /// </summary>
        public bool IsHealing
        { get; set; }

        /// <summary>
        /// Heal count. Medics can only heal 3 times per match.
        /// </summary>
        private int _healCount;
        /// <summary>
        /// Heal count. Medics can only heal 3 times per match.
        /// </summary>
        public int HealCount
        {
            get { return _healCount; }
            set
            {
                if (_healCount <= 3)
                {
                    _healCount = value;
                }
                else
                {
                    _healCount = 3;
                }
            }
        }

        public bool CanHeal
        {
            get { return HealCount < 3; }
        }

        // *** Constructors ***

        /// <summary>
        /// Empty constructor class for each player.
        /// </summary>
        public Player()
        {
            Name = name;
            _class = PlayerClass.None;
            ActionTaken = false;
            HealCount = 0;
        }
        
        /// <summary>
        /// Key info
        /// </summary>
        private ConsoleKeyInfo key;

        /// <summary>
        /// Random number generator
        /// </summary>
        private readonly Random rng = new Random();


        /// <summary>
        /// Builds a character sheet for the character.
        /// </summary>
        /// <returns>A string containing the character sheet and a fancy asterisk border.</returns>
        public string BuildCharacterSheet()
        {
            // String stats and length of said strings
            int charNameLength = Name.Length;
            string charHealth = $"Health: {Health} / {MaxHealth}";
            int charHealthLength = charHealth.Length;
            string charClass = $"Class: {Class.ToString()}";
            int charClassLength = charClass.Length;
            string charAttack = $"Attack: {Attack}";
            int charAttackLength = charAttack.Length;
            string charDefense = $"Defense: {Defense}";
            int charDefenseLength = charDefense.Length;
            string charSpeed = $"Speed: {Speed}";
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
            charSheetBuilder.AppendLine($"* {Name}{nameSpacer}*");
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
        /// Builds a set of actions for the player to choose from. Different actions are available depending on the player's class. Sets the actions to the player's ActionList variable.
        /// </summary>
        public void BuildActionList()
        {
            List<string> actions = new();
            // Set string variables
            string swingOption = "Swing your sword.\n";
            string blockOption = "Block with your shield.\n";
            string dodgeOption = "Dodge your opponent's attack.\n";
            string healOption = "Heal yourself.\n";
            string helpOption = "Get help.\n";

            // Create a new instance of StringBuilder to build the action list.
            StringBuilder actionBuilder = new StringBuilder();

            actions.Add(swingOption);
            actions.Add(blockOption);
            actions.Add(dodgeOption);
            if (Class == PlayerClass.Medic)
            {
                actions.Add(healOption);
            }
            actions.Add(helpOption);

            // Add appropriate number to the start of each entry and build into a string with the string builder.
            for (int i = 0; i < actions.Count; i++)
            {
                int num = i + 1;
                string newAction = actions[i].Insert(0, num.ToString() + ". ");
                actions[i] = newAction;
                actionBuilder.AppendLine(actions[i]);
            }

            ActionList = actionBuilder.ToString();
        }

        /// <summary>
        /// Resets the character to their default health and stats for a rematch.
        /// </summary>
        public void ResetCharacterHealth()
        {
            Health = MaxHealth;
        }

        // ** Damage Methods **

        /// <summary>
        /// Calculate base damage for the attacking player.
        /// </summary>
        /// <param name="defPlayer">The defending player.</param>
        /// <returns></returns>
        public int CalculateBaseDamage(Player defPlayer)
        {
            int baseDamage = Attack - (defPlayer.Defense / 2);
            BaseDamage = baseDamage;
            return BaseDamage;
        }

        /// <summary>
        /// Calculate whether or not a hit is a critical hit.
        /// </summary>
        /// <param name="defPlayer">The defending player. Used to determine stagger condition.</param>
        /// <returns>true = Critical hit.</returns>
        public bool IsCrit(Player defPlayer)
        {
            int critRoll = rng.Next(0, 20) + (Speed / 2);
            // increase chance to crit if attacking player is countering and the defending player is not blocking.
            if (IsCountering == true && (defPlayer.ChosenAction != Action.blockL && defPlayer.ChosenAction != Action.blockR))
            { critRoll = critRoll + 5; }
            // Crit is successful if the crit roll is >= 12, if the defending player is staggered, or if the defending player is dodging.
            if (critRoll >= 12 || defPlayer.IsStaggered == true || (defPlayer.ChosenAction == Action.dodgeL || defPlayer.ChosenAction == Action.dodgeR))
            { return true; }
            else if (IsCountering == true && (defPlayer.ChosenAction == Action.blockL || defPlayer.ChosenAction == Action.blockR))
            { return false; }
            else { return false; }
        }

        /// <summary>
        /// Calculate damage for a critical hit. Critical hits ignore opposing player's defense, and the damage is applied after base damage is calculated. Critical damage is based on RNG + player's attack / 3.
        /// </summary>
        /// <returns></returns>
        public int CalculateCritDamage()
        {
            int critDamage = rng.Next(1, 5) + (Attack / 2);
            return critDamage;
        }

        /// <summary>
        /// Roll to determine whether or not a round results in a counter.
        /// </summary>
        /// <returns>True = The player counters.</returns>
        public bool IsCounter()
        {
            int counterRoll = rng.Next(0, 20) + Speed;
            IsCountering = counterRoll >= 15 ? true : false;

            return IsCountering;
        }

        /// <summary>
        /// Calculate heal amount, set heal flag for actions.
        /// </summary>
        /// <returns></returns>
        public void HealSelf()
        {
            if (CanHeal)
            {
                int healAmount = rng.Next(1, 10);
                string healAmountString;
                if (healAmount + Health <= MaxHealth)
                {
                    healAmountString = healAmount.ToString();
                }
                else
                {
                    healAmount = MaxHealth - Health;
                    healAmountString = healAmount.ToString();
                }
                Health = Health + healAmount;
                HealCount++;
                Console.WriteLine($"{Name} heals for {healAmountString} health!");

                IsHealing = true;
            }
            else
            {
                IsHealing = false;
            }
            
        }
    }
}
