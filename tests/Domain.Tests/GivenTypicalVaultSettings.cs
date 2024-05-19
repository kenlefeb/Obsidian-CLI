using FakeItEasy;
using Obsidian.Domain;
using Obsidian.Domain.Abstractions.Services;
using Obsidian.Domain.Services;
using Obsidian.Domain.Settings;
using Obsidian.Persistence;
using DailyNotes = Obsidian.Domain.Settings.DailyNotes;

namespace Domain.Tests
{
    public class GivenTypicalVaultSettings
    {
        private readonly VaultSettings _subject = new()
        {
            DailyNotes = new DailyNotes
            {
                Path = "@", 
                Name = "{{ NoteDate | format_date: \"yyyy-MM-dd\"}}.md", 
                TemplateType = "Daily Note", 
                SearchPattern = @"\d{4}-\d{2}-\d{2}\.md"
            },
            Path = @"O:\Vault",
            Templates = new Templates
            {
                Path = "$",
                Items = new List<Template>
                {
                    new Template
                    {
                        Type = "Daily Note",
                        Name = "{{ NoteDate | format_date: \"yyyy-MM-dd\"}}.md",
                        IsDefault = true,
                        Recurrence = new EveryDayRecurrence(),
                        Extends = null
                    },
                    new Template
                    {
                        Type = "Weekday",
                        IsDefault = false,
                        Recurrence = new EveryWeekdayRecurrence(),
                        Extends = new Template{Type = "Daily Note"}
                    }
                }
            }
        };

        private readonly ITemplater _templater = new Templater();

        [Fact]
        public void Render_ShouldReturnRenderedVaultSettings()
        {
            // Arrange

            // Act
            var result = _subject.Render(_templater);

            // Assert
            Assert.Equal(@"O:\Vault", result.Path);
            Assert.Equal("@", result.DailyNotes.Path);
            Assert.Equal($"{DateTime.Today:yyyy-MM-dd}.md", result.DailyNotes.Name);
            Assert.Equal("$", result.Templates.Path);
        }
    }
}