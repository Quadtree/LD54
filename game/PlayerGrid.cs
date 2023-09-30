public class PlayerGrid
{
    public int Width => 7;
    public int Height => 7;

    public bool[,] CellsAvailable;
    public bool[,] CellsUsed;

    public PlayerGrid()
    {
        CellsAvailable = new bool[Width, Height];

        // temporary!
        for (var y = 0; y < Height; ++y)
        {
            for (var x = 0; x < Width; ++x)
            {
                CellsAvailable[x, y] = true;
            }
        }

        CellsUsed = new bool[Width, Height];
    }

    public bool IsCellOpen(IntVec2 pos)
    {
        if (pos.x < 0 || pos.y < 0 || pos.x >= Width || pos.y >= Height) return false;

        if (CellsAvailable[pos.x, pos.y] && !CellsUsed[pos.x, pos.y]) return true;

        return false;
    }
}