using Microsoft.Extensions.Logging;

using Obsidian.Domain;
using Obsidian.Domain.Abstractions;
using Obsidian.Domain.Settings;

using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Text.RegularExpressions;

namespace Obsidian.Persistence
{
    public class NoteRepository(VaultSettings settings, IFileSystem filesystem, ILogger<NoteRepository> logger) : IRepository<Note>
    {
        public IEnumerable<Note> GetAll()
        {
            var filespec = "*.md";
            var files = filesystem.Directory.GetFiles(settings.Path, filespec, SearchOption.AllDirectories);
            foreach (var file in files)
            {
                yield return Get(file);
            }
        }

        public Note Get(string id)
        {
            if (!Exists(id))
                throw new NoteNotFoundException(id);

            var info = filesystem.FileInfo.New(id);
            var contents = filesystem.File.ReadAllText(info.FullName);
            return new Note
            {
                Id = info.FullName,
                Contents = contents
            };
        }

        public void Add(Note note)
        {
            if (Exists(note.Id))
                throw new NoteAlreadyExistsException(note.Id);

            var id = GuaranteeUniqueId(note.Id);
            var info = filesystem.FileInfo.New(id);
            if (!info?.Directory?.Exists ?? false)
                filesystem.Directory.CreateDirectory(info.Directory.FullName);

            filesystem.File.WriteAllText(info.FullName, note.Contents);
        }

        private string GuaranteeUniqueId(string id)
        {
            var fileInfo = filesystem.FileInfo.New(id);
            var path = fileInfo.Directory.FullName;
            var name = GetBaseName(fileInfo);
            var extension = fileInfo.Extension;
            var counter = 1;

            while (Exists(id))
            {
                id = Path.Combine(path, $"{name} ({counter++}){extension}");
            }
            return id;
        }

        private string GetBaseName(IFileInfo fileInfo)
        {
            var baseName = Path.GetFileNameWithoutExtension(fileInfo.Name);
            var regex = new Regex(@"\s+\(\d+\)$");

            if (regex.IsMatch(baseName))
            {
                var index = baseName.LastIndexOf(" (");
                if (index > 0)
                {
                    baseName = baseName.Substring(0, index);
                }
            }
            return baseName.Trim();
        }

        private bool Exists(string id)
        {
            return filesystem.FileInfo.New(id).Exists;
        }

        public void Update(Note note)
        {
            if (!Exists(note.Id))
                throw new NoteNotFoundException(note.Id);

            var info = filesystem.FileInfo.New(note.Id);
            filesystem.File.WriteAllText(info.FullName, note.Contents);
        }

        public void Delete(string id)
        {
            if (!Exists(id))
                throw new NoteNotFoundException(id);

            var info = filesystem.FileInfo.New(id);
            info.Delete();
        }

        public IEnumerable<Note> Find(string pattern)
        {
            var files = filesystem.Directory.GetFiles(settings.Path, pattern, SearchOption.AllDirectories);
            foreach (var file in files)
            {
                yield return Get(file);
            }
        }
    }
}