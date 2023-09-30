using Godot;
using System;

public class Default : Node2D
{
    MatchState MS = new MatchState();

    Spell SelectedSpell;

    Spell[] PossibleSpells = new Spell[]{
        new BurningBolt()
    };

    public override void _Ready()
    {
        var c1 = this.FindChildByName<CombatantStatus>("CombatantStatus");
        var c2 = this.FindChildByName<CombatantStatus>("CombatantStatus2");
        c1.Src = () => MS.Combatants[0];
        c2.Src = () => MS.Combatants[1];

        c1.CellClickedListeners.Add(v2 => TryCastSpell(SelectedSpell, 0, 1, v2));

        // @TODO: Remove me
        c2.CellClickedListeners.Add(v2 => TryCastSpell(SelectedSpell, 1, 0, v2));
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        for (var i = 0; i < 1; ++i)
        {
            if (Input.IsActionJustPressed($"select_spell_{i}"))
            {
                SelectedSpell = PossibleSpells[i];
                GD.Print($"Spell {SelectedSpell.Name} selected");
            }
        }
    }

    private void TryCastSpell(Spell spell, int casterId, int targetId, IntVec2 cell)
    {
        if (!spell.IsValidForCaster(MS.Combatants[casterId])) { GD.Print("Not enough SP!"); return; }
        if (!spell.IsValidAtPoint(cell, MS.Combatants[casterId].Grid)) { GD.Print("Not a valid target"); return; }


    }
}
