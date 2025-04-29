---
uid: actions
---

# Actions

At the start of a round, each player selects an **action**. Understanding what actions to take in certain situations is essential to surviving long enough to be crowned the most dueling Duelster of them all.

> [!NOTE]
> Each player's actions during a round combine to form the round's outcome. For more information on what combinations of actions result in each outcome, refer to [Outcomes in the API Reference section](xref:DuelingDuelsters.Classes.Match.Outcome).

There are four actions in Dueling Duelsters:

* [Swing](#swing)
* [Block](#block)
* [Dodge](#dodge)
* [Heal](#heal-medic-only) (Medic only)

## Swing

![Screenshot of the Dueling Duelsters action list at the start of a round. The swing action is circled in aqua. The action reads: "1. Swing your sword."](~/docs/images/action-list-swing.png)

The **swing** action is the primary mode of attack for all classes. It's pretty simple: swing your sword and hope it hits your opponent. If you're lucky, you may even score a critical hit! But be careful: if your attack is [blocked](#block), you'll be [staggered](#staggering) and open for a counterattack in the next round.

The swing action is particularly effective for the [Leeroy class](xref:classes#leeroy), as their high [Attack stat](xref:stats#attack) makes their sword a formidable foe.

## Block

![Screenshot of the Dueling Duelsters action list at the start of a round. The block action is circled in aqua. The action reads: "2. Block with your shield."](~/docs/images/action-list-block.png)

The **block** action allows you to reduce the damage you take from an attack or to negate an attack entirely. If you successfully block a [swing](#swing), your opponent becomes [staggered](#staggering). If you block during an opponent's [dodge](#dodge), any counterattack mounted by your opponent deals half damage and cannot result in a critical hit.

The block action is a key strategic weapon in the arsenal of the [Fridge class](xref:classes#fridge). Their high [Defense stat](xref:stats#defense) means that even a blocked counterattack can result in next to no damage, and a staggered opponent can be easily overpowered even by the Fridge's lackluster Attack stat.

### Staggering

When a player's swing is successfully blocked, the attacking player becomes **staggered**. When a player is staggered, any successful attack results in an automatic critical hit. The staggered effect is triggered by a successfully blocked swing and lasts until the staggered player is hit by an attack.

## Dodge

![Screenshot of the Dueling Duelsters action list at the start of a round. The dodge action is circled in aqua. The action reads: "3. Dodge your opponent's attack."](~/docs/images/action-list-dodge.png)

The **dodge** action is a high risk, high reward action. If you successfully dodge an attack, you take no damage and have a chance to mount a powerful counterattack. This counterattack has a much higher chance to land a critical hit than a normal [swing](#swing) attack. But be careful: if you dodge in the wrong direction, you'll be vulnerable to a critical hit yourself!

The dodge action is an ideal choice for the [Gymnast class](xref:classes#gymnast): their high [Speed stat](xref:stats#speed) makes landing critical hits on counterattacks all but guaranteed, and their relatively low Defense can make it difficult to weather too many direct hits. However, any class can make good use of a well-placed dodge if they are on their last leg or wish to take their opponent by surprise.

## Heal (Medic Only)

![Screenshot of the Dueling Duelsters action list at the start of a round. The heal action is circled in aqua. The action reads: "4. Heal yourself."](~/docs/images/action-list-heal.png)

The **heal** action is a unique action that is only available to the [Medic class](xref:classes#medic). When you successfully heal, you restore between 1 and 10 points of [Health](xref:stats#health). The Medic has a chance to dodge incoming swings while healing, but safety is far from guaranteed. 

Health is restored *before* your opponent's counterattack is calculated. Because of this, healing is best done before your Health reaches the one hit kill stage.

> [!WARNING]
> Medics can only heal **three times** per match. After that, the player can still select the Heal action, but the action has no effect and opens the player up to a counterattack. Keep an eye on your Medic's `Heals Left` counter to avoid this unfortunate blunder!
>
> ![Screenshot from Dueling Duelsters showing a Medic's character sheet in a round header. The Medic is named "WideEyedGreenhornDuelster". The player's "Heals Left" counter is circled in aqua and has the number 3 next to it.](~/docs/images/heals-left.png)
