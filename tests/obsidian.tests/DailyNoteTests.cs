using FluentAssertions;
using Obsidian.Domain;
using Obsidian.Domain.Settings;

namespace obsidian.tests;

public class DailyNoteTests : IDisposable
{
    private readonly string _tempPath;

    public DailyNoteTests()
    {
        // Create a unique temp directory for each test
        _tempPath = Path.Combine(Path.GetTempPath(), $"ObsidianTests_{Guid.NewGuid()}");
        Directory.CreateDirectory(_tempPath);
    }

    public void Dispose()
    {
        // Clean up temp directory after each test
        if (Directory.Exists(_tempPath))
        {
            Directory.Delete(_tempPath, true);
        }
    }

    private Vault CreateTestVault(string templatesSubPath = "Templates")
    {
        var vault = new Vault
        {
            Name = "Test Vault",
            Id = "test",
            Path = _tempPath,
            Settings = new VaultSettings
            {
                DailyNotes = new Obsidian.Domain.Settings.DailyNotes
                {
                    Root = "Daily Notes",
                    Folder = "{{date \"yyyy-MM\"}}",
                    Name = "{{date \"yyyy-MM-dd\"}}.md",
                    TemplateType = "daily-note"
                },
                Templates = new Obsidian.Domain.Settings.Templates
                {
                    Path = templatesSubPath,
                    Items = new List<Template>
                    {
                        new Template
                        {
                            Type = "daily-note",
                            Name = "default",
                            IsDefault = true
                        }
                    }
                }
            }
        };

        return vault;
    }

    private void CreateTemplateFile(Vault vault, string templateName, string content)
    {
        var templateDir = Path.Combine(vault.Path, vault.Settings.Templates.Path);
        Directory.CreateDirectory(templateDir);
        var templatePath = Path.Combine(templateDir, $"{templateName}.md");
        File.WriteAllText(templatePath, content);
    }

    [Fact]
    public void Constructor_WithValidDateAndTemplate_CreatesNoteFile()
    {
        // Arrange
        var vault = CreateTestVault();
        var templateContent = "# Daily Note for {{date \"yyyy-MM-dd\"}}\n\nToday's date: {{date \"MMMM d, yyyy\"}}";
        CreateTemplateFile(vault, "default", templateContent);
        var testDate = new DateOnly(2026, 1, 17);

        // Act
        var dailyNote = new DailyNote(vault, testDate);

        // Assert
        dailyNote.Should().NotBeNull();
        dailyNote.File.Should().NotBeNull();
        dailyNote.File.Exists.Should().BeTrue();
        dailyNote.File.Name.Should().Be("2026-01-17.md");
        dailyNote.Content.Should().Contain("# Daily Note for 2026-01-17");
        dailyNote.Content.Should().Contain("Today's date: January 17, 2026");
    }

    [Fact]
    public void Constructor_WithValidDate_CreatesCorrectFolderStructure()
    {
        // Arrange
        var vault = CreateTestVault();
        CreateTemplateFile(vault, "default", "# Test");
        var testDate = new DateOnly(2026, 1, 17);

        // Act
        var dailyNote = new DailyNote(vault, testDate);

        // Assert
        var expectedPath = Path.Combine(_tempPath, "Daily Notes", "2026-01");
        Directory.Exists(expectedPath).Should().BeTrue();
        dailyNote.File.DirectoryName.Should().Be(expectedPath);
    }

    [Fact]
    public void Constructor_WithValidDate_ComposesFileNameFromHandlebarsTemplate()
    {
        // Arrange
        var vault = CreateTestVault();
        vault.Settings.DailyNotes.Name = "Daily_{{date \"yyyy-MM-dd\"}}_Note.md";
        CreateTemplateFile(vault, "default", "# Test");
        var testDate = new DateOnly(2026, 3, 15);

        // Act
        var dailyNote = new DailyNote(vault, testDate);

        // Assert
        dailyNote.File.Name.Should().Be("Daily_2026-03-15_Note.md");
    }

    [Fact]
    public void Constructor_WithValidDate_ComposesFolderPathFromHandlebarsTemplate()
    {
        // Arrange
        var vault = CreateTestVault();
        vault.Settings.DailyNotes.Folder = "{{date \"yyyy\"}}/{{date \"MM\"}}";
        CreateTemplateFile(vault, "default", "# Test");
        var testDate = new DateOnly(2026, 12, 25);

        // Act
        var dailyNote = new DailyNote(vault, testDate);

        // Assert
        var expectedPath = Path.Combine(_tempPath, "Daily Notes", "2026", "12");
        dailyNote.File.DirectoryName.Should().Be(expectedPath);
    }

    [Fact]
    public void Constructor_WithMissingTemplateFile_ThrowsFileNotFoundException()
    {
        // Arrange
        var vault = CreateTestVault();
        // Intentionally NOT creating the template file
        var testDate = new DateOnly(2026, 1, 17);

        // Act
        var act = () => new DailyNote(vault, testDate);

        // Assert
        act.Should().Throw<FileNotFoundException>()
            .WithMessage("Template file not found:*");
    }

