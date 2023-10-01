using System.Collections.Generic;
using System.Linq;
using Godot;

public class Shield : Spell
{
    public override Color RuneColor => Colors.Blue;
    public override byte RuneType => 5;

    private static IReadOnlyList<IntVec2> _Footprint = new IntVec2[]{
        new IntVec2(0,1),
        new IntVec2(0,0),
        new IntVec2(0,-1),
    };

    public override IReadOnlyList<IntVec2> Footprint => _Footprint;

    public override bool IsReaction => true;

    public override void StartCast(Combatant caster, Combatant target, IntVec2 pos)
    {
        base.StartCast(caster, target, pos);

        caster.Shield += 4;
    }

    public override string Desc => "Adds 4 shield. Shield dissipates at the start of your turn";
}