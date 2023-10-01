using System.Collections.Generic;
using Godot;

public class BurningBolt : Spell
{
    public override Color RuneColor => Colors.Orange;
    public override byte RuneType => 0;

    public override IReadOnlyList<IntVec2> Footprint => new IntVec2[] { new IntVec2(0, 0) };

    public override bool FinishCasting(Combatant caster, Combatant target, IntVec2 pos)
    {
        if (!base.FinishCasting(caster, target, pos)) return false;

        target.TakeDamage(2);

        return true;
    }

    public override string Name => "Burning Bolt";

    public override string Desc => "Deals 2 damage. Click on this card and click on the left grid to cast";

    public override string SoundEffect => "res://sounds/burning_bolt.wav";
}