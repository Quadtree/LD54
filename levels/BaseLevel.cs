using Godot;
using System;

public class BaseLevel : Node2D
{
    [Export]
    public Default.SpellEnum[] KnownSpells;

    public override void _Ready()
    {

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //
    //  }
}