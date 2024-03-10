using System;

namespace Obsidian.Domain;

public class Template
{
    public string Type { get; set; }
    public string Name { get; set; }
    public bool IsDefault { get; set; }
    public Recurrence Recurrence { get; set; } = new EveryDayRecurrence();
    public Template? Extends { get; set; } = null;

    public bool AppliesTo(DateOnly date)
    {
        return true;
    }
}