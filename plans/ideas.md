# Wizard Duel
A sort of combination of Magic the Gathering and Tetris. You have a set of spells you can cast. Each spell uses space on the board in front of you, which is shaped differently in each level.

## Spells
All spells have some potential traits like:
- MP cost
- Reaction?: If a spell is instant, it can be cast after the enemy has decided on spells but _before_ they resolve. Note that any MP changes happen immediately

### Spell List
- Flame Wave (2MP): 1x4: Deals 7 fire damage
- Burning Bolt (1MP): 1x1: Deals 2 fire damage
- Counterspell (2MP / Reaction): 3x1: Targets the same point on the other side. Any spell that has just been cast to the left or right of the targeted point is nullified
- Energize (0MP): 3x3: Generates 1 MP
- Shield (1MP / Reaction): Provides 5 protection from fire damage
- Etheric Bolt (2MP): 1x3: Deals 4 Etheric damage.
- Fireball (1MP): 3x3 Circle: Deals 6 fire damage
- Nullify (1MP): 2x3: Also consumes this space on the other side
- Drain (1MP): 1x1: All 9 spaces at the same spot on the other side must be filled. Deals 5 Etheric damage.
- Luminous Lance (2MP): 1x5: Grants the target 1MP _immediately_, before their reaction phase. If the spell is not subsequently counterspelled, deals 12 fire damage.

## Problems
- Matches may turn into pathetic-matches where nobody can deal damage. What will cause it to end?
  - Possible solution: Just have it be points-based instead of actually reducing your opponent's health to zero
  - Time limits
  - The bloodthirsty throng gets bored and starts throwing stuff at the contestants
  - Whoever fills their grid first overloads their magic and explodes. Also, you must cast at least 1 spell ✅
- This is already a mobile game. I don't know it's name, but it exists
- I'd have to be very careful with the layouts so the player only starts out with a couple spells and doesn't get overwhelmed.
- Some of the spell descriptions (esp. counterspell) are a bit confusing!

# Names
"Spellfire"