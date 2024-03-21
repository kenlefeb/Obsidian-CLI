using Microsoft.VisualStudio.TestPlatform.Utilities;
using Obsidian.Domain.Services;
using Obsidian.Domain.Settings;

using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using Divergic.Logging.Xunit;
using FluentAssertions;
using Obsidian.Domain.Abstractions.Settings;
using Obsidian.Persistence;
using Xunit.Abstractions;

namespace Persistence.Tests
{
    public class GivenExistingDailyNote
    {
        private readonly ICacheLogger<Vault> _logger;
        private readonly Templater _templater;
        private readonly MockFileSystem _filesystem;
        private readonly IVaultSettings _settings;
        private readonly Vault _vault;

        public GivenExistingDailyNote(ITestOutputHelper output)
        {
            _logger = output.BuildLoggerFor<Vault>();
            _templater = new Templater(new TemplateData
            {
                Environment = new Dictionary<string, string>
                {
                    { "USERPROFILE", "O:\\kenlefeb" },
                    { "EnvironmentVariable2", "Value2" }
                },
                NoteDate = new DateOnly(2025, 01, 01)
            });
            _filesystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {@"O:\kenlefeb\Documents\Obsidian\Vault\@\2024\03 March\21 Thursday\2024-03-21.md", new MockFileData("Existing Daily Note")}
            });
            _settings = new VaultSettings
            {
                Path = @"{{ Environment.USERPROFILE }}\Documents\Obsidian\Vault",
                DailyNotes = new DailyNotes
                {
                    Path = @"@\{{ NoteDate | format_date: ""yyyy\MM MMMM\dd dddd"" }}",
                    Name = "{{ NoteDate | format_date: \"yyyy-MM-dd\" }}.md",
                    TemplateType = "Daily Note",
                    SearchPattern = @"\d{4}-\d\d-\d\d\.md"
                },
                Templates = new Templates
                {
                    Path = @"=\Obsidian\Templates",
                }
            }.Render(_templater);
            _vault = Vault.Create(_logger, _filesystem, _settings);
        }

        [Fact]
        public void WhenWeAddWithoutForce_ThenExistingNoteRemains()
        {
            // arrange
            var expected = new DailyNote
            {
                Date = new DateOnly(2024, 03, 21),
                Contents = "Existing Daily Note"
            };

            // act
            var actual = _vault.AddDailyNote(date: new DateOnly(2024, 03, 21), force:false);

            // assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void WhenWeAddWithForce_ThenExistingNoteIsReplaced()
        {
            // arrange
            var expected = new DailyNote
            {
                Date = new DateOnly(2024, 03, 21),
                Contents = "Existing Daily Note"
            };

            // act
            var actual = _vault.AddDailyNote(date: new DateOnly(2024, 03, 21), force: true);

            // assert
            actual.Should().NotBeEquivalentTo(expected);
        }

    }
}
