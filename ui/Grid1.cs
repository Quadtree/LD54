using Godot;
using System;

public class Grid1 : GridContainer
{
    Func<PlayerGrid> Src = () => null;

    public override void _Ready()
    {
        var template = this.FindChildByName<TextureRect>("Template");
        template.GetParent().RemoveChild(template);

        var grid = Src();

        for (var y = 0; y < grid.Height; ++y)
        {
            for (var x = 0; x < grid.Width; ++x)
            {

            }
        }
    }

    public override void _Process(float delta)
    {

    }
}
