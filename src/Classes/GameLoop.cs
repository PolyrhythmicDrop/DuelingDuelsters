﻿using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace DuelingDuelsters.Classes
{
    public enum State
    {
        TitleScreen,
        PlayerSelect,
        CharacterCreation,
        ActionSelect,
        OutcomeDisplay,
        VictoryScreen,
        HelpDisplay
    }

    internal static class GameLoop
    {

        public static State GameState;

        static void Main(string[] args)
        {
            // Keep here for now to preserve other menus.
            ConsoleKeyInfo key;

            // Narrator to handle the menus and input.
            Narrator narrator = new Narrator();

            // Main game loop
            do
            {
            Title:
                GameState = State.TitleScreen;
                // Draw the title screen and create the initial narrator.
                do
                {
                    Console.WriteLine(DrawTitleScreen());
                }
                while (!narrator.RunTitleMenu());

                // Branch off into new game creation, help screen, or exit.
                switch (narrator.Choice)
                {
                    default:
                        goto Title;
                    case Narrator.Choices.NewGame:
                    {
                        break;
                    }
                    case Narrator.Choices.Help:
                    {
                        while (!narrator.RunHelpScreen(GameState));
                        Console.Clear();
                        goto Title;
                    }
                    case Narrator.Choices.Exit:
                    {
                        Environment.Exit(0);
                        break;
                    }
                }

            PlayerCountSelect:
                // Select single player or two player mode.
                Player.PlayerBrain? nullableBrain;
                Player.PlayerBrain brain;
                GameState = State.PlayerSelect;

                do
                {
                    Console.WriteLine(DrawTitleScreen());
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

                // Ready the players    
                Player player1 = new Player(Player.PlayerBrain.Human);
                Player player2 = new Player(brain);
                Console.Clear();

            CharacterCreation:
                GameState = State.CharacterCreation;
                // Character creation for both players.
                while (!narrator.RunCharacterCreation(player1, 1)) ;
                if (narrator.Choice == Narrator.Choices.Back)
                {
                    Console.Clear();
                    goto PlayerCountSelect;
                }

                Console.Clear();

                while (!narrator.RunCharacterCreation(player2, 2));
                if (narrator.Choice == Narrator.Choices.Back)
                {
                    Console.Clear();
                    goto PlayerCountSelect;
                }

            PreMatchSummary:

                Match match = new Match(player1, player2, narrator);
                Console.Clear();
                do
                {
                    Console.WriteLine(match.DrawRoundHeader());
                }
                while (!narrator.SelectBinary("\nAre you ready to duel like you've never duelled before?\n\n1. Yes, let's do this!\n2. No, let's start over."));
                if (narrator.Choice == Narrator.Choices.Back || narrator.Choice == Narrator.Choices.No)
                {
                    Console.Clear();
                    goto CharacterCreation;
                }
                
                Console.Clear();

            // ** Start of round loop **
            // Loop returns here if player selects Rematch after the round.
            MatchStart:
                do
                {
                    // Match plays out until one player's health reaches 0.
                    do
                    {
                        match.PlayRound();
                    }
                    while (player1.Health > 0 && player2.Health > 0);

                    // Set the victor
                    string victor;
                    if (player1.Health == 0 && player2.Health > 0)
                    {
                        victor = player2.Name;
                    }
                    else if (player2.Health == 0 && player1.Health > 0)
                    {
                        victor = player1.Name;
                    }
                    else
                    {
                        victor = "nobody";
                    }

                    // Capitalize victor for maximum victoriousness
                    string upperVictor = victor.ToUpper();

                    // Declare the victor for all to see
                    Console.Clear();
                    Console.WriteLine($"*** {victor} is victorious! ***\n\nAll hail the most dueling duelster of them all:\n\n*** {upperVictor} ***");
                    Console.WriteLine("\n\n\n");

                    // Write out post-match options
                    string postMatchOptions = "What do you want to do next?\n\n1. Return to title\n2. Rematch\n3. Exit the game\n";
                    Console.WriteLine(postMatchOptions);

                    key = Console.ReadKey(true);

                    // 1. Break out of the gameplay loop and return to the title screen.
                    if (key.Key == ConsoleKey.D1)
                    {
                        break;
                    }
                    // 2. Rematch. Reset characters' health and reset the round counter.
                    else if (key.Key == ConsoleKey.D2)
                    {
                        Console.Clear();
                        player1.ResetCharacterHealth();
                        player2.ResetCharacterHealth();
                        match.RoundCounter = 1;
                        continue;
                    }
                    // 3. Exit the game.
                    else if (key.Key == ConsoleKey.D3)
                    {
                        Environment.Exit(0);
                    }
                }
                while (key.Key != ConsoleKey.Escape);
                // Return to title
                Console.Clear();
                continue;
            }
            while (key.Key != ConsoleKey.Escape);

        }

        /// <summary>
        /// Draws the title screen at the start of the game using a StringBuilder object.
        /// </summary>
        static string DrawTitleScreen()
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
            System.Text.StringBuilder titleBuilder = new System.Text.StringBuilder();
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