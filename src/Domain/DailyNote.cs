using Microsoft.VisualBasic;

using System;
using System.IO;
using HandlebarsDotNet;
using HandlebarsDotNet.PathStructure;

namespace Obsidian.Domain;

public class DailyNote : Note
{
    private readonly Vault _vault;
    private readonly string _content;
    private readonly FileInfo _file;

    public DailyNote(Vault vault, DateOnly date)
    {
        _vault = vault;
        var path = DeterminePath(_vault, date);
        (_content, _file) = GetContentAndFile(_vault, date, path);
    }

    public DailyNote(Vault vault, string path)
    {
        _vault = vault;
        _file = new FileInfo(path);
        var date = DetermineDate(_file);
        (_content, _file) = GetContentAndFile(_vault, date, path);
    }

    private (string, FileInfo) GetContentAndFile(Vault vault, DateOnly date, string path)
    {
        var folder = GetFolder(path);
        var tempate = DetermineTemplate(vault, date);
        var content = CreateNoteContent(tempate, vault, date);
        var file = CreateFile(folder, date, content);
        return (content, file);
    }

    private DateOnly DetermineDate(FileInfo file)
    {
        var name = file.Name;
        var date = DateOnly.Parse(name);
        return date;
    }

    private FileInfo CreateFile(DirectoryInfo folder, DateOnly date, string note)
    {
        throw new NotImplementedException();
    }

    private string CreateNoteContent(Template tempate, Vault vault, DateOnly date)
    {
        throw new NotImplementedException();
    }

    private Template DetermineTemplate(Vault vault, DateOnly date)
    {
        throw new NotImplementedException();
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