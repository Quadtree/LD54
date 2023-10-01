# Wizard Duel
A sort of combination of Magic the Gathering and Tetris. You have a set of spells you can cast. Each spell uses space on the board in front of you, which is shaped differently in each level.

## Spells
All spells have some potential traits like:
- MP cost
- Reaction?: If a spell is instant, it can be cast after the enemy has decided on spells but _before_ they resolve. Note that any MP changes happen immediately

### Spell List
1. Flame Lance (2MP): 1x4: Deals 7 fire damage
2. Burning Bolt (1MP): 1x1: Deals 2 fire damage
3. Counterspell (2MP / Reaction): 2x1: Also places the same shape on the other side, before spells are placed. Spells will try to move to fit, but if they cannot they are removed.
4. Energize (0MP): 3x3: Generates 1 MP
5. Shield (1MP / Reaction): Provides 5 protection from fire damage
6. Chillblast (1MP / Reaction): 1x3: Deals 4 Cold damage.
7. Fireball (1MP): 3x3 Circle: Deals 6 fire damage
8. Nullify (1MP): 1x2: Also consumes this space on the other side
9. Drain (1MP): 1x1: All 9 spaces at the same spot on the other side must be filled. Deals 5 Etheric damage.
0. Luminous Lance (2MP): 1x5: Grants the target 1MP _immediately_, before their reaction phase. If the spell is not subsequently counterspelled, deals 12 fire damage.

## Levels
1. Extremely simple and easy level where you just place down Burning Bolts. Energize is also available
2. More complex scenario where there is now room for Flame Lance. The opponent still has no space for Flame Lance
3. Enemy has big wide area, and casts in bursts. Introduce Shield.
4. Enemy has big wedge-shaped area. Introduce Feedback
5. Enemy has long and thin area. Introduce counterspelling
6. Enemy has big square area. All spells available

1. Welcome to the Spellfire Tournament! Your first opponent looks pretty easy. Just click on Burning Bolt, and click in the rune grid on the left to cast it! Then press end turn. Keep it up and you'll probably win eventually.
2. Now you've got a more worthy adversary, but you've also got Flame Lance! It takes up more space, but it packs a punch!
3. This enemy is notorious for using energize to lay down a hail of flame! Fortunately, you've now got the Shield spell, which you can cast during the Reaction phase before the enemy's spells actually hit. I'd recommend reserving some SP for this.
4. This enemy also loves the Energize spell, but you've got another secret weapon: Feedback. Cast it on your side when the matching cells on the other side are full.
5. This enemy particularly prefers Flame Lance. You have a new spell, Counterspell, which you can use during your reaction phase. Use it to cause the Flame Lances to fizzle!
6. This enemy is just generally tough. You'll need to go all out to win!

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

# Graphics
We are doing large pixel style. 4x4 pixels on a 1080p screen, so 8x8 pixels on my 4K screen.

# TODO
- Some way to tell where you're targeting on the other side
- Spell descriptions on cards
- Prevent AI from doubling up their spells
- Sound
- Music

# Done
- Restarting levels on defeat
- Final graphics for runes
- Advancing from one level to the next
- Restarting levels
- Proper display of who's turn it is, etc
- Font, final button graphics
- Title screen
- Mage robe colors
- Show "cards" indicating which spells are available
- Start of level texts