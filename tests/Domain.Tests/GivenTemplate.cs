using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Obsidian.Domain;
using Obsidian.Domain.Services;
using Obsidian.Persistence;

namespace Domain.Tests
{
    public class GivenTemplater
    {
        private readonly Templater _subject;

        public GivenTemplater()
        {
            _subject = new Templater();
        }

        [Fact]
        public void WhenWeInvokeWithName_ThenHelloNameIsReturned()
        {
            // arrange
            var template = "Hello {{ name }}!";
            var expected = "Hello Ken!";
            
            // act
            var actual = _subject.Render(template, new { name = "Ken" });
            
            // assert
            actual.Should().Be(expected);
        }

        [Fact]
        public void WhenWeInvokeWithDate_ThenDateIsFormatted()
        {
            // arrange
            var template = "Today is {{ NoteDate | format_date: \"dddd MMMM yyyy\" }}";
            var expected = "Today is Friday January 2021";
            
            // act
            var actual = _subject.Render(template, new { NoteDate = new DateOnly(2021, 1, 1) });
            
            // assert
            actual.Should().Be(expected);
        }

        [Fact]
        public void WhenWeInvokeWithEnvironmentVariables_ThenUserProfileIsReturned()
        {
            // arrange
            var template = "My user profile is {{ Environment.USERPROFILE }}";
            var expected = $"My user profile is {Environment.GetEnvironmentVariable("userprofile")}";
            
            // act
            var actual = _subject.Render(template, new { Environment = new EnvironmentVariables() });
            
            // assert
            actual.Should().Be(expected);
        }

        [Fact]
        public void WhenWeInvokeWithDailyNotePath_ThenPathContainsDate()
        {
            // arrange
            var template = "{{ Environment.USERPROFILE }}\\dev\\tmp\\vault\\Daily Notes\\{{ NoteDate | format_date: \"yyyy-MM\" }}";
            var expected = $"{Environment.GetEnvironmentVariable("userprofile")}\\dev\\tmp\\vault\\Daily Notes\\2021-01";

            // act
            var actual = _subject.Render(template, new { NoteDate = new DateOnly(2021, 1, 1), Environment = new EnvironmentVariables() });

            // assert
            actual.Should().Be(expected);
        }
    }
}
