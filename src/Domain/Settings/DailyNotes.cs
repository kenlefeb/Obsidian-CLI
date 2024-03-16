using Obsidian.Domain.Abstractions.Services;
using Obsidian.Domain.Abstractions.Settings;

namespace Obsidian.Domain.Settings;

public class DailyNotes : IDailyNotes
{
    public string Path { get; set; } = "@\\{{ NoteDate | format_date: \"yyyy-MM\" }}";
    public string Name { get; set; } = "{{ NoteDate | format_date: \"yyyy-MM-dd\" }}.md";
    public string TemplateType { get; set; } = "Daily Note";
    public string SearchPattern { get; set; } = @"\d{4}-\d\d-\d\d\.md";
}