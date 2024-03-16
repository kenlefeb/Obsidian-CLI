using System.CommandLine;

namespace Obsidian.CLI.Configuration.List
{
    public class AllOption() : Option<bool>(["--all", "-a"],
        "List all valid settings even if they're not currently set.");
}
