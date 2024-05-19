namespace Obsidian.Domain;

public class EveryDayRecurrence : Recurrence
{
    public override string Pattern { get; set; } = "*";
}

public class EveryWeekdayRecurrence : Recurrence
{
    public override string Pattern { get; set; } = "1-5";
}