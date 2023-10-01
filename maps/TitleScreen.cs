using Godot;
using System;

public class TitleScreen : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.FindChildByName<Button>("StartGameButton").Connect("pressed", this, nameof(OnStartGameButton));
    }

    void OnStartGameButton()
    {
        Default.CurrentLevel = 1;
        GetTree().ChangeScene("res://maps/Default.tscn");
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //
    //  }
}