    [Fact]
    public void Constructor_WithNoMatchingTemplate_ThrowsFileNotFoundException()
    {
        // Arrange
        var vault = CreateTestVault();
        vault.Settings.Templates.Items = new List<Template>
        {
            new Template
            {
                Type = "different-type", // Wrong type
                Name = "default"
            }
        };
        CreateTemplateFile(vault, "default", "# Test");
        var testDate = new DateOnly(2026, 1, 17);

        // Act
        var act = () => new DailyNote(vault, testDate);

        // Assert
        act.Should().Throw<FileNotFoundException>()
            .WithMessage("No template found for daily-note on*");
    }

    [Fact]
    public void Constructor_WithTemplateRecurrence_SelectsCorrectTemplate()
    {
        // Arrange
        var vault = CreateTestVault();
        vault.Settings.Templates.Items = new List<Template>
        {
            new Template
            {
                Type = "daily-note",
                Name = "weekday",
                Recurrence = new Recurrence
                {
                    Start = new DateOnly(2026, 1, 1),
                    End = new DateOnly(2026, 6, 30),
                    Pattern = "*"
                }
            },
            new Template
            {
                Type = "daily-note",
                Name = "default",
                Recurrence = new Recurrence
                {
                    Start = new DateOnly(2026, 7, 1),
                    Pattern = "*"
                }
            }
        };
        CreateTemplateFile(vault, "weekday", "# Weekday Template\nDate: {{date \"yyyy-MM-dd\"}}");
        CreateTemplateFile(vault, "default", "# Default Template\nDate: {{date \"yyyy-MM-dd\"}}");

        var dateInFirstHalf = new DateOnly(2026, 3, 15);
        var dateInSecondHalf = new DateOnly(2026, 9, 20);

        // Act
        var note1 = new DailyNote(vault, dateInFirstHalf);
        var note2 = new DailyNote(vault, dateInSecondHalf);

        // Assert
        note1.Content.Should().Contain("# Weekday Template");
        note2.Content.Should().Contain("# Default Template");
    }

    [Fact]
    public void Constructor_WithComplexHandlebarsTemplate_RendersCorrectly()
    {
        // Arrange
        var vault = CreateTestVault();
        var templateContent = @"# Daily Note

Date: {{date ""yyyy-MM-dd""}}
Year: {{date ""yyyy""}}
Month: {{date ""MMMM""}}
Day: {{date ""dddd""}}

## Tasks
- [ ] Review yesterday
- [ ] Plan today";

        CreateTemplateFile(vault, "default", templateContent);
        var testDate = new DateOnly(2026, 1, 17); // Saturday

        // Act
        var dailyNote = new DailyNote(vault, testDate);

        // Assert
        dailyNote.Content.Should().Contain("Date: 2026-01-17");
        dailyNote.Content.Should().Contain("Year: 2026");
        dailyNote.Content.Should().Contain("Month: January");
        dailyNote.Content.Should().Contain("Day: Saturday");
        dailyNote.Content.Should().Contain("## Tasks");
    }

    [Fact]
    public void Constructor_CreatesNonExistentDirectories()
    {
        // Arrange
        var vault = CreateTestVault();
        // Use Path.DirectorySeparatorChar to ensure cross-platform compatibility
        vault.Settings.DailyNotes.Folder = $"{{{{date \"yyyy\"}}}}{Path.DirectorySeparatorChar}Q{{{{date \"%M\"}}}}{Path.DirectorySeparatorChar}Week{{{{date \"MM\"}}}}";
        CreateTemplateFile(vault, "default", "# Test");
        var testDate = new DateOnly(2026, 1, 17);

        // Act
        var dailyNote = new DailyNote(vault, testDate);

        // Assert
        // Verify the file was created successfully
        dailyNote.File.Exists.Should().BeTrue();
        // Verify nested directory structure was created
        dailyNote.File.DirectoryName.Should().Contain("2026");
        dailyNote.File.DirectoryName.Should().Contain("Q1");
        dailyNote.File.DirectoryName.Should().Contain("Week01");
    }

    [Fact]
    public void Constructor_WithMultipleTemplatesOfSameType_SelectsFirstMatching()
    {
        // Arrange
        var vault = CreateTestVault();
        vault.Settings.Templates.Items = new List<Template>
        {
            new Template
            {
                Type = "daily-note",
                Name = "first",
                Recurrence = new Recurrence { Start = new DateOnly(2020, 1, 1), Pattern = "*" }
            },
            new Template
            {
                Type = "daily-note",
                Name = "second",
                Recurrence = new Recurrence { Start = new DateOnly(2020, 1, 1), Pattern = "*" }
            }
        };
        CreateTemplateFile(vault, "first", "# First Template");
        CreateTemplateFile(vault, "second", "# Second Template");
        var testDate = new DateOnly(2026, 1, 17);

        // Act
        var dailyNote = new DailyNote(vault, testDate);

        // Assert
        dailyNote.Content.Should().Contain("# First Template");
    }
}
