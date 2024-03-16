using System.CommandLine;

namespace Obsidian.CLI.Global
{
    public class DryRunOption()
        : Option<bool>(new string[] { "--dry-run", "-d" }, "Validates and displays configuration")
    {
    }
}
