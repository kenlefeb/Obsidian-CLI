using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Obsidian.Domain;
using Obsidian.Domain.Abstractions;
using Obsidian.Persistence;

using Parlot.Fluent;

namespace Obsidian.Persistence
{
    public class NoteRepository(Vault vault) : IRepository<Note>
    {
        private readonly Vault _vault = vault ?? throw new ArgumentNullException(nameof(vault));

        public IEnumerable<Note> GetAll()
        {
            return _vault.GetFiles("*.md").Select(file => new Note(_vault, file.FullName));
        }

        public Note GetById(string id)
        {
            var file = _vault.GetFile(id);
            if (file == null)
                throw new FileNotFoundException($"Note with id '{id}' not found.");
            return new Note(_vault, file.FullName);
        }

        public void Add(Note note)
        {
            _vault.WriteTextFile(note.Id, note.Contents);
        }

        public void Update(Note note)
        {
            var file = _vault.GetFile(note.Id);
            if (file == null)
                throw new FileNotFoundException($"Note with id '{note.Id}' not found.");
            _vault.WriteTextFile(note.Id, note.Contents);
        }

        public void Delete(string id)
        {
            var file = _vault.GetFile(id);
            if (file == null)
                throw new FileNotFoundException($"Note with id '{id}' not found.");
            _vault.DeleteFile(file.FullName);
        }
    }
}