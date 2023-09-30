using Godot;
using System;

public class CombatantStatus : VBoxContainer
{
    public Func<Combatant> Src = () => null;

    bool Initialized = false;

    public override void _Ready()
    {

    }

    public override void _Process(float delta)
    {
        if (!Initialized && Src() != null)
        {
            Initialized = true;
            this.FindChildByType<Grid1>().Src = () => Src().Grid;
        }

        var cmb = Src();
        if (cmb != null)
        {
            this.FindChildByName<Label>("HPLabel").Text = $"HP: {cmb.HP}";
            this.FindChildByName<Label>("MPLabel").Text = $"SP: {cmb.SP}";
        }
    }
}
