using FluentAssertions;

using Obsidian.Domain;
using Obsidian.Domain.Settings;

using System.IO.Abstractions.TestingHelpers;

using Xunit.Abstractions;

namespace Obsidian.Persistence.Tests
{
    public class GivenNoteRepositoryWithExistingNotes
    {
        private readonly MockFileSystem _fileSystem;
        private readonly NoteRepository _subject;

        public GivenNoteRepositoryWithExistingNotes(ITestOutputHelper output)
        {
            var settings = new VaultSettings
            {
                Path = "O:\\Vault"
            };
            _fileSystem = new MockFileSystem();

            for (var date = new DateTime(2024, 1, 1); date <= new DateTime(2024, 2, 29); date = date.AddDays(1))
            {
                var title = $"{date:dd} {date:ddddd}";
                var path = $"O:\\Vault\\@\\{date:yyyy}\\{date:MM} {date:MMMM}\\{date:dd} {date:ddddd}\\{date:yyyy-MM-dd}.md";
                _fileSystem.AddFile(path, new MockFileData($"# {title}\n\nSample note for {date:yyyy-MM-dd}"));
            }

            _subject = new NoteRepository(settings, _fileSystem, output.BuildLoggerFor<NoteRepository>());
        }

        [Fact]
        public void GetAll_ShouldReturnAllNotes()
        {
            // Arrange
            var expected = _fileSystem.AllFiles.Count();

            // Act
            var actual = _subject.GetAll().Count();

            // Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public void GetWithFullPath_ShouldReturnNote()
        {
            // Arrange
            var id = "O:\\Vault\\@\\2024\\02 February\\29 Thursday\\2024-02-29.md";

            // Act
            var actual = _subject.Get(id);

            // Assert
            actual.Id.Should().Be(id);
        }

        [Fact]
        public void FindWithFilename_ShouldReturnNote()
        {
            // Arrange
            var id = "2024-02-29.md";

            // Act
            var actual = _subject.Find(id).FirstOrDefault();

            // Assert
            actual?.Id.Should().EndWith(id);
        }

        [Fact]
        public void FindWithBasename_ShouldReturnNote()
        {
            // Arrange
            var id = "2024-02-29";

            // Act
            var actual = _subject.Find(id).FirstOrDefault();

            // Assert
            actual?.Id.Should().EndWith($"{id}.md");
        }

        [Fact]
        public void AddWithNewPath_ShouldCreateNewNote()
        {
            // Arrange
            var newNote = new Note
            {
                Id = "O:\\Vault\\@\\2024\\03 March\\01 Friday\\2024-03-01.md",
                Title = "01 Friday",
                Contents = "This is a new note"
            };

            // Act
            _subject.Add(newNote);

            // Assert the file exists
            var actual = _fileSystem.FileInfo.New(newNote.Id);
            actual.Exists.Should().BeTrue();

            // Assert the contents are correct
            var contents = _fileSystem.File.ReadAllText(actual.FullName);
            contents.Should().Be(newNote.Contents);
        }

        [Fact]
        public void ForceAddWithExistingPath_ShouldCreateNewNote()
        {
            // Arrange
            var existingId = "O:\\Vault\\@\\2024\\02 February\\01 Thursday\\2024-02-01.md";
            var newNote = new Note
            {
                Id = existingId,
                Title = "01 Thursday (again)",
                Contents = "This is a new note with a \"uniqueified\" name"
            };

            // Act
            _subject.Add(newNote, force: true);

            // Assert the file exists
            var actual = _fileSystem.FileInfo.New(newNote.Id);
            actual.Exists.Should().BeTrue();

            // Assert the contents are correct
            var contents = _fileSystem.File.ReadAllText(actual.FullName);
            contents.Should().Be(newNote.Contents);

            // Assert the Id was changed
            newNote.Id.Should().NotBe(existingId);
        }

        [Fact]
        public void AddWithExistingPath_ShouldThrowException()
        {
            // Arrange
            var existingId = "O:\\Vault\\@\\2024\\02 February\\01 Thursday\\2024-02-01.md";
            var newNote = new Note
            {
                Id = existingId,
                Title = "01 Thursday (again)",
                Contents = "This is a new note with a \"uniqueified\" name"
            };

            // Act
            _subject.Invoking(x => x.Add(newNote))

                // Assert
                .Should().Throw<NoteAlreadyExistsException>()
                .WithMessage($"A note with Id '{existingId}' already exists.");
        }
    }
}
