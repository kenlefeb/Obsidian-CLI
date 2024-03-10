using System;

namespace Obsidian.Domain;

public class Recurrence
{
    public virtual DateOnly? Start { get; set; } = null;
    public virtual DateOnly? End { get; set; } = null;
    public virtual string Pattern { get; set; } = "*";
}