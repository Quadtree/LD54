using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public class Spell
{
    public virtual string Name => this.GetType().ToString();

    public virtual string Desc => "";

    public virtual int SPCost => 1;

    public virtual bool IsReaction => false;

    public virtual bool IsInstant => IsReaction;

    public virtual IReadOnlyList<IntVec2> Footprint => Array.Empty<IntVec2>();

    public virtual Color RuneColor => Colors.White;
    public virtual byte RuneType => 0;

    public virtual string SoundEffect => "";

    public virtual bool IsValidAtPoint(IntVec2 point, PlayerGrid grid, PlayerGrid enemyGrid)
    {
        return Footprint.All(it => grid.IsCellOpen(point + it));
    }

    public virtual bool IsValidForCaster(Combatant comb)
    {
        return comb.SP >= SPCost;
    }

    public virtual void StartCast(Combatant caster, Combatant target, IntVec2 pos)
    {
        caster.SP -= SPCost;
    }

    private static IntVec2[] DELTAS = new IntVec2[]{
        new IntVec2(0, 0),
        new IntVec2(-1, 0),
        new IntVec2(1, 0),
        new IntVec2(0, -1),
        new IntVec2(0, 1),
        new IntVec2(-2, 0),
        new IntVec2(2, 0),
        new IntVec2(0, -2),
        new IntVec2(0, 2),
    };

    public virtual bool FinishCasting(Combatant caster, Combatant target, IntVec2 pos)
    {
        foreach (var delta in DELTAS)
        {
            if (Footprint.Select(it => pos + it + delta).All(it => caster.Grid.IsCellOpen(it)))
            {
                foreach (var it in Footprint.Select(it => pos + it + delta))
                {
                    if (!caster.Grid.IsCellOpen(it)) GD.PushWarning("Invalid spell location!");
                    caster.Grid.CellsUsed[it.x, it.y] = true;
                    caster.Grid.SpellOverlays[it.x, it.y] = new PlayerGrid.SpellOverlay
                    {
                        Color = RuneColor,
                        RuneId = (byte)(RuneType + 1),
                    };
                }

                GD.Print($"{Name} has moved by {delta} to {pos + delta}");

                return true;
            }
        }

        GD.Print($"{Name} fizzles!");

        return false;
    }
}