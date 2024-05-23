using FluentAssertions;

using Obsidian.Domain;

namespace Domain.Tests
{
    public class GivenFrontMatter
    {
        [Fact]
        public void WhenTypicalYaml_ThenParsesToGraph()
        {
            // arrange
            var input = """
                        ---
                        type: daily-note
                        date: 2021-01-01
                        template:
                            type: daily-note
                            path: "@/{{ date | format_date: yyyy/MM mmmm/dd dddd/yyyy-MM-dd}}.md"
                        tags:
                            - daily
                            - journal
                            - personal
                        ---
                        """;

            // act
            var actual = FrontMatter.Parse(input);

            // assert
            actual.Keys.Count.Should().Be(4);
            actual["type"].Should().Be("daily-note");
        }
    }
}
