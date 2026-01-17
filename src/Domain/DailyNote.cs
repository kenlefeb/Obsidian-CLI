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
        var path = DeterminePath(Vault, date);
        (Content, File) = GetContentAndFile(Vault, date, path);
    }

    public DailyNote(Vault vault, string path)
    {
        Vault = vault;
        File = new FileInfo(path);

        // If file exists, load it; otherwise create it
        if (File.Exists)
        {
            Content = System.IO.File.ReadAllText(path);
        }
        else
        {
            var date = DetermineDate(File);
            (Content, File) = GetContentAndFile(Vault, date, path);
        }
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
        var template = vault.Settings.DailyNotes.Name;
        Handlebars.RegisterHelper("date", DateHelper);
        var compiled = Handlebars.Compile(template);
        var data = new { date };
        return compiled(data);
    }

    private static DateOnly DetermineDate(FileInfo file)
    {
        // Remove the file extension before parsing
        var nameWithoutExtension = Path.GetFileNameWithoutExtension(file.Name);
        var date = DateOnly.Parse(nameWithoutExtension);
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
        Handlebars.RegisterHelper("date", DateHelper);
        var compiled = Handlebars.Compile(contents);
        var data = new { date };
        return compiled(data);
    }

    private static FileInfo GetTemplateFile(Template template, Vault vault)
    {
        var path = Path.Combine(vault.Path, vault.Settings.Templates.Path, $"{template.Name}.md");
        return new FileInfo(path);
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

    private static string DeterminePath(Vault vault, DateOnly date)
    {
        var template = vault.Settings.DailyNotes.Folder;
        Handlebars.RegisterHelper("date", DateHelper);
        var compiled = Handlebars.Compile(template);
        var data = new { date };
        var path = compiled(data);
        return Path.Combine(vault.Path, vault.Settings.DailyNotes.Root, path);
    }

    private static void DateHelper(EncodedTextWriter output, Context context, Arguments arguments)
    {
        var date = DateOnly.Parse($"{context["date"]}");
        var format = $"{arguments[0]}";
        output.WriteSafeString(date.ToString(format));
    }
}