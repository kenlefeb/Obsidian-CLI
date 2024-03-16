
using Obsidian.Persistence;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Obsidian.Domain.Services;

namespace Obsidian.Domain;

public class DailyNote : Note
{

    public DailyNote(Vault vault, DateOnly date, EnvironmentVariables environment)
    {
        Vault = vault;
        var path = DeterminePath(Vault, date, environment);
        (Content, File) = GetContentAndFile(Vault, date, path);
    }

    public DailyNote(Vault vault, string path)
    {
        Vault = vault;
        File = new FileInfo(path);
        var date = DetermineDate(File);
        (Content, File) = GetContentAndFile(Vault, date, path);
    }

    private static (string, FileInfo) GetContentAndFile(Vault vault, DateOnly date, string path)
    {
        var folder = GetFolder(path);
        var template = DetermineTemplate(vault, date);
        var content = CreateNoteContent(template, vault, date);
        var name = ComposeFileName(vault, date);
        var file = CreateFile(folder, name, content);
        return (content, file);
    }

    private static string ComposeFileName(Vault vault, DateOnly date)
    {
        var templater = new Templater();
        return templater.Render(vault.Settings.DailyNotes.Name, new { NoteDate = date });
    }

    private static DateOnly DetermineDate(FileInfo file)
    {
        var name = file.Name;
        var date = DateOnly.Parse(name);
        return date;
    }

    private static FileInfo CreateFile(DirectoryInfo folder, string name, string note)
    {
        var path = Path.Combine(folder.FullName, name);
        System.IO.File.WriteAllText(path, note);
        return new FileInfo(path);
    }

    private static string CreateNoteContent(Template template, Vault vault, DateOnly date)
    {
        var file = GetTemplateFile(template, vault);
        if (!file.Exists)
            throw new FileNotFoundException($"Template file not found: {file.FullName}");
        var contents = System.IO.File.ReadAllText(file.FullName);
        var templater = new Templater();
        return templater.Render(contents, new { NoteDate = date });
    }

    private static FileInfo GetTemplateFile(Template template, Vault vault)
    {
        var path = Path.Combine(vault.Path, vault.Settings.Templates.Path, $"{template.Name}.md");
        var templater = new Templater();
        return new FileInfo(templater.Render(path, new {Environment = new EnvironmentVariables()}));
    }

    private static Template DetermineTemplate(Vault vault, DateOnly date)
    {
        var type = vault.Settings.DailyNotes.TemplateType;
        var candidates = vault.Settings.Templates.Items.Where(t => t.Type == type);
        var template = candidates.FirstOrDefault(t => t.AppliesTo(date));
        return template ?? throw new FileNotFoundException($"No template found for {type} on {date}");
    }

    private static DirectoryInfo GetFolder(string path)
    {
        if (!Directory.Exists(path))
            _ = Directory.CreateDirectory(path!);
        return new DirectoryInfo(path);
    }

    private static string DeterminePath(Vault vault, DateOnly date, EnvironmentVariables environment)
    {
        var path = Path.Combine(vault.Path, vault.Settings.DailyNotes.Root, vault.Settings.DailyNotes.Folder);
        var templater = new Templater();
        return templater.Render(path, new { NoteDate = date, Environment = environment });
    }
}