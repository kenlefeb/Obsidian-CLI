using System.CommandLine;

namespace Obsidian.CLI.DailyNotes.Add
{
    public class VaultOption() : Option<string>(["--vault", "-v"],
        "Specifies the vault in which to create a new daily note.");
}
