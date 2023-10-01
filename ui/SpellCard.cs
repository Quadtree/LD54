using Godot;
using System;
using System.Linq;

public class SpellCard : TextureRect
{
    public Spell Spell;

    Vector2 LastMouseInPos;

    public override void _Ready()
    {
        //this.Connect("gui_event", this, nameof(HandleEvent));
        MouseFilter = MouseFilterEnum.Stop;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (Spell is Spell spell)
        {
            this.FindChildByName<Label>("Name").Text = spell.Name;
            this.FindChildByName<Label>("SPCost").Text = spell.SPCost > 0 ? $"{spell.SPCost} SP Cost" : $"+{-spell.SPCost} SP";
            this.FindChildByName<Label>("Desc").Text = spell.Desc;
            this.FindChildByName<Label>("ReactionLabel").Visible = spell.IsReaction;
            this.FindChildByName<Label>("InstantLabel").Visible = spell.IsInstant;

            for (var y = 0; y < 4; ++y)
            {
                for (var x = 0; x < 4; ++x)
                {
                    var eff = new IntVec2(x - 1, y - 1);
                    var n = y * 4 + x + 1;
                    this.FindChildByName<TextureRect>($"PC{n}").Modulate = spell.Footprint.Contains(eff) ? Colors.White : Colors.Transparent;
                }
            }
        }

        //GD.Print(GetViewport().GetMousePosition());
    }

    // public override void _UnhandledInput(InputEvent @event)
    // {
    //     base._UnhandledInput(@event);


    // }

    // public override void _Input(InputEvent @event)
    // {
    //     base._Input(@event);

    //     if (@event is InputEventMouseMotion iem)
    //     {
    //         //GD.Print(@event);
    //         LastMouseInPos = GetViewport().GetMousePosition();
    //     }
    // }

    // void HandleEvent(InputEvent @event)
    // {
    //     GD.Print(@event);
    // }

    public bool IsMouseHovering => new Rect2(new Vector2(0, 0), this.RectSize).HasPoint(GetLocalMousePosition());
}
