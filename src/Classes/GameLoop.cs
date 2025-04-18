using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace DuelingDuelsters.Classes
{
    /// <summary>
    /// Available states of the game. The current game state affects the help screen shown when the user selects "Help".
    /// </summary>
    public enum State
    {
        /// <summary>
        /// The game is in the title screen.
        /// </summary>
        TitleScreen,
        /// <summary>
        /// The game is in the player count select menu. The player is choosing between single player and two player modes.
        /// </summary>
        PlayerSelect,
        /// <summary>
        /// The game is in character creation. The player is selecting each character's name and class.
        /// </summary>
        CharacterCreation,
        /// <summary>
        /// The game is in a match and the players are selecting their actions.
        /// </summary>
        ActionSelect,
        /// <summary>
        /// The game is in a match and the outcome narration for the current game round is being displayed to the player(s).
        /// </summary>
        OutcomeDisplay,
        /// <summary>
        /// The game is displaying the victor of the match.
        /// </summary>
        VictoryScreen,
        /// <summary>
        /// The game is in a help screen.
        /// </summary>
        HelpScreen
    }

    
    
    internal static class GameLoop
    {
        /// <summary>
        /// The current state of the game.
        /// </summary>
        public static State GameState;

        static void Main(string[] args)
        {
            // Set initial variables:

            // UTF-8 encoding to support the character glyphs
            Console.OutputEncoding = Encoding.UTF8;

            // Narrator to handle the menus and input.
            Narrator narrator = new Narrator();

            // Create and assign the title banner.
            string titleBanner = CreateTitleBanner();

            // Main game loop
            do
            {
                // Label for the title screen.
            Title:

                GameState = State.TitleScreen;

                // Draw the title screen and enable input on the title menu.
                do
                {
                    Console.WriteLine(titleBanner);
                }
                while (!narrator.RunTitleMenu());

                Console.Clear();

                // Branch off into new game creation, help screen, or exit.
                switch (narrator.Choice)
                {
                    default:
                        Console.Clear();
                        goto Title;
                    case Narrator.Choices.NewGame:
                    {
                        Console.Clear();
                        break;
                    }
                    case Narrator.Choices.Help:
                    {
                        Console.Clear();
                        while (!narrator.RunHelpScreen());
                        Console.Clear();
                        goto Title;
                    }
                    case Narrator.Choices.Exit:
                    {
                        Environment.Exit(0);
                        break;
                    }
                }

            // Select single player or two player mode.
            PlayerCountSelect:
                Player.PlayerBrain? nullableBrain;
                Player.PlayerBrain brain;
                GameState = State.PlayerSelect;

                do
                {
                    Console.WriteLine(CreateTitleBanner());
                }
                while (!narrator.RunPlayerCountMenu(out nullableBrain));

                try
                {
                    if (narrator.Choice == Narrator.Choices.ReturnToTitle)
                    {
                        Console.Clear();
                        goto Title;
                    }
                    brain = nullableBrain ?? throw new NullReferenceException("Brain cannot be null!");
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine(e.Message);
                    goto Title;
                }

                // Create the Player objects and set their brain to human or computer    
                Player player1 = new Player(Player.PlayerBrain.Human);
                Player player2 = new Player(brain);
                Console.Clear();

            P1CharacterCreation:
                GameState = State.CharacterCreation;
                // Character creation for both players.
                while (!narrator.RunCharacterCreation(player1, 1)) ;
                if (narrator.Choice == Narrator.Choices.Back)
                {
                    narrator.SelectBinary("Return to player selection? Y/n");
                    switch (narrator.Choice)
                    {
                        default:
                            narrator.Choice = Narrator.Choices.Reset;
                            Console.Clear();
                            goto P1CharacterCreation;
                        case Narrator.Choices.Yes:
                            narrator.Choice = Narrator.Choices.Reset;
                            Console.Clear();
                            goto PlayerCountSelect;                            
                    }   
                }

                Console.Clear();

            P2CharacterCreation:
                while (!narrator.RunCharacterCreation(player2, 2)) ;
                if (narrator.Choice == Narrator.Choices.Back)
                {
                    // Confirm returning to the very start of character creation.
                    narrator.SelectBinary($"Go back to character creation for {player1.Name}? Y/n");
                    switch (narrator.Choice)
                    {
                        default:
                            break;
                        case Narrator.Choices.Yes:
                            player1.Name = null;
                            player1.Class = Player.PlayerClass.None;
                            Console.Clear();
                            goto P1CharacterCreation;
                        case Narrator.Choices.No:
                        case Narrator.Choices.Back:
                            narrator.Choice = Narrator.Choices.Reset;
                            Console.Clear();
                            goto P2CharacterCreation;
                    }
                }

                Match match = new Match(player1, player2, narrator);

            PreMatchSummary:
                Console.Clear();
                do
                {
                    Console.WriteLine(match.DrawRoundHeader());
                }
                while (!narrator.SelectBinary("\nAre you ready to duel like you've never duelled before?\n\n1. Yes, let's do this!\n2. No, let's start over."));
                if (narrator.Choice != Narrator.Choices.Yes)
                {
                    // Confirm returning to the very start of character creation.
                    narrator.SelectBinary($"\nAre you sure you want to start over? Y/n");
                    if (narrator.Choice == Narrator.Choices.Yes)
                    {
                        Console.Clear();
                        player1.Name = null;
                        player2.Name = null;
                        player1.Class = Player.PlayerClass.None;
                        player2.Class = Player.PlayerClass.None;
                        match = null;
                        goto P1CharacterCreation;
                    }
                    else
                    {
                        Console.Clear();
                        goto PreMatchSummary;
                    }
                    
                }
                
                Console.Clear();

            // ** Start of round loop **
            // Loop returns here if player selects Rematch after the round.
            MatchStart:
                Console.Clear();
                do
                {
                    // Match plays out until one player's health reaches 0.
                    do
                    {
                        match.PlayRound();
                    }
                    while (player1.Health > 0 && player2.Health > 0);

                    match.DeclareVictor();

                    narrator.SelectBinary(Narrator.postMatchOptions);
                    switch (narrator.Choice)
                    {
                        // Return to title
                        case ((Narrator.Choices)6):
                        case (Narrator.Choices.Back):
                            break;
                        // Rematch
                        case ((Narrator.Choices)7):
                            Console.Clear();
                            match.PlayerOne.ResetCharacterHealth();
                            match.PlayerTwo.ResetCharacterHealth();
                            match.RoundCounter = 1;
                            continue;
                    }
                }
                while (narrator.Key != ConsoleKey.Escape);
                // Return to title
                Console.Clear();
                continue;
            }
            while (narrator.Key != ConsoleKey.Escape);

        }

        /// <summary>
        /// Creates the title screen banner that is displayed at the start of the game.
        /// </summary>
        static private string CreateTitleBanner()
        {
            
            // Initial variables, including border, ASCII art, and spacing
            int width = 82;
            int sideBorderWidth = width - 2;
            string copyright = "\u00a9 2024 Hobby Horse Studios, absolutely no rights reserved.";
            int copyrightLength = copyright.Length;
            int copyrightSpaceLength = sideBorderWidth - copyrightLength - 2;
            string copyrightSpaces = new string(' ', copyrightSpaceLength);
            string centerSpaces = new string(' ', sideBorderWidth);

            // Get the full path for the banner ASCII art file, throw an exception if null.
            Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("DuelingDuelsters.res.banner.txt");
            try 
            { 
                if (stream == null)
                {
                    throw new ArgumentNullException("Could not load the title screen banner!");
                }
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.Message);
                // If, for some reason, we couldn't load the banner, simply display the title of the game.
                return ("Dueling Duelsters");
            }

            StreamReader? streamReader = new StreamReader(stream);

            // Build the complete title screen string.
            StringBuilder titleBuilder = new StringBuilder();
            // Top border
            titleBuilder.Append('*', width);
            titleBuilder.Append("\n");
            titleBuilder.AppendLine($"*{centerSpaces}*");
            titleBuilder.AppendLine($"*{centerSpaces}*");

            // Apply asterisks around the ASCII art banner.
            string? splashLine = streamReader.ReadLine();
            do
            {
                titleBuilder.AppendLine($"* {splashLine} *");
                splashLine = streamReader.ReadLine();
            }
            while (splashLine != null);

            titleBuilder.AppendLine($"*{centerSpaces}*");
            titleBuilder.AppendLine($"*{centerSpaces}*");

            // Copyright section
            titleBuilder.AppendLine($"* {copyright}{copyrightSpaces} *");
            titleBuilder.AppendLine($"*{centerSpaces}*");
            titleBuilder.AppendLine($"*{centerSpaces}*");
            titleBuilder.Append('*', width);
            titleBuilder.Append("\n");
            titleBuilder.Append("\n");

            streamReader.Dispose();

            string splashScreen = titleBuilder.ToString();
            return splashScreen;
        }

        /// <summary>
        /// Draws the pre-match summary, including each player's name and character sheet.
        /// </summary>
        /// <param name="playerOne">Player one.</param>
        /// <param name="playerTwo">Player two.</param>
        public static void DrawPreMatchSummary(Player playerOne, Player playerTwo)
        {
            Console.Clear();
            Console.WriteLine("\nLet's get ready to D U E L!!!\n");
            Console.WriteLine(playerOne.CharSheet);
            Console.WriteLine("\n** VS. **\n");
            Console.WriteLine(playerTwo.CharSheet);
        }

    }
}