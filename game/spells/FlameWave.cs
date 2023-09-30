using System.Collections.Generic;

public class FlameWave : Spell
{
    public override void Cast(Combatant caster, Combatant target, IntVec2 pos)
    {
        base.Cast(caster, target, pos);

        target.HP -= 7;
    }

    private static IReadOnlyList<IntVec2> _Footprint = new IntVec2[]{
        new IntVec2(0,0),
        new IntVec2(-1,0),
        new IntVec2(1,0),
        new IntVec2(2,0),
    };

    public override IReadOnlyList<IntVec2> Footprint => _Footprint;

    public override int SPCost => 2;

    public override string Name => "Flame Ray";
}