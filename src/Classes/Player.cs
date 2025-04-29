using System.Text;

namespace DuelingDuelsters.Classes
{
    /// <summary>
    /// Contains and manages player character stats, AI, class details, actions, and other data.
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Default constructor for a <c>Player</c> object.
        /// </summary>
        /// <param name="brain"></param>
        public Player(PlayerBrain brain)
        {
            Brain = brain;
            Name = _name;
            _class = PlayerClass.None;
            ActionTaken = false;
            HealsPerformed = 0;
        }
        
        /// <exclude />
        private string _name;
        /// <summary>
        /// The player's name, chosen during character creation.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Available classes for a player. Each class has its own strengths and weaknesses.
        /// </summary>
        public enum PlayerClass
        {
            /// <summary>
            /// The default class, used for error checking and initialization.
            /// </summary>
            None,
            /// <summary>
            /// Neutral class with average stats all around.
            /// </summary>
            Normie,
            /// <summary>
            /// Defense-oriented class with high health and defensive stats, but low speed and attack. Great for blocking and taking advantage of staggered opponents.
            /// </summary>
            Fridge,
            /// <summary>
            /// Attack-oriented class with high attack, average speed and health, but low defense. Great for endlessly swinging a sword.
            /// </summary>
            Leeroy,
            /// <summary>
            /// Speed-oriented class with high speed, average health and defense, and low attack. Great for counterattacks and dodges.
            /// </summary>
            Gymnast,
            /// <summary>
            /// The only class that can heal. Generally average stats, with the exception of slightly higher health. Great for staying alive and outlasting opponents.
            /// </summary>
            Medic
        }
        
        /// <exclude />
        private PlayerClass _class;
        /// <summary>
        /// The player's chosen class, set during character creation.
        /// </summary>
        public PlayerClass Class
        {
            get { return _class; }
            set { _class = value; }
        }

        /// <summary>
        /// Available types of brain controlling the player, human or computer.
        /// </summary>
        public enum PlayerBrain
        {
            /// <summary>
            /// The player is controlled by a human. 
            /// </summary>
            Human,
            /// <summary>
            /// The player is controlled by AI. Used in single-player matches.
            /// </summary>
            Computer
        }

        /// <summary>
        /// This player's brain, set during player count selection.
        /// </summary>
        public PlayerBrain Brain;
        
