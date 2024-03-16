using System.CommandLine;

namespace Obsidian.CLI.Global
{
    public class VerboseOption()
        : Option<bool>(new string[] { "--verbose", "-v" }, "Show verbose output")
    {
    }
}
