using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Default : Node2D
{
    MatchState MS = new MatchState();

    Spell SelectedSpell;

    public static Spell[] PossibleSpells = new Spell[]{
        new FlameWave(),
        new BurningBolt(),
        new Nullify(),
        new Counterspell(),
        new Shield(),
        new Energize(),
        new Feedback(),
    };

    CombatantStatus[] CS;

    IEnumerator<bool> Operation;

    BasicAI AI = new BasicAI();

    Tuple<int, Spell>[] AvailableSpells = Array.Empty<Tuple<int, Spell>>();

    public bool IsCurrentlyPlayersTurn => (MS.CurrentTurn == 0 && MS.CurrentPhase == MatchState.Phase.Main) || (MS.CurrentTurn == 1 && MS.CurrentPhase == MatchState.Phase.Reaction);

    public override void _Ready()
    {
        var c1 = this.FindChildByName<CombatantStatus>("CombatantStatus");
        var c2 = this.FindChildByName<CombatantStatus>("CombatantStatus2");
        c1.Src = () => MS.Combatants[0];
        c2.Src = () => MS.Combatants[1];

        CS = new CombatantStatus[] { c1, c2 };

        c1.CellClickedListeners.Add(v2 => { if (IsCurrentlyPlayersTurn) MS.TryCastSpell(SelectedSpell, 0, 1, v2); });

        c2.CellClickedListeners.Add(v2 => { if (IsCurrentlyPlayersTurn) MS.TryCastSpell(SelectedSpell, 0, 1, v2); });

        MS.ChangeListeners.Add(ComputeAvailableSpells);

        MS.StartGame();
        ComputeAvailableSpells();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (Operation != null)
        {
            if (Operation.MoveNext()) Operation = null;
        }

        if (MS.CurrentTurn == 1 && MS.CurrentPhase == MatchState.Phase.Main)
        {
            Operation = AI.RunMainTurn(MS.CurrentTurn, MS).GetEnumerator();
        }

        if (MS.CurrentTurn == 0 && MS.CurrentPhase == MatchState.Phase.Reaction)
        {
            Operation = AI.RunReactionTurn(MS.CurrentTurn, MS).GetEnumerator();
        }

        this.FindChildByName<Label>("TurnStatusLabel").Text = $"{MS.CurrentTurn} / {MS.CurrentPhase}";

        this.FindChildByName<Label>("IncomingSpellsLabel").Text = $"Incoming: {string.Join(", ", MS.PendingSpells.Select(it => $"{it.Item1.Name}>{it.Item2}"))}";

        this.FindChildByName<Label>("AvailableSpellsLabel").Text = $"Available: {string.Join(", ", AvailableSpells.Select(it => $"{it.Item2.Name} ({it.Item1})"))}";

        // stop the player from doing anything if it's not their turn
        if (!IsCurrentlyPlayersTurn) return;

        for (var i = 0; i < 10; ++i)
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

        for (var i = 0; i < 2; ++i)
        {
            CS[i].Grid1.HoveredCellsSource = () => Array.Empty<Tuple<IntVec2, Grid1.HoverType>>();

            var curHover = CS[i].Grid1.CurrentHover;

            if (SelectedSpell != null && curHover is IntVec2 curHoverNotNull)
            {
                //GD.Print("CUR HOVER! " + string.Join(" ", SelectedSpell.Footprint.Select(it => Tuple.Create(it + curHoverNotNull, Grid1.HoverType.SpellBase))));
                CS[i].Grid1.HoveredCellsSource = () => SelectedSpell.Footprint.Select(it => Tuple.Create(it + curHoverNotNull, Grid1.HoverType.SpellBase)).ToArray();
            }
        }
    }

    public void ComputeAvailableSpells()
    {
        var availSpells = new List<Tuple<int, Spell>>();
        int idx = 0;

        foreach (var spell in PossibleSpells)
        {
            if (spell.IsReaction == (MS.CurrentPhase == MatchState.Phase.Reaction))
            {
                if (!spell.IsValidForCaster(MS.Combatants[MS.CurrentTurn])) continue;

                var isValid = false;

                for (var x = 0; x < MS.Combatants[MS.CurrentTurn].Grid.Width && !isValid; ++x)
                {
                    for (var y = 0; y < MS.Combatants[MS.CurrentTurn].Grid.Height && !isValid; ++y)
                    {
                        if (spell.IsValidAtPoint(new IntVec2(x, y), MS.Combatants[MS.CurrentTurn].Grid, MS.Combatants[1 - MS.CurrentTurn].Grid))
                        {
                            isValid = true;
                        }
                    }
                }

                if (isValid) availSpells.Add(Tuple.Create(idx, spell));
            }

            ++idx;
        }

        AvailableSpells = availSpells.ToArray();
    }
}
