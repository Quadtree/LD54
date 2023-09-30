using System.Collections.Generic;

public class BurningBolt : Spell
{
    public override IReadOnlyList<IntVec2> Footprint => new IntVec2[] { new IntVec2(0, 0) };

    public override bool FinishCasting(Combatant caster, Combatant target, IntVec2 pos)
    {
        if (!base.FinishCasting(caster, target, pos)) return false;

        target.HP -= 2;

        return true;
    }
}