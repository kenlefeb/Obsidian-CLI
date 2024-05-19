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
    public class GivenTemplaterWithNoDataProvided
    {
        private readonly Templater _subject = new Templater();

        [Fact]
        public void WhenWeInvokeWithNoData_ThenDefaultDataIsUsed()
        {
            // arrange
            var template = "Today is {{ NoteDate | format_date: \"yyyy-MM-dd\" }}";
            var expected = $"Today is {DateTime.Today:yyyy-MM-dd}";

            // act
            var actual = _subject.Render(template);

            // assert
            actual.Should().Be(expected);
        }
    }
}
