using System.Reflection;
using System.Text;

namespace DuelingDuelsters.Classes
{
    internal static class GameLoop
    {
        static void Main(string[] args)
        {
            // ConsoleKey variable to manage input.
            ConsoleKeyInfo key;         

            // Main game loop
            do
            {
                Title: 
                // Draw the title screen and create the initial menu.
                do
                {                    
                    Console.WriteLine(DrawTitleScreen());

                    // Splash screen variables
                    string newGame = "1. New Game\n";
                    string exitGame = "2. Exit\n";
                    string help = "3. Help\n";

                    Console.WriteLine($"{newGame}\n{exitGame}\n{help}");
                    key = Console.ReadKey(true);
                    switch (key.Key)
                    {
                        // If the user selects "New Game", begin character creation for Player 1.
                        case ConsoleKey.D1:
                            {
                                Console.Clear();
                                break;
                            }
                        // If the user selects "Exit," close the program.
                        case ConsoleKey.D2:
                            {
                                Environment.Exit(0);
                                break;
                            }
                        // If the user selects "Instructions", displays the help screen.
                        case ConsoleKey.D3:
                            {
                                Console.Clear();
                                Console.WriteLine(DrawHelpScreen());
                                Console.WriteLine("Press any key to continue...");
                                Console.ReadKey();
                                Console.Clear();
                                continue;
                            }
                    }
                    Console.Clear();
                }
                while (key.Key != ConsoleKey.D1);

                Player.PlayerBrain brain = Player.PlayerBrain.Human;
                // Select single player or two player mode.
                do
                {
                    Console.WriteLine(DrawTitleScreen());
                    Console.WriteLine("1. One Player\n");
                    Console.WriteLine("2. Two Players\n");
                    Console.WriteLine("3. Back to Title\n");
                    key = Console.ReadKey(true);
                    switch (key.Key)
                    {
                        default:
                            continue;
                        case ConsoleKey.D1:
                            {
                                brain = Player.PlayerBrain.Computer;
                                Console.Clear();
                                break;
                            }
                        case ConsoleKey.D2:
                            {
                                brain = Player.PlayerBrain.Human;
                                Console.Clear();
                                break;
                            }
                        case ConsoleKey.D3:
                            {
                                Console.Clear();
                                goto Title;
                            }
                    }
                }
                while (key.Key != ConsoleKey.D1);

                // Ready the players
                Player player1 = new Player(Player.PlayerBrain.Human);
                Player player2 = new Player(brain);

                do
                {
                    CharacterCreation(player1, player2);
                    DrawPreMatchSummary(player1, player2);
                    Thread.Sleep(1000);

                    Console.WriteLine("\nAre you ready to duel like you've never duelled before?\n\n1. Yes, let's do this!\n2. No, let's start over.");
                    key = Console.ReadKey(true);
                    switch (key.Key)
                    {
                        // If the player selects 1, continue with the duel.
                        case ConsoleKey.D1:
                            break;
                        // If the player selects 2, clear the console and return to character creation.
                        case ConsoleKey.D2:
                            Console.Clear();
                            continue;
                        // If the player pushes any other button, return to character creation.
                        default:
                            Console.WriteLine("Is that a whimper of defeat? Or a rousing call to battle? Let's start over to be sure...");
                            Thread.Sleep(500);
                            Console.Clear();
                            continue;
                    }
                }
                while (key.Key != ConsoleKey.D1);

                Match gameRound = new Match(player1, player2);
                Console.Clear();

                // ** Start of round loop **
                // Loop returns here if player selects Rematch after the round.
                do
                {
                    // Round plays out until one player's health reaches 0.
                    do
                    {
                        string roundHeader = gameRound.DrawRoundHeader();
                        Console.Clear();
                        Console.WriteLine(roundHeader);
                        Thread.Sleep(500);

                        // Player 1 action selection
                        do
                        {
                            player1.SelectAction();
                            Console.Clear();
                            Console.WriteLine(roundHeader);
                        }
                        while (player1.ActionTaken == false);
                        Console.Clear();
                        Console.WriteLine(roundHeader);

                        // Player 2 action selection
                        do
                        {
                            player2.SelectAction();
                            Console.Clear();
                            Console.WriteLine(roundHeader);
                        }
                        while (player2.ActionTaken == false);

                        Console.Clear();
                        Console.WriteLine(roundHeader);

                        gameRound.PlayRound(player1, player2);

                        Console.WriteLine("\nPress any key to continue...\n");
                        Console.ReadKey(true);
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
                        gameRound.RoundCounter = 1;
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
        /// Create both characters and get them ready to duel.
        /// </summary>
        /// <param name="player1">The first player.</param>
        /// <param name="player2">The second player.</param>
        static void CharacterCreation(Player player1, Player player2)
        {
            // Player 1 character creation begins                    
            Console.WriteLine("*** PLAYER 1, CREATE YOUR CHARACTER *** \n");
            // Player 1 creates their character
            player1.CreateCharacter();
            Console.WriteLine($"Welcome our newest Duelster:\n\n~~ {player1.Name} the {player1.PlayerClass} ~~\n");
            Thread.Sleep(1000);
            Console.WriteLine(player1.Name + " is entering the arena...\n");
            Console.WriteLine(DrawArena());
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
            Console.Clear();

            // Player 2 character creation begins                    
            Console.WriteLine("*** PLAYER 2, CREATE YOUR CHARACTER ***\n");
            player2.CreateCharacter();
            Console.WriteLine($"Welcome our newest Duelster:\n\n~~ {player2.Name} the {player2.PlayerClass} ~~\n");
            Thread.Sleep(1000);
            Console.WriteLine(player2.Name + " is entering the arena...\n");
            Console.WriteLine(DrawArena());
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
            Console.Clear();
        }

        static string DrawArena()
        {
            // Get the file path for the arena.
            Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("DuelingDuelsters.res.arena-entrance.txt");
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

        /// <summary>
        /// Builds and draws the help screen. Called if the player asks for help at any time.
        /// </summary>
        /// <returns>Returns the help screen as a string.</returns>
        public static string DrawHelpScreen()
        {
            Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("DuelingDuelsters.README.md");
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
                return ("Real duelsters require no assistance in the art of the duel.\nPress some buttons, execute some martial pirouettes, see what happens!");
            }
            StreamReader streamReader = new StreamReader(stream);

            System.Text.StringBuilder helpBuilder = new System.Text.StringBuilder();

            string? helpLine = streamReader.ReadLine();
            do
            {
                helpBuilder.AppendLine($"{helpLine}");
                helpLine = streamReader.ReadLine();
            }
            while (helpLine != null);

            string helpScreen = helpBuilder.ToString();
            return helpScreen;
        }


    }
}