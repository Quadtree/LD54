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
        this.FindChildByName<CombatantStatus>("CombatantStatus").Src = () => MS.Combatants[0];
        this.FindChildByName<CombatantStatus>("CombatantStatus2").Src = () => MS.Combatants[1];
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
}
