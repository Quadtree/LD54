using System;
using System.Collections.Generic;
using System.Linq;

public class Spell
{
    public virtual string Name => this.GetType().ToString();

    public virtual IReadOnlyList<IntVec2> Footprint => Array.Empty<IntVec2>();

    public virtual bool IsValidAtPoint(IntVec2 point, PlayerGrid grid)
    {
        return Footprint.All(it => grid.IsCellOpen(point + it));
    }
}