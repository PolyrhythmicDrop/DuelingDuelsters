using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuelingDuelsters.Classes
{
    // *** Interfaces ***
    
    public class GameContext
    {
        // *** Properties ***

        // Pass in players as PlayerOne and PlayerTwo so we can get their properties

        private Player _playerOne;
        public Player PlayerOne
        {
            get { return _playerOne; }
            set { _playerOne = value; }
        }

        private Player _playerTwo;
        public Player PlayerTwo
        {
            get { return _playerTwo; }
            set { _playerTwo = value; }
        }

        // Round number
        public int RoundCounter
        { get; set;}


        // *** Constructors ***

        public GameContext(Player playerOne, Player playerTwo)
        {
            this.PlayerOne = playerOne;
            this.PlayerTwo = playerTwo;

            this.RoundCounter = 1;
        }


        // *** Methods ***

        // Interface implementations

        /// <summary>
        /// Plays out the round
        /// </summary>
        /// <param name="playerOne">Player one.</param>
        /// <param name="playerTwo">Player two.</param>
        public void PlayRound(Player playerOne, Player playerTwo)
        {

            do
            {
                // Describe player individual actions
                Thread.Sleep(500);
                Console.WriteLine(playerOne.DescribePlayerAction());
                Thread.Sleep(500);
                Console.WriteLine(playerTwo.DescribePlayerAction());
                Thread.Sleep(500);

                // SWING actions
                // Player One and Player Two swing in the same direction. Neither player takes damage.
                if ((playerOne.ChosenAction == Player.Action.swingL && playerTwo.ChosenAction == Player.Action.swingL) || (playerOne.ChosenAction == Player.Action.swingR && playerTwo.ChosenAction == Player.Action.swingR))
                {
                    string swordClash = $"\nThe swords of {playerOne.Name} and {playerTwo.Name} clash, the sound of ringing steel echoing throughout the arena!\nThe two combatants eye each other over their crossed blades.\nIs this the start of an enemies-to-lovers romance? Or another chapter in a long tale of bitter rivalry?\n{playerOne.Name} and {playerTwo.Name} part with a puff of dust and return to their ready stances.\n";
                    Console.WriteLine(swordClash);
                }
                // Player One and Player Two swing in opposite directions. Both players take full damage and have a chance to crit. 
                else if ((playerOne.ChosenAction == Player.Action.swingL && playerTwo.ChosenAction == Player.Action.swingR) || (playerOne.ChosenAction == Player.Action.swingR && playerTwo.ChosenAction == Player.Action.swingL))
                {
                    string bothHit = $"\n{playerOne.Name} and {playerTwo.Name} slash each other at the same time, slicing through armor and egos!\n";
                    Console.WriteLine(bothHit);
                    Thread.Sleep(500);
                    // Calculate and apply the result of player one's attack against player two
                    this.ApplyAttackResult(playerOne, playerTwo);
                    Thread.Sleep(500);
                    // Calculate and apply the result of player two's attack against player one
                    this.ApplyAttackResult(playerTwo, playerOne);
                }

                // SWING + BLOCK actions
                // P1 swings and P2 blocks in the same direction.  P1 is staggered.
                else if ((playerOne.ChosenAction == Player.Action.swingL && playerTwo.ChosenAction == Player.Action.blockL) || (playerOne.ChosenAction == Player.Action.swingR && playerTwo.ChosenAction == Player.Action.blockR))
                {
                    string blockAttack = $"\n{playerTwo.Name} handily blocks {playerOne.Name}'s hardy blow!\n{playerOne.Name}'s arm shudders from the impact, staggering them!\n";
                    Console.WriteLine(blockAttack);
                    // Stagger player one
                    playerOne.IsStaggered = true;
                }
                // P2 swings and P1 blocks in the same direction. P2 is staggered.
                else if ((playerTwo.ChosenAction == Player.Action.swingL && playerOne.ChosenAction == Player.Action.blockL) || (playerTwo.ChosenAction == Player.Action.swingR && playerOne.ChosenAction == Player.Action.blockR))
                {
                    string blockAttack = $"\n{playerOne.Name} handily blocks {playerTwo.Name}'s hardy blow!\n{playerTwo.Name}'s arm shudders from the impact, staggering them!";
                    Console.WriteLine(blockAttack);
                    // Stagger player two
                    playerTwo.IsStaggered = true;
                }
                // P1 swings and P2 blocks in the opposite direction. P2 takes damage.
                else if ((playerOne.ChosenAction == Player.Action.swingL && playerTwo.ChosenAction == Player.Action.blockR) || (playerOne.ChosenAction == Player.Action.swingR && playerTwo.ChosenAction == Player.Action.blockL))
                {
                    string failedBlock = $"\n{playerOne.Name}'s sword slices into {playerTwo.Name}'s unguarded side!\n";
                    Console.WriteLine(failedBlock);
                    Thread.Sleep(500);
                    this.ApplyAttackResult(playerOne, playerTwo);
                }
                // P2 swings and P1 blocks in the opposite direction. P1 takes damage.
                else if ((playerTwo.ChosenAction == Player.Action.swingL && playerOne.ChosenAction == Player.Action.blockR) || (playerTwo.ChosenAction == Player.Action.swingR && playerOne.ChosenAction == Player.Action.blockL))
                {
                    string failedBlock = $"\n{playerTwo.Name}'s sword slices into {playerOne.Name}'s unguarded side!\n";
                    Console.WriteLine(failedBlock);
                    Thread.Sleep(500);
                    this.ApplyAttackResult(playerTwo, playerOne);
                }

                // Double BLOCK action
                // Both players block in any direction. Neither player takes damage or is staggered.
                else if ((playerOne.ChosenAction == Player.Action.blockL || playerOne.ChosenAction == Player.Action.blockR) && (playerTwo.ChosenAction == Player.Action.blockL || playerTwo.ChosenAction == Player.Action.blockR))
                {
                    string bothBlock = $"\n{playerOne.Name} and {playerTwo.Name} hide behind their shields, two turtles scared of the world outside their shells.\nMaybe they will find the courage to face each other in the next round!\n";
                    Console.WriteLine(bothBlock);
                }

                // SWING + DODGE actions
                // P1 swings and P2 dodges in the opposite direction. P1 gets an automatic crit.
                else if ((playerOne.ChosenAction == Player.Action.swingL && playerTwo.ChosenAction == Player.Action.dodgeR) || (playerOne.ChosenAction == Player.Action.swingR && playerTwo.ChosenAction == Player.Action.dodgeL))
                {
                    string dodgeFail = $"\n{playerTwo.Name} tries to dodge directly into {playerOne.Name}'s attack!\nAn unfortunate blunder, the price of which {playerTwo.Name} will pay in blood and shame!\n";
                    Console.WriteLine(dodgeFail);
                    Thread.Sleep(500);
                    this.ApplyAttackResult(playerOne, playerTwo);
                }
                // P2 swings and P1 dodges in the opposite direction. P2 gets an automatic crit.
                else if ((playerTwo.ChosenAction == Player.Action.swingL && playerOne.ChosenAction == Player.Action.dodgeR) || (playerTwo.ChosenAction == Player.Action.swingR && playerOne.ChosenAction == Player.Action.dodgeL))
                {
                    string dodgeFail = $"\n{playerOne.Name} tries to dodge directly into {playerTwo.Name}'s attack!\nAn unfortunate blunder, the price of which {playerOne.Name} will pay in blood and shame!\n";
                    Console.WriteLine(dodgeFail);
                    Thread.Sleep(500);
                    this.ApplyAttackResult(playerTwo, playerOne);
                }
                // P1 swings and P2 dodges in the same direction. P2 gets the chance to counter, and a higher chance to crit on that counter.
                else if ((playerOne.ChosenAction == Player.Action.swingL && playerTwo.ChosenAction == Player.Action.dodgeL) || (playerOne.ChosenAction == Player.Action.swingR && playerTwo.ChosenAction == Player.Action.dodgeR))
                {
                    string dodgeSuccess = $"\n{playerTwo.Name} nimbly sidesteps {playerOne.Name}'s powerful attack!\n{playerTwo.Name} uses their cat-like reflexes to mount a bewildering counterattack!\n";
                    Console.WriteLine(dodgeSuccess);
                    Thread.Sleep(500);
                    playerTwo.IsCounter();
                    if (playerTwo.IsCountering == false)
                    {
                        Console.WriteLine($"{playerTwo.Name} whiffs their counter in embarrassing fashion! {playerOne.Name} breathes a sigh of relief.");
                    }
                    else
                    {
                        this.ApplyAttackResult(playerTwo, playerOne);
                        playerTwo.IsCountering = false;
                    }
                }
                // P2 swings and P1 dodges in the same direction. P1 gets the chance to counter, and a higher chance to crit on that counter.
                else if ((playerTwo.ChosenAction == Player.Action.swingL && playerOne.ChosenAction == Player.Action.dodgeL) || (playerTwo.ChosenAction == Player.Action.swingR && playerOne.ChosenAction == Player.Action.dodgeR))
                {
                    string dodgeSuccess = $"\n{playerOne.Name} nimbly sidesteps {playerTwo.Name}'s powerful attack!\n{playerOne.Name} uses their cat-like reflexes to mount a bewildering counterattack!\n";
                    Console.WriteLine(dodgeSuccess);
                    Thread.Sleep(500);
                    playerOne.IsCounter();
                    if (playerOne.IsCountering == false)
                    {
                        Console.WriteLine($"{playerOne.Name} whiffs their counter in embarrassing fashion! {playerTwo.Name} breathes a sigh of relief.");
                    }
                    else
                    {
                        this.ApplyAttackResult(playerOne, playerTwo);
                        playerOne.IsCountering = false;
                    }
                }

                // BLOCK + DODGE actions
                // P1 blocks and P2 dodges in the same direction. P2 gets chance to counter, though they cannot crit and the damage is reduced by half.
                else if ((playerOne.ChosenAction == Player.Action.blockL && playerTwo.ChosenAction == Player.Action.dodgeL) || (playerOne.ChosenAction == Player.Action.blockR && playerTwo.ChosenAction == Player.Action.dodgeR))
                {
                    string dodgeSuccess = $"\n{playerTwo.Name} dodges toward {playerOne.Name}'s vulnerable side!\n{playerTwo.Name} prepares to take advantage of the situation with extreme prejudice!\n";
                    Console.WriteLine(dodgeSuccess);
                    Thread.Sleep(500);
                    playerTwo.IsCounter();
                    if (playerTwo.IsCountering == false)
                    {
                        Console.WriteLine($"{playerTwo.Name} whiffs their counter in embarrassing fashion! {playerOne.Name} breathes a sigh of relief.");
                    }
                    else
                    {
                        this.ApplyAttackResult(playerTwo, playerOne);
                        playerTwo.IsCountering = false;
                    }
                }
                // P2 blocks and P1 dodges in the same direction. P1 gets chance to counter, though they cannot crit and the damage is reduced by half.
                else if ((playerTwo.ChosenAction == Player.Action.blockL && playerOne.ChosenAction == Player.Action.dodgeL) || (playerTwo.ChosenAction == Player.Action.blockR && playerOne.ChosenAction == Player.Action.dodgeR))
                {
                    string dodgeSuccess = $"\n{playerOne.Name} dodges toward {playerTwo.Name}'s vulnerable side!\n{playerOne.Name} prepares to take advantage of the situation with extreme prejudice!\n";
                    Console.WriteLine(dodgeSuccess);
                    Thread.Sleep(500);
                    playerOne.IsCounter();
                    if (playerOne.IsCountering == false)
                    {
                        Console.WriteLine($"{playerOne.Name} whiffs their counter in embarrassing fashion! {playerTwo.Name} breathes a sigh of relief.");
                    }
                    else
                    {
                        this.ApplyAttackResult(playerOne, playerTwo);
                        playerOne.IsCountering = false;
                    }
                }
                // P1 blocks and P2 dodges in the opposite direction. Neither player takes damage.
                else if ((playerOne.ChosenAction == Player.Action.blockL && playerTwo.ChosenAction == Player.Action.dodgeR) || (playerOne.ChosenAction == Player.Action.blockR && playerTwo.ChosenAction == Player.Action.dodgeL))
                {
                    string nothingHappens = $"\n{playerOne.Name} rolls their eyes at {playerTwo.Name}'s fruitless dancing over the top of their mighty shield.\nThe crowd showers the combatants with boos, thirsting for blood, or at least a little less fancy footwork.\n";
                    Console.WriteLine(nothingHappens);
                    Thread.Sleep(500);
                }
                // P2 blocks and P1 dodges in the opposite direction. Neither player takes damage.
                else if ((playerTwo.ChosenAction == Player.Action.blockL && playerOne.ChosenAction == Player.Action.dodgeR) || (playerTwo.ChosenAction == Player.Action.blockR && playerOne.ChosenAction == Player.Action.dodgeL))
                {
                    string nothingHappens = $"\n{playerOne.Name} rolls their eyes at {playerTwo.Name}'s fruitless dancing over the top of their mighty shield.\nThe crowd showers the combatants with boos, thirsting for blood, or at least a little less fancy footwork.\n";
                    Console.WriteLine(nothingHappens);
                    Thread.Sleep(500);
                }

                // Double DODGE action
                // Both players dodge. Nobody takes damage.
                else if ((playerOne.ChosenAction == Player.Action.dodgeL || playerOne.ChosenAction == Player.Action.dodgeR) && (playerTwo.ChosenAction == Player.Action.dodgeL || playerTwo.ChosenAction == Player.Action.dodgeR))
                {
                    string nothingHappens = $"\nThe crowd claps along to an unheard beat as {playerOne.Name} and {playerTwo.Name} groove and wiggle across the battlefield, matching each other's steps in a deadly dance of martial prowess.\nNobody takes damage, but everybody has a good time.\n";
                    Console.WriteLine(nothingHappens);
                }

                this.RoundCounter++;
                playerOne.ActionTaken = false;
                playerTwo.ActionTaken = false;
            }
            while (playerOne.ActionTaken == true && playerTwo.ActionTaken == true);
        }

        /// <summary>
        /// Calculates damage to a defending player after a successful attack by an attacking player, calculates whether the hit crits and for how much damage, and then applies the damage to the defending player.
        /// </summary>
        /// <param name="atkPlayer">The attacking player.</param>
        /// <param name="defPlayer">The defending player.</param>
        public void ApplyAttackResult(Player atkPlayer, Player defPlayer)
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
        /// Draws the round header during a match.
        /// </summary>
        /// <returns></returns>
        public string DrawRoundHeader()
        {
            // Consider writing your own method using arrays to create the strings, the borders, and justification so that you don't "round" numbers in the wrong direction.

            // Get the length of the current player names, health, and classes
            int p1CharNameLength = this.PlayerOne.Name.Length;
            int p2CharNameLength = this.PlayerTwo.Name.Length;
            int p1ClassNameLength = this.PlayerOne.PlayerClass.Length;
            int p2ClassNameLength = this.PlayerTwo.PlayerClass.Length;
            string p1healthToString = this.PlayerOne.Health.ToString();
            string p2healthToString = this.PlayerTwo.Health.ToString();
            string p1MaxHealthToString = this.PlayerOne.MaxHealth.ToString();
            string p2MaxHealthToString = this.PlayerTwo.MaxHealth.ToString();
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
            String roundOddMod = new string(' ', roundOddSpace);
            // Create string for spaces before and after the round readout
            System.String roundBlanks = new string(' ', (headerLength / 2 ) - (roundReadoutLength / 2) - (versusLength / 2 ));
            // Create string for blank spaces between character classes
            System.String classBlanks = new string(' ', (headerLength - (p1ClassNameLength + p2ClassNameLength) - 4));


            // String builder actions
            // Create instance of string builder to build the round header
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
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
