using Godot;
using System;
using System.Collections.Generic;

public class Grid1 : GridContainer
{
    public Func<PlayerGrid> Src = () => null;

    public List<Action<IntVec2>> CellClickedListeners = new List<Action<IntVec2>>();

    public enum HoverType
    {
        None,
        SpellBase,
    }

    public Func<IEnumerable<Tuple<IntVec2, HoverType>>> HoveredCellsSource = () => Array.Empty<Tuple<IntVec2, HoverType>>();

    TextureRect Template;

    bool Initialized = false;

    TextureRect[,] Cells;
    HoverType[,] CellHovers;

    public override void _Ready()
    {
        Template = this.FindChildByName<TextureRect>("Template");
        Template.GetParent().RemoveChild(Template);
    }

    public override void _Process(float delta)
    {
        var grid = Src();

        if (!Initialized && grid != null)
        {
            Columns = grid.Width;

            Cells = new TextureRect[grid.Width, grid.Height];

            for (var y = 0; y < grid.Height; ++y)
            {
                for (var x = 0; x < grid.Width; ++x)
                {
                    var nu = Template.Duplicate();
                    nu.Connect("gui_input", this, nameof(GridCellGUIInput), new Godot.Collections.Array(new object[] { x, y }));
                    AddChild(nu);
                    Cells[x, y] = (TextureRect)nu;
                }
            }

            Initialized = true;

            GD.Print($"Grid initialized with {grid.Width}x{grid.Height}");
        }

        if (grid != null)
        {
            if (CellHovers == null) CellHovers = new HoverType[grid.Width, grid.Height];

            for (var y = 0; y < grid.Height; ++y)
            {
                for (var x = 0; x < grid.Width; ++x)
                {
                    CellHovers[x, y] = HoverType.None;
                }
            }

            foreach (var it in HoveredCellsSource()) CellHovers[it.Item1.x, it.Item1.y] = it.Item2;

            for (var y = 0; y < grid.Height; ++y)
            {
                for (var x = 0; x < grid.Width; ++x)
                {
                    if (!grid.CellsAvailable[x,y])
                    {
                        Cells[x,y].Visible = false;
                    }
                    else
                    {
                        Cells[x,y].Visible = true;
                        if (CellHovers[x,y] == HoverType.SpellBase && )
                    }
                }
            }
        }
    }

    public void GridCellGUIInput(InputEvent evt, int x, int y)
    {
        if (evt is InputEventMouseButton)
        {
            var btn = (InputEventMouseButton)evt;
            if (btn.Pressed)
            {
                GD.Print($"Cell {x},{y} clicked {btn.Pressed}");
                foreach (var it in CellClickedListeners) it(new IntVec2(x, y));
            }
        }

    }
}
