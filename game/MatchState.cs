using System;
using System.Collections.Generic;
using Godot;

public class MatchState
{
    public enum Phase
    {
        Main,
        Reaction
    }

    public Combatant[] Combatants = new Combatant[] { new Combatant(), new Combatant() };

    public int CurrentTurn;
    public Phase CurrentPhase;

    public List<Tuple<Spell, IntVec2>> PendingSpells = new List<Tuple<Spell, IntVec2>>();

    public void TryCastSpell(Spell spell, int casterId, int targetId, IntVec2 cell)
    {
        if (spell == null) { GD.Print("No spell selected!"); return; }
        if (!spell.IsValidForCaster(Combatants[casterId])) { GD.Print("Not enough SP!"); return; }
        if (!spell.IsValidAtPoint(cell, Combatants[casterId].Grid)) { GD.Print("Not a valid target"); return; }

        spell.StartCast(Combatants[casterId], Combatants[targetId], cell);

        if (casterId == CurrentTurn && CurrentPhase == Phase.Main)
        {
            PendingSpells.Add(Tuple.Create(spell, cell));
        }
        else if (casterId != CurrentTurn && CurrentPhase == Phase.Reaction)
        {
            spell.FinishCasting(Combatants[casterId], Combatants[targetId], cell);
        }
        else
        {
            GD.PushWarning("This is not valid!");
        }
    }

    public void EndTurn()
    {
        if (CurrentPhase == Phase.Reaction)
        {
            foreach (var it in PendingSpells) it.Item1.FinishCasting(Combatants[CurrentTurn], Combatants[1 - CurrentTurn], it.Item2);

            CurrentTurn = (CurrentTurn + 1) % 2;
            GD.Print($"Now turn for player {CurrentTurn}");
            Combatants[CurrentTurn].SP += 2;
            CurrentPhase = Phase.Main;
        }
        else
        {
            CurrentPhase = Phase.Reaction;
        }
    }
}