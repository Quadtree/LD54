using System.Collections.Generic;
using System.Linq;

public class Feedback : Spell
{
    private static IReadOnlyList<IntVec2> _Footprint = new IntVec2[]{
        new IntVec2(0,0),
        new IntVec2(1,0),
    };

    public override IReadOnlyList<IntVec2> Footprint => _Footprint;

    public override bool FinishCasting(Combatant caster, Combatant target, IntVec2 pos)
    {
        if (!base.FinishCasting(caster, target, pos)) return false;

        target.TakeDamage(8);

        return true;
    }

    public override bool IsValidAtPoint(IntVec2 point, PlayerGrid grid, PlayerGrid enemyGrid)
    {
        if (!Footprint.All(it => grid.IsCellOpen(point + it))) return false;

        for (var x = point.x - 1; x <= point.x + 2; ++x)
        {
            for (var y = point.y - 1; x <= point.y + 1; ++y)
            {
                if (!enemyGrid.CellsUsed[x, y]) return false;
            }
        }

        return true;
    }
}