using Microsoft.VisualBasic;

using System;
using System.IO;
using System.Linq;
using HandlebarsDotNet;
using HandlebarsDotNet.PathStructure;

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
        var date = DetermineDate(File);
        (Content, File) = GetContentAndFile(Vault, date, path);
    }

    private (string, FileInfo) GetContentAndFile(Vault vault, DateOnly date, string path)
    {
        var folder = GetFolder(path);
        var template = DetermineTemplate(vault, date);
        var content = CreateNoteContent(template, vault, date);
        var name = ComposeFileName(vault, date);
        var file = CreateFile(folder, name, content);
        return (content, file);
    }

    private string ComposeFileName(Vault vault, DateOnly date)
    {
        var template = vault.Settings.DailyNotes.Name;
        Handlebars.RegisterHelper("date", DateHelper);
        var compiled = Handlebars.Compile(template);
        var data = new { date = date };
        return compiled(data);
    }

    private DateOnly DetermineDate(FileInfo file)
    {
        var name = file.Name;
        var date = DateOnly.Parse(name);
        return date;
    }

    private FileInfo CreateFile(DirectoryInfo folder, string name, string note)
    {
        var path = Path.Combine(folder.FullName, name);
        System.IO.File.WriteAllText(path, note);
        return new FileInfo(path);
    }

    private string CreateNoteContent(Template template, Vault vault, DateOnly date)
    {
        var file = GetTemplateFile(template, vault);
        var contents = System.IO.File.ReadAllText(file.FullName);
        Handlebars.RegisterHelper("date", DateHelper);
        var compiled = Handlebars.Compile(contents);
        var data = new { date = date };
        return compiled(data);
    }

    private FileInfo GetTemplateFile(Template template, Vault vault)
    {
        var path = Path.Combine(vault.Path, vault.Settings.Templates.Path, $"{template.Name}.md");
        return new FileInfo(path);
    }

    private Template DetermineTemplate(Vault vault, DateOnly date)
    {
        var type = vault.Settings.DailyNotes.TemplateType;
        var candidates = vault.Settings.Templates.Items.Where(t => t.Type == type);
        return candidates.FirstOrDefault(t => t.AppliesTo(date));
    }

    private DirectoryInfo GetFolder(string path)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        return new DirectoryInfo(path);
    }

    private string DeterminePath(Vault vault, DateOnly date)
    {
        var template = vault.Settings.DailyNotes.Folder;
        Handlebars.RegisterHelper("date", DateHelper);
        var compiled = Handlebars.Compile(template);
        var data = new { date = date };
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