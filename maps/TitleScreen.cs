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
        this.FindChildByName<Button>("StartGameButton")?.Connect("pressed", this, nameof(OnStartGameButton));

        this.FindChildByName<Button>("LevelSelectButton")?.Connect("pressed", this, nameof(OnLevelSelectButton));

        for (var i = 1; i < 7; ++i) this.FindChildByName<Button>($"StartLevel{i}")?.Connect("pressed", this, nameof(OnStartLevel), new Godot.Collections.Array(new object[] { i }));
    }

    void OnStartGameButton()
    {
        Default.CurrentLevel = 1;
        GetTree().ChangeScene("res://maps/Default.tscn");
    }

    void OnLevelSelectButton()
    {
        this.FindChildByName<VBoxContainer>("LevelSelectList").Visible = true;
    }

    void OnStartLevel(int level)
    {
        Default.CurrentLevel = level;
        GetTree().ChangeScene("res://maps/Default.tscn");
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //
    //  }
}
