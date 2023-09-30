using Godot;
using System;
using System.Linq;

public class Default : Node2D
{
    MatchState MS = new MatchState();

    Spell SelectedSpell;

    Spell[] PossibleSpells = new Spell[]{
        new BurningBolt()
    };

    CombatantStatus[] CS;

    public override void _Ready()
    {
        var c1 = this.FindChildByName<CombatantStatus>("CombatantStatus");
        var c2 = this.FindChildByName<CombatantStatus>("CombatantStatus2");
        c1.Src = () => MS.Combatants[0];
        c2.Src = () => MS.Combatants[1];

        CS = new CombatantStatus[] { c1, c2 };

        c1.CellClickedListeners.Add(v2 => MS.TryCastSpell(SelectedSpell, 0, 1, v2));

        // @TODO: Remove me
        c2.CellClickedListeners.Add(v2 => MS.TryCastSpell(SelectedSpell, 1, 0, v2));
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        for (var i = 0; i < 1; ++i)
        {
            if (Input.IsActionJustPressed($"select_spell_{i}"))
            {
                SelectedSpell = PossibleSpells[i];
                GD.Print($"Spell {SelectedSpell.Name} selected");
            }
        }

        if (Input.IsActionJustPressed("end_turn"))
        {
            MS.EndTurn();
        }

        for (var i = 0; i < 2; ++i) CS[MS.CurrentTurn].Grid1.HoveredCellsSource = () => Array.Empty<Tuple<IntVec2, Grid1.HoverType>>();

        var curHover = CS[MS.CurrentTurn].Grid1.CurrentHover;

        if (SelectedSpell != null && curHover is IntVec2 curHoverNotNull)
        {
            //GD.Print("CUR HOVER! " + string.Join(" ", SelectedSpell.Footprint.Select(it => Tuple.Create(it + curHoverNotNull, Grid1.HoverType.SpellBase))));
            CS[MS.CurrentTurn].Grid1.HoveredCellsSource = () => SelectedSpell.Footprint.Select(it => Tuple.Create(it + curHoverNotNull, Grid1.HoverType.SpellBase)).ToArray();
        }
    }


}
