using System.Collections.Generic;
using Godot;

public class BasicAI
{
    public IEnumerable<bool> RunMainTurn(int myId, MatchState ms)
    {
        var opoId = 1 - myId;

        var startTimeMs = Time.GetTicksMsec();

        for (var i = 0; i < 10_000; ++i)
        {
            var trgPos = new IntVec2(
                Util.RandInt(0, ms.Combatants[myId].Grid.Width),
                Util.RandInt(0, ms.Combatants[myId].Grid.Height)
            );

            var spell = Util.Choice(Default.PossibleSpells);

            ms.TryCastSpell(spell, myId, opoId, trgPos);

            if (ms.Combatants[myId].SP <= 0) break;

            if (Time.GetTicksMsec() - startTimeMs > 8)
            {
                yield return false;
                startTimeMs = Time.GetTicksMsec();
            }
        }

        ms.EndTurn();

        yield return true;
    }

    public IEnumerable<bool> RunReactionTurn(int myId, MatchState ms)
    {
        yield return true;
    }
}