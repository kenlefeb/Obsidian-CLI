using System.CommandLine;

namespace Obsidian.CLI.DailyNotes.Add
{
    internal class VaultOption() : Option<string>(["--vault", "-v"],
        "Specifies the vault in which to create a new daily note.");
}
