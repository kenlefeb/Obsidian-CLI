namespace Obsidian.Domain.Settings;

public class DailyNotes
{
    public string Root { get; set; } = "Daily Notes";
    public string Folder { get; set; } = "YYYY-MM";
    public string Name { get; set; } = "YYYY-MM-DD.md";
    public string TemplateType { get; set; } = "Daily Note";
    public string SearchPattern { get; set; } = @"\d{4}-\d\d-\d\d\.md";
}