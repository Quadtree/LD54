using Godot;
using System;
using System.Collections.Generic;

public class CombatantStatus : VBoxContainer
{
    [Export]
    bool Flipped;

    public Func<Combatant> Src = () => null;

    public List<Action<IntVec2>> CellClickedListeners = new List<Action<IntVec2>>();

    public Grid1 Grid1;

    bool Initialized = false;

    public override void _Ready()
    {
        Grid1 = this.FindChildByType<Grid1>();

        if (Flipped) this.FindChildByName<Node2D>("Sprite").Scale = new Vector2(-4, 4);
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
            this.FindChildByName<Label>("ShieldLabel").Text = $"Shield: {cmb.Shield}";
            this.FindChildByName<Sprite>("ShieldSprite").Modulate = new Color(1, 1, 1, Util.Clamp(cmb.Shield / 12f, 0f, 1f));
        }
    }
}
