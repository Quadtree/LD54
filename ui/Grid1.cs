using Godot;
using System;

public class Grid1 : GridContainer
{
    public Func<PlayerGrid> Src = () => null;

    TextureRect Template;

    bool Initialized = false;

    public override void _Ready()
    {
        Template = this.FindChildByName<TextureRect>("Template");
        Template.GetParent().RemoveChild(Template);
    }

    public override void _Process(float delta)
    {
        if (!Initialized && Src() != null)
        {
            var grid = Src();

            Columns = grid.Width;

            for (var y = 0; y < grid.Height; ++y)
            {
                for (var x = 0; x < grid.Width; ++x)
                {
                    AddChild(Template.Duplicate());
                }
            }

            Initialized = true;
        }
    }
}
