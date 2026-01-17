using FluentAssertions;
using Obsidian.Domain;

namespace obsidian.tests;

public class RecurrenceTests
{
    [Fact]
    public void Includes_WithDateBeforeStart_ReturnsFalse()
    {
        // Arrange
        var recurrence = new Recurrence
        {
            Start = new DateOnly(2026, 1, 15),
            Pattern = "*"
        };
        var date = new DateOnly(2026, 1, 10); // Before start date

        // Act
        var result = recurrence.Includes(date);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Includes_WithDateOnStart_ReturnsTrue()
    {
        // Arrange
        var recurrence = new Recurrence
        {
            Start = new DateOnly(2026, 1, 15),
            Pattern = "*"
        };
        var date = new DateOnly(2026, 1, 15); // Exactly on start date

        // Act
        var result = recurrence.Includes(date);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Includes_WithDateAfterStart_ReturnsTrue()
    {
        // Arrange
        var recurrence = new Recurrence
        {
            Start = new DateOnly(2026, 1, 15),
            Pattern = "*"
        };
        var date = new DateOnly(2026, 1, 20); // After start date

        // Act
        var result = recurrence.Includes(date);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Includes_WithDateAfterEnd_ReturnsFalse()
    {
        // Arrange
        var recurrence = new Recurrence
        {
            Start = new DateOnly(2026, 1, 1),
            End = new DateOnly(2026, 12, 31),
            Pattern = "*"
        };
        var date = new DateOnly(2027, 1, 1); // After end date

        // Act
        var result = recurrence.Includes(date);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Includes_WithDateOnEnd_ReturnsTrue()
    {
        // Arrange
        var recurrence = new Recurrence
        {
            Start = new DateOnly(2026, 1, 1),
            End = new DateOnly(2026, 12, 31),
            Pattern = "*"
        };
        var date = new DateOnly(2026, 12, 31); // Exactly on end date

        // Act
        var result = recurrence.Includes(date);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Includes_WithDateWithinRange_ReturnsTrue()
    {
        // Arrange
        var recurrence = new Recurrence
        {
            Start = new DateOnly(2026, 1, 1),
            End = new DateOnly(2026, 12, 31),
            Pattern = "*"
        };
        var date = new DateOnly(2026, 6, 15); // Middle of range

        // Act
        var result = recurrence.Includes(date);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Includes_WithEmptyPattern_ReturnsFalse()
    {
        // Arrange
        var recurrence = new Recurrence
        {
            Start = new DateOnly(2026, 1, 1),
            Pattern = "" // Empty pattern
        };
        var date = new DateOnly(2026, 6, 15);

        // Act
        var result = recurrence.Includes(date);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Includes_WithNullPattern_ReturnsFalse()
    {
        // Arrange
        var recurrence = new Recurrence
        {
            Start = new DateOnly(2026, 1, 1),
            Pattern = null // Null pattern
        };
        var date = new DateOnly(2026, 6, 15);

        // Act
        var result = recurrence.Includes(date);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Includes_WithNoEndDate_AllowsFutureDates()
    {
        // Arrange
        var recurrence = new Recurrence
        {
            Start = new DateOnly(2026, 1, 1),
            End = null, // No end date
            Pattern = "*"
        };
        var date = new DateOnly(2030, 12, 31); // Far future date

        // Act
        var result = recurrence.Includes(date);

        // Assert
        result.Should().BeTrue();
    }
}
