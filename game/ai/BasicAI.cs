using System;
using System.Collections.Generic;
using Godot;

public class BasicAI
{
    public static ulong LastAITick = 0;

    private static Spell[] POSSIBLE_SPELLS = new Spell[]{
        new Feedback(),
        new FlameWave(),
        new BurningBolt(),
    };

    public IEnumerable<bool> RunMainTurn(int myId, MatchState ms)
    {
        if (ms.Combatants[myId].SP >= ms.AIMinSPToCast)
        {
            var opoId = 1 - myId;

            var startTimeMs = Time.GetTicksMsec();

            for (var i = 0; i < 100_000; ++i)
            {
                LastAITick = Time.GetTicksMsec();

                var trgPos = new IntVec2(
                    Util.RandInt(0, ms.Combatants[myId].Grid.Width),
                    Util.RandInt(0, ms.Combatants[myId].Grid.Height)
                );

                var spell = ms.EnemyAvailableSpells[Math.Min(i / 2000, ms.EnemyAvailableSpells.Length - 1)];

                ms.TryCastSpell(spell, myId, opoId, trgPos);

                if (ms.Combatants[myId].SP <= 0) break;

                if (Time.GetTicksMsec() - startTimeMs > 8)
                {
                    //GD.Print($"AI is yielding at step {i}");
                    yield return false;
                    startTimeMs = Time.GetTicksMsec();
                }
            }
        }

        ms.EndTurn();

        yield return true;
    }

    public IEnumerable<bool> RunReactionTurn(int myId, MatchState ms)
    {
        ms.EndTurn();
        yield return true;
    }
}