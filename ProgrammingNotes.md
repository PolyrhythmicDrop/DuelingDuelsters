# Notes

## Action Implementation

- Player Class
	- Stores a method that asks the player to choose an action (ChooseAction()), then returns that action as a string (playerAction?).
		- Should playerAction also be a property of Player? Could store it as a variable there, then clear it every time the ChooseAction() method runs.
		- Pass playerAction to the GameRound Class as part of its constructor.
		- ChooseAction():
			- Ask the player what action they want to do.
			- List all the actions and the keyboard command that will activate them.
			- Player inputs key.
			- Ask the player what direction they want to perform the action (except for Heal, which automatically targets the player)
			- List the directions and the keyboard command that will activate them.
			- Player inputs key
			- Set Player.RoundAction to the action as a string (lSwing, rSwing, etc);
	- In the Game Loop, run ChooseAction() for each player: player1.ChooseAction(), then player2.ChooseAction(). This will store each player action in their respective instance.
		- if player1's ChooseAction() returns "lSwing", "lSwing" is stored in player1.Player's playerAction property.

## Round Implementation

- Round Class (do I even need this class?)
	- Contains a round counter

## GameContext

- Gets and sets values for each Player object.
- Gets and sets values for each Round object.
- Can pass values back and forth between Player and Round.
- GameLoop can access values from GameContext at any time (and update them?).

### GameContext Implementation

- Constructor requires Player1 and Player2 arguments:
	- GameContext(Player playerOne, Player playerTwo, Round gameRound)
- Properties
	- playerOne info
	- playerTwo info
	- roundNumber
- Methods:
	- ChooseAction() - (Implement as interfaces? i.e. IChooseActionPOne, IChooseActionPTwo. Write output to different variables for each player.)
		- Ask the player what action they want to do.
			- List all the actions and the keyboard command that will activate them.
			- Player inputs key.
			- Ask the player what direction they want to perform the action (except for Heal, which automatically targets the player)
			- List the directions and the keyboard command that will activate them.
			- Player inputs key
			- Output action as a string (lSwing, rSwing, etc) and set it to a specific variable for each player (actionPOne, actionPTwo, etc.);
	- PlayRound()
		- Takes actionPOne, actionPTwo as input.
		- Calculates what happens due to player actions.
			* Calculate hit/miss.
			* Calculate BD.
			* Calculate crit hit/miss.
			* Calculate total damage.
		- Prints strings describing what happens.
		- Updates health for each player after actions take place.
		- Clears player1.playerAction and player2.playerAction for the next round.
		- Increments the round counter.
	- DeclareVictory()
		- Checks to see if Player 1's health is 0.
		- Checks to see if Player 2's health is 0.
		- If either player's health is 0, print victory screen.
			- Victory screen displays winning player's name.


## Example Game Loop

1. Call player1.ChooseAction().
	a. ChooseAction() runs for Player 1.
	b. ChooseAction() writes the action to player1.RoundAction.
2. Clear the console.
3. Call player2.ChooseAction().
	a. ChooseAction() runs for Player 2.
	b. ChooseAction() writes the action to player2.RoundAction.
4. Clear the console.
5. Create a new instance of the Round class (gameRound).
6. Call gameRound.PlayRound().
7. Return to the top, run player1.ChooseAction().
8. Repeat loop until either player1.Health or player2.Health is 0.
9. Show victory screen.
10. Prompt players to either rematch, new game, or exit.