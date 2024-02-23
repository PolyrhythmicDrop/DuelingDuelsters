using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuelingDuelsters.Classes
{
    public class Round
    {
        // Properties

        public string Player1Name
        { get; set; }

        public string Player2Name
        { get; set; }

        public int Player1Health
        { get; set; }

        public int Player2Health
        { get; set; }


        // Constructor

        public Round(Player playerOne, Player playerTwo)
        {
            this.Player1Name = playerOne.Name;
            this.Player2Name = playerTwo.Name;
            this.Player1Health = playerOne.Health;
            this.Player2Health = playerTwo.Health;

        }

        // Method

        public string PrintPlayerNames()
        {
            return $"The first player's name is {Player1Name}, and the second player's name is {Player2Name}";
        }

        public void AdjustPlayerHealth()
        {
            Console.WriteLine($"{this.Player1Health} is the initial value for {Player1Name}'s health.");
            Player1Health = Player1Health - 5;
            
        }
    }
}
