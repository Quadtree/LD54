using System.Collections.Generic;

public class BasicAI
{
    public IEnumerable<bool> RunTurn(int myId, MatchState ms)
    {
        var opoId = 1 - myId;

        for (var i = 0; i < 1000; ++i)
        {
            var trgPos = new IntVec2(
                Util.RandInt(0, ms.Combatants[myId].Grid.Width),
                Util.RandInt(0, ms.Combatants[myId].Grid.Height)
            );

            var spell = Util.Choice(Default.PossibleSpells);

            ms.TryCastSpell(spell, myId, opoId, trgPos);

            if (ms.Combatants[myId].SP <= 0) break;

            yield return false;
        }

        ms.EndTurn();

        yield return true;
    }
}