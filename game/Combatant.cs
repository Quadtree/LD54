using Godot;

public class Combatant
{
    public int HP = 15;
    public int SP = 1;
    public int Shield = 0;

    public string Name;

    public PlayerGrid Grid = new PlayerGrid();

    public void TakeDamage(int amt)
    {
        if (amt > Shield && Shield > 0)
        {
            amt -= Shield;
            GD.Print($"{Name} takes {amt} damage, {Shield} absorbed by shield");
            Shield = 0;
            HP -= amt;
        }
        else if (Shield > 0)
        {
            GD.Print($"{Name}'s shield absorbs {amt} damage");
            Shield -= amt;
        }
        else
        {
            GD.Print($"{Name} takes {amt} damage");
            HP -= amt;
        }
    }
}