        /// <exclude />
        private int _health;
        /// <summary>
        /// The player's current health. The player's <c>Health</c> value cannot be less than 0 or higher than the player's <see cref="MaxHealth">MaxHealth</see>.
        /// </summary>
        public int Health
        {
            get { return _health; }
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
        /// The player's maximum health. Each class has a different <c>MaxHealth</c>.
        /// </summary>
        public int MaxHealth
        { get; set; }
        
        /// <exclude />
        private string _healthReadout;
        /// <summary>
        /// The player's health readout, displayed on the round header while a match is in progress. This helps the player keep track of their health during a match so they can act accordingly.
        /// </summary>
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
        /// The player's attack statistic. The higher the <c>Attack</c>, the more damage the player does with each hit. 
        /// <para>
        /// The player's <c>Attack</c> value is used to calculate base damage and critical damage.
        /// </para>
        /// </summary>
        public int Attack
        { get; set; }
        /// <summary>
        /// The player's defense statistic. The higher the <c>Defense</c>, the less damage the player takes when they are hit. 
        /// </summary>
        /// <remarks><c>Defense</c> does not apply to critical hit damage! Skilled players can use this fact to their advantage when playing against a Fridge character.</remarks>
        public int Defense
        { get; set; }
        /// <summary>
        /// The player's speed statistic. 
        /// <para>
        /// The higher the <c>Speed</c>, the more likely the player is to land critical hits and execute counterattacks on successful dodges.
        /// </para>
        /// </summary>
        public int Speed
        { get; set; }
        /// <summary>
        /// The player's character sheet. The sheet is built using the player's current health and stats every time it is retrieved.
        /// </summary>
        public string CharSheet
        {
            get
            {
                return BuildCharacterSheet();
            }
        }
        /// <summary>
        /// A list of actions the player can select from during action selection. This list is generated after the player selects a class.
        /// </summary>
        public string ActionList;

        /// <summary>
        /// The available actions a <see cref="Player">Player</see> can take. Each action consists of an act combined with a direction.
        /// </summary>
        /// <remarks>An <see cref="Match.Outcome">Outcome</see> is generated based on a combination of both player's selected <c>Action</c>.</remarks>
        public enum Action
        {
            /// <summary>
            /// No action has been taken. This is the default Action, used for error checking and initialization.
            /// </summary>
            none,
            /// <summary>
            /// The player swings their sword to the right.
            /// </summary>
            swingR,
            /// <summary>
            /// The player swings their sword to the left.
            /// </summary>
            swingL,
            /// <summary>
            /// The player blocks to the right.
            /// </summary>
            blockR,
            /// <summary>
            /// The player blocks to the left.
            /// </summary>
            blockL,
            /// <summary>
            /// The player dodges to the right.
            /// </summary>
            dodgeR,
            /// <summary>
            /// The player dodges to the left.
            /// </summary>
            dodgeL,
            /// <summary>
            /// The player heals in any direction.
            /// </summary>
            heal
        }

        /// <summary>
        /// The <c>Action</c> the player has chosen this round.
        /// </summary>
        public Action ChosenAction
        { get; set; }

        /// <exclude />
        private int _baseDamage;
        /// <summary>
        /// The base damage for this player. BaseDamage is calculated using the attacking player's <see cref="Player.Attack">Attack</see> and the defending player's <see cref="Defense">Defense</see>.
        /// </summary>
        public int BaseDamage
        {
            get { return _baseDamage; }

            set
            {
                if (value <= 0)
                { _baseDamage = 0; }
                else { _baseDamage = value; }
            }
        }

        // ** Status Flags **

        /// <summary>
        /// Flag that indicates whether or not the player has selected an action this round.
        /// <para>
        /// <c>true</c> if the player has selected an action.<br/>
        /// <c>false</c> if the player have not selected an action.
        /// </para>
        /// </summary>
        public bool ActionTaken
        { get; set; }

        /// <summary>
        /// Flag that indicates whether or not the player is staggered.
        /// </summary>
        /// <remarks>A player becomes staggered when the player's attack is blocked by a defending player. A staggered player is more susceptible to a critical hit if they are attacked in the next round.</remarks>
        public bool IsStaggered
        { get; set; }

        /// <summary>
        /// Flag the indicates whether or not the player is countering.
        /// </summary>
        /// <remarks>Players have a chance to counter when they successfully <see cref="Player.Action.dodgeL">dodge</see> an attack. Countering players score an automatic critical hit on their opponents.</remarks>
        public bool IsCountering
        { get; set; }

        /// <summary>
        /// Flag that indicates whether or not the player is healing.
        /// </summary>
        /// <remarks>Healing players are susceptible to counterattacks if the other player took a <see cref="Player.Action.swingL">swing</see> Action during the same round.</remarks>
        public bool IsHealing
        { get; set; }

        /// <exclude />
        private int _healsPerformed;
        /// <summary>
        /// The number of times the player has healed during the match.
        /// </summary>
        /// <remarks>A player's <c>HealsPerformed</c> starts at <c>0</c> and increments every time the player heals. When <c>HealsPerformed</c> reaches <c>3</c>, the player's <see cref="CanHeal">CanHeal</see> flag changes to <c>false</c> and the player can no longer heal for the rest of the match.</remarks>
        public int HealsPerformed
        {
            get { return _healsPerformed; }
            set
            {
                if (_healsPerformed <= 3)
                {
                    _healsPerformed = value;
                }
                else
                {
                    _healsPerformed = 3;
                }
            }
        }

        /// <summary>
        /// Flag that indicates whether or not the player can heal.
        /// </summary>
        /// <remarks><c>CanHeal</c> is <c>true</c> as long as <see cref="HealsPerformed">HealsPerformed</see> is less than 3.</remarks>
        public bool CanHeal
        {
            get { return HealsPerformed < 3; }
        }

        /// <summary>
        /// Builds a character sheet for the character.
        /// </summary>
        /// <remarks>This method runs when the value of <see cref="CharSheet">CharSheet</see> is read, typically at the start of a round.</remarks>
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
            StringBuilder blankBuilder = new StringBuilder();
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
            StringBuilder charSheetDivider = new StringBuilder();
            charSheetDivider.Append('-', charNameLength);
            string charSheetDiv = charSheetDivider.ToString();

            // Build character sheet
            StringBuilder charSheetBuilder = new StringBuilder();
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
        /// Builds a set of actions for the player to choose from and sets the <see cref="ActionList"/> member variable to the resulting string. Different actions are available depending on the player's class.
        /// </summary>
        /// <remarks>This method is run once for each player after character creation.</remarks>
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
        /// Resets the player's health to their <see cref="MaxHealth"/>.
        /// </summary>
        /// <remarks>This method is called at the start of a rematch.</remarks>
        public void ResetCharacterHealth()
        {
            Health = MaxHealth;
        }

        /// <summary>
        /// Calculates base damage for this player. Base damage is calculated using the player's <see cref="Attack">Attack</see> and the <see cref="Defense">Defense</see> of the <paramref name="defPlayer">defending player</paramref>.
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
        /// Determines whether or not the player lands a critical hit.
        /// </summary>
        /// <remarks>The player's chance to land a critical hit rises if the player's <see cref="IsCountering">IsCountering</see> flag is set to <c>true</c>. A higher <see cref="Speed">Speed</see> stat also increases the chance for a critical hit.
        /// <para>
        /// A critical hit is successful if:
        /// <list type="bullet">
        ///  <item>
        ///    <description>The player rolls a `12` or greater on their critical roll (determined by <see cref="Match.rng">RNG</see>) and the <paramref name="defPlayer"/> is not blocking.</description>
        ///  </item>
        ///  <item>
        ///    <description>The defending player is staggered.</description>
        ///  </item>
        ///  <item>
        ///    <description>The defending player dodges into the attack.</description>
        ///  </item>
        /// </list>
        /// </para>
        /// </remarks>
        /// <param name="defPlayer">The defending player.</param>
        /// <returns><c>true</c> if the player successfully landed a critical hit.<br/>
        ///<c>false</c> if the player did not land a critical hit.</returns>
        public bool IsCrit(Player defPlayer)
        {
            int critRoll = Match.rng.Next(0, 20) + (Speed / 2);
            // increase chance to crit if attacking player is countering and the defending player is not blocking.
            if (IsCountering == true && (defPlayer.ChosenAction != Action.blockL && defPlayer.ChosenAction != Action.blockR))
            { critRoll += 5; }
            // Crit is successful if the crit roll is >= 12, if the defending player is staggered, or if the defending player is dodging.
            if (critRoll >= 12 || defPlayer.IsStaggered == true || (defPlayer.ChosenAction == Action.dodgeL || defPlayer.ChosenAction == Action.dodgeR))
            { return true; }
            else if (IsCountering == true && (defPlayer.ChosenAction == Action.blockL || defPlayer.ChosenAction == Action.blockR))
            { return false; }
            else { return false; }
        }

        /// <summary>
        /// Calculates damage for a critical hit. 
        /// </summary>
        /// <remarks>Critical hits ignore opposing player's defense, and critical hit damage is applied on top of any base damage. Critical hit damage is based on <see cref="Match.rng">RNG</see> and the player's <see cref="Attack">Attack</see>.</remarks>
        /// <returns>Critical hit damage.</returns>
        public int CalculateCritDamage()
        {
            int critDamage = Match.rng.Next(1, 5) + (Attack / 2);
            return critDamage;
        }

        /// <summary>
        /// Determines whether or not the player successfully executes a counterattack. The player's <see cref="IsCountering">IsCountering</see> variable is set to the result.
        /// </summary>
        /// <remarks>A counterattack's success is based on <see cref="Match.rng">RNG</see> and the player's <see cref="Player.Speed">Speed</see>.</remarks>
        /// <returns><c>true</c> if the player successfully counters.<br/>
        /// <c>false</c> if the player does not successfully counter.</returns>
        public bool IsCounter()
        {
            int counterRoll = Match.rng.Next(0, 20) + Speed;
            IsCountering = counterRoll >= 15 ? true : false;

            return IsCountering;
        }

        /// <summary>
        /// Heals the player if they have heals remaining.
        /// </summary>
        public void HealSelf()
        {
            if (CanHeal)
            {
                int healAmount = Match.rng.Next(1, 10);
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
                Health += healAmount;
                HealsPerformed++;
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
