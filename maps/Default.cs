using Godot;
using System;

public class Default : Node2D
{
    MatchState MS = new MatchState();

    public override void _Ready()
    {
        this.FindChildByName<CombatantStatus>("CombatantStatus").Src = () => MS.Player;
        this.FindChildByName<CombatantStatus>("CombatantStatus2").Src = () => MS.Opponent;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {

    }
}
