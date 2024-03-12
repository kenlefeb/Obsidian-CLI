using HandlebarsDotNet;

using System;
using System.IO;
using System.Linq;

namespace Obsidian.Domain;

public class DailyNote : Note
{

    public DailyNote(Vault vault, DateOnly date)
    {
        Vault = vault;
        string path = DeterminePath(Vault, date);
        (Content, File) = GetContentAndFile(Vault, date, path);
    }

    public DailyNote(Vault vault, string path)
    {
        Vault = vault;
        File = new FileInfo(path);
        DateOnly date = DetermineDate(File);
        (Content, File) = GetContentAndFile(Vault, date, path);
    }

    private (string, FileInfo) GetContentAndFile(Vault vault, DateOnly date, string path)
    {
        DirectoryInfo folder = GetFolder(path);
        Template template = DetermineTemplate(vault, date);
        string content = CreateNoteContent(template, vault, date);
        string name = ComposeFileName(vault, date);
        FileInfo file = CreateFile(folder, name, content);
        return (content, file);
    }

    private string ComposeFileName(Vault vault, DateOnly date)
    {
        string template = vault.Settings.DailyNotes.Name;
        Handlebars.RegisterHelper("date", DateHelper);
        HandlebarsTemplate<object, object> compiled = Handlebars.Compile(template);
        var data = new { date };
        return compiled(data);
    }

    private DateOnly DetermineDate(FileInfo file)
    {
        string name = file.Name;
        DateOnly date = DateOnly.Parse(name);
        return date;
    }

    private FileInfo CreateFile(DirectoryInfo folder, string name, string note)
    {
        string path = Path.Combine(folder.FullName, name);
        System.IO.File.WriteAllText(path, note);
        return new FileInfo(path);
    }

    private string CreateNoteContent(Template template, Vault vault, DateOnly date)
    {
        FileInfo file = GetTemplateFile(template, vault);
        string contents = System.IO.File.ReadAllText(file.FullName);
        Handlebars.RegisterHelper("date", DateHelper);
        HandlebarsTemplate<object, object> compiled = Handlebars.Compile(contents);
        var data = new { date };
        return compiled(data);
    }

    private FileInfo GetTemplateFile(Template template, Vault vault)
    {
        string path = Path.Combine(vault.Path, vault.Settings.Templates.Path, $"{template.Name}.md");
        return new FileInfo(path);
    }

    private Template DetermineTemplate(Vault vault, DateOnly date)
    {
        string type = vault.Settings.DailyNotes.TemplateType;
        System.Collections.Generic.IEnumerable<Template> candidates = vault.Settings.Templates.Items.Where(t => t.Type == type);
        return candidates.FirstOrDefault(t => t.AppliesTo(date));
    }

    private DirectoryInfo GetFolder(string path)
    {
        if (!Directory.Exists(path))
            _ = Directory.CreateDirectory(path);
        return new DirectoryInfo(path);
    }

    private string DeterminePath(Vault vault, DateOnly date)
    {
        string template = vault.Settings.DailyNotes.Folder;
        Handlebars.RegisterHelper("date", DateHelper);
        HandlebarsTemplate<object, object> compiled = Handlebars.Compile(template);
        var data = new { date };
        string path = compiled(data);
        return Path.Combine(vault.Path, vault.Settings.DailyNotes.Root, path);
    }

    private static void DateHelper(EncodedTextWriter output, Context context, Arguments arguments)
    {
        DateOnly date = DateOnly.Parse($"{context["date"]}");
        string format = $"{arguments[0]}";
        output.WriteSafeString(date.ToString(format));
    }
}