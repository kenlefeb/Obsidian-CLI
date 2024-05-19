using System.IO.Abstractions.TestingHelpers;

using Divergic.Logging.Xunit;

using FluentAssertions;
using Obsidian.Domain.Settings;
using Xunit.Abstractions;

namespace Obsidian.Persistence.Tests
{
    public class GivenExistingDailyNote
    {
        private readonly ICacheLogger<Persistence.Vault> _logger;
        private readonly Domain.Services.Templater _templater;
        private readonly MockFileSystem _filesystem;
        private readonly VaultSettings _settings;
        private readonly Vault _vault;
        private readonly IEnvironmentVariables _environment;

        public GivenExistingDailyNote(ITestOutputHelper output)
        {
            _logger = output.BuildLoggerFor<Vault>();
            _environment = new EnvironmentVariables(new Dictionary<string, string>{
                { "USERPROFILE", "O:\\kenlefeb" },
                { "EnvironmentVariable2", "Value2" }
            });
            _templater = new Domain.Services.Templater(new Domain.Services.TemplateData
            {
                Environment = _environment,
                NoteDate = new DateOnly(2025, 01, 01)
            });
            _filesystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {@"O:\kenlefeb\Documents\Obsidian\Vault\@\2024\03 March\21 Thursday\2024-03-21.md", new MockFileData("Existing Daily Note")}
            });
            _settings = new Domain.Settings.VaultSettings
            {
                Path = @"{{ Environment.USERPROFILE }}\Documents\Obsidian\Vault",
                DailyNotes = new Domain.Settings.DailyNotes
                {
                    Path = @"{{ NoteDate | format_date: ""yyyy\\\\MM MMMM\\\\dd dddd"" }}",
                    Name = "{{ NoteDate | format_date: \"yyyy-MM-dd\" }}.md",
                    TemplateType = "Daily Note",
                    SearchPattern = @"\d{4}-\d\d-\d\d\.md"
                },
                Templates = new Domain.Settings.Templates
                {
                    Path = @"=\Obsidian\Templates",
                }
            }.Render(_templater);
            _vault = Vault.Create(_logger, _filesystem, _settings, _environment);
        }

        [Fact]
        public void WhenWeAddWithoutForce_ThenExistingNoteRemains()
        {
            // arrange
            var expected = new DailyNote(_vault, new DateOnly(2024, 03, 21))
            {
                Contents = "Existing Daily Note"
            };

            // act
            var actual = _vault.AddDailyNote(date: new DateOnly(2024, 03, 21), force: false);

            // assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void WhenWeAddWithForce_ThenExistingNoteIsReplaced()
        {
            // arrange
            var expected = new DailyNote(_vault, new DateOnly(2024, 03, 21))
            {
                Contents = "Existing Daily Note"
            };

            // act
            var actual = _vault.AddDailyNote(date: new DateOnly(2024, 03, 21), force: true);

            // assert
            actual.Should().NotBeEquivalentTo(expected);
        }

    }
}
