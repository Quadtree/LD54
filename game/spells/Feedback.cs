using System.Collections.Generic;
using System.Linq;
using Godot;

public class Feedback : Spell
{
    public override Color RuneColor => new Color("c000de");
    public override byte RuneType => 3;

    private static IReadOnlyList<IntVec2> _Footprint = new IntVec2[]{
        new IntVec2(0,0),
        //new IntVec2(1,0),
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

        for (var x = point.x - 1; x <= point.x + 1; ++x)
        {
            for (var y = point.y - 1; y <= point.y + 1; ++y)
            {
                if (!enemyGrid.IsCellInBounds(new IntVec2(x, y)) || !enemyGrid.CellsUsed[x, y]) return false;
            }
        }

        return true;
    }

    public override string Desc => "Deals 8 damage if the matching spot on the enemy grid has a 3x3 area filled";

    public override string SoundEffect => "res://sounds/feedback.wav";
}