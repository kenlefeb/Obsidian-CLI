using FluentAssertions;
using Obsidian.Domain;

namespace obsidian.tests;

public class TemplateTests
{
    [Fact]
    public void AppliesTo_WithNullRecurrence_ReturnsTrue()
    {
        // Arrange
        var template = new Template
        {
            Type = "daily-note",
            Name = "default",
            Recurrence = null
        };
        var date = new DateOnly(2026, 1, 17);

        // Act
        var result = template.AppliesTo(date);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void AppliesTo_WithRecurrenceThatIncludesDate_ReturnsTrue()
    {
        // Arrange
        var recurrence = new Recurrence
        {
            Start = new DateOnly(2026, 1, 1),
            End = new DateOnly(2026, 12, 31),
            Pattern = "weekdays"
        };
        var template = new Template
        {
            Type = "daily-note",
            Name = "weekday-template",
            Recurrence = recurrence
        };
        var date = new DateOnly(2026, 6, 15); // Mid-year date within range

        // Act
        var result = template.AppliesTo(date);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void AppliesTo_WithRecurrenceThatExcludesDate_ReturnsFalse()
    {
        // Arrange
        var recurrence = new Recurrence
        {
            Start = new DateOnly(2026, 1, 1),
            End = new DateOnly(2026, 12, 31),
            Pattern = "weekdays"
        };
        var template = new Template
        {
            Type = "daily-note",
            Name = "weekday-template",
            Recurrence = recurrence
        };
        var date = new DateOnly(2027, 6, 15); // Date outside range

        // Act
        var result = template.AppliesTo(date);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void AppliesTo_WithDefaultRecurrence_ReturnsTrue()
    {
        // Arrange - Template creates EveryDayRecurrence by default
        var template = new Template
        {
            Type = "daily-note",
            Name = "default"
            // Recurrence will be EveryDayRecurrence by default
        };
        var date = new DateOnly(2026, 1, 17);

        // Act
        var result = template.AppliesTo(date);

        // Assert
        result.Should().BeTrue();
    }
}
