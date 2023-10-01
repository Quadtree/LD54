using System.Collections.Generic;
using Godot;

public class FlameWave : Spell
{
    public override Color RuneColor => Colors.Red;
    public override byte RuneType => 4;

    public override bool FinishCasting(Combatant caster, Combatant target, IntVec2 pos)
    {
        if (!base.FinishCasting(caster, target, pos)) return false;

        target.TakeDamage(7);

        return true;
    }

    private static IReadOnlyList<IntVec2> _Footprint = new IntVec2[]{
        new IntVec2(0,0),
        new IntVec2(-1,0),
        new IntVec2(1,0),
        new IntVec2(2,0),
    };

    public override IReadOnlyList<IntVec2> Footprint => _Footprint;

    public override int SPCost => 2;

    public override string Name => "Flame Lance";

    public override string Desc => "Deals 7 damage";
}