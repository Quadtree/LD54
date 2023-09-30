using System;
using System.Collections.Generic;
using System.Linq;

public class Spell
{
    public virtual string Name => this.GetType().ToString();

    public virtual int SPCost => 1;

    public virtual IReadOnlyList<IntVec2> Footprint => Array.Empty<IntVec2>();

    public virtual bool IsValidAtPoint(IntVec2 point, PlayerGrid grid)
    {
        return Footprint.All(it => grid.IsCellOpen(point + it));
    }

    public virtual bool IsValidForCaster(Combatant comb)
    {
        return comb.SP >= SPCost;
    }

    public virtual void Cast(Combatant caster, Combatant target, IntVec2 pos)
    {
        foreach (var it in Footprint.Select(it => pos + it))
        {
            if (caster.Grid.IsCellInBounds(it)) caster.Grid.CellsUsed[it.x, it.y] = true;
        }

        caster.SP -= SPCost;
    }
}