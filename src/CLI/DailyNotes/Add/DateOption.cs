using System;
using System.CommandLine;
using System.CommandLine.Parsing;

namespace Obsidian.CLI.DailyNotes.Add
{
    internal class DateOption() : Option<DateOnly>(ALIASES, ParseArgument, false,
        "The for which to create a Daily Note. Defaults to today.")
    {
        private static readonly string[] ALIASES = ["--date", "-d"];

        private static DateOnly ParseArgument(ArgumentResult result)
        {
            return result.Tokens.Count == 0 ? DateOnly.FromDateTime(DateTime.Now) : DateOnly.Parse(result.Tokens[0].Value);
        }
    }
}
