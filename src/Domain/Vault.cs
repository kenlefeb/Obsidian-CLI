using Obsidian.Domain.Settings;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Obsidian.Domain
{
    public class Vault
    {
        public Vault()
        {
            DailyNotes = new DailyNotes(this);
        }

        public string Name { get; set; }
        public string Id { get; set; }
        public string Path { get; set; }
        public VaultSettings Settings { get; set; } = new VaultSettings();
        public DailyNotes DailyNotes { get; set; }
    }

    public class DailyNotes : IQueryable<DailyNote>
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

        private string ComposeSearchPattern(Vault vault)
        {
            return vault.Settings.DailyNotes.SearchPattern;
        }

        private DirectoryInfo FindRootFolderForDailyNotes(Vault vault)
        {
            var path = Path.Combine(vault.Path, vault.Settings.DailyNotes.Root);
            if (!Directory.Exists(path))
                _ = Directory.CreateDirectory(path);

            return new DirectoryInfo(path);
        }

        public IEnumerator<DailyNote> GetEnumerator()
        {
            return _notes.Value.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_notes.Value).GetEnumerator();
        }

        public Type ElementType => _notes.Value.ElementType;

        public Expression Expression => _notes.Value.Expression;

        public IQueryProvider Provider => _notes.Value.Provider;

        public DailyNote Create(DateOnly? date = null)
        {
            var theDate = DetermineDate(date);
            return new DailyNote(_vault, theDate);
        }

        private DateOnly DetermineDate(DateOnly? date)
        {
            return date ?? DateOnly.FromDateTime(DateTime.Now);
        }
    }
}
