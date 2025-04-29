---
uid: stats
---

# Stats

A character's stats can be the difference between life and death, a powerful blow and a light love tap, or a clever counter and a klutzy failure. 

There are two types of stat in Dueling Duelsters: **base stats** and **calculated stats**. 

* [Base stats](#base-stats) represent the foundation of your character and are set when you choose a class. 
* [Calculated stats](#calculated-stats) are influenced by your character's base stats, your opponent's base stats and (sometimes) a dice roll. For example, a player's base damage is calculated using the player's Attack stat and the opposing player's Defense stat.

## Base Stats

Your character's base stats are determined by their class. Each class has different values for Health, Attack, Defense, and Speed. You can use these differences in ability to form your strategy and execute the right moves for your character.

> [!NOTE]
> For more information on each class and their base stats, refer to <xref:classes>.

There are four base stats in Dueling Duelsters:

* [Health](#health)
* [Attack](#attack)
* [Defense](#defense)
* [Speed](#speed)

### Health

The **Health** stat is quite literally the lifeblood of any Duelster. When a player's Health reaches 0, that player has been defeated. The match is over, and the opposing character has won the day.

> [!NOTE]
> If the Health of both players reaches 0 in the same round, the match ends in a draw.

Managing your Health is the most important part of keeping your Duelster alive. Only one class (the Medic) has the ability to restore Health, and even that ability is limited. Look at your remaining Health to determine when to pounce, when to hang back and wait for an opening, and when to risk it all on a boom or bust dodge.

During a match, you can see your character's current Health and their maximum Health in the round header:

![Screenshot from Dueling Duelsters showing a Medic's character sheet in a round header. The Medic is named "WideEyedGreenhornDuelster". The character's current Health and maximum Health is circled in aqua.](~/docs/images/round-health.png)

### Attack

The **Attack** stat determines how much damage the player can deal. This stat affects all kinds of damage, including base damage and critical hit damage. The higher a player's Attack, the more damage they will deal on a successful hit.

A player's Attack value influences:

* [Base Damage](#base-damage)
* [Critical Damage](#critical-damage)

### Defense

The **Defense** stat determines how much damage the player takes when being hit by their opponent. The higher a player's Defense, the less damage they take from an opponent's regular attack.

> [!NOTE]
> A player's Defense does not influence how much damage the player takes from a critical hit!

A player's Defense value influences:

* Their opponent's [Base Damage](#base-damage)

### Speed

The **Speed** stat determines whether or not the player lands a critical hit and whether or not the player executes a counterattack after a successful dodge. The higher a player's Speed, the more likely they are to successfully perform either of these actions.

A player's Speed value influences:

* [Critical Chance](#critical-chance)
* [Counter Chance](#counter-chance)

## Calculated Stats

There are four calculated stats in Dueling Duelsters:

* [Base Damage](#base-damage)
* [Critical Damage](#critical-damage)
* [Critical Chance](#critical-chance)
* [Counter Chance](#counter-chance)

### Formula Legend

The formulas for calculated stats use the following symbols and variables:

| Symbol | Description |
|--|--|
| $\alpha$ | The action-taking player, or subject player. Used as a prefix and attached to a stat to indicate a stat for the player who is taking an action.<br/><br/>For example, $\alpha\text{Att}$ indicates the attacking player's Attack stat. |
| $\beta$ | The acted-upon player, or object player. Used as a prefix and attached to a stat to indicate a stat for the player who is being affected by an action.<br/><br/>For example, $\beta\text{Def}$ indicates the defending player's Defense stat.  |
| $\text{D}n$ | A randomly determined dice roll. The $n$ variable represents the maximum number for the dice roll.<br/><br/>For example, $\text{D}20$ refers to a dice roll whose maximum value is 20 (a "D20" in Dungeons and Dragons parlance). |
| $\text{Att}$ | The Attack base stat.  |
| $\text{Def}$  | The Defense base stat. |
| $\text{Spd}$ | The Speed base stat. |

### Base Damage

Base damage is calculated every time one player deals damage to another. The formula for base damage is below:

$$
\begin{equation}
  \text{BaseDamage} = \alpha\text{Att} - \frac{\beta\text{Def}}{2}\
\end{equation}
$$

### Critical Damage

Critical damage is calculated every time a player lands a critical hit. Critical damage is always applied on top of base damage and ignores the opposing player's Defense. The formula for critical damage is below:

$$
\begin{equation}
  \text{CriticalDamage} = \text{D}5 + \frac{\alpha\text{Att}}{2}
\end{equation}
$$

### Critical Chance

Every time one player successfully attacks another, the attacking player has a chance to land a critical hit: this chance is called the **critical chance**. The player's critical chance is determined by a dice roll (the **critical roll**), combined with the attacking player's Speed stat and the round's [Outcome](xref:DuelingDuelsters.Classes.Match.Outcome).

If a player's critical roll is at least 12, the player lands a critical hit.

The formula for calculating the player's critical roll is below:

$$
\begin{equation}
  \text{CriticalRoll} = \text{D}20 + \frac{\alpha\text{Spd}}{2}
\end{equation}
$$

Other than Speed, a few other factors can influence the player's critical chance:

* If the attacking player is countering after a successful dodge, the attacking player's critical roll increases by 5.
* If the defending player is staggered, the attacking player automatically lands a critical hit.
* If the defending player dodged into the attacking player's attack, the attacking player automatically lands a critical hit.

### Counter Chance

When a player successfully dodges, the player has a chance to execute a counterattack: this chance is called the **counter chance**. A successful counter greatly increases the player's [critical chance](#critical-chance). The player's counter chance is determined by a dice roll (the **counter roll**) and the player's Speed value.

If the player's counter roll is at least 15, the player successfully executes a counterattack.

The formulat for calculating the player's counter roll is below:

$$
\begin{equation}
  \text{CounterRoll} = \text{D}20 + \alpha\text{Spd}
\end{equation}
$$