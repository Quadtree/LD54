using Godot;
using System;

public class SpellCard : TextureRect
{
    public Spell Spell;

    public override void _Ready()
    {

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (Spell is Spell spell)
        {
            this.FindChildByName<Label>("Name").Text = spell.Name;
        }
    }
}
