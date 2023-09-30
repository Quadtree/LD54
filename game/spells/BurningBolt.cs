using System.Collections.Generic;

public class BurningBolt : Spell
{
    public override IReadOnlyList<IntVec2> Footprint => new IntVec2[] { new IntVec2(0, 0) };

    public override void Cast(Combatant caster, Combatant target, IntVec2 pos)
    {
        base.Cast(caster, target, pos);

        target.HP -= 2;
    }
}