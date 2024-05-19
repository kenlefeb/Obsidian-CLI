using Obsidian.Domain.Abstractions.Services;

namespace Obsidian.Domain.Settings
{
    public class VaultSettings
    {
        public DailyNotes DailyNotes { get; set; }
        public Templates Templates { get; set; }
        public string Path { get; set; }
        public VaultSettings Render(ITemplater templater, object? data = null)
        {
            var rendered = new VaultSettings
            {
                Path = templater.Render(Path, data),
                DailyNotes = DailyNotes,
                Templates = Templates
            };
            rendered.DailyNotes.Path = templater.Render(DailyNotes.Path, data);
            rendered.DailyNotes.Name = templater.Render(DailyNotes.Name, data);
            rendered.Templates.Path = templater.Render(Templates.Path, data);
            return rendered;
        }
    }
}
