using System.Collections.Generic;

public class BurningBolt : Spell
{
    public override IReadOnlyList<IntVec2> Footprint => new IntVec2[] { new IntVec2(0, 0) };
}