using Obsidian.Domain;

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Obsidian.Persistence;

public class DailyNotes
{
    private readonly Vault _vault;
    private readonly Lazy<IQueryable<DailyNote>> _notes;

    public DailyNotes(Vault vault)
    {
        _vault = vault;
        _notes = new Lazy<IQueryable<DailyNote>>(() =>
        {
            var pattern = ComposeSearchPattern(vault);
            var folder = FindRootFolderForDailyNotes(vault);
            return FindDailyNotes(folder, pattern);
        });
    }
    private IQueryable<DailyNote> FindDailyNotes(DirectoryInfo folder, string pattern)
    {
        Regex regex = new(pattern);

        return Directory.GetFiles(folder.FullName, pattern)
            .Where(path => regex.IsMatch(path))
            .Select(path => new DailyNote(_vault, path))
            .AsQueryable();
    }

    private static string ComposeSearchPattern(Vault vault)
    {
        return vault.Settings.DailyNotes.SearchPattern;
    }

    private static DirectoryInfo FindRootFolderForDailyNotes(Vault vault)
    {
        var info = new DirectoryInfo(vault.Path);
        var path = Path.Combine(vault.Path, info.Root.Name);
        if (!Directory.Exists(path))
            _ = Directory.CreateDirectory(path);

        return new DirectoryInfo(path);
    }

    public DailyNote Create(DateOnly? date = null)
    {
        var theDate = DetermineDate(date);
        return new DailyNote(_vault, theDate);
    }

    private static DateOnly DetermineDate(DateOnly? date)
    {
        return date ?? DateOnly.FromDateTime(DateTime.Now);
    }

}