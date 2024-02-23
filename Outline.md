# Dueling Duelsters

Dueling Duelsters (hereafter known as DD) is a two player, text-based combat game. 

*** Key ***
P1 - Player 1
P2 - Player 2
PAtk - Attacking player. The player who is hitting.
PDef - Defending player. The player who is being hit.
Atk - Attack. Determines RNG values for crit and counter damage.
Def - Defense
HP - Health
Spd - Speed. Determines RNG values for counter-attack chance.
BD - Base Damage.
RNG(n-n) - Random number generator. (n-n) is the range of numbers to be used (e.x. 1-3).
***********

## Game Flow

At the start of the game, players create a character by setting their name and choosing a class. After both players create a character, combat begins.

DD is based on rounds. At the start of a round, player one (P1) chooses their action. Then, player 2 (P2) chooses their action. The round plays out accordingly. Once damage has been tallied and all actions have been taken, the next round begins. The first player to run out of health loses. If both players run out of health at the same time, it is a draw.

### Round Outcomes

<P1 Action> + <P2 Action>: <Result>

Swing Same Direction
- Swing Left + Swing Left: The swords meet in the middle. Neither player takes damage.
- Swing Right + Swing Right: The swords meet in the middle. Neither player takes damage.
Swing Opposite Direction
- Swing Left + Swing Right: Both players hit each other with the full force of their blades. Both players take BD and have a chance to crit.
- Swing Right + Swing Left: Both players hit each other with the full force of their blades. Both players take BD and have a chance to crit.

Swing Block Same Direction
- Swing Left + Block Left: P2 blocks P1's swing. Neither player takes damage. P1 is Staggered.
- Swing Right + Block Right: P2 blocks P1's swing. Neither player takes damage. P1 is Staggered.
Swing Block Opposite Direction
- Swing Left + Block Right: P1's swing hits P2 and P1 has a chance to crit. P2 takes BD+Crit (if any).
- Swing Right + Block Left: P1's swing hits P2 and P1 has a chance to crit. P2 takes BD+Crit (if any).
- Block <Direction> + Block <Direction>: Both players block, and nobody lands a hit. Let's laugh about this one and move on.

Swing Dodge Same Direction
- Swing Left + Dodge Left: P1's swing misses P2 and does no damage. P2 rolls to counter.
- Swing Right + Dodge Right: P1's swing misses P2 and does no damage. P2 rolls to counter.
Swing Dodge Opposite Direction
- Swing Left + Dodge Right: P1's swing hits P2 and instantly crits. P1 rolls to determine crit damage. P2 takes BD+Crit.
- Swing Right + Dodge Left: P1's swing hits P2 and instantly crits. P1 rolls to determine crit damage. P2 takes BD+Crit.

- Dodge <Direction> + Dodge <Direction>: Both players dodge, and nobody gets the chance to counter.

Block Dodge Opposite Direction
- Block Left + Dodge Right: Neither player takes damage.
- Block Right + Dodge Left: Neither player takes damage.
Block Dodge Same directions
- Block Left + Dodge Left: P2 rolls to counter. If successful, counter damage is reduced by half and P2 does not get the chance to crit.
- Block Right + Dodge Right: P2 rolls to counter. If successful, counter damage is reduced by half and P2 does not get the chance to crit.

- Heal + Swing <Direction>: P1 heals. P1 rolls to avoid P2's swing. If roll is successful, P1 takes no damage. If roll is unsuccessful, P1 takes BD+Crit (if any).
- Heal + Block <Direction>: P1 heals.
- Heal + Dodge <Direction>: P1 heals.

## Classes

- Normie: Perfectly average at everything. Akin to a Fighter in DnD. Average attack, average defense, average speed, nothing special.
- Fridge: High defense, lower attack and speed. Tanky.
- Leeroy: High attack, very low defense, average speed. Bashin' and crashin' with no regard for personal safety.
- Gymnast: High speed, lower defense, slightly lower attack. Dodge master.
- Medic: Slightly lower attack, average defense and speed. Only class that can heal.

## Mechanics

### Actions

#### Swinging

Swinging is the basic method of attacking. The player can swing their sword using either the Swing Left or Swing Right action:

* Swing Left - Swings the player's sword to the left. 
* Swing Right - Swings the player's sword to the right.

#### Blocking

Blocking is the basic method of defense. If the player blocks in the same direction as their opponent's swing, the player takes no damage. If the player blocks in the opposite direction of their opponent's swing, the player takes full damage. 

The player can block using either the Block Left or Block Right action:

* Block Left - Blocks to the left.
* Block Right - Blocks to the right.

#### Dodging

Dodging is a high risk, high reward action. If the player dodges in the **same** direction of their opponent's swing, the player takes no damage and can roll to counter. If the player dodges in the **opposite** direction as their opponent's swing, their opponent gets an automatic crit.

Dodging when your opponent blocks is similar. If the player dodges in the **same** direction of their opponent's block, the player takes no damage and can roll to counter. Counter damage is reduced by half and the counter cannot crit. If the player dodges in the **opposite** direction as their opponent's block, neither player takes damage.

The player can dodge using the Dodge Left or Dodge Right action:

* Dodge Left - Dodges to the left.
* Dodge Right - Dodges to the right.

#### Healing

The Medic class can heal themselves as an action. They can do this up to 3 times per match. After selecting the Heal action, they roll RNG(1-10) to determine the heal amount. If their opponent swings in any direction during a Heal action, the Medic rolls to avoid the attack. This is not a dodge; the Medic cannot counter after successfully avoiding an attack, they simply take no damage.

if (RNG(1-10) >= 6) then No Damage
if (RNG(1-10) < 6) then PAtk does a regular attack with chance to crit


### Chance and Damage Calculation

#### Base Damage

Base damage (BD) is calculated using the following formula:

BD = PAtk - (PDef / 2)

BD cannot be a negative number.

For example, if PAtk's Attack is 6 and PDef's Defense is 10, PAtk will deal 1 BD to PDef.
BD = 6 - (10 / 2)

If PAtk's Attack is 15 and PDef's Defense is 5, PAtk will deal 13 BD to PDef.
BD = 15 - (5 / 2)

#### Counters

Counters can occur when a player successfully dodges in the opposite direction of their opponent's swing. The countering player (PAtk) rolls to counter. If the counter is successful, PAtk attacks and has a higher chance to crit than with a normal attack. 

The roll to counter is partially determined by the countering player's speed:

if (RNG(1-10) + Spd >= 7) then Counter

When PDef is blocking, PAtk's counter damage is reduced by half and PAtk cannot crit.

#### Critical Hits

After almost every successful attack (except if both players swing in the same direction), the attacking player has a chance for a critical hit. They roll RNG(1-5)+(Speed / 2) to determine whether they crit, then roll RNG(2-3) to determine extra damage. Both the crit chance RNG and crit damage RNG can be affected by player stats. 

Critical damage is always applied after BD is calculated: BD+Crit

#### Stagger

If a player is staggered, the opposing player will automatically land a critical hit on their next successful attack.