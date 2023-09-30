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

    public void TryCastSpell(Spell spell, int casterId, int targetId, IntVec2 cell)
    {
        if (!spell.IsValidForCaster(Combatants[casterId])) { GD.Print("Not enough SP!"); return; }
        if (!spell.IsValidAtPoint(cell, Combatants[casterId].Grid)) { GD.Print("Not a valid target"); return; }

        spell.Cast(Combatants[casterId], Combatants[targetId], cell);
    }
}