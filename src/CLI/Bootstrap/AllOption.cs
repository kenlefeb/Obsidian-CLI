using System.CommandLine;

namespace Obsidian.CLI.Bootstrap;

internal class AllOption() : Option<bool>(new[] { "--all", "-a" },
    "example of using sub-command specific options and validation");
