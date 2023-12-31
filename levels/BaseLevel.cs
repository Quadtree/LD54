using Godot;
using System;

public class BaseLevel : Node2D
{
    [Export]
    public Default.SpellEnum[] KnownSpells;

    [Export]
    public int PlayerHP;

    [Export]
    public int EnemyHP;

    [Export]
    public int AIMinSPToCast;

    [Export]
    public Default.SpellEnum[] EnemySpells;

    [Export]
    public Color EnemyMageRobeColor;

    [Export]
    public string StartOfLevelText;

    public override void _Ready()
    {

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //
    //  }
}
