using System.Collections.Generic;
using System.Linq;

public class Nullify : Spell
{
    private static IReadOnlyList<IntVec2> _Footprint = new IntVec2[]{
        new IntVec2(0,0),
    };

    private static IReadOnlyList<IntVec2> _Footprint2 = new IntVec2[]{
        new IntVec2(0,0),
        new IntVec2(0,1),
        new IntVec2(0,-1),
        new IntVec2(1,0),
        new IntVec2(-1,0),
    };

    public override IReadOnlyList<IntVec2> Footprint => _Footprint;

    public override void Cast(Combatant caster, Combatant target, IntVec2 pos)
    {
        base.Cast(caster, target, pos);

        foreach (var it in _Footprint2.Select(it => pos + it))
        {
            if(target.Grid.IsCellInBounds(it)) target.Grid.CellsUsed[it.x, it.y] = true;
        }
    }
}