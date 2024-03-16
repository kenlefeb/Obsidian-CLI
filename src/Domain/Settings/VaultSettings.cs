using System.Reflection.Metadata.Ecma335;
using Obsidian.Domain.Abstractions.Services;
using Obsidian.Domain.Abstractions.Settings;

namespace Obsidian.Domain.Settings
{
    public class VaultSettings : IVaultSettings
    {
        public IDailyNotes DailyNotes { get; set; }
        public ITemplates Templates { get; set; }
        public string Path { get; set; }
        public IVaultSettings Render(ITemplater templater, object? data = null)
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
