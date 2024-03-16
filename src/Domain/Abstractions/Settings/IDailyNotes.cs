namespace Obsidian.Domain.Abstractions.Settings;

public interface IDailyNotes
{
    string Path { get; set; }
    string Name { get; set; }
    string TemplateType { get; set; }
    string SearchPattern { get; set; }
}