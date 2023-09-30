using System;
using System.Collections.Generic;
using System.Linq;
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

    public void StartGame()
    {
        Combatants[0].SP += 1;

        foreach (var cmb in Combatants)
        {
            // var openCells = 0;
            // for (var i=0;i<cmb.Grid.Width;++i)
            // {
            //     for (var j=0;j<cmb.Grid.Height;++j){
            //         if (cmb.Grid.IsCellOpen(i,j)) openCells++;
            //     }
            // }

            // cmb.HP =
        }
    }

    public void TryCastSpell(Spell spell, int casterId, int targetId, IntVec2 cell)
    {
        if (spell == null) { GD.Print("No spell selected!"); return; }
        if (!spell.IsValidForCaster(Combatants[casterId])) { GD.Print("Not enough SP!"); return; }
        if (!spell.IsValidAtPoint(cell, Combatants[casterId].Grid)) { GD.Print("Not a valid target"); return; }
        if (spell.IsReaction && CurrentPhase != Phase.Reaction) { GD.Print("Incorrect phase"); return; }
        if (!spell.IsReaction && CurrentPhase != Phase.Main) { GD.Print("Incorrect phase"); return; }

        spell.StartCast(Combatants[casterId], Combatants[targetId], cell);

        if (spell.IsInstant)
        {
            spell.FinishCasting(Combatants[casterId], Combatants[targetId], cell);
        }
        else
        {
            PendingSpells.Add(Tuple.Create(spell, cell));
            ComputeImminentSpellsFor(CurrentTurn);
        }
    }

    public void EndTurn()
    {
        if (CurrentPhase == Phase.Reaction)
        {
            foreach (var it in PendingSpells) it.Item1.FinishCasting(Combatants[CurrentTurn], Combatants[1 - CurrentTurn], it.Item2);
            PendingSpells.Clear();

            Util.ZeroMemory(Combatants[CurrentTurn].Grid.ImminentSpells);

            CurrentTurn = (CurrentTurn + 1) % 2;
            GD.Print($"Now turn for player {CurrentTurn}");
            Combatants[CurrentTurn].SP += 2;
            Combatants[CurrentTurn].Shield = 0;
            CurrentPhase = Phase.Main;
        }
        else
        {
            ComputeImminentSpellsFor(CurrentTurn);
            CurrentPhase = Phase.Reaction;
        }
    }

    public void ComputeImminentSpellsFor(int cmbId)
    {
        Util.ZeroMemory(Combatants[cmbId].Grid.ImminentSpells);

        foreach (var it in PendingSpells) foreach (var it2 in it.Item1.Footprint.Select(it3 => it3 + it.Item2)) if (Combatants[cmbId].Grid.IsCellInBounds(it2)) Combatants[cmbId].Grid.ImminentSpells[it2.x, it2.y] = true;
    }
}