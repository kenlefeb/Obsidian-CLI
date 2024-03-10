using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obsidian.CLI.DailyNotes.Add
{
    public class DateOption() : Option<DateOnly>(ALIASES, ParseArgument, false,
        "The for which to create a Daily Note. Defaults to today.")
    {
        private static readonly string[] ALIASES = ["--date", "-d"];

        private static DateOnly ParseArgument(ArgumentResult result)
        {
            if (result.Tokens.Count == 0)
            {
                return DateOnly.FromDateTime(DateTime.Now);
            }
            else
            {
                return DateOnly.Parse(result.Tokens[0].Value);
            }
        }
    }
}
