using System;
using System.Collections.Generic;

public class Spell
{
    public virtual string Name => this.GetType().ToString();

    public virtual IReadOnlyList<IntVec2> Footprint => Array.Empty<IntVec2>();
}