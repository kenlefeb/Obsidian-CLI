using System.CommandLine;

namespace Obsidian.CLI.Global
{
    internal class VerboseOption()
        : Option<bool>(new string[] { "--verbose", "-v" }, "Show verbose output")
    {
    }
}
