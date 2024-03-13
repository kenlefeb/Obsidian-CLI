namespace Obsidian.Domain;

internal class EveryDayRecurrence : Recurrence
{
    public override string Pattern { get; set; } = "*";
}