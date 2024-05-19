using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Divergic.Logging.Xunit;

using FluentAssertions;

using Obsidian;
using Obsidian.Domain.Settings;
using Xunit.Abstractions;

namespace Obsidian.Persistence.Tests
{
    public class GivenVault
    {
        private readonly ICacheLogger<Persistence.Vault> _logger;
        private readonly MockFileSystem _filesystem;
        private readonly VaultSettings _settings;
        private readonly Domain.Services.Templater _templater;
        private readonly IEnvironmentVariables _environment;

        public GivenVault(ITestOutputHelper output)
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
            _filesystem = new MockFileSystem();
            _settings = new Domain.Settings.VaultSettings
            {
                Path = @"{{ Environment.USERPROFILE }}\Documents\Obsidian\Vault",
                DailyNotes = new Domain.Settings.DailyNotes
                {
                    Path = @"@\{{ NoteDate | format_date: ""yyyy\MM MMMM\dd dddd"" }}",
                    Name = "{{ NoteDate | format_date: \"yyyy-MM-dd\" }}.md",
                    TemplateType = "Daily Note",
                    SearchPattern = @"\d{4}-\d\d-\d\d\.md"
                },
                Templates = new Domain.Settings.Templates
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
            var subject = Vault.Create(_logger, _filesystem, _settings, _environment);

            // assert
            subject.Exists.Should().BeTrue();
        }

        [Fact]
        public void WhenWeInstantiateInEmptyFolder_ThenItDoesNotExistInFileSytem()
        {
            // arrange

            // act
            var subject = new Vault(_logger, _filesystem, _settings, _environment);

            // assert
            subject.Exists.Should().BeFalse();
        }
    }
}
