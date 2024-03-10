namespace Obsidian.Domain;

public class EveryDayRecurrence : Recurrence
{
    public override string Pattern { get; set; } = "*";
}