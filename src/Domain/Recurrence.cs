using System;

namespace Obsidian.Domain;

public class Recurrence
{
    public virtual DateOnly Start { get; set; }
    public virtual DateOnly? End { get; set; } = null;
    public virtual string Pattern { get; set; } = "*";

    public bool Includes(DateOnly date)
    {
        if (date < Start)
        {
            return false;
        }

        if (End.HasValue && date > End)
        {
            return false;
        }
        
        return !string.IsNullOrEmpty(Pattern);
    }
}