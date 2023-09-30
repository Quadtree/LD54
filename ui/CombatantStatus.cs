using Godot;
using System;
using System.Collections.Generic;

public class CombatantStatus : VBoxContainer
{
    public Func<Combatant> Src = () => null;

    public List<Action<IntVec2>> CellClickedListeners = new List<Action<IntVec2>>();

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
            this.FindChildByType<Grid1>().CellClickedListeners.Add((v2) => { foreach (var it in CellClickedListeners) it(v2); });
        }

        var cmb = Src();
        if (cmb != null)
        {
            this.FindChildByName<Label>("HPLabel").Text = $"HP: {cmb.HP}";
            this.FindChildByName<Label>("MPLabel").Text = $"SP: {cmb.SP}";
        }
    }
}
