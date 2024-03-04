﻿using DuelingDuelsters.Classes;

namespace DuelingDuelsters
{
    internal class GameLoop
    {
        static void Main(string[] args)
        {
            // variables
            ConsoleKeyInfo key;



            // ** Game start **
            // ** Title Screen Menu **
            // Print the splash screen
            do
            {
                Console.WriteLine(DrawTitleScreen());

                // Ask whether the user wants to start a new game or exit
                // Splash screen variables
                string newGame = "1. New Game\n";
                string exitGame = "2. Exit\n";
                // If the user chooses to exit, close the program
                // If the user starts a new game, begin character creation for Player 1
                Console.WriteLine($"{newGame}\n{exitGame}");
                key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.D1:
                        {
                            Console.Clear();
                            break;
                        }
                    case ConsoleKey.D2:
                        {
                            System.Environment.Exit(0);
                            break;
                        }
                }
                Console.Clear();
            }
            while (key.Key != ConsoleKey.D1 || key.Key != ConsoleKey.D2);
            // ** End title screen menu **



            // ** Character Creation **

            // Player 1 character creation begins
            // player1 object is instantiated
            Player player1 = new Player();
            Console.WriteLine("***PLAYER 1, CREATE YOUR CHARACTER***\n");
            // Player 1 creates their character
            player1.CreateCharacter();
            Console.WriteLine($"Welcome our newest Duelster, {player1.Name} the {player1.PlayerClass}!\n");

            // Player 2 character creation begins
            Player player2 = new Player();
            Console.WriteLine("***PLAYER 2, CREATE YOUR CHARACTER***\n");
            player2.CreateCharacter();
            Console.WriteLine($"Welcome our newest Duelster, {player2.Name} the {player2.PlayerClass}!\n");

            // Pre-match character summary
            Console.Clear();
            Console.WriteLine("\nLet's get ready to D U E L!!!\n");
            Console.WriteLine($"Duelster #1:\n\n{player1.CharSheet}\n\n");
            Console.WriteLine($"Duelster #2:\n\n{player2.CharSheet}\n\n");
            
            GameContext gameRound = new GameContext(player1, player2);
            Console.Clear();

            // Establish roundHeader variable
            string roundHeader = gameRound.DrawRoundHeader();
            // Round plays out until one player's health reaches 0.
            do
            {
                roundHeader = gameRound.DrawRoundHeader();
                Console.Clear();
                Console.WriteLine(roundHeader);

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

                Console.WriteLine("\nPress any key to begin the next round...\n");
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
                victor = "Nobody";
            }

            // Capitalize victor for maximum victoriousness
            string upperVictor = victor.ToUpper();

            // Declare the victor for all to see
            Console.Clear();
            Console.WriteLine($"*** {victor} is victorious!\nAll hail the most dueling duelster of them all:\n\n*** {upperVictor} ***");

            

            
            
            
            
            
            /*
            Test player 1 action selection output
            string p1Action = player1.ChosenAction.ToString();
            Console.WriteLine(p1Action);
            */




                // ** Match **
                // First round begins
                // Player 1 is prompted to choose an action
                // Print a list of Player 1's possible actions, plus their current stats
                // Player 1 enters an action, which is hidden from the console so Player 2 cannot see it
                // Player 2 is prompted to choose an action
                // Print a list of Player 2's possible actions, plus their current stats
                // Player 2 enters an action, which is hidden from the console so Player 2 cannot see it
                // Action plays out
                // Status for Player 1 and Player 2 are updated.
                // Second round begins.
                // Repeat until one player is out of health.

                // ** Post-Match **
                // Display the winner's name and a victory message.
                // Ask if the players want a rematch, to start a new game, or to exit.
                // Rematch pits the same characters against each other, starting with Round 1
                // New game returns to character creation
                // Exit game exits the program




                /* CharacterCreation player1 = new CharacterCreation();
                player1.CreateCharacter(); */
         
        }

        /// <summary>
        /// Draws the title screen at the start of the game.
        /// </summary>
        static string DrawTitleScreen()
        {
            System.Text.StringBuilder titleBuilder = new System.Text.StringBuilder();
            // set variables for the width of the box and empty spaces
            int width = 82;
            int sideBorderWidth = width - 2;
            string copyright = "\u00a9 2024 Hobby Horse Studios, absolutely no rights reserved.";
            int copyrightLength = copyright.Length;
            int copyrightSpaceLength = sideBorderWidth - copyrightLength - 2;
            System.String copyrightSpaces = new string(' ', copyrightSpaceLength);
            System.String centerSpaces = new string(' ', sideBorderWidth);
            // Get the full path for the banner file and assign it to a variable
            string fullPath = System.IO.Path.GetFullPath("banner.txt");
            // Read from banner.txt.
            StreamReader streamReader = new StreamReader(fullPath);
            // Count the lines in banner.txt
            int lineNumber = File.ReadLines(fullPath).Count();            

            // Build the string itself
            titleBuilder.Append('*', width);
            titleBuilder.Append("\n");
            titleBuilder.AppendLine($"*{centerSpaces}*");
            titleBuilder.AppendLine($"*{centerSpaces}*");
            // Loop to read banner.txt and add it to the string between asterisks.
            for (int i = 0; i < lineNumber; i++)
            {
                // Read a line from banner.txt and assign it to a variable.
                string? splashLine = streamReader.ReadLine();
                titleBuilder.AppendLine($"* {splashLine} *");
            }
            // Dispose the streamReader memory
            streamReader.Dispose();
            titleBuilder.AppendLine($"*{centerSpaces}*");
            titleBuilder.AppendLine($"*{centerSpaces}*");
            // Copyright section
            titleBuilder.AppendLine($"* {copyright}{copyrightSpaces} *");
            titleBuilder.AppendLine($"*{centerSpaces}*");
            titleBuilder.AppendLine($"*{centerSpaces}*");
            titleBuilder.Append('*', width);
            titleBuilder.Append("\n");
            titleBuilder.Append("\n");
            // Convert titleBuilder to a string
            string splashScreen = titleBuilder.ToString();
            // return the string
            return splashScreen;

        }
    }
}