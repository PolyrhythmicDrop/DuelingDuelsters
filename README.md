# Dueling Duelsters

Dueling Duelsters is a game in which two combatants slice, dice, dodge, groove, block, bash, and otherwise defeat each other through chance and strategy. Let's play!

## Actions

### Swinging

Swinging is the basic method of attacking. The player can swing their sword using either the Swing Left or Swing Right action:

* Swing Left - Swings the player's sword to the left. 
* Swing Right - Swings the player's sword to the right.

### Blocking

Blocking is the basic method of defense. If the player blocks in the same direction as their opponent's swing, the player takes no damage. If the player blocks in the opposite direction of their opponent's swing, the player takes full damage. 

The player can block using either the Block Left or Block Right action:

* Block Left - Blocks to the left.
* Block Right - Blocks to the right.

### Dodging

Dodging is a high risk, high reward action. If the player dodges in the **same** direction of their opponent's swing, the player takes no damage and can roll to counter. If the player dodges in the **opposite** direction as their opponent's swing, their opponent gets an automatic crit.

Dodging when your opponent blocks is similar. If the player dodges in the **same** direction of their opponent's block, the player takes no damage and can roll to counter. Counter damage is reduced by half and the counter cannot crit. If the player dodges in the **opposite** direction as their opponent's block, neither player takes damage.

The player can dodge using the Dodge Left or Dodge Right action:

* Dodge Left - Dodges to the left.
* Dodge Right - Dodges to the right.

### Healing

The Medic class can heal themselves as an action. They can do this up to 3 times per match. After selecting the Heal action, they roll RNG(1-10) to determine the heal amount. The heal is applied before their opponent's attack, if any.

If their opponent swings in any direction during a Heal action, the Medic rolls to avoid the attack. This is not a dodge; the Medic cannot counter after successfully avoiding an attack, they simply take no damage. Any damage done is applied after the heal has been completed.