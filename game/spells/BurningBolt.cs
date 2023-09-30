using System.Collections.Generic;

public class BurningBolt : Spell
{
    public override IReadOnlyList<IntVec2> Footprint => new IntVec2[] { new IntVec2(0, 0) };

    public override void StartCast(Combatant caster, Combatant target, IntVec2 pos)
    {
        base.StartCast(caster, target, pos);

        target.HP -= 2;
    }
}