using Obsidian.Domain.Abstractions.Services;

namespace Obsidian.Domain.Abstractions.Settings;

public interface IVaultSettings
{
    IDailyNotes DailyNotes { get; set; }
    ITemplates Templates { get; set; }
    string Path { get; set; }
    IVaultSettings Render(ITemplater templater, object? data = null);
}