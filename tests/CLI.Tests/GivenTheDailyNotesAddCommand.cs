using FluentAssertions;
using Obsidian.CLI.DailyNotes.Add;
using Obsidian.CLI.Global;
using Obsidian.Domain;
using Obsidian.Domain.Settings;
using DailyNotes = Obsidian.Domain.DailyNotes;
using Options = Obsidian.CLI.DailyNotes.Add.Options;

namespace CLI.Tests
{
    public class GivenTheDailyNotesAddCommand
    {
        private readonly Command _subject;
        private readonly Configuration _configuration;

        public GivenTheDailyNotesAddCommand()
        {
            _configuration = new Configuration
            {
                Vaults =
                [
                    new Vault
                    {
                        Name = "Testing Vault",
                        Id = "vault",
                        Path = "{{ Environment.USERPROFILE }}\\Documents\\Obsidian\\Vault",
                        Settings = new VaultSettings
                        {
                            DailyNotes = new Obsidian.Domain.Settings.DailyNotes
                            {
                                Root = "@",
                                Folder = "{{ NoteDate | format_date: \"yyyy-MM\" }}",
                                Name = "{{ NoteDate | format_date: \"yyyy-MM-dd\" }}.md",
                                TemplateType = "Daily Note",
                                SearchPattern = @"\d{4}-\d\d-\d\d\.md"
                            },
                            Templates = new Obsidian.Domain.Settings.Templates
                            {
                                Path = "library\\templates",
                                Items = new List<Template>
                                {
                                    new Template
                                    {
                                        Type = "Daily Note",
                                        Name = "daily\\default",
                                        IsDefault = true,
                                        Recurrence = new EveryDayRecurrence(),
                                        Extends = null
                                    }
                                }
                            }
                        }
                    }
                ]
            };
            _subject = new Command(_configuration);
        }

        [Fact]
        public void WhenWeInvokeIt_ThenDailyNoteIsCreated()
        {
            // arrange
            var options = new Options
            {
                Date = DateOnly.FromDateTime(DateTime.Today),
                DryRun = false,
                Verbose = false,
                Vault = "vault"
            };
            
            // act
            var actual = _subject.DoCommand(options);
            
            // assert
            actual.Should().Be(0);
        }
    }
}