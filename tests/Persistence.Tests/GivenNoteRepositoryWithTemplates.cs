using FluentAssertions;

using Obsidian.Domain;
using Obsidian.Domain.Settings;

using System.IO.Abstractions.TestingHelpers;

using Xunit.Abstractions;

namespace Obsidian.Persistence.Tests
{
    public class GivenNoteRepositoryWithTemplates
    {
        private readonly MockFileSystem _fileSystem;
        private readonly NoteRepository _subject;

        public GivenNoteRepositoryWithTemplates(ITestOutputHelper output)
        {
            var settings = new VaultSettings
            {
                Path = "O:\\Vault",
                Templates = new Templates
                {
                    Path = "O:\\Vault\\Templates"
                }
            };
            _fileSystem = new MockFileSystem();
            _fileSystem.AddFile("Daily Note.md", new MockFileData("---\ntemplate-type: daily-note\ntemplate-path: @{{ date | format_date: \"yyyy\\MM mmmm\\dd dddd\\yyyy-MM-dd.md\" }}\ntype: daily-note\ndate: {{ \"now\" | format_date: \"yyyy-MM-dd\" }}\n---\n# {{ \"now\" | format_date: \"MM dddd\" }}\n\n{{ contents }}"));

            _subject = new NoteRepository(settings, _fileSystem, output.BuildLoggerFor<NoteRepository>());
        }


        [Fact]
        public void AddWithNewPath_ShouldCreateNewNoteWithRenderedContent()
        {
            // Arrange
            var newNote = new Note
            {
                Id = "O:\\Vault\\@\\2024\\03 March\\01 Friday\\2024-03-01.md",
                Title = "01 Friday",
                Contents = "This is a new note"
            };
            var now = DateTime.Now;
            var expected = $"---\ntemplate-type: daily-note\ntype: daily-note\ndate: {now:yyy-MM-dd}\n---\n# {now:MM dddd}\n\n{newNote.Contents}";

            // Act
            _subject.Add(newNote);

            // Assert the file exists
            var actual = _fileSystem.FileInfo.New(newNote.Id);
            actual.Exists.Should().BeTrue();

            // Assert the contents are correct
            var contents = _fileSystem.File.ReadAllText(actual.FullName);
            contents.Should().Be(expected);
        }

    }
}
