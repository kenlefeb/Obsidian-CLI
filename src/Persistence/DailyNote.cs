using Obsidian.Domain.Services;
using Obsidian.Domain;

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;

namespace Obsidian.Persistence
{
    public class DailyNote : Note
    {
        public DailyNote(Vault vault, string path) : base(vault, path)
        {
            var date = DetermineDate(File);
            (Contents, File) = GetContentAndFile(Vault, date, path);
        }

        public DailyNote(Vault vault, DateOnly date, bool force = false) : base(vault)
        {
            var path = DeterminePath(Vault, date, Vault.Environment);
            (Contents, File) = GetContentAndFile(Vault, date, path, force);
        }

        public DateOnly Date { get; set; }

        private (string, IFileInfo) GetContentAndFile(Vault vault, DateOnly date, string path, bool force = false)
        {
            var folder = GetFolder(path);
            var name = ComposeFileName(vault, date);
            var filespec = Path.Combine(folder.FullName, name);
            var file = vault.GetFile(filespec);
            var content = default(string);
            
            if (force || !file.Exists)
            {
                var template = DetermineTemplate(vault, date);
                content = CreateNoteContent(template, vault, date);
                file = CreateFile(folder, name, content);
            }
            else
            {
                content = Vault.ReadTextFile(file.FullName);
            }
            return (content, file);
        }

        private string ComposeFileName(Vault vault, DateOnly date)
        {
            return Vault.Templater.Render(vault.Settings.DailyNotes.Name, new { NoteDate = date });
        }

        private static DateOnly DetermineDate(IFileInfo file)
        {
            var name = file.Name;
            var date = DateOnly.Parse(name);
            return date;
        }

        private IFileInfo CreateFile(IDirectoryInfo folder, string name, string note)
        {
            var path = Path.Combine(folder.FullName, name);
            Vault.WriteTextFile(path, note);
            return Vault.GetFile(path);
        }

        private string CreateNoteContent(Template template, Vault vault, DateOnly date)
        {
            var file = GetTemplateFile(template, vault);
            if (!file.Exists)
                throw new FileNotFoundException($"Template file not found: {file.FullName}");
            var contents = Vault.ReadTextFile(file.FullName);
            return Vault.Templater.Render(contents, new { NoteDate = date });
        }

        private IFileInfo GetTemplateFile(Template template, Vault vault)
        {
            var path = Path.Combine(vault.Path, vault.Settings.Templates.Path, $"{template.Name}.md");
            return Vault.GetFile(Vault.Templater.Render(path, new { Environment = new EnvironmentVariables() }));
        }

        private static Template DetermineTemplate(Vault vault, DateOnly date)
        {
            var type = vault.Settings.DailyNotes.TemplateType;
            var candidates = vault.Settings.Templates.Items.Where(t => t.Type == type);
            var template = candidates.FirstOrDefault(t => t.AppliesTo(date));
            return template ?? throw new FileNotFoundException($"No template found for {type} on {date}");
        }

        private IDirectoryInfo GetFolder(string path)
        {
            if (!Vault.DirectoryExists(path))
                _ = Vault.CreateDirectory(path!);
            return Vault.GetDirectory(path);
        }

        private string DeterminePath(Vault vault, DateOnly date, IEnvironmentVariables environment)
        {
            var path = Path.Combine(vault.Path, vault.Settings.Render(Vault.Templater).DailyNotes.Path);
            return Vault.Templater.Render(path, new { NoteDate = date, Environment = environment });
        }

        
    }
}
