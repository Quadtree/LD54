using Godot;
using System;

public class Default : Node2D
{
    MatchState MS = new MatchState();

    public override void _Ready()
    {
        this.FindChildByName<Grid1>("Grid1").Src = () => MS.Player.Grid;
        this.FindChildByName<Grid1>("Grid2").Src = () => MS.Opponent.Grid;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {

    }
}
