using FluentAssertions;

namespace Obsidian.Persistence.Tests
{
    public class GivenEnvironmentVariables
    {
        private readonly EnvironmentVariables _subject = new(new Dictionary<string, string>
        {
            { "USERPROFILE", @"C:\Users\kenlefeb" }
        });

        [Fact]
        public void WhenWeGetUserProfile_ThenWeGetCorrectPath()
        {
            // arrange
            var expected = @"C:\Users\kenlefeb";
            
            // act
            var actual = _subject["USERPROFILE"];

            // assert
            actual.Should().Be(expected);
        }
    }
}
