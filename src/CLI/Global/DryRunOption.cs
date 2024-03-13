using System.CommandLine;

namespace Obsidian.CLI.Global
{
    internal class DryRunOption()
        : Option<bool>(new string[] { "--dry-run", "-d" }, "Validates and displays configuration")
    {
    }
}
