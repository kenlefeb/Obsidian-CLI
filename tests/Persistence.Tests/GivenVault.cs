using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Divergic.Logging.Xunit;
using FluentAssertions;
using Obsidian.Domain.Abstractions.Settings;
using Obsidian.Domain.Services;
using Obsidian.Domain.Settings;
using Obsidian.Persistence;
using Xunit.Abstractions;

namespace Persistence.Tests
{
    public class GivenVault
    {
        private readonly ICacheLogger<Vault> _logger;
        private readonly MockFileSystem _filesystem;
        private readonly IVaultSettings _settings;
        private readonly Templater _templater;

        public GivenVault(ITestOutputHelper output)
        {
            _logger = output.BuildLoggerFor<Vault>();
            _templater = new Templater(new TemplateData
            {
                Environment = new Dictionary<string, string>
                {
                    { "USERPROFILE", "O:\\kenlefeb" },
                    { "EnvironmentVariable2", "Value2" }
                },
                NoteDate = new DateOnly(2025,01,01)
            });
            _filesystem = new MockFileSystem();
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
        }

        [Fact]
        public void WhenWeCreate_ThenItExistsInFileSytem()
        {
            // arrange

            // act
            var subject = Vault.Create(_logger, _filesystem, _settings);

            // assert
            subject.Exists.Should().BeTrue();
        }

        [Fact]
        public void WhenWeInstantiateInEmptyFolder_ThenItDoesNotExistInFileSytem()
        {
            // arrange

            // act
            var subject = new Vault(_logger, _filesystem, _settings);

            // assert
            subject.Exists.Should().BeFalse();
        }
    }
}